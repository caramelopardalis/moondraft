using PropertyChanged;
using Realms;

namespace moondraft.RealmObjects
{

    [DoNotNotify]
    public class SettingsRealmObject : RealmObject
    {
        public bool EnabledDarkTheme { get; set; }

        public NodeRealmObject CurrentNode { get; set; }
    }
}
