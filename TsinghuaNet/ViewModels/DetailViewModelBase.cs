using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using TsinghuaNet.Models;

namespace TsinghuaNet.ViewModels
{
    public abstract class DetailViewModelBase : NetViewModelBase
    {
        public List<NetDetail> InitialDetails { get; set; }

        protected abstract void SetGroupedDetails(IEnumerable<KeyValuePair<DateTime, ByteSize>> source);

        protected abstract void SetTimeDetails(IEnumerable<KeyValuePair<int, ByteSize>> source);

        public DetailViewModelBase()
        {
            RefreshDetails();
            RefreshCommand = new Command(this, RefreshDetails);
        }

        public async void RefreshDetails()
        {
            if (!string.IsNullOrEmpty(Credential.Username))
            {
                try
                {
                    IsBusy = true;
                    var helper = Credential.GetUseregHelper();
                    await helper.LoginAsync();
                    InitialDetails = await helper.GetDetailsAsync(NetDetailOrder.LogoutTime, false).ToListAsync();
                    DateTime now = DateTime.Now;
                    SetGroupedDetails(from d in InitialDetails group d.Flux by d.LogoutTime.Day into g select new KeyValuePair<DateTime, ByteSize>(new DateTime(now.Year, now.Month, g.Key), g.Sum()));
                    SetTimeDetails(from d in InitialDetails group d.Flux by d.LogoutTime.Hour into g select new KeyValuePair<int, ByteSize>(g.Key, g.Sum()));
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

        public ICommand RefreshCommand { get; }
    }
}
