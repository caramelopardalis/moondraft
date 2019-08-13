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
            FixIssue6996();
        }

        void FixIssue6996()
        {
            var oldSource = EnabledDarkThemeSwitchCell.IconSource as FontImageSource;
            var newSource = new FontImageSource()
            {
                Glyph = oldSource.Glyph,
            };
            newSource.SetDynamicResource(FontImageSource.FontFamilyProperty, "MaterialFont");
            newSource.SetDynamicResource(FontImageSource.ColorProperty, "TextColor");
            EnabledDarkThemeSwitchCell.IconSource = newSource;
        }
    }
}
