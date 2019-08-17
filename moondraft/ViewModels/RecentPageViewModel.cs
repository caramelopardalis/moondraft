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

                    await Refresh();

                    IsRefreshing = false;
                });
            }
        }

        public IList<ThreadRealmObject> ItemsSource { get; set; } = new List<ThreadRealmObject>();

        public RecentPageViewModel()
        {
            _ = Refresh();
        }

        async Task Refresh()
        {
            var realm = Realm.GetInstance();
            var currentNode = realm.All<SettingsRealmObject>().First().CurrentNode;

            await currentNode.UpdateThreads();

            ItemsSource = currentNode.Threads.OrderByDescending(o => o.ThreadModifiedDateTime).ToList();
            if (ItemsSource.Any())
            {
                realm.Write(() =>
                {
                    ItemsSource.First().IsFirst = true;
                    ItemsSource.Last().IsLast = true;
                });
            }
        }
    }
}
