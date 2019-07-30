using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace TsinghuaNet.XF.Models
{
    public static class CredentialStore
    {
        private const string StoredUsernameKey = "Username";

        public static async Task<(string Username, string Password)> LoadCredentialAsync()
        {
            string un = Preferences.Get(StoredUsernameKey, string.Empty);
            try
            {
                return (un, await SecureStorage.GetAsync(un));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            return (un, string.Empty);
        }

        public static async Task SaveCredentialAsync((string Username, string Password) credential)
        {
            Preferences.Set(StoredUsernameKey, credential.Username);
            try
            {
                await SecureStorage.SetAsync(credential.Username, credential.Password);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
    }
}
