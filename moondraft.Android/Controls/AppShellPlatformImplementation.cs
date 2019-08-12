using moondraft.Controls;
using moondraft.Droid.Renderers;
using moondraft.Droid.Views;
using Xamarin.Forms;

[assembly: Dependency(typeof(AppShellPlatformImplementation))]
namespace moondraft.Droid.Views
{
    public class AppShellPlatformImplementation : IAppShellPlatformImplementation
    {
        public void ApplyTheme()
        {
            AppShellRenderer.Instance.FlyoutTemplatedContentRenderer.ApplyTheme();
        }
    }
}
