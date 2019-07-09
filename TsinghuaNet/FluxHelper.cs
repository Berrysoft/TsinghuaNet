using System;
using TsinghuaNet.Models;

namespace TsinghuaNet
{
    public static class FluxHelper
    {
        public static readonly ByteSize BaseFlux = ByteSize.FromGigaBytes(25);

        public static ByteSize GetMaxFlux(ByteSize flux, decimal balance)
        => ByteSize.FromBytes(Math.Max(flux.Bytes, BaseFlux.Bytes)) + ByteSize.FromGigaBytes((double)balance / 2);
    }
}
