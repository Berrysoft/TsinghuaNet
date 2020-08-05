using System.Collections.Generic;
using System.ComponentModel;
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
                var colors = new List<int>();
                var positions = new List<float>();
                foreach (var stop in oriBrush.GradientStops)
                {
                    colors.Add(stop.Color.ToAndroid());
                    positions.Add(stop.Offset);
                }
                var width = Control.MeasuredWidth;
                var height = Control.MeasuredHeight;
                Shader shader = new LinearGradient(
                    (float)oriBrush.StartPoint.X * width, (float)oriBrush.StartPoint.Y * height,
                    (float)oriBrush.EndPoint.X * width, (float)oriBrush.EndPoint.Y * height,
                    colors.ToArray(), positions.ToArray(), Shader.TileMode.Clamp);
                Control.Paint.SetShader(shader);
                Control.Invalidate();
            }
        }
    }
}