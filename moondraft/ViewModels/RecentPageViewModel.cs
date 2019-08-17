using moondraft.RealmObjects;
using Realms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace moondraft.ViewModels
{
    [Preserve(AllMembers = true)]
    class RecentPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public IList<ThreadRealmObject> ItemsSource { get; set; } = new List<ThreadRealmObject>();

        public RecentPageViewModel()
        {
            Initialize();
        }

        async void Initialize()
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
