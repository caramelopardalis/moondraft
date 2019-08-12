using moondraft.Themes;
using Xamarin.Forms;

namespace moondraft.Controls
{
    public class AppShell : Shell
    {
        public AppShell()
        {
            MessagingCenter.Subscribe<ThemeMessage>(this, ThemeMessage.ThemeChanged, ThemeChanged);
        }

        void ThemeChanged(object sender)
        {
            var platformImplementation = DependencyService.Get<IAppShellPlatformImplementation>();
            platformImplementation?.ApplyTheme();
        }
    }
}
