using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Java.Util;
using Windows.UI.Xaml;

namespace TsinghuaNet.Uno.Droid
{
    [Activity(
            MainLauncher = true,
            ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize,
            WindowSoftInputMode = SoftInput.AdjustPan | SoftInput.StateHidden
    )]
    public class MainActivity : ApplicationActivity
    {
    }
}

