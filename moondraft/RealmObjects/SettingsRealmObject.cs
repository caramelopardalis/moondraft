using Realms;

namespace moondraft.RealmObjects
{
    public class SettingsRealmObject : RealmObject
    {
        public int Theme { get; set; } = (int)Themes.Theme.Dark;

        public NodeRealmObject CurrentNode { get; set; }
    }
}
