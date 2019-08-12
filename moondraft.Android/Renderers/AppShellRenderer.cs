﻿using Android.Content;
using moondraft.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Shell), typeof(AppShellRenderer))]
namespace moondraft.Droid.Renderers
{
    public class AppShellRenderer : ShellRenderer
    {
        public static AppShellRenderer Instance;

        public AppShellFlyoutTemplatedContentRenderer FlyoutTemplatedContentRenderer;

        public AppShellRenderer(Context context) : base(context)
        {
            Instance = this;
        }

        protected override IShellItemRenderer CreateShellItemRenderer(ShellItem shellItem)
        {
            return new AppShellItemRenderer(this);
        }

        protected override IShellFlyoutContentRenderer CreateShellFlyoutContentRenderer()
        {
            FlyoutTemplatedContentRenderer = new AppShellFlyoutTemplatedContentRenderer(this);
            return FlyoutTemplatedContentRenderer;
        }

        protected override IShellFlyoutRenderer CreateShellFlyoutRenderer()
        {
            return new AppShellFlyoutRenderer(this, AndroidContext);
        }
    }
}
