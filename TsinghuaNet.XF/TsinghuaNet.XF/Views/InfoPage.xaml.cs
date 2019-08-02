using System;
using Rg.Plugins.Popup.Services;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TsinghuaNet.XF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InfoPage : ContentPage
    {
        public InfoPage()
        {
            InitializeComponent();
        }

        private void InfoPage_SizeChanged(object sender, EventArgs e)
        {
            VisualStateManager.GoToState(InfoLayout, Width > Height ? "HorizontalState" : "VerticalState");
        }

        bool firstAppeared;
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (!firstAppeared)
            {
                if (string.IsNullOrEmpty(Model.Credential.Username))
                {
                    await PopupNavigation.Instance.PushAsync(new ChangeUserPage());
                }
                firstAppeared = true;
            }
        }

        internal void SaveSettings() => Model.SaveSettings();

        private async void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            var info = args.Info;
            var canvas = args.Surface.Canvas;
            const string TEXT = "\xE12B";
            using (var paint = new SKPaint())
            {
                paint.TextSize = Math.Min(info.Width, info.Height);
                switch (Device.RuntimePlatform)
                {
                    case Device.UWP:
                        paint.Typeface = SKTypeface.FromFamilyName("Segoe MDL2 Assets", SKFontStyleWeight.Normal, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright);
                        break;
                    default:
                        using (var stream = await FileSystem.OpenAppPackageFileAsync("segmdl2.ttf"))
                        {
                            paint.Typeface = SKTypeface.FromStream(stream);
                        }
                        break;
                }
                SKRect textBounds = new SKRect();
                paint.MeasureText(TEXT, ref textBounds);
                float xText = info.Width / 2 - textBounds.MidX;
                float yText = info.Height / 2 - textBounds.MidY;
                textBounds.Offset(xText, yText);
                paint.Shader = SKShader.CreateLinearGradient(
                                new SKPoint((textBounds.Left + textBounds.Right) / 2, textBounds.Bottom),
                                new SKPoint((textBounds.Left + textBounds.Right) / 2, textBounds.Top),
                                new SKColor[] { App.SystemAccentColor.ToSKColor(), App.SystemAccentColorDark1.ToSKColor(), App.SystemAccentColorDark1.ToSKColor(), App.SystemAccentColorDark2.ToSKColor() },
                                new float[] { Model.FluxOffset, Model.FluxOffset, Model.FreeOffset, Model.FreeOffset },
                                SKShaderTileMode.Clamp);
                canvas.Clear();
                canvas.DrawText(TEXT, xText, yText, paint);
            }
        }

        private void Model_Refreshed(object sender, EventArgs e) => FluxCanvas.InvalidateSurface();
    }
}