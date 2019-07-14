using TsinghuaNet.Models;

namespace TsinghuaNet.XF.Services
{
    public interface INetXFSettings : INetSettings
    {
        bool BackgroundAutoLogin { get; set; }
        bool BackgroundLiveTile { get; set; }
    }
}
