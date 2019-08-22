using moondraft.RealmObjects;
using Realms;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace moondraft.ViewModels
{
    [Preserve(AllMembers = true)]
    class RecentPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

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

        public ObservableCollection<ThreadRealmObject> ItemsSource { get; set; } = new ObservableCollection<ThreadRealmObject>();

        public RecentPageViewModel()
        {
            _ = RefreshAsync();
        }

        async Task RefreshAsync()
        {
            var realm = Realm.GetInstance();

            var currentNode = realm.All<SettingsRealmObject>().First().CurrentNode;

            await currentNode.UpdateThreadsAsync();

            ItemsSource.Clear();
            var threads = currentNode.Threads.OrderByDescending(o => o.ThreadModifiedDateTime).ToList();
            realm.Write(() =>
            {
                for (var i = 0; i < threads.Count(); i++)
                {
                    var thread = threads[i];
                    thread.IsFirst = false;
                    thread.IsLast = false;
                    if (i == 0)
                    {
                        thread.IsFirst = true;
                    }
                    if (i == threads.Count() - 1)
                    {
                        thread.IsLast = true;
                    }
                    ItemsSource.Add(thread);
                }
            });
        }
    }
}
