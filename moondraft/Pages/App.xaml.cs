using moondraft.Themes;
using Xamarin.Forms;

namespace moondraft.Pages
{
    [HotReloader.CSharpVisual]
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

#if DEBUG
            HotReloader.Current.Run(this);
#endif

            ThemeHelper.ChangeTheme(new DarkTheme());

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
