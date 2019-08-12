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

        public bool EnabledDarkTheme { get; set; } = Realm.GetInstance().All<SettingsRealmObject>().First().Theme == (int)Values.Theme.Dark;

        public SettingsPageViewModel()
        {
        }

        public void OnEnabledDarkThemeChanged()
        {
            ThemeHelper.ChangeTheme(EnabledDarkTheme ? (ResourceDictionary)new DarkTheme() : (ResourceDictionary)new LightTheme());
            var realm = Realm.GetInstance();
            realm.Write(() =>
            {
                var settings = realm.All<SettingsRealmObject>().First();
                settings.Theme = (int)(EnabledDarkTheme ? Values.Theme.Dark : Values.Theme.Light);
            });
        }
    }
}
