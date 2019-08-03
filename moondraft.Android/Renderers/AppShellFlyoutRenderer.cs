using Android.Content;
using Xamarin.Forms.Platform.Android;

namespace moondraft.Droid.Renderers
{
    public class AppShellFlyoutRenderer : ShellFlyoutRenderer
    {
        public AppShellFlyoutRenderer(IShellContext shellContext, Context context) : base(shellContext, context)
        {
        }
    }
}
