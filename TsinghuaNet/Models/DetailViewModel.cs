using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;
using TsinghuaNet.Helpers;

namespace TsinghuaNet.Models
{
    public abstract class DetailViewModel : NetObservableBase
    {
        protected abstract IEnumerable<NetDetail> InitialDetails { get; set; }
        protected abstract void SetSortedDetails(IEnumerable<NetDetail> source);

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
                InitialDetails = await helper.GetDetailsAsync(NetDetailOrder.LogoutTime, false);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public ICommand RefreshCommand { get; }

        public void SortSource(NetDetailOrder? order, bool descending)
        {
            if (order == null)
                SetSortedDetails(InitialDetails);
            else
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
