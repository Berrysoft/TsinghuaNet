using Syncfusion.XForms.PopupLayout;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TsinghuaNet.XF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InfoPage : ContentPage
    {
        private SfPopupLayout layout = new SfPopupLayout();
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
            App app = (App)Application.Current;
            FluxLabel.Foreground = new LinearGradientBrush(new GradientStopCollection()
            {
                new GradientStop(app.SystemAccentColor, 0),
                new GradientStop(app.SystemAccentColor, Model.FluxOffset),
                new GradientStop(app.SystemAccentColorDark1, Model.FluxOffset),
                new GradientStop(app.SystemAccentColorDark1, Model.FreeOffset),
                new GradientStop(app.SystemAccentColorDark2, Model.FreeOffset),
                new GradientStop(app.SystemAccentColorDark2, 1)
            }, new Point(0.5, 1), new Point(0.5, 0));
        }

        private void Model_SettingsLoaded(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Model.Credential.Username))
            {
                layout.SetValue(SfPopupLayout.PopupViewProperty, new ChangeUserPage());
                layout.Show();
            }
        }
    }
}