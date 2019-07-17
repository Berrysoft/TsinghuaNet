using System.Threading.Tasks;
using TsinghuaNet.XF.Droid.Services;
using TsinghuaNet.XF.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(BackgroundManager))]

namespace TsinghuaNet.XF.Droid.Services
{
    public class BackgroundManager : IBackgroundManager
    {
        public Task<bool> RequestAccessAsync() => Task.FromResult(false);

        public void RegisterLiveTile(bool reg) { }

        public void RegisterLogin(bool reg) { }
    }
}