using System;
using TsinghuaNet.Helpers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TsinghuaNet.Uno.Contents
{
    public sealed partial class ArcUserContent : UserControl, IUserContent
    {
        public ArcUserContent()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty UserProperty = DependencyProperty.Register(nameof(User), typeof(FluxUser), typeof(ArcUserContent), new PropertyMetadata(null, UserPropertyChanged));
        public FluxUser User
        {
            get => (FluxUser)GetValue(UserProperty);
            set => SetValue(UserProperty, value);
        }
        private static void UserPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ArcUserContent content = (ArcUserContent)d;
            FluxUser flux = (FluxUser)e.NewValue;
            content.OnlineTime = flux.OnlineTime;
            var maxf = FluxHelper.GetMaxFlux(flux.Flux, flux.Balance);
            content.FluxAnimation.To = flux.Flux / maxf;
            content.FreeAnimation.To = FluxHelper.BaseFlux / maxf;
        }

        public static readonly DependencyProperty OnlineTimeProperty = DependencyProperty.Register(nameof(OnlineTime), typeof(TimeSpan), typeof(ArcUserContent), new PropertyMetadata(TimeSpan.Zero));
        public TimeSpan OnlineTime
        {
            get => (TimeSpan)GetValue(OnlineTimeProperty);
            set => SetValue(OnlineTimeProperty, value);
        }

        public static readonly DependencyProperty FreeOffsetProperty = DependencyProperty.Register(nameof(FreeOffset), typeof(double), typeof(ArcUserContent), new PropertyMetadata(0.0));
        public double FreeOffset
        {
            get => (double)GetValue(FreeOffsetProperty);
            set => SetValue(FreeOffsetProperty, value);
        }

        public static readonly DependencyProperty FluxOffsetProperty = DependencyProperty.Register(nameof(FluxOffset), typeof(double), typeof(ArcUserContent), new PropertyMetadata(0.0));
        public double FluxOffset
        {
            get => (double)GetValue(FluxOffsetProperty);
            set => SetValue(FluxOffsetProperty, value);
        }

        public bool IsProgressActive
        {
            get => Progress.IsActive;
            set => Progress.IsActive = value;
        }

        public void BeginAnimation()
        {
            FluxStoryboard.Begin();
        }

        public bool AddOneSecond()
        {
            if (User.Username == null || string.IsNullOrEmpty(User.Username))
                return false;
            else
            {
                OnlineTime += TimeSpan.FromSeconds(1);
                return true;
            }
        }
    }
}
