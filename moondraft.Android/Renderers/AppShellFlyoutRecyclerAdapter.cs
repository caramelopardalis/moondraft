using Android.Support.V7.Widget;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace moondraft.Droid.Renderers
{
    public class AppShellFlyoutRecyclerAdapter : ShellFlyoutRecyclerAdapter
    {
        DataTemplate _defaultItemTemplate;

        DataTemplate _defaultMenuItemTemplate;

        public AppShellFlyoutRecyclerAdapter(IShellContext shellContext, Action<Element> selectedCallback) : base(shellContext, selectedCallback)
        {
        }

        protected override DataTemplate DefaultItemTemplate =>
            _defaultItemTemplate ?? (_defaultItemTemplate = new DataTemplate(() => GenerateDefaultCell("Title", "FlyoutIcon")));

        protected override DataTemplate DefaultMenuItemTemplate =>
            _defaultMenuItemTemplate ?? (_defaultMenuItemTemplate = new DataTemplate(() => GenerateDefaultCell("Text", "Icon")));

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            base.OnBindViewHolder(holder, position);
        }

        public void ApplyTheme()
        {
            _defaultItemTemplate = new DataTemplate(() => GenerateDefaultCell("Title", "FlyoutIcon"));
            _defaultMenuItemTemplate = new DataTemplate(() => GenerateDefaultCell("Text", "Icon"));
            NotifyDataSetChanged();
        }

        View GenerateDefaultCell(string textBinding, string iconBinding)
        {
            var grid = new Grid();
            var groups = new VisualStateGroupList();

            var commonGroup = new VisualStateGroup();
            commonGroup.Name = "CommonStates";
            groups.Add(commonGroup);

            var normalState = new VisualState();
            normalState.Name = "Normal";
            commonGroup.States.Add(normalState);

            var selectedState = new VisualState();
            selectedState.Name = "Selected";
            selectedState.Setters.Add(new Setter
            {
                Property = VisualElement.BackgroundColorProperty,
                Value = Application.Current.Resources["FlyoutSelectedBackgroundColor"]
            });

            commonGroup.States.Add(selectedState);

            VisualStateManager.SetVisualStateGroups(grid, groups);

            grid.HeightRequest = 50;
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = 54 });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

            var image = new Image();
            image.VerticalOptions = image.HorizontalOptions = LayoutOptions.Center;
            image.HeightRequest = image.WidthRequest = 24;
            image.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(Image.Source) && image.Source != null)
                {
                    (image.Source as FontImageSource).Color = (Color)Application.Current.Resources["FlyoutTextColor"];
                }
            };
            image.SetBinding(Image.SourceProperty, iconBinding);
            grid.Children.Add(image);

            var label = new Label();
            label.Margin = new Thickness(20, 0, 0, 0);
            label.VerticalTextAlignment = TextAlignment.Center;
            label.SetBinding(Label.TextProperty, textBinding);
            grid.Children.Add(label, 1, 0);

            label.FontSize = 14;
            label.TextColor = (Color)Application.Current.Resources["FlyoutTextColor"];
            // label.SetDynamicResource(Label.FontFamilyProperty, "MaterialFont");

            label.Triggers.Add(new DataTrigger(typeof(Label))
            {
                Binding = new Binding
                {
                    Source = grid,
                    Path = "BackgroundColor",
                },
                Value = Application.Current.Resources["FlyoutSelectedBackgroundColor"],
                Setters =
                {
                    new Setter
                    {
                        Property = Label.TextColorProperty,
                        Value = Application.Current.Resources["FlyoutSelectedTextColor"],
                    }
                }
            });

            return grid;
        }
    }
}
