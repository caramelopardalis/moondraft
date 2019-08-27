using AsyncAwaitBestPractices.MVVM;
using System.Collections;
using System.Threading;
using Xamarin.Forms;

namespace moondraft.Controls
{
    public class AppCollectionView : CollectionView
    {
        public static readonly BindableProperty LoadMoreNewerAsyncCommandProperty = BindableProperty.Create(nameof(LoadMoreNewerAsyncCommand), typeof(IAsyncCommand), typeof(AppCollectionView), null);

        public static readonly BindableProperty LoadMoreNewerThresholdProperty = BindableProperty.Create(nameof(LoadMoreNewerThreshold), typeof(int), typeof(AppCollectionView), 5);

        public static readonly BindableProperty MayAppearCellAsyncCommandProperty = BindableProperty.Create(nameof(MayAppearCellAsyncCommand), typeof(IAsyncCommand), typeof(AppCollectionView), null);

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

        public IAsyncCommand MayAppearCellAsyncCommand
        {
            get => (IAsyncCommand)GetValue(MayAppearCellAsyncCommandProperty);
            set => SetValue(MayAppearCellAsyncCommandProperty, value);
        }

        public int MayAppearCellThreshold
        {
            get => (int)GetValue(MayAppearCellThresholdProperty);
            set => SetValue(MayAppearCellThresholdProperty, value);
        }

        int LoadMoreLock;

        int MayAppearCellLock;

        public AppCollectionView() : base()
        {
            Scrolled += ScrolledHandler;
        }

        async void ScrolledHandler(object sender, ItemsViewScrolledEventArgs e)
        {
            var count = (ItemsSource as ICollection).Count;

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

            if (Interlocked.Exchange(ref MayAppearCellLock, 1) != 0)
            {
                return;
            }
            try
            {
                await LoadMoreNewerAsyncCommand?.ExecuteAsync();
            }
            finally
            {
                Interlocked.Exchange(ref MayAppearCellLock, 0);
            }
        }
    }
}
