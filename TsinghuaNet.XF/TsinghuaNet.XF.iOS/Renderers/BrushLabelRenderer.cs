using System.ComponentModel;
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
        private readonly CAGradientLayer gradient = new CAGradientLayer();
        private readonly UILabel realLabel = new UILabel();

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
    }
}