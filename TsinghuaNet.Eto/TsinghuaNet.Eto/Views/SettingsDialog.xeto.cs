using System;
using System.Diagnostics;
using Eto;
using Eto.Forms;
using Eto.Serialization.Xaml;
using TsinghuaNet.Eto.Models;
using TsinghuaNet.Eto.ViewModels;

namespace TsinghuaNet.Eto.Views
{
    public class SettingsDialog : Dialog
    {
        private SettingsViewModel Model;

#pragma warning disable 0649
        private TabControl SettingsTab;
#pragma warning restore 0649

        public SettingsDialog(int page)
        {
            XamlReader.Load(this);
            DataContext = Model = new SettingsViewModel();
            SettingsTab.SelectedIndex = page;
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

        private void OpenSettingsFile(object sender, EventArgs e)
        {
            try
            {
                var path = SettingsHelper.Helper.FilePath;
                var os = EtoEnvironment.Platform;
                if (os.IsWindows)
                    Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
                else if (os.IsLinux)
                    Process.Start("xdg-open", path);
                else if (os.IsMac)
                    Process.Start("open", path);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
    }
}
