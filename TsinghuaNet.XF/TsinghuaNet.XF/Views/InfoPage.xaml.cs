using System;
using System.Threading.Tasks;
using SkiaSharp;
using SkiaSharp.Views.Forms;
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

        internal Task SaveSettingsAsync() => Model.SaveSettingsAsync();

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            var info = args.Info;
            var canvas = args.Surface.Canvas;
            const string TEXT = "\xE12B";
            using (var paint = new SKPaint())
            {
                paint.TextSize = Math.Min(info.Width, info.Height);
                paint.Typeface = SKTypeface.FromFamilyName("Segoe MDL2 Assets", SKFontStyleWeight.Normal, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright);
                SKRect textBounds = new SKRect();
                paint.MeasureText(TEXT, ref textBounds);
                float xText = info.Width / 2 - textBounds.MidX;
                float yText = info.Height / 2 - textBounds.MidY;
                textBounds.Offset(xText, yText);
                paint.Shader = SKShader.CreateLinearGradient(
                                new SKPoint((textBounds.Left + textBounds.Right) / 2, textBounds.Bottom),
                                new SKPoint((textBounds.Left + textBounds.Right) / 2, textBounds.Top),
                                new SKColor[] { new SKColor(0xFF0078D7), new SKColor(0xFF005A9E), new SKColor(0xFF005A9E), new SKColor(0xFF004275) },
                                new float[] { (float)Model.FluxOffset, (float)Model.FluxOffset, (float)Model.FreeOffset, (float)Model.FreeOffset },
                                SKShaderTileMode.Clamp);
                canvas.DrawText(TEXT, xText, yText, paint);
            }
        }
    }
}