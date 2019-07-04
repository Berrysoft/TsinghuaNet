using System;
using Eto.Forms;
using Eto.Serialization.Xaml;
using TsinghuaNet.Eto.ViewModels;
using TsinghuaNet.Helpers;

namespace TsinghuaNet.Eto.Views
{
    public class MainForm : Form
    {
        private MainViewModel Model;

        public MainForm()
        {
            XamlReader.Load(this);
            Model = new MainViewModel();
            DataContext = Model;
            var NetStateList = FindChild<EnumRadioButtonList<NetState>>("NetStateList");
            NetStateList.AddValue += NetStateList_AddValue;
            NetStateList.SelectedValueBinding.
                BindDataContext(Binding.Property((MainViewModel m) => m.Credential.State));
            FindChild<Label>("OnlineUserBalanceLabel").TextBinding.
                BindDataContext(Binding.Property((MainViewModel m) => m.OnlineUser.Balance).
                Convert(StringHelper.GetCurrencyString));
        }

        private void NetStateList_AddValue(object sender, AddValueEventArgs<NetState> e)
        {
            if (e.Value == NetState.Unknown)
                e.ShouldAdd = false;
        }

        private void ShowConnection(object sender, EventArgs e)
        {
            using (ConnectionDialog dialog = new ConnectionDialog())
            {
                dialog.ShowModal(this);
            }
        }

        private void ShowDetails(object sender, EventArgs e)
        {
            using (DetailsDialog dialog = new DetailsDialog())
            {
                dialog.ShowModal(this);
            }
        }

        private void ShowAbout(object sender, EventArgs e)
        {
            using (SettingsDialog dialog = new SettingsDialog(1))
            {
                dialog.ShowModal(this);
            }
        }

        private void ShowSettings(object sender, EventArgs e)
        {
            using (SettingsDialog dialog = new SettingsDialog())
            {
                dialog.ShowModal(this);
            }
        }

        private void MainForm_Closed(object sender, EventArgs e)
        {
            Model.SaveSettings();
            Application.Instance.Quit();
        }
    }
}
