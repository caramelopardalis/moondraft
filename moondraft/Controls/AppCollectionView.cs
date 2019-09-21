using AsyncAwaitBestPractices.MVVM;
using System;
using System.Collections;
using System.Threading;
using System.Windows.Input;
using Xamarin.Forms;

namespace moondraft.Controls
{
    public class AppCollectionView : CollectionView
    {
        public static readonly BindableProperty LoadMoreNewerAsyncCommandProperty = BindableProperty.Create(nameof(LoadMoreNewerAsyncCommand), typeof(IAsyncCommand), typeof(AppCollectionView), null);

        public static readonly BindableProperty LoadMoreNewerThresholdProperty = BindableProperty.Create(nameof(LoadMoreNewerThreshold), typeof(int), typeof(AppCollectionView), 5);

        public static readonly BindableProperty MayAppearCellCommandProperty = BindableProperty.Create(nameof(MayAppearCellCommand), typeof(ICommand), typeof(AppCollectionView), null);

        public static readonly BindableProperty MayAppearCellThresholdProperty = BindableProperty.Create(nameof(MayAppearCellThreshold), typeof(int), typeof(AppCollectionView), 5);

        public IAsyncCommand LoadMoreNewerAsyncCommand
        {
            get => (IAsyncCommand)GetValue(LoadMoreNewerAsyncCommandProperty);
            set => SetValue(LoadMoreNewerAsyncCommandProperty, value);
        }

        public int LoadMoreNewerThreshold
        {
            get => (int)GetValue(LoadMoreNewerThresholdProperty);
            set => SetValue(LoadMoreNewerThresholdProperty, value);
        }

        public ICommand MayAppearCellCommand
        {
            get => (ICommand)GetValue(MayAppearCellCommandProperty);
            set => SetValue(MayAppearCellCommandProperty, value);
        }

        public int MayAppearCellThreshold
        {
            get => (int)GetValue(MayAppearCellThresholdProperty);
            set => SetValue(MayAppearCellThresholdProperty, value);
        }

        int LoadMoreLock;

        public AppCollectionView() : base()
        {
            Scrolled += ScrolledHandler;
        }

        async void ScrolledHandler(object sender, ItemsViewScrolledEventArgs e)
        {
            var count = (ItemsSource as ICollection).Count;

            System.Diagnostics.Debug.WriteLine("Scrolled. LastVisibleItemIndex: " + e.LastVisibleItemIndex);

            if (e.LastVisibleItemIndex <= count - 1 - LoadMoreNewerThreshold)
            {
                if (Interlocked.Exchange(ref LoadMoreLock, 1) != 0)
                {
                    return;
                }
                try
                {
                    await LoadMoreNewerAsyncCommand?.ExecuteAsync();
                }
                finally
                {
                    Interlocked.Exchange(ref LoadMoreLock, 0);
                }
            }

            for (var i = Math.Max(e.LastVisibleItemIndex - MayAppearCellThreshold, 0); i < Math.Min(e.LastVisibleItemIndex + MayAppearCellThreshold, count); i++)
            {
                MayAppearCellCommand?.Execute(i);
            }
        }
    }
}
