using System;
using System.Collections.Generic;
using System.Text;

namespace TsinghuaNet.CrossPlatform.ViewModels
{
    public class DetailViewModel : Helper.DetailViewModel
    {
        private IEnumerable<NetDetail> details;

        protected override IEnumerable<NetDetail> InitialDetails
        {
            get => details;
            set
            {
                details = value;
                DetailsSource = details;
            }
        }

        private IEnumerable<NetDetail> detailsSource;
        public IEnumerable<NetDetail> DetailsSource
        {
            get => detailsSource;
            set => SetProperty(ref detailsSource, value);
        }

        protected override void SetSortedDetails(IEnumerable<NetDetail> source) => DetailsSource = source;
    }
}
