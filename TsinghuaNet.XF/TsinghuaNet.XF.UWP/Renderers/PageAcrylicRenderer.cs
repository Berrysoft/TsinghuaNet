using TsinghuaNet.XF.UWP.Renderers;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(Page), typeof(PageAcrylicRenderer))]
[assembly: ExportRenderer(typeof(TabbedPage), typeof(TabbedPageAcrylicRenderer))]

namespace TsinghuaNet.XF.UWP.Renderers
{
    public class PageAcrylicRenderer : PageRenderer
    {
        public PageAcrylicRenderer() : base()
        {
            ActualThemeChanged += (sender, e) => ChangeBackground();
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);
            ChangeBackground();
        }

        private void ChangeBackground()
        {
            Background = (Brush)Windows.UI.Xaml.Application.Current.Resources["SystemControlAcrylicWindowBrush"];
        }
    }

    public class TabbedPageAcrylicRenderer : TabbedPageRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                Control.ActualThemeChanged += OnActualThemeChanged;
                Control.Title = Element.Title;
                Control.TitleVisibility = Visibility.Visible;
            }
            else
            {
                Control.ActualThemeChanged -= OnActualThemeChanged;
            }
            ChangeBackground();
        }

        private void OnActualThemeChanged(FrameworkElement sender, object e) => ChangeBackground();

        private void ChangeBackground()
        {
            var foreground = Control.ActualTheme == ElementTheme.Dark ? Colors.WhiteSmoke : Colors.Black;
            Control.Background = (Brush)Windows.UI.Xaml.Application.Current.Resources["SystemControlAcrylicWindowBrush"];
            Control.ToolbarForeground = new SolidColorBrush(foreground);
        }
    }
}
