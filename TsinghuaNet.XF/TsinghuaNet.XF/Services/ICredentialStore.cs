using TsinghuaNet.Models;

namespace TsinghuaNet.XF.Services
{
    public interface ICredentialStore
    {
        bool CredentialExists();
        void SaveCredential(NetCredential credential);
        NetCredential LoadCredential();
    }
}
