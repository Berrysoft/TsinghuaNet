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

        public override void SetGroupedDetails(IEnumerable<KeyValuePair<DateTime, ByteSize>> source)
        {
            GroupedDetails = source.ToList();
        }
    }
}
