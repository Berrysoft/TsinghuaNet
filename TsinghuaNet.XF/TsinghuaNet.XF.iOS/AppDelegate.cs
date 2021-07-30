using Foundation;
using Syncfusion.SfChart.XForms.iOS.Renderers;
using Syncfusion.SfDataGrid.XForms.iOS;
using Syncfusion.XForms.iOS.PopupLayout;
using TsinghuaNet.XF.iOS.Renderers;
using UIKit;
using Xamarin.Forms;

namespace TsinghuaNet.XF.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Forms.SetFlags("Brush_Experimental");
            Forms.Init();
            SfChartRenderer.Init();
            SfDataGridRenderer.Init();
            SfPopupLayoutRenderer.Init();
            LoadApplication(new App(UIScreen.MainScreen.TraitCollection.GetColorTheme()));

            return base.FinishedLaunching(app, options);
        }
    }
}
