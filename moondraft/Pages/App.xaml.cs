using moondraft.RealmObjects;
using moondraft.Themes;
using Realms;
using System.Linq;
using Xamarin.Forms;

namespace moondraft.Pages
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var realm = Realm.GetInstance();
            realm.Refresh();
            var settings = realm.All<SettingsRealmObject>().FirstOrDefault();
            if (settings == null)
            {
                realm.Write(() =>
                {
                    settings = new SettingsRealmObject();
                    realm.Add(settings);
                });
            }

            ThemeHelper.ChangeTheme(settings.Theme == (int)Theme.Dark ? (ResourceDictionary)new DarkTheme() : (ResourceDictionary)new LightTheme());

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
