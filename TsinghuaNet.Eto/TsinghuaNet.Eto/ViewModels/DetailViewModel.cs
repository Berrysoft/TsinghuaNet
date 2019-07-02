using System.Collections.Generic;
using System.Linq;
using MvvmHelpers;
using TsinghuaNet.Models;

namespace TsinghuaNet.Eto.ViewModels
{
    public class DetailViewModel : DetailViewModelBase
    {
        public DetailViewModel() : base()
        {
            DetailsInitialized += Model_DetailsInitialized;
        }

        public ObservableRangeCollection<NetDetail> DetailsSource { get; } = new ObservableRangeCollection<NetDetail>();

        private void Model_DetailsInitialized(object sender, List<NetDetail> details)
        {
            DetailsSource.ReplaceRange(details);
        }

        protected override void SetSortedDetails(IEnumerable<NetDetail> source) => DetailsSource.ReplaceRange(source);
    }
}
