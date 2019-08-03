using Android.Content;
using Xamarin.Forms.Platform.Android;

namespace moondraft.Droid.Renderers
{
    public class AppShellFlyoutContentRenderer : ShellFlyoutContentRenderer
    {
        public AppShellFlyoutContentRenderer(IShellContext shellContext, Context context) : base(shellContext, context)
        {
        }
    }
}
