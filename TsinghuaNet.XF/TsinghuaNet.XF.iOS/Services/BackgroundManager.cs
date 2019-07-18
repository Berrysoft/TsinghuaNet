using System.Threading.Tasks;
using TsinghuaNet.XF.iOS.Services;
using TsinghuaNet.XF.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(BackgroundManager))]

namespace TsinghuaNet.XF.iOS.Services
{
    public class BackgroundManager : IBackgroundManager
    {
        public Task<bool> RequestAccessAsync() => Task.FromResult(false);

        public void RegisterLiveTile(bool reg) { }

        public void RegisterLogin(bool reg) { }
    }
}
