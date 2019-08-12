using Android.Support.V7.Widget;
using Xamarin.Forms.Platform.Android;

namespace moondraft.Droid.Renderers
{
    public class AppShellFlyoutTemplatedContentRenderer : ShellFlyoutTemplatedContentRenderer
    {
        protected AppShellFlyoutRecyclerAdapter recyclerAdapter;

        public AppShellFlyoutTemplatedContentRenderer(IShellContext shellContext) : base(shellContext)
        {
        }

        protected override void LoadView(IShellContext shellContext)
        {
            base.LoadView(shellContext);

            var recycler = AndroidView.FindViewById<RecyclerView>(Resource.Id.flyoutcontent_recycler);
            recyclerAdapter = new AppShellFlyoutRecyclerAdapter(shellContext, OnElementSelected);
            recycler.SetAdapter(recyclerAdapter);
        }

        public void ApplyTheme()
        {
            recyclerAdapter.ApplyTheme();
        }
    }
}
