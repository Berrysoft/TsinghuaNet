using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Syncfusion.XForms.Android.PopupLayout;
using Syncfusion.XForms.Themes;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using xe = Xamarin.Essentials;

namespace TsinghuaNet.XF.Droid
{
    [Activity(Label = "清华校园网", Icon = "@drawable/icon", Theme = "@style/splashscreen", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, LaunchMode = LaunchMode.SingleTop)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.SetTheme(Resource.Style.MainTheme);

            base.OnCreate(savedInstanceState);

            xe.Platform.Init(this, savedInstanceState);
            Forms.SetFlags("Brush_Experimental");
            Forms.Init(this, savedInstanceState);
            SfPopupLayoutRenderer.Init();
            var app = new App(GetAppTheme());
            LoadApplication(app);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            xe.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private ResourceDictionary GetAppTheme()
        {
            if (Resources.Configuration.UiMode.HasFlag(UiMode.NightYes))
            {
                return new DarkTheme();
            }
            else
            {
                return new LightTheme();
            }
        }
    }
}