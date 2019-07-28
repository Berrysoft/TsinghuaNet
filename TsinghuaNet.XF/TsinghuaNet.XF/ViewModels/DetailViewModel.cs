using System;
using System.Collections.Generic;
using System.Linq;
using Microcharts;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using TsinghuaNet.Models;
using TsinghuaNet.ViewModels;

namespace TsinghuaNet.XF.ViewModels
{
    class DetailViewModel : DetailViewModelBase
    {
        public DetailViewModel() : base()
        {
            Chart = new LineChart
            {
                BackgroundColor = SKColors.Transparent,
                IsAnimated = true
            };
        }

        public LineChart Chart { get; set; }

        public override void SetGroupedDetails(IEnumerable<KeyValuePair<DateTime, ByteSize>> source)
        {
            Chart.Entries = source.GetTotalDetails().Select(DetailsHelper.GetChartEntry);
        }
    }

    static class DetailsHelper
    {
        public static IEnumerable<KeyValuePair<DateTime, ByteSize>> GetTotalDetails(this IEnumerable<KeyValuePair<DateTime, ByteSize>> source)
        {
            ByteSize total = default;
            foreach (var p in source)
            {
                total += p.Value;
                yield return new KeyValuePair<DateTime, ByteSize>(p.Key, total);
            }
        }

        public static ChartEntry GetChartEntry(KeyValuePair<DateTime, ByteSize> p) => new ChartEntry((float)p.Value.GigaBytes)
        {
            Label = p.Key.Day.ToString(),
            ValueLabel = p.Value.ToString(),
            Color = App.SystemAccentColor.ToSKColor()
        };
    }
}
