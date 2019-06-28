using System;
using System.Collections.Generic;

namespace TsinghuaNet.Uno.ViewModels
{
    class DetailViewModel : TsinghuaNet.Models.DetailViewModel
    {
        public DetailViewModel() : base() { }

        private List<NetDetail> details;

        private IEnumerable<NetDetail> detailsSource;
        public IEnumerable<NetDetail> DetailsSource
        {
            get => detailsSource;
            set => SetProperty(ref detailsSource, value);
        }

        protected override IEnumerable<NetDetail> InitialDetails
        {
            get => details;
            set
            {
                details = (List<NetDetail>)value;
                DetailsInitialized?.Invoke(this, value);
                DetailsSource = details;
            }
        }

        public event EventHandler<IEnumerable<NetDetail>> DetailsInitialized;

        protected override void SetSortedDetails(IEnumerable<NetDetail> source) => DetailsSource = source;
    }
}
