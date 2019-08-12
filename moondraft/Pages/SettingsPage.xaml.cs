using moondraft.Themes;
using moondraft.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace moondraft.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();

            MessagingCenter.Subscribe<ThemeMessage>(this, ThemeMessage.ThemeChanged, ThemeChanged);

            BindingContext = new SettingsPageViewModel();
        }

        void ThemeChanged(object sender)
        {
            BackgroundColor = (Color)Application.Current.Resources["BackgroundColor"];
            darkThemeSwitchLabel.TextColor = (Color)Application.Current.Resources["TextColor"];
        }
    }
}
