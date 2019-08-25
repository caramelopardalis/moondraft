using AsyncAwaitBestPractices.MVVM;
using System.Threading;
using Xamarin.Forms;

namespace moondraft.Controls
{
    public class AppCollectionView : CollectionView
    {
        public static readonly BindableProperty LoadMoreNewerAsyncCommandProperty = BindableProperty.Create(nameof(LoadMoreNewerAsyncCommand), typeof(IAsyncCommand), typeof(AppCollectionView), null);

        public static readonly BindableProperty LoadMoreNewerThresholdProperty = BindableProperty.Create(nameof(LoadMoreNewerThreshold), typeof(int), typeof(AppCollectionView), 5);

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

        int LoadMoreLock;

        public AppCollectionView() : base()
        {
            Scrolled += ScrolledHandler;
        }

        async void ScrolledHandler(object sender, ItemsViewScrolledEventArgs e)
        {
            if (LoadMoreNewerAsyncCommand == null)
            {
                return;
            }

            var count = 0;
            foreach (var itemSource in ItemsSource)
            {
                ++count;
            }
            if (e.LastVisibleItemIndex <= count - 1 - LoadMoreNewerThreshold)
            {
                if (Interlocked.Exchange(ref LoadMoreLock, 1) != 0)
                {
                    return;
                }
                try
                {
                    await LoadMoreNewerAsyncCommand.ExecuteAsync();
                }
                finally
                {
                    Interlocked.Exchange(ref LoadMoreLock, 0);
                }
            }
        }
    }
}
