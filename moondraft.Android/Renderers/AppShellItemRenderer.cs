using Android.OS;
using Android.Views;
using Xamarin.Forms.Platform.Android;

namespace moondraft.Droid.Renderers
{
    public class AppShellItemRenderer : ShellItemRenderer
    {
        public AppShellItemRenderer(IShellContext context) : base(context)
        {
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return base.OnCreateView(inflater, container, savedInstanceState);
        }
    }
}
