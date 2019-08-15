using moondraft.Models;
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

        public IList<RecentThreadItemSource> ItemsSource { get; set; } = new List<RecentThreadItemSource>();

        public RecentPageViewModel()
        {
            Initialize();
        }

        async void Initialize()
        {
            var realm = Realm.GetInstance();
            var currentNode = realm.All<SettingsRealmObject>().First().CurrentNode;

            await ThreadModel.UpdateThreads(currentNode);

            ItemsSource.Clear();
            var threads = currentNode.Threads;
            foreach (var thread in threads)
            {
                ItemsSource.Add(new RecentThreadItemSource
                {
                    ThreadTitle = thread.ThreadTitle,
                });
            }
        }

        [Preserve(AllMembers = true)]
        public class RecentThreadItemSource : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            public string ThreadTitle { get; set; }
        }
    }
}
