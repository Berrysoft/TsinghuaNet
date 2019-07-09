using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using TsinghuaNet.Models;

namespace TsinghuaNet.ViewModels
{
    public class DetailViewModel : NetViewModelBase
    {
        public List<NetDetail> InitialDetails { get; set; }

        public List<KeyValuePair<DateTime, ByteSize>> GroupedDetails { get; set; }

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
                DateTime now = DateTime.Now;
                GroupedDetails = (from d in InitialDetails group d.Flux by d.LogoutTime.Day into g select new KeyValuePair<DateTime, ByteSize>(new DateTime(now.Year, now.Month, g.Key), g.Sum())).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public ICommand RefreshCommand { get; }
    }
}
