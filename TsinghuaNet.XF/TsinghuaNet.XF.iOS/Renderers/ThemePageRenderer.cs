using Syncfusion.XForms.Themes;
using System;
using System.Diagnostics;
using TsinghuaNet.XF.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
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
            var background = TraitCollection.UserInterfaceStyle == UIUserInterfaceStyle.Dark ? Color.Black : Color.White;
            app.Resources["PopupBackground"] = background;

            var mergedDictionaries = app.Resources.MergedDictionaries;
            if (TraitCollection.UserInterfaceStyle == UIUserInterfaceStyle.Dark)
            {
                mergedDictionaries.Clear();
                mergedDictionaries.Add(new DarkTheme());
            }
            else
            {
                mergedDictionaries.Clear();
                mergedDictionaries.Add(new LightTheme());
            }
        }
    }
}
