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
    }
}