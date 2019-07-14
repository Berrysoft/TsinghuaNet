using System.ComponentModel;

namespace TsinghuaNet.Models
{
    public interface INetSettings : INotifyPropertyChanged
    {
        bool AutoLogin { get; set; }

        bool EnableFluxLimit { get; set; }

        ByteSize FluxLimit { get; set; }
    }
}
