using System;

using TsinghuaNet.CrossPlatform.Models;

namespace TsinghuaNet.CrossPlatform.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public Item Item { get; set; }
        public ItemDetailViewModel(Item item = null)
        {
            Title = item?.Text;
            Item = item;
        }
    }
}
