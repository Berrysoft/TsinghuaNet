using System.Globalization;
using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.Widget;
using TsinghuaNet.Models;
using TsinghuaNet.XF.Droid.Services;

namespace TsinghuaNet.XF.Droid.Background
{
    [BroadcastReceiver(Label = "TsinghuaNet.Widget")]
    [IntentFilter(new string[] { "android.appwidget.action.APPWIDGET_UPDATE" })]
    [MetaData("android.appwidget.provider", Resource = "@xml/appwidgetprovider")]
    public class AppWidget : AppWidgetProvider
    {
        public override async void OnUpdate(Context context, AppWidgetManager appWidgetManager, int[] appWidgetIds)
        {
            var me = new ComponentName(context, Java.Lang.Class.FromType(typeof(AppWidget)).Name);
            var widgetView = new RemoteViews(context.PackageName, Resource.Layout.Widget);
            InternetStatus status = new InternetStatus();
            NetCredential credential = new NetCredential();
            credential.State = await status.SuggestAsync();
            var helper = credential.GetHelper();
            if (helper != null)
            {
                FluxUser user = await helper.GetFluxAsync();
                widgetView.SetTextViewText(Resource.Id.widgetTitle, user.Username);
                widgetView.SetTextViewText(Resource.Id.widgetFlux, $"流量：{user.Flux}");
                widgetView.SetTextViewText(Resource.Id.widgetBalance, string.Format(CultureInfo.GetCultureInfo("zh-CN"), "余额：{0:C2}", user.Balance));
            }
            var intent = new Intent(context, typeof(AppWidget));
            intent.SetAction(AppWidgetManager.ActionAppwidgetUpdate);
            intent.PutExtra(AppWidgetManager.ExtraAppwidgetId, appWidgetIds);
            var piBackground = PendingIntent.GetBroadcast(context, 0, intent, PendingIntentFlags.UpdateCurrent);
            widgetView.SetOnClickPendingIntent(Resource.Id.widgetBackground, piBackground);
            appWidgetManager.UpdateAppWidget(me, widgetView);
        }
    }
}