using moondraft.Themes;
using Xamarin.Forms;

namespace moondraft.Pages
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            ThemeHelper.ChangeTheme(new LightTheme());

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
