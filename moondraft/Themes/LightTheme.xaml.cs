
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace moondraft.Themes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [HotReloader.CSharpVisual]
    public partial class LightTheme : ResourceDictionary
    {
        public LightTheme()
        {
            InitializeComponent();
        }
    }
}