using System;

namespace TsinghuaNet.Uno.Contents
{
    public interface IUserContent
    {
        FluxUser User { get; set; }
        TimeSpan OnlineTime { get; set; }
        bool IsProgressActive { get; set; }
        void BeginAnimation();
        bool AddOneSecond();
    }

    static class UserContentHelper
    {
        public static double Max(double d1, double d2) => Math.Max(d1, d2);
    }
}
