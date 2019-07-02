using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using TsinghuaNet.Helpers;

namespace TsinghuaNet.Models
{
    public abstract class DetailViewModelBase : NetViewModelBase
    {
        public List<NetDetail> InitialDetails { get; set; }
        protected abstract void SetSortedDetails(IEnumerable<NetDetail> source);

        public DetailViewModelBase()
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
                DetailsInitialized?.Invoke(this, InitialDetails);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public event EventHandler<List<NetDetail>> DetailsInitialized;

        public ICommand RefreshCommand { get; }

        public void SortSource(NetDetailOrder? order, bool descending)
        {
            if (order == null)
                SetSortedDetails(InitialDetails);
            else
            {
                switch (order.Value)
                {
                    case NetDetailOrder.LoginTime:
                        SetSortedDetails(InitialDetails.OrderBy(d => d.LoginTime, descending));
                        break;
                    case NetDetailOrder.LogoutTime:
                        SetSortedDetails(InitialDetails.OrderBy(d => d.LogoutTime, descending));
                        break;
                    case NetDetailOrder.Flux:
                        SetSortedDetails(InitialDetails.OrderBy(d => d.Flux, descending));
                        break;
                }
            }
        }
    }
}
