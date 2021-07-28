using System;
using System.Collections.Generic;
using System.Linq;
using TsinghuaNet.Models;
using TsinghuaNet.ViewModels;

namespace TsinghuaNet.XF.ViewModels
{
    class DetailViewModel : DetailViewModelBase
    {
        public DetailViewModel() : base()
        {
        }

        public List<TimeChartData> TimeChart { get; set; }

        protected override void SetTimeDetails(IEnumerable<KeyValuePair<int, ByteSize>> source)
        {
            TimeChart = source.
                        GroupBy(p => p.Key / 6, p => p.Value).
                        OrderBy(g => g.Key).
                        Select(g => new KeyValuePair<int, ByteSize>(g.Key, g.Sum())).
                        Supplement(0, 3, p => p.Key, h => new KeyValuePair<int, ByteSize>(h, default)).
                        Select(p => new TimeChartData { Time = $"{p.Key * 6} ~ {(p.Key + 1) * 6 - 1}", Flux = p.Value.GigaBytes }).
                        ToList();
        }

        public List<DailyChartData> DailyChart { get; set; }

        protected override void SetGroupedDetails(IEnumerable<KeyValuePair<DateTime, ByteSize>> source)
        {
            DateTime now = DateTime.Now;
            DailyChart = source.
                         Supplement(1, p => p.Key.Day, d => new KeyValuePair<DateTime, ByteSize>(new DateTime(now.Year, now.Month, d), default)).
                         GetTotalDetails().
                         ToList();
        }
    }

    class TimeChartData
    {
        public string Time { get; set; }
        public double Flux { get; set; }
    }

    class DailyChartData
    {
        public DateTime Date { get; set; }
        public double Flux { get; set; }
    }

    static class DetailsHelper
    {
        public static IEnumerable<DailyChartData> GetTotalDetails(this IEnumerable<KeyValuePair<DateTime, ByteSize>> source)
        {
            ByteSize total = default;
            foreach (var p in source)
            {
                total += p.Value;
                yield return new DailyChartData { Date = p.Key, Flux = total.GigaBytes };
            }
        }
    }
}
