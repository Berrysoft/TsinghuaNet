using System.Threading.Tasks;

namespace TsinghuaNet.XF.Services
{
    public interface IBackgroundManager
    {
        Task<bool> RequestAccessAsync();
        void RegisterLiveTile(bool reg);
        void RegisterLogin(bool reg);
    }
}
