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
                Control.Background = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Transparent);
        }
    }
}
