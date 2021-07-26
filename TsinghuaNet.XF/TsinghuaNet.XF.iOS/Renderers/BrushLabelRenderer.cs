using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using CoreGraphics;
using TsinghuaNet.XF.Controls;
using TsinghuaNet.XF.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BrushLabel), typeof(BrushLabelRenderer))]

namespace TsinghuaNet.XF.iOS.Renderers
{
    public class BrushLabelRenderer : LabelRenderer
    {
        public override void Draw(CGRect rect)
        {
            base.Draw(rect);
            if (Control != null)
            {
                SetTextColor();
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            SetTextColor();
        }

        private void SetTextColor()
        {
            var image = GetGradientImage(Control.Frame.Size);
            if (image != null)
            {
                Control.TextColor = UIColor.FromPatternImage(image);
            }
        }

        private UIImage GetGradientImage(CGSize size)
        {
            var oriBrush = (Element as BrushLabel)?.Foreground as LinearGradientBrush;
            if (oriBrush != null)
            {
                UIGraphics.BeginImageContextWithOptions(size, false, 0);
                var context = UIGraphics.GetCurrentContext();
                if (context != null)
                {
                    context.SetFillColor(UIColor.Blue.CGColor);
                    context.FillRect(new RectangleF(new PointF(0, 0), new SizeF((float)size.Width, (float)size.Height)));
                    var colorspace = CGColorSpace.CreateDeviceRGB();
                    var colors = new List<CGColor>();
                    var locations = new List<nfloat>();
                    foreach (var stop in oriBrush.GradientStops)
                    {
                        colors.Add(stop.Color.ToCGColor());
                        locations.Add(stop.Offset);
                    }
                    var gradient = new CGGradient(colorspace, colors.ToArray(), locations.ToArray());
                    var start = oriBrush.StartPoint.ToPointF();
                    start.X *= size.Width;
                    start.Y *= size.Height;
                    start.Y = size.Height - start.Y;
                    var end = oriBrush.EndPoint.ToPointF();
                    end.X *= size.Width;
                    end.Y *= size.Height;
                    end.Y = size.Height - end.Y;
                    context.DrawLinearGradient(gradient, start, end, CGGradientDrawingOptions.DrawsAfterEndLocation);
                    var img = UIGraphics.GetImageFromCurrentImageContext();
                    UIGraphics.EndImageContext();
                    return img;
                }
            }
            return null;
        }
    }
}