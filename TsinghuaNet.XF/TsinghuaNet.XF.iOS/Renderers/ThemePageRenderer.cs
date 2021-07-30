using System;
using System.Diagnostics;
using Syncfusion.XForms.Themes;
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
            var mergedDictionaries = app.Resources.MergedDictionaries;
            mergedDictionaries.Clear();
            mergedDictionaries.Add(TraitCollection.GetColorTheme());
        }
    }

    static class TraitCollectionEx
    {
        public static ResourceDictionary GetColorTheme(this UITraitCollection tc)
        {
            if (tc.UserInterfaceStyle == UIUserInterfaceStyle.Dark)
            {
                return new DarkTheme();
            }
            else
            {
                return new LightTheme();
            }
        }
    }
}
