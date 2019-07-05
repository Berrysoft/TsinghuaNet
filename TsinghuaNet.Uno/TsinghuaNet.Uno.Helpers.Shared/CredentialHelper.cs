using System.Linq;
using Windows.Security.Credentials;

namespace TsinghuaNet.Uno.Helpers
{
    public static class CredentialHelper
    {
        private const string CredentialResource = "TsinghuaNetUWP";
        private static readonly PasswordVault vault = new PasswordVault();

        public static string GetCredential(string username)
        {
            foreach (var cre in from c in vault.RetrieveAll()
                                where c.Resource == CredentialResource && c.UserName == username
                                select c)
            {
                cre.RetrievePassword();
                return cre.Password;
            }
            return null;
        }

        public static void SaveCredential(string username, string password)
        {
            vault.Add(new PasswordCredential(CredentialResource, username, password));
        }

        public static void RemoveCredential(string username)
        {
            foreach (var cre in from c in vault.RetrieveAll()
                                where c.Resource == CredentialResource && c.UserName == username
                                select c)
            {
                vault.Remove(cre);
            }
        }
    }
}
