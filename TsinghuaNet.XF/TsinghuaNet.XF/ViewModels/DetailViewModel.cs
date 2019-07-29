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
        public static IEnumerable<KeyValuePair<int, ByteSize>> GetTotalDetails(this IEnumerable<KeyValuePair<DateTime, ByteSize>> source)
        {
            ByteSize total = default;
            int current_day = 0;
            foreach (var p in source)
            {
                for (; p.Key.Day - current_day > 1; current_day++)
                {
                    yield return new KeyValuePair<int, ByteSize>(current_day, total);
                }
                total += p.Value;
                current_day = p.Key.Day;
                yield return new KeyValuePair<int, ByteSize>(p.Key.Day, total);
            }
        }

        public static ChartEntry GetChartEntry(KeyValuePair<int, ByteSize> p) => new ChartEntry((float)p.Value.GigaBytes)
        {
            Label = p.Key.ToString(),
            ValueLabel = p.Value.ToString(),
            Color = App.SystemAccentColor.ToSKColor()
        };
    }
}
