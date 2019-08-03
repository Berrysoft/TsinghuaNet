// https://xamarin.azureedge.net/developer/xamarin-forms-book/XamarinFormsBook-Ch27-Apr2016.pdf

using System.ComponentModel;
using TsinghuaNet.XF.Controls;
using TsinghuaNet.XF.UWP.Renderers;
using Windows.UI.Xaml.Controls.Primitives;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using Win = Windows.UI.Xaml.Controls;
[assembly: ExportRenderer(typeof(StepSlider), typeof(StepSliderRenderer))]

namespace TsinghuaNet.XF.UWP.Renderers
{
    public class StepSliderRenderer : ViewRenderer<StepSlider, Win.Slider>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<StepSlider> args)
        {
            base.OnElementChanged(args);
            if (Control == null)
            {
                SetNativeControl(new Win.Slider());
            }
            if (args.NewElement != null)
            {
                SetMinimum();
                SetMaximum();
                SetSteps();
                SetValue();
                Control.ValueChanged += OnControlValueChanged;
            }
            else
            {
                Control.ValueChanged -= OnControlValueChanged;
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
            else if (args.PropertyName == StepSlider.StepsProperty.PropertyName)
            {
                SetSteps();
            }
            else if (args.PropertyName == StepSlider.ValueProperty.PropertyName)
            {
                SetValue();
            }
        }

        void SetMinimum() => Control.Minimum = Element.Minimum;
        void SetMaximum() => Control.Maximum = Element.Maximum;
        void SetSteps() => Control.StepFrequency = (Element.Maximum - Element.Minimum) / Element.Steps;
        void SetValue() => Control.Value = Element.Value;

        void OnControlValueChanged(object sender, RangeBaseValueChangedEventArgs args)
        {
            ((IElementController)Element).SetValueFromRenderer(StepSlider.ValueProperty, args.NewValue);
        }
    }
}
