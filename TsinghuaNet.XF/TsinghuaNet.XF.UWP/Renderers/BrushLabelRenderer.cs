using System;
using System.ComponentModel;
using System.Numerics;
using TsinghuaNet.XF.Controls;
using TsinghuaNet.XF.UWP.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(BrushLabel), typeof(BrushLabelRenderer))]

namespace TsinghuaNet.XF.UWP.Renderers
{
    public class BrushLabelRenderer : LabelRenderer
    {
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
                var collections = new Windows.UI.Xaml.Media.GradientStopCollection();
                foreach (var stop in oriBrush.GradientStops)
                {
                    collections.Add(new Windows.UI.Xaml.Media.GradientStop() { Color = stop.Color.ToWindowsColor(), Offset = stop.Offset });
                }
                var vec = oriBrush.EndPoint.ToWindows().ToVector2() - oriBrush.StartPoint.ToWindows().ToVector2();
                var brush = new Windows.UI.Xaml.Media.LinearGradientBrush(collections, Math.Atan2(vec.Y, vec.X));
                Control.Foreground = brush;
            }
        }
    }
}
