using System;
using Xamarin.Forms;

namespace TsinghuaNet.XF.Controls
{
    [ContentProperty(nameof(Text))]
    public class HyperlinkLabel : Label
    {
        public static readonly BindableProperty NavigateUriProperty = BindableProperty.Create(nameof(NavigateUri), typeof(string), typeof(HyperlinkLabel));
        public string NavigateUri
        {
            get => (string)GetValue(NavigateUriProperty);
            set => SetValue(NavigateUriProperty, value);
        }

        public HyperlinkLabel() : base()
        {
            TextColor = App.SystemAccentColor;
            GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => Device.OpenUri(new Uri(NavigateUri))) });
        }
    }
}
