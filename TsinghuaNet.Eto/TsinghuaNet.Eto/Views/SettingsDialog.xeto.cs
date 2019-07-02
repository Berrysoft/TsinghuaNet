using System;
using System.Diagnostics;
using Eto;
using Eto.Forms;
using Eto.Serialization.Xaml;
using TsinghuaNet.Eto.ViewModels;

namespace TsinghuaNet.Eto.Views
{
    public class SettingsDialog : Dialog
    {
        private SettingsViewModel Model;

        public SettingsDialog(int page = 0)
        {
            XamlReader.Load(this);
            Model = new SettingsViewModel();
            DataContext = Model;
            FindChild<TabControl>("SettingsTab").SelectedIndex = page;
        }

        private const string RepoUri = "https://github.com/Berrysoft/TsinghuaNet";
        private void ShowRepo(object sender, EventArgs e)
        {
            try
            {
                var os = EtoEnvironment.Platform;
                if (os.IsWindows)
                    Process.Start(new ProcessStartInfo(RepoUri) { UseShellExecute = true });
                else if (os.IsLinux)
                    Process.Start("xdg-open", RepoUri);
                else if (os.IsMac)
                    Process.Start("open", RepoUri);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
    }
}
