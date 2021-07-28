using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Syncfusion.XForms.Android.PopupLayout;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TsinghuaNet.XF.Droid
{
    [Activity(Label = "TsinghuaNet.XF", Icon = "@drawable/icon", Theme = "@style/splashscreen", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, LaunchMode = LaunchMode.SingleTop)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.SetTheme(Resource.Style.MainTheme);

            base.OnCreate(savedInstanceState);

            Platform.Init(this, savedInstanceState);
            Forms.SetFlags("Brush_Experimental");
            Forms.Init(this, savedInstanceState);
            SfPopupLayoutRenderer.Init();
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}