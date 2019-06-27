using System;
using System.Numerics;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace TsinghuaNet.Uno.Contents
{
    public sealed partial class Arc : Canvas
    {
        public Arc()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ThicknessProperty = DependencyProperty.Register(nameof(Thickness), typeof(double), typeof(Arc), new PropertyMetadata(30.0, OnSizePropertyChanged));
        public double Thickness
        {
            get => (double)GetValue(ThicknessProperty);
            set => SetValue(ThicknessProperty, value);
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(double), typeof(Arc), new PropertyMetadata(0, OnSizePropertyChanged));
        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        private static void OnSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((Arc)d).DrawArc();

        public static readonly DependencyProperty FillProperty = DependencyProperty.Register(nameof(Fill), typeof(Brush), typeof(Arc), new PropertyMetadata(null));
        public Brush Fill
        {
            get => (Brush)GetValue(FillProperty);
            set => SetValue(FillProperty, value);
        }

        private static double GetAngle(double value)
        {
            var angle = value * 2 * Math.PI;
            if (angle >= 2 * Math.PI)
                angle = 2 * Math.PI - 0.000001;
            return angle;
        }

        private static Point ScaleUnitCirclePoint(Point origin, double radius, double angle)
        {
            Vector2 dir = new Vector2(-(float)Math.Sin(angle), (float)Math.Cos(angle));
            var result = (dir * (float)radius + new Vector2((float)origin.X, (float)origin.Y));
            return new Point(result.X, result.Y);
        }

        private void DrawArc()
        {
            var size = RenderSize;
            var length = Math.Min(size.Width, size.Height);
            var radius = Math.Abs((length - Thickness) / 2);
            var centerPoint = new Point(size.Width / 2, size.Height / 2);
            var angle = GetAngle(Value);
            Point circleStart = new Point(centerPoint.X, centerPoint.Y + radius);
            ArcFigureSegment.IsLargeArc = angle > Math.PI;
            ArcFigureSegment.Point = ScaleUnitCirclePoint(centerPoint, radius, angle);
            ArcFigureSegment.Size = new Size(radius, radius);
            ArcFigure.StartPoint = circleStart;
        }
    }
}
