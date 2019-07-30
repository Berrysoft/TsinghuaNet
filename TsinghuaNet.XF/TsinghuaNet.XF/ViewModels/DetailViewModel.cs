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
            DailyChart = new LineChart
            {
                BackgroundColor = SKColors.Transparent,
                LineMode = LineMode.Straight,
                IsAnimated = true
            };
            TimeChart = new BarChart
            {
                BackgroundColor = SKColors.Transparent,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
                IsAnimated = true
            };
        }

        public BarChart TimeChart { get; set; }

        protected override void SetTimeDetails(IEnumerable<KeyValuePair<int, ByteSize>> source)
        {
            TimeChart.Entries = (from p in source
                                 group p.Value by p.Key / 6 into g
                                 orderby g.Key
                                 select new KeyValuePair<int, ByteSize>(g.Key, g.Sum())).
                                 Supplement(0, p => p.Key, h => new KeyValuePair<int, ByteSize>(h, default)).
                                 Select(DetailsHelper.GetTimeChartEntry);
        }

        public LineChart DailyChart { get; set; }

        protected override void SetGroupedDetails(IEnumerable<KeyValuePair<DateTime, ByteSize>> source)
        {
            DateTime now = DateTime.Now;
            DailyChart.Entries = source.
                                 Supplement(1, p => p.Key.Day, d => new KeyValuePair<DateTime, ByteSize>(new DateTime(now.Year, now.Month, d), default)).
                                 GetTotalDetails().
                                 Select(DetailsHelper.GetDailyChartEntry);
        }
    }

    static class DetailsHelper
    {
        public static IEnumerable<KeyValuePair<int, ByteSize>> GetTotalDetails(this IEnumerable<KeyValuePair<DateTime, ByteSize>> source)
        {
            ByteSize total = default;
            foreach (var p in source)
            {
                total += p.Value;
                yield return new KeyValuePair<int, ByteSize>(p.Key.Day, total);
            }
        }

        public static ChartEntry GetDailyChartEntry(KeyValuePair<int, ByteSize> p) => new ChartEntry((float)p.Value.GigaBytes)
        {
            Label = p.Key.ToString(),
            ValueLabel = " ",
            Color = App.SystemAccentColor.ToSKColor()
        };

        public static ChartEntry GetTimeChartEntry(KeyValuePair<int, ByteSize> p) => new ChartEntry((float)p.Value.GigaBytes)
        {
            Label = $"{p.Key * 6} ~ {(p.Key + 1) * 6 - 1} h",
            ValueLabel = p.Value.ToString(),
            Color = App.SystemAccentColor.ToSKColor()
        };
    }
}
