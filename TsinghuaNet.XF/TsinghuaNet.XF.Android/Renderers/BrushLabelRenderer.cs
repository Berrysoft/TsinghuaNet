using System.ComponentModel;
using System.Linq;
using Android.Content;
using Android.Graphics;
using TsinghuaNet.XF.Controls;
using TsinghuaNet.XF.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(BrushLabel), typeof(BrushLabelRenderer))]

namespace TsinghuaNet.XF.Droid.Renderers
{
    public class BrushLabelRenderer : LabelRenderer
    {
        public BrushLabelRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            SetColors();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            SetColors();
        }

        private void SetColors()
        {
            var oriBrush = (Element as BrushLabel)?.Foreground as LinearGradientBrush;
            if (oriBrush != null)
            {
                var colors = oriBrush.GradientStops.Select(stop => (int)stop.Color.ToAndroid()).ToArray();
                var positions = oriBrush.GradientStops.Select(stop => stop.Offset).ToArray();
                var width = Control.MeasuredWidth;
                var height = Control.MeasuredHeight;
                Shader shader = new LinearGradient(
                    (float)oriBrush.StartPoint.X * width, (float)oriBrush.StartPoint.Y * height,
                    (float)oriBrush.EndPoint.X * width, (float)oriBrush.EndPoint.Y * height,
                    colors, positions, Shader.TileMode.Clamp);
                Control.Paint.SetShader(shader);
                Control.Invalidate();
            }
        }
    }
}