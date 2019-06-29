using System.Collections.Generic;
using MvvmHelpers;

namespace TsinghuaNet.Eto.ViewModels
{
    public class DetailViewModel : TsinghuaNet.Models.DetailViewModel
    {
        public DetailViewModel() : base() { }

        private List<NetDetail> details;

        public ObservableRangeCollection<NetDetail> DetailsSource { get; } = new ObservableRangeCollection<NetDetail>();

        protected override IEnumerable<NetDetail> InitialDetails
        {
            get => details;
            set
            {
                details = (List<NetDetail>)value;
                DetailsSource.ReplaceRange(details);
            }
        }

        protected override void SetSortedDetails(IEnumerable<NetDetail> source) => DetailsSource.ReplaceRange(source);
    }
}
