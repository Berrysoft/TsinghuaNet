using System;
using System.Linq;
using Eto.Forms;
using Eto.Serialization.Xaml;
using TsinghuaNet.Eto.Controls;
using TsinghuaNet.Models;
using TsinghuaNet.ViewModels;

namespace TsinghuaNet.Eto.Views
{
    public class ConnectionDialog : Dialog
    {
        private ConnectionViewModel Model;

#pragma warning disable 0649
        private SortableGridView ConnectionView;
#pragma warning restore 0649

        public ConnectionDialog()
        {
            XamlReader.Load(this);
            DataContext = Model = new ConnectionViewModel();
        }

        private async void DropSelection(object sender, EventArgs e)
        {
            await Model.DropAsync(ConnectionView.SelectedItems.Select(user => ((NetUser)user).Address));
        }
    }
}
