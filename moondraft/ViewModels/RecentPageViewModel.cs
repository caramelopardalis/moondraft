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

            for (var i = 0; i < threads.Count(); i++)
            {
                ItemsSource.Add(new RecentThreadItemSource
                {
                    IsFirst = i == 0,
                    IsLast = i == threads.Count() - 1,
                    ThreadTitle = threads[i].ThreadTitle,
                });
            }
        }
    }

    [Preserve(AllMembers = true)]
    public class RecentThreadItemSource : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string ThreadTitle { get; set; }

        public bool IsFirst { get; set; }

        public bool IsLast { get; set; }
    }
}
