using System;
using System.Diagnostics;
using TsinghuaNet.XF.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.DataGrid;
using Xamarin.Forms.Platform.iOS;
using xf = Xamarin.Forms;

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
            var app = xf.Application.Current;
            var foreground = TraitCollection.UserInterfaceStyle == UIUserInterfaceStyle.Dark ? Color.White : Color.Black;
            var background = TraitCollection.UserInterfaceStyle == UIUserInterfaceStyle.Dark ? Color.Black : Color.White;
            ((PaletteCollection)app.Resources["DataGridForegroundPalette"])[0] = foreground;
            app.Resources["PopupBackground"] = background;
        }
    }
}
