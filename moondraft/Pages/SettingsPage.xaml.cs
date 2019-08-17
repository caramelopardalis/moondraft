using moondraft.RealmObjects;
using moondraft.Themes;
using Realms;
using System.Linq;
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

            var settings = Realm.GetInstance().All<SettingsRealmObject>().First();
            settings.PropertyChanged += (sender, e) =>
            {
                ThemeHelper.ChangeTheme(settings.EnabledDarkTheme ? (ResourceDictionary)new DarkTheme() : (ResourceDictionary)new LightTheme());
            };
            BindingContext = settings;
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
