using System.Windows.Input;
using Xamarin.Forms;

namespace moondraft.Controls
{
    public class AppCollectionView : CollectionView
    {
        public static readonly BindableProperty LoadMoreNewerCommandProperty = BindableProperty.Create(nameof(LoadMoreNewerCommand), typeof(ICommand), typeof(AppCollectionView), null);

        public ICommand LoadMoreNewerCommand
        {
            get => (ICommand)GetValue(LoadMoreNewerCommandProperty);
            set => SetValue(LoadMoreNewerCommandProperty, value);
        }

        public AppCollectionView() : base()
        {
            Scrolled += ScrolledHandler;
        }

        void ScrolledHandler(object sender, ItemsViewScrolledEventArgs e)
        {
            if (LoadMoreNewerCommand == null)
            {
                return;
            }

            var count = 0;
            foreach (var itemSource in ItemsSource)
            {
                ++count;
            }
            if (e.LastVisibleItemIndex == count - 1)
            {
                LoadMoreNewerCommand.Execute(null);
            }
        }
    }
}
