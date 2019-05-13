namespace TsinghuaNet.CrossPlatform.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public NetUser Item { get; set; }
        public ItemDetailViewModel(NetUser item = default)
        {
            Title = item.Client;
            Item = item;
        }
    }
}
