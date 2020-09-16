using System;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TsinghuaNet.XF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InfoPage : ContentPage
    {
        public InfoPage()
        {
            InitializeComponent();
            Model.LoadSettings();
        }

        private void InfoPage_SizeChanged(object sender, EventArgs e)
        {
            VisualStateManager.GoToState(InfoLayout, Width > Height ? "HorizontalState" : "VerticalState");
        }

        internal void SaveSettings() => Model.SaveSettings();

        private void Model_Refreshed(object sender, EventArgs e)
        {
            FluxLabel.Foreground = new LinearGradientBrush(new GradientStopCollection()
            {
                new GradientStop(App.SystemAccentColor, Model.FluxOffset),
                new GradientStop(App.SystemAccentColorDark1, Model.FluxOffset),
                new GradientStop(App.SystemAccentColorDark1, Model.FreeOffset),
                new GradientStop(App.SystemAccentColorDark2, Model.FreeOffset)
            }, new Point(0.5, 1), new Point(0.5, 0));
        }

        private async void Model_SettingsLoaded(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Model.Credential.Username))
            {
                await PopupNavigation.Instance.PushAsync(new ChangeUserPage());
            }
        }
    }
}