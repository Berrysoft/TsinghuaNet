using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmHelpers;
using Xamarin.Forms;

namespace TsinghuaNet.CrossPlatform.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public ObservableRangeCollection<NetUser> Items { get; set; }
        public ICommand LoadItemsCommand { get; set; }

        public ItemsViewModel()
        {
            Title = "在线信息";
            Items = new ObservableRangeCollection<NetUser>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var helper = Credential.GetUseregHelper();
                var items = await helper.GetUsersAsync();
                Items.AddRange(items);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}