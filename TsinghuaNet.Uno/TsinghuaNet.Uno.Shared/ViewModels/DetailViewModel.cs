using System;
using System.Collections.Generic;
using TsinghuaNet.Models;

namespace TsinghuaNet.Uno.ViewModels
{
    class DetailViewModel : DetailViewModelBase
    {
        public DetailViewModel() : base()
        {
            DetailsInitialized += Model_DetailsInitialized;
        }

        public IEnumerable<NetDetail> DetailsSource { get; set; }

        private void Model_DetailsInitialized(object sender, List<NetDetail> details)
        {
            DetailsSource = details;
        }

        protected override void SetSortedDetails(IEnumerable<NetDetail> source) => DetailsSource = source;
    }
}
