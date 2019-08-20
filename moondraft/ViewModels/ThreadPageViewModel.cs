using moondraft.RealmObjects;
using Realms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

        public IList<CommentRealmObject> ItemsSource { get; set; } = new List<CommentRealmObject>();

        public ICommand OnScrolledCommand
        {
            get
            {
                return new Command((object parameter) =>
                {
                    var e = parameter as ItemsViewScrolledEventArgs;
                    System.Diagnostics.Debug.WriteLine("e: " + e.LastVisibleItemIndex);
                });
            }
        }

        public int CurrentPageNumber = 1;

        public ThreadPageViewModel()
        {
            Thread = Realm.GetInstance().All<SettingsRealmObject>().First().CurrentNode.CurrentThread;
            _ = RefreshAsync();
        }

        async Task RefreshAsync()
        {
            CurrentPageNumber = 1;

            var realm = Realm.GetInstance();

            var currentThread = realm.All<SettingsRealmObject>().First().CurrentNode.CurrentThread;

            await currentThread.UpdateAsync();

            ItemsSource = currentThread.Comments.OrderByDescending(o => o.CommentDateTime).ToList();
            realm.Write(() =>
            {
                foreach (var itemSource in ItemsSource)
                {
                    itemSource.IsFirst = false;
                    itemSource.IsLast = false;
                }
                if (ItemsSource.Any())
                {
                    ItemsSource.First().IsFirst = true;
                    ItemsSource.Last().IsLast = true;
                }
            });
        }
    }
}
