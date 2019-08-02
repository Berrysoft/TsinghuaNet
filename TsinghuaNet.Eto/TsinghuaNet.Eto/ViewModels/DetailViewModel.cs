using System;
using System.Collections.Generic;
using System.Linq;
using TsinghuaNet.Models;
using TsinghuaNet.ViewModels;

namespace TsinghuaNet.Eto.ViewModels
{
    public class DetailViewModel : DetailViewModelBase
    {
        public DetailViewModel() : base() { }

        public List<KeyValuePair<DateTime, ByteSize>> GroupedDetails { get; set; }

        protected override void SetGroupedDetails(IEnumerable<KeyValuePair<DateTime, ByteSize>> source)
        {
            GroupedDetails = source.ToList();
        }

        protected override void SetTimeDetails(IEnumerable<KeyValuePair<int, ByteSize>> source) { }
    }
}
