using moondraft.RealmObjects;
using Realms;
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

        public ICommand OnScrolledCommand
        {
            get
            {
                return new Command((object parameter) =>
                {
                    var e = parameter as ItemsViewScrolledEventArgs;
                    System.Diagnostics.Debug.WriteLine("e: " + e.LastVisibleItemIndex);
                    if (e.LastVisibleItemIndex == ItemsSource.Count - 1)
                    {
                        OnAppearingLastItem();
                    }
                });
            }
        }

        int CurrentPageNumber;

        int MaxPageNumber;

        int LoadMoreLock;

        public ThreadPageViewModel()
        {
            Thread = Realm.GetInstance().All<SettingsRealmObject>().First().CurrentNode.CurrentThread;
            _ = RefreshAsync();
        }

        async Task RefreshAsync()
        {
            CurrentPageNumber = 0;

            var realm = Realm.GetInstance();

            var currentThread = realm.All<SettingsRealmObject>().First().CurrentNode.CurrentThread;

            MaxPageNumber = await currentThread.UpdateAsync();

            ItemsSource.Clear();
            var comments = currentThread.Comments.OrderByDescending(o => o.CommentDateTime).ToList();
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

        async void OnAppearingLastItem()
        {
            if (Interlocked.Exchange(ref LoadMoreLock, 1) != 0)
            {
                return;
            }
            try
            {
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
            finally
            {
                Interlocked.Exchange(ref LoadMoreLock, 0);
            }
        }
    }
}
