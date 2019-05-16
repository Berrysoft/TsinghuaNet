using System.Threading.Tasks;
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

        public async Task DropAsync()
        {
            var helper = Credential.GetUseregHelper();
            await helper.LoginAsync();
            await helper.LogoutAsync(Item.Address);
        }
    }
}
