using System;
using PropertyChanged;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TsinghuaNet.XF.Controls
{
    [DoNotNotify]
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
            GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(async () => await Launcher.OpenAsync(new Uri(NavigateUri))) });
        }
    }
}
