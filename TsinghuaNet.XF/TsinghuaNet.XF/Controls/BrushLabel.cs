using Xamarin.Forms;

namespace TsinghuaNet.XF.Controls
{
    [ContentProperty(nameof(Text))]
    public class BrushLabel : Label
    {
        public static readonly BindableProperty ForegroundProperty = BindableProperty.Create(nameof(Foreground), typeof(Brush), typeof(BrushLabel));
        public Brush Foreground
        {
            get => (Brush)GetValue(ForegroundProperty);
            set => SetValue(ForegroundProperty, value);
        }
    }
}
