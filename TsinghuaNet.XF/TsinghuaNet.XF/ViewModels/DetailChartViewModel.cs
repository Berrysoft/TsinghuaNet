using System;
using System.Collections.Generic;
using System.Linq;
using Microcharts;
using MvvmHelpers;
using PropertyChanged;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using TsinghuaNet.Models;
using TsinghuaNet.ViewModels;

namespace TsinghuaNet.XF.ViewModels
{
    class DetailChartViewModel : DetailViewModel
    {
        public DetailChartViewModel() : base()
        {
            Chart = new LineChart
            {
                Entries = ChartEntries,
                BackgroundColor = SKColors.Transparent,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
                IsAnimated = true
            };
        }

        [DoNotNotify]
        public ObservableRangeCollection<ChartEntry> ChartEntries { get; set; } = new ObservableRangeCollection<ChartEntry>();

        public LineChart Chart { get; set; }

        public override void SetGroupedDetails(IEnumerable<KeyValuePair<DateTime, ByteSize>> source)
        {
            ChartEntries.ReplaceRange(source.GetTotalDetails().Select(p => new ChartEntry((float)p.Value.GigaBytes)
            {
                Label = p.Key.Day.ToString(),
                ValueLabel = p.Value.ToString(),
                Color = App.SystemAccentColor.ToSKColor()
            }));
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
    }
}
