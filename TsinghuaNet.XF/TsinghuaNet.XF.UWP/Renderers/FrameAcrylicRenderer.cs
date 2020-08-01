using TsinghuaNet.XF.UWP.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(Frame), typeof(FrameAcrylicRenderer))]

namespace TsinghuaNet.XF.UWP.Renderers
{
    public class FrameAcrylicRenderer : FrameRenderer
    {
        protected override void UpdateBackgroundColor()
        {
            base.UpdateBackgroundColor();
            if (Control != null)
                Control.Background = (Windows.UI.Xaml.Media.Brush)Windows.UI.Xaml.Application.Current.Resources["SystemControlAcrylicElementBrush"];
        }
    }
}
