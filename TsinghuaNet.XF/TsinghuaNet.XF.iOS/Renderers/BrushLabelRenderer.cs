using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using CoreAnimation;
using CoreGraphics;
using Foundation;
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
        private CAGradientLayer gradient = new CAGradientLayer();
        private UILabel realLabel = new UILabel();

        public BrushLabelRenderer() : base()
        {
            realLabel.BackgroundColor = UIColor.Clear;
            realLabel.Layer.AddSublayer(gradient);
        }

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);
            if (Control != null)
            {
                SetTextColor();
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            foreach (var view in realLabel.Subviews)
            {
                view.RemoveFromSuperview();
            }
            realLabel.Frame = Control.Frame;
            Control.Superview.AddSubview(realLabel);
            Control.RemoveFromSuperview();
            gradient.Mask = Control.Layer;
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            SetTextColor();
        }

        private void SetTextColor()
        {
            gradient.Frame = Control.Bounds;
            var oriBrush = (Element as BrushLabel)?.Foreground as LinearGradientBrush;
            if (oriBrush != null)
            {
                gradient.Colors = oriBrush.GradientStops.Select(stop => stop.Color.ToCGColor()).ToArray();
                gradient.Locations = oriBrush.GradientStops.Select(stop => new NSNumber(stop.Offset)).ToArray();
                gradient.StartPoint = oriBrush.StartPoint.ToPointF();
                gradient.EndPoint = oriBrush.EndPoint.ToPointF();
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
                    context.SetFillColor(UIColor.White.CGColor);
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
                    var end = oriBrush.EndPoint.ToPointF();
                    end.X *= size.Width;
                    end.Y *= size.Height;
                    context.DrawLinearGradient(gradient, start, end, CGGradientDrawingOptions.None);
                    var img = UIGraphics.GetImageFromCurrentImageContext();
                    UIGraphics.EndImageContext();
                    return img;
                }
            }
            return null;
        }
    }
}