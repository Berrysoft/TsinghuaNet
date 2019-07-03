using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;

namespace TsinghuaNet.Models
{
    public class DetailViewModel : NetViewModelBase
    {
        public List<NetDetail> InitialDetails { get; set; }

        public DetailViewModel()
        {
            InitializeDetails();
            RefreshCommand = new Command(this, InitializeDetails);
        }

        private async void InitializeDetails()
        {
            try
            {
                var helper = Credential.GetUseregHelper();
                await helper.LoginAsync();
                InitialDetails = await helper.GetDetailsAsync(NetDetailOrder.LogoutTime, false).ToListAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public ICommand RefreshCommand { get; }
    }
}
