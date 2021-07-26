// https://xamarin.azureedge.net/developer/xamarin-forms-book/XamarinFormsBook-Ch27-Apr2016.pdf

using System;
using System.ComponentModel;
using TsinghuaNet.XF.Controls;
using TsinghuaNet.XF.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(StepSlider), typeof(StepSliderRenderer))]

namespace TsinghuaNet.XF.iOS.Renderers
{
    public class StepSliderRenderer : ViewRenderer<StepSlider, UISlider>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<StepSlider> args)
        {
            base.OnElementChanged(args);
            if (Control == null)
            {
                SetNativeControl(new UISlider());
                Control.SizeToFit();
                Element.WidthRequest = 100;
            }
            if (args.NewElement != null)
            {
                SetMinimum();
                SetMaximum();
                SetValue();
                Control.ValueChanged += OnUISliderValueChanged;
            }
            else
            {
                Control.ValueChanged -= OnUISliderValueChanged;
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(sender, args);
            if (args.PropertyName == StepSlider.MinimumProperty.PropertyName)
            {
                SetMinimum();
            }
            else if (args.PropertyName == StepSlider.MaximumProperty.PropertyName)
            {
                SetMaximum();
            }
            else if (args.PropertyName == StepSlider.ValueProperty.PropertyName)
            {
                SetValue();
            }
        }

        void SetMinimum() => Control.MinValue = (float)Element.Minimum;
        void SetMaximum() => Control.MaxValue = (float)Element.Maximum;
        void SetValue() => Control.Value = (float)Element.Value;

        void OnUISliderValueChanged(object sender, EventArgs args)
        {
            double increment = (Element.Maximum - Element.Minimum) / Element.Steps;
            double value = increment * Math.Round(Control.Value / increment);
            ((IElementController)Element).SetValueFromRenderer(StepSlider.ValueProperty, value);
        }
    }
}