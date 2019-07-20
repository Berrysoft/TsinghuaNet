using System;
using Eto.Forms;
using Eto.Forms.Controls.SkiaSharp;
using Eto.Serialization.Xaml;
using SkiaSharp;
using TsinghuaNet.Eto.ViewModels;

namespace TsinghuaNet.Eto.Views
{
    public class MainForm : Form
    {
        private MainViewModel Model;

#pragma warning disable 0649
        private SKControl FluxCanvas;
#pragma warning restore 0649

        public MainForm()
        {
            XamlReader.Load(this);
            DataContext = Model = new MainViewModel();
            FluxCanvas.PaintSurfaceAction = PaintFlux;
            Model.Refreshed += (sender, e) => FluxCanvas.Invalidate();
        }

        private void NetStateList_AddValue(object sender, AddValueEventArgs<NetState> e)
        {
            e.ShouldAdd = e.Value != NetState.Unknown;
        }

        private void PaintFlux(SKSurface surface)
        {
            var canvas = surface.Canvas;
            SKPoint center = new SKPoint(FluxCanvas.Width / 2.0F * Screen.Scale, FluxCanvas.Height / 2.0F * Screen.Scale);
            float radius = Math.Min(FluxCanvas.Width, FluxCanvas.Height) * Screen.Scale * 0.3F;
            float strokeWidth = radius / 1.5F;
            SKRect rect = new SKRect(center.X - radius, center.Y - radius, center.X + radius, center.Y + radius);
            canvas.DrawArc(rect, 360, strokeWidth, App.SystemAccentColorDark2);
            canvas.DrawArc(rect, 360 * Model.FreeOffset, strokeWidth, App.SystemAccentColorDark1);
            canvas.DrawArc(rect, 360 * Model.FluxOffset, strokeWidth, App.SystemAccentColor);
        }

        private void ShowDialog<T>(T dialog)
            where T : Dialog
        {
            using (dialog)
            {
                dialog.ShowModal(this);
            }
        }

        private void ShowDialog<T>()
            where T : Dialog, new()
        {
            ShowDialog(new T());
        }

        private void ShowConnection(object sender, EventArgs e)
            => ShowDialog<ConnectionDialog>();

        private void ShowDetails(object sender, EventArgs e)
            => ShowDialog<DetailsDialog>();

        private void ShowAbout(object sender, EventArgs e)
            => ShowDialog(new SettingsDialog(1));

        private void ShowSettings(object sender, EventArgs e)
            => ShowDialog(new SettingsDialog(0));

        private async void MainForm_Closed(object sender, EventArgs e)
        {
            if (Model != null)
                await Model.SaveSettingsAsync();
            Application.Instance.Quit();
        }
    }

    static class ArcHelper
    {
        public static void DrawArc(this SKCanvas canvas, SKRect rect, float sweepAngle, float strokeWidth, SKColor color)
        {
            if (sweepAngle >= 360F)
                sweepAngle = 359.999F;
            using (SKPath path = new SKPath())
            using (var paint = new SKPaint())
            {
                path.ArcTo(rect, 90, sweepAngle, false);
                paint.Style = SKPaintStyle.Stroke;
                paint.StrokeWidth = strokeWidth;
                paint.Color = color;
                canvas.DrawPath(path, paint);
            }
        }
    }
}
