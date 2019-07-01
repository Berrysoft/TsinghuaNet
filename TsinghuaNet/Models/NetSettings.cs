using System.ComponentModel;

namespace TsinghuaNet.Models
{
    public class NetSettings : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public bool AutoLogin { get; set; }

        public bool EnableFluxLimit { get; set; }

        public ByteSize FluxLimit { get; set; }
    }
}
