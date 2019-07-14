using System.Collections.Generic;
using System.Linq;
using TsinghuaNet.Models;
using TsinghuaNet.XF.Services;
using TsinghuaNet.XF.UWP.Services;
using Windows.Security.Credentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(CredentialStore))]

namespace TsinghuaNet.XF.UWP.Services
{
    public class CredentialStore : ICredentialStore
    {
        private const string CredentialResource = "TsinghuaNetUWP";
        private static readonly PasswordVault vault = new PasswordVault();
        private readonly IDictionary<string, object> values;

        public CredentialStore()
        {
            values = Application.Current.Properties;
        }

        private const string StoredUsernameKey = "Username";

        public bool CredentialExists() => values.ContainsKey(StoredUsernameKey);

        public void LoadCredential(NetCredential credential)
        {
            string un = (string)values[StoredUsernameKey];
            credential.Username = un;
            var cre = vault.RetrieveAll().Where(c => c.Resource == CredentialResource && c.UserName == un).FirstOrDefault();
            if (cre != null)
            {
                cre.RetrievePassword();
                credential.Password = cre.Password;
            }
        }

        public void SaveCredential(NetCredential credential)
        {
            vault.Add(new PasswordCredential(CredentialResource, credential.Username, credential.Password));
        }
    }
}
