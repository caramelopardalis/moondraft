
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace moondraft.Themes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [HotReloader.CSharpVisual]
    public partial class DarkTheme : ResourceDictionary
    {
        public DarkTheme()
        {
            InitializeComponent();
        }
    }
}