using System;
using Eto.Forms;
using Eto.Serialization.Xaml;
using TsinghuaNet.Eto.ViewModels;

namespace TsinghuaNet.Eto.Views
{
    public class MainForm : Form
    {
        private MainViewModel Model;

        public MainForm()
        {
            XamlReader.Load(this);
            DataContext = Model = new MainViewModel();
        }

        private void NetStateList_AddValue(object sender, AddValueEventArgs<NetState> e)
        {
            e.ShouldAdd = e.Value != NetState.Unknown;
        }

        private void ShowDialog<T>(T dialog)
            where T : Dialog
        {
            using (dialog)
            {
                dialog.ShowModal(this);
            }
        }

        private void ShowDialog<T>()
            where T : Dialog, new()
        {
            ShowDialog(new T());
        }

        private void ShowConnection(object sender, EventArgs e)
            => ShowDialog<ConnectionDialog>();

        private void ShowDetails(object sender, EventArgs e)
            => ShowDialog<DetailsDialog>();

        private void ShowAbout(object sender, EventArgs e)
            => ShowDialog(new SettingsDialog(1));

        private void ShowSettings(object sender, EventArgs e)
            => ShowDialog(new SettingsDialog(0));

        private async void MainForm_Closed(object sender, EventArgs e)
        {
            if (Model != null)
                await Model.SaveSettingsAsync();
            Application.Instance.Quit();
        }
    }
}
