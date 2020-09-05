using System;
using System.ComponentModel;
using System.Diagnostics;
using TsinghuaNet.XF.UWP.Renderers;
using Windows.Foundation;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using Win = Windows.UI.Xaml.Controls;

[assembly: ExportRenderer(typeof(ListView), typeof(ListViewRefreshRenderer))]

namespace TsinghuaNet.XF.UWP.Renderers
{
    public class ListViewRefreshRenderer : ListViewRenderer
    {
        private Win.RefreshContainer container;

        private Deferral refreshDeferral;

        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                if (Element.IsPullToRefreshEnabled)
                {
                    if (container == null)
                        container = new Win.RefreshContainer();
                    container.RefreshRequested += Container_RefreshRequested;
                    var nativeControl = Control;
                    SetNativeControl(container);
                    container.Content = nativeControl;
                }
            }
            else
            {
                if (Element.IsPullToRefreshEnabled && container != null)
                {
                    container.RefreshRequested -= Container_RefreshRequested;
                    container = null;
                }
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == ListView.IsRefreshingProperty.PropertyName)
            {
                if (!Element.IsRefreshing && refreshDeferral != null)
                {
                    refreshDeferral.Complete();
                    refreshDeferral.Dispose();
                }
            }
        }

        private void Container_RefreshRequested(Win.RefreshContainer sender, Win.RefreshRequestedEventArgs args)
        {
            refreshDeferral = args.GetDeferral();
            Element.RefreshCommand?.Execute(null);
        }
    }
}
