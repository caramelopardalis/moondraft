using Realms;

namespace moondraft.RealmObjects
{
    public class SettingsRealmObject : RealmObject
    {
        public int Theme { get; set; } = (int)Values.Theme.Dark;

        public int Counter { get; set; } = 0;
    }
}
