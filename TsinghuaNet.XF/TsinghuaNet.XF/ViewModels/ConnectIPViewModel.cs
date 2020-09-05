using System.Net;
using System.Threading.Tasks;
using TsinghuaNet.ViewModels;

namespace TsinghuaNet.XF.ViewModels
{
    class ConnectIPViewModel : NetViewModelBase
    {
        public ConnectionViewModel ConnectionModel { get; set; }

        public string IP { get; set; }

        public Task ConfirmAsync()
        {
            return ConnectionModel.ConnectAsync(IPAddress.Parse(IP));
        }
    }
}
