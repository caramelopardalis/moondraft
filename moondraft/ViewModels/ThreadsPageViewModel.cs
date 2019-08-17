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
    class ThreadsPageViewModel : INotifyPropertyChanged
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

        public ThreadsPageViewModel()
        {
            _ = Refresh();
        }

        async Task Refresh()
        {
            var realm = Realm.GetInstance();
            var currentNode = realm.All<SettingsRealmObject>().First().CurrentNode;

            await currentNode.UpdateThreads();

            ItemsSource = currentNode.Threads.OrderByDescending(o => o.ThreadModifiedDateTime).ToList();
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
