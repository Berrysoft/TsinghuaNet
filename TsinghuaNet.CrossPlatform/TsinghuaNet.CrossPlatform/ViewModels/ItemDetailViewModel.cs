using TsinghuaNet.Helper;

namespace TsinghuaNet.CrossPlatform.ViewModels
{
    public class ItemDetailViewModel : NetObservableBase
    {
        public NetUser Item { get; set; }
        public ItemDetailViewModel(NetUser item = default)
        {
            Item = item;
        }
    }
}
