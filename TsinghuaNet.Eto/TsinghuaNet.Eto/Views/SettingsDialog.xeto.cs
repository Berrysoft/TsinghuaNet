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
    }
}
