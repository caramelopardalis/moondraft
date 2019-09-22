using AsyncAwaitBestPractices.MVVM;
using moondraft.RealmObjects;
using moondraft.Services;
using Realms;
using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace moondraft.ViewModels
{
    [Preserve(AllMembers = true)]
    class ThreadPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ThreadRealmObject Thread { get; set; }

        public bool IsRefreshing { get; set; }

        public ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    IsRefreshing = true;

                    await RefreshAsync();

                    IsRefreshing = false;
                });
            }
        }

        public ObservableCollection<CommentRealmObject> ItemsSource { get; set; } = new ObservableCollection<CommentRealmObject>();

        public IAsyncCommand LoadMoreNewerAsyncCommand
        {
            get
            {
                return new AsyncCommand(async () =>
                {
                    await LoadMoreNewerAsync();
                });
            }
        }

        public ICommand MayAppearCellCommand
        {
            get
            {
                return new Command((object parameter) =>
                {
                    var cellIndex = (int)parameter;
                    MayAppearCell(cellIndex);
                });
            }
        }

        int CurrentPageNumber;

        int MaxPageNumber;

        const int MaxDownloadingCount = 5;

        int DownloadingCount;

        ConcurrentQueue<int> RequiredDownloadingCellIndexesQueue = new ConcurrentQueue<int>();

        ConcurrentDictionary<int, object> RequiredDownloadingCellIndexesDictionary = new ConcurrentDictionary<int, object>();

        public ThreadPageViewModel()
        {
            Thread = Realm.GetInstance().All<SettingsRealmObject>().First().CurrentNode.CurrentThread;
            _ = RefreshAsync();
            ProcessQueue();
        }

        async Task RefreshAsync()
        {
            CurrentPageNumber = 0;

            var realm = Realm.GetInstance();

            var currentThread = realm.All<SettingsRealmObject>().First().CurrentNode.CurrentThread;

            MaxPageNumber = await currentThread.UpdateAsync();

            ItemsSource.Clear();
            var comments = currentThread.Comments.OrderByDescending(o => o.CommentDateTime).ToList();
            System.Diagnostics.Debug.WriteLine("Comments count: " + comments.Count);
            realm.Write(() =>
            {
                for (var i = 0; i < comments.Count(); i++)
                {
                    var comment = comments[i];
                    comment.IsFirst = false;
                    comment.IsLast = false;
                    if (i == 0)
                    {
                        comment.IsFirst = true;
                    }
                    if (i == comments.Count() - 1)
                    {
                        comment.IsLast = true;
                    }
                    ItemsSource.Add(comment);
                }
            });
        }

        async Task LoadMoreNewerAsync()
        {
            await DetectThreadService.LogAsync();

            if (CurrentPageNumber < MaxPageNumber)
            {
                var realm = Realm.GetInstance();

                var currentThread = realm.All<SettingsRealmObject>().First().CurrentNode.CurrentThread;

                var lastCommentId = ItemsSource.Last().CommentId;
                MaxPageNumber = await currentThread.UpdateAsync(++CurrentPageNumber);
                var newItemsSource = currentThread.Comments.OrderByDescending(o => o.CommentDateTime).ToList();
                var lastComment = newItemsSource.Where(o => o.CommentId == lastCommentId).First();
                var newItemSourceBeginIndex = newItemsSource.IndexOf(lastComment) + 1;
                System.Diagnostics.Debug.WriteLine("newItemSourceBeginIndex: " + newItemSourceBeginIndex);
                await Device.InvokeOnMainThreadAsync(() =>
                {
                    for (var i = newItemSourceBeginIndex; i < newItemsSource.Count; i++)
                    {
                        if (i < newItemsSource.Count)
                        {
                            ItemsSource.Add(newItemsSource[i]);
                        }
                    }
                });

                System.Diagnostics.Debug.WriteLine("New Page: " + CurrentPageNumber);
                System.Diagnostics.Debug.WriteLine("New ItemsSource.Count: " + ItemsSource.Count);
            }
        }

        void MayAppearCell(int index)
        {
            if (!RequiredDownloadingCellIndexesDictionary.TryAdd(index, new object()))
            {
                return;
            }
            RequiredDownloadingCellIndexesQueue.Enqueue(index);
        }

        void ProcessQueue()
        {
            var threadTitle = Thread.ThreadTitle;

            Task.Run(async () =>
            {
                await DetectThreadService.LogAsync();

                int index;
                // This variable has no meaning
                var value = new object();
                while (true)
                {
                    await Task.Delay(1000);

                    if (!RequiredDownloadingCellIndexesQueue.TryPeek(out index))
                    {
                        System.Diagnostics.Debug.WriteLine("Skipped because the queue is empty.");
                        continue;
                    }

                    if (Interlocked.Increment(ref DownloadingCount) > MaxDownloadingCount)
                    {
                        Interlocked.Decrement(ref DownloadingCount);
                        System.Diagnostics.Debug.WriteLine("Skipped because over max concurrent updating.");
                        continue;
                    }

                    var _ = Task.Run(async () =>
                    {
                        try
                        {
                            var commentId = await Device.InvokeOnMainThreadAsync(() => ItemsSource[index].CommentId);
                            System.Diagnostics.Debug.WriteLine("Update: " + commentId);
                            await ItemsSource[index].UpdateAttachment(threadTitle, commentId);
                        }
                        catch (Exception e)
                        {
                            System.Diagnostics.Debug.WriteLine("Exception has occurred: " + index);
                            System.Diagnostics.Debug.WriteLine("Message: " + e.Message);
                            System.Diagnostics.Debug.WriteLine("StackTrace: " + e.StackTrace);
                        }
                        finally
                        {
                            RequiredDownloadingCellIndexesQueue.TryDequeue(out index);
                            Interlocked.Decrement(ref DownloadingCount);
                            RequiredDownloadingCellIndexesDictionary.TryRemove(index, out value);
                        }
                    });
                }
            });
        }
    }
}
