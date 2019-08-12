using Xamarin.Forms;

namespace moondraft.Themes
{
    class ThemeHelper
    {
        public static ResourceDictionary CurrentTheme;

        public static void ChangeTheme(ResourceDictionary newTheme)
        {
            if (newTheme == CurrentTheme)
            {
                return;
            }

            ResourceDictionary applicationResourceDictionary = Application.Current.Resources;

            foreach (var merged in newTheme.MergedDictionaries)
            {
                applicationResourceDictionary.MergedDictionaries.Add(merged);
            }

            ManuallyCopyThemes(newTheme, applicationResourceDictionary);

            CurrentTheme = newTheme;
            MessagingCenter.Send(new ThemeMessage(), ThemeMessage.ThemeChanged);
        }

        static void ManuallyCopyThemes(ResourceDictionary fromResource, ResourceDictionary toResource)
        {
            foreach (var item in fromResource.Keys)
            {
                toResource[item] = fromResource[item];
            }
        }
    }
}
