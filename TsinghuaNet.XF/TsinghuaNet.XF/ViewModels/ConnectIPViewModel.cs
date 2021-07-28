using System.Net;
using System.Windows.Input;
using TsinghuaNet.Models;
using TsinghuaNet.ViewModels;

namespace TsinghuaNet.XF.ViewModels
{
    class ConnectIPViewModel : NetViewModelBase
    {
        public ConnectIPViewModel() : base()
        {
            ConfirmCommand = new Command(this, Confirm);
        }

        public ConnectionViewModel ConnectionModel { get; set; }

        public string IP { get; set; }

        public async void Confirm()
        {
            await ConnectionModel.ConnectAsync(IPAddress.Parse(IP));
        }

        public ICommand ConfirmCommand { get; }
    }
}
