using moondraft.RealmObjects;
using moondraft.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace moondraft.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecentPage : ContentPage
    {
        public RecentPage()
        {
            InitializeComponent();

            BindingContext = new RecentPageViewModel();
        }

        void CellBindingContextChanged(object sender, System.EventArgs e)
        {
            var itemSource = (sender as Grid).BindingContext as ThreadRealmObject;
            if (itemSource == null)
            {
                return;
            }
            var state = itemSource.IsFirst && itemSource.IsLast ? "FirstAndLast" : itemSource.IsFirst ? "First" : itemSource.IsLast ? "Last" : "Middle";
            VisualStateManager.GoToState(sender as Grid, state);
        }
    }
}