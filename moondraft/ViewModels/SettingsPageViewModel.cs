using moondraft.RealmObjects;
using moondraft.Themes;
using Realms;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;

namespace moondraft.ViewModels
{
    class SettingsPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public bool EnabledDarkTheme { get; set; } = Realm.GetInstance().All<SettingsRealmObject>().FirstOrDefault()?.EnabledDarkTheme ?? false;

        SettingsRealmObject Settings { get; set; }

        public SettingsPageViewModel()
        {
            LoadSettings();
        }

        public void OnEnabledDarkThemeChanged()
        {
            ThemeHelper.ChangeTheme(EnabledDarkTheme ? (ResourceDictionary)new DarkTheme() : (ResourceDictionary)new LightTheme());
            var realm = Realm.GetInstance();
            var settings = realm.All<SettingsRealmObject>().FirstOrDefault();
            realm.Write(() =>
            {
                settings.EnabledDarkTheme = EnabledDarkTheme;
            });
        }

        void LoadSettings()
        {
            var realm = Realm.GetInstance();
            var settings = realm.All<SettingsRealmObject>().FirstOrDefault();
            if (settings == null)
            {
                realm.Write(() =>
                {
                    settings = new SettingsRealmObject();
                    realm.Add(settings);
                });
            }
            Settings = settings;

            EnabledDarkTheme = Settings.EnabledDarkTheme;
        }
    }
}
