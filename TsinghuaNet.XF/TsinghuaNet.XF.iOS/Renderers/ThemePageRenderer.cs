using System;
using System.Diagnostics;
using TsinghuaNet.XF.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.DataGrid;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ContentPage), typeof(ThemePageRenderer))]

namespace TsinghuaNet.XF.iOS.Renderers
{
    public class ThemePageRenderer : PageRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null && Element != null)
            {
                try
                {
                    SetAppTheme();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
        }

        public override void TraitCollectionDidChange(UITraitCollection previousTraitCollection)
        {
            base.TraitCollectionDidChange(previousTraitCollection);

            if (TraitCollection.UserInterfaceStyle != previousTraitCollection.UserInterfaceStyle)
            {
                SetAppTheme();
            }
        }

        private void SetAppTheme()
        {
            var foreground = TraitCollection.UserInterfaceStyle == UIUserInterfaceStyle.Dark ? Color.White : Color.Black;
            ((PaletteCollection)App.Current.Resources["DataGridForegroundPalette"])[0] = foreground;
        }
    }
}
