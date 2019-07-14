using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using TsinghuaNet.Models;
using Xamarin.Essentials;

namespace TsinghuaNet.XF.Models
{
    public class CredentialStore
    {
        private const string StoredUsernameKey = "Username";

        public bool CredentialExists() => Preferences.ContainsKey(StoredUsernameKey);

        public async Task LoadCredentialAsync(NetCredential credential)
        {
            string un = Preferences.Get(StoredUsernameKey, string.Empty);
            credential.Username = un;
            try
            {
                credential.Password = await SecureStorage.GetAsync(un);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        public async Task SaveCredentialAsync(NetCredential credential)
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
