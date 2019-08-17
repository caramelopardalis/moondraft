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

            InitializeRealm();

            var settings = Realm.GetInstance().All<SettingsRealmObject>().First();
            ThemeHelper.ChangeTheme(settings.EnabledDarkTheme ? (ResourceDictionary)new DarkTheme() : (ResourceDictionary)new LightTheme());

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

        void InitializeRealm()
        {
            var realm = Realm.GetInstance();

            if (!realm.All<NodeRealmObject>().Any())
            {
                realm.Write(() =>
                {
                    var initialNode = new NodeRealmObject
                    {
                        Url = "http://bbs.shingetsu.info/",
                    };
                    realm.Add(initialNode);
                });
            }

            var settings = realm.All<SettingsRealmObject>().FirstOrDefault();
            if (settings == null)
            {
                realm.Write(() =>
                {
                    settings = new SettingsRealmObject();
                    realm.Add(settings);
                });
            }

            if (settings.CurrentNode == null)
            {
                realm.Write(() =>
                {
                    settings.CurrentNode = realm.All<NodeRealmObject>().First();
                });
            }
        }
    }
}
