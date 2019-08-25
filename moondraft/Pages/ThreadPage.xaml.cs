using moondraft.RealmObjects;
using moondraft.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace moondraft.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ThreadPage : ContentPage
    {
        public ThreadPage()
        {
            InitializeComponent();

            BindingContext = new ThreadPageViewModel();
        }

        void CellBindingContextChanged(object sender, System.EventArgs e)
        {
            var layout = (sender as Layout);
            var itemSource = layout.BindingContext as CommentRealmObject;
            if (itemSource == null)
            {
                return;
            }
            var state = itemSource.IsFirst && itemSource.IsLast ? "FirstAndLast" : itemSource.IsFirst ? "First" : itemSource.IsLast ? "Last" : "Middle";
            VisualStateManager.GoToState(layout, state);

            // Force relayout recycled cells by Layout#InvalidateLayout()
            var originalPadding = layout.Padding;
            layout.Padding = new Thickness();
            layout.Padding = originalPadding;
        }
    }
}