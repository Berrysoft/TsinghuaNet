using System;
using Xamarin.Forms;

namespace TsinghuaNet.XF.Controls
{
    [ContentProperty(nameof(Content))]
    public class Viewbox : Layout<View>
    {
        public static readonly BindableProperty ContentProperty = BindableProperty.Create(
            nameof(Content), typeof(View), typeof(Viewbox), propertyChanged: OnContentChanged);

        public static void OnContentChanged(BindableObject bindable, object oldVal, object newVal)
        {
            var vb = (Viewbox)bindable;

            vb.Children.Clear();
            vb.Children.Add((View)newVal);
        }

        private SizeRequest? ContentMeasurementCache { get; set; }

        public View Content
        {
            get { return (View)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        protected override void OnAdded(View view)
        {
            if (Children.Count > 1)
                throw new InvalidOperationException("ViewBox can only contain a single child. Use Content property.");
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            var zero = new SizeRequest(Size.Zero);

            var content = Content;

            // if content is not set or constraints don't make sense, return zero
            if (content == null ||
                double.IsNaN(widthConstraint) || double.IsNegativeInfinity(widthConstraint) ||
                double.IsNaN(heightConstraint) || double.IsNegativeInfinity(heightConstraint))
                return zero;

            // measure the content without constraints and cache it for later
            ContentMeasurementCache = MeasureFull(content);

            var request = ContentMeasurementCache.Value.Request;

            var rw = request.Width;
            var rh = request.Height;

            // if we have infinite space, request content at full-scale
            if (double.IsPositiveInfinity(widthConstraint) && double.IsPositiveInfinity(heightConstraint))
                return new SizeRequest(request);

            // if we only have infinite width, request content scaled to fit heightConstraint (or zero, if impossible)
            if (double.IsPositiveInfinity(widthConstraint))
                return new SizeRequest(request * DivideOrZero(heightConstraint, rh));

            // if we only have infinite height, request content scaled to fit widthConstraint (or zero, if impossible)
            if (double.IsPositiveInfinity(heightConstraint))
                return new SizeRequest(request * DivideOrZero(widthConstraint, rw));

            // Otherwise, request content scaled to fit minimum constraint ratio (or zero, if impossible)
            return new SizeRequest(request * Math.Min(DivideOrZero(widthConstraint, rw), DivideOrZero(heightConstraint, rh)));
        }

        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            var content = Content;
            var requestCache = ContentMeasurementCache;

            if (content == null)
                return;

            if (requestCache == null)
                requestCache = ContentMeasurementCache = MeasureFull(content);

            var request = requestCache.Value.Request;

            var rw = request.Width;
            var rh = request.Height;

            // scale from center (we could add LayoutOptions detection so that we could have start/end/center scaling)
            content.AnchorX = content.AnchorY = 0.5;
            content.Scale = Math.Min(DivideOrZero(width, rw), DivideOrZero(height, rh));
            content.Layout(new Rectangle(x + ((width - rw) / 2), y + ((height - rh) / 2), rw, rh));
        }

        private SizeRequest MeasureFull(View view) => view.Measure(double.PositiveInfinity, double.PositiveInfinity, MeasureFlags.IncludeMargins);

        private double DivideOrZero(double top, double btm) => btm == 0 ? 0 : top / btm;
    }
}
