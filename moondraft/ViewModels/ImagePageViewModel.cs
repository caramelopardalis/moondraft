using moondraft.RealmObjects;
using Realms;
using System.ComponentModel;
using System.Linq;

namespace moondraft.ViewModels
{
    [Preserve(AllMembers = true)]
    class ImagePageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public CommentRealmObject Comment { get; set; }

        public ImagePageViewModel()
        {
            Comment = Realm.GetInstance().All<SettingsRealmObject>().First().CurrentNode.CurrentThread.CurrentComment;
        }
    }
}
