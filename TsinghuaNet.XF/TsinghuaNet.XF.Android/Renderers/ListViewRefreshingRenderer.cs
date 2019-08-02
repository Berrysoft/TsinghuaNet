// https://stackoverflow.com/questions/42165245/xamarin-forms-listview-programatic-refresh-not-stopping-on-android-when-page-loa

using System.ComponentModel;
using Android.Content;
using Android.Support.V4.Widget;
using TsinghuaNet.XF.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ListView), typeof(ListViewRefreshingRenderer))]

namespace TsinghuaNet.XF.Droid.Renderers
{
    public class ListViewRefreshingRenderer : ListViewRenderer
    {
        /// <summary>
        /// The refresh layout that wraps the native ListView.
        /// </summary>
        private SwipeRefreshLayout _refreshLayout;

        public ListViewRefreshingRenderer(Context context) : base(context)
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _refreshLayout = null;
            }
            base.Dispose(disposing);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);
            _refreshLayout = (SwipeRefreshLayout)Control.Parent;
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == ListView.IsRefreshingProperty.PropertyName)
            {
                // Do not call base method: we are handling it manually
                UpdateIsRefreshing();
                return;
            }
            base.OnElementPropertyChanged(sender, e);
        }

        /// <summary>
        /// Updates SwipeRefreshLayout animation status depending on the IsRefreshing Element 
        /// property.
        /// </summary>
        protected void UpdateIsRefreshing()
        {
            // I'm afraid this method can be called after the ListViewRenderer is disposed
            // So let's create a new reference to the SwipeRefreshLayout instance
            SwipeRefreshLayout refreshLayoutInstance = _refreshLayout;

            if (refreshLayoutInstance == null)
            {
                return;
            }

            bool isRefreshing = Element.IsRefreshing;
            refreshLayoutInstance.Post(() =>
            {
                refreshLayoutInstance.Refreshing = isRefreshing;
            });
        }
    }
}