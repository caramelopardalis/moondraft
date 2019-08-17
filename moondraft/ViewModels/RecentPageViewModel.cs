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

            var itemsSource = new List<RecentThreadItemSource>();
            foreach (var thread in currentNode.Threads)
            {
                itemsSource.Add(new RecentThreadItemSource
                {
                    ThreadTitle = thread.ThreadTitle,
                    ThreadModifiedDateTime = thread.ThreadModifiedDateTime.ToString("yyyy-MM-dd hh:mm:ss"),
                });
            }
            ItemsSource = itemsSource.OrderByDescending(o => o.ThreadModifiedDateTime).ToList();
            if (ItemsSource.Any())
            {
                ItemsSource.First().IsFirst = true;
                ItemsSource.Last().IsLast = true;
            }
        }
    }

    [Preserve(AllMembers = true)]
    public class RecentThreadItemSource : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string ThreadTitle { get; set; }

        public string ThreadModifiedDateTime { get; set; }

        public bool IsFirst { get; set; }

        public bool IsLast { get; set; }
    }
}
