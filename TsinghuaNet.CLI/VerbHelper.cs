using System;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TsinghuaNet.Helpers;
using TsinghuaNet.Models;

namespace TsinghuaNet.CLI
{
    static class VerbHelper
    {
        public static NetPingStatus Status = new NetPingStatus();

        public async static Task<IConnect> GetHelperAsync(this NetVerbBase opts)
        {
            if (opts.Host == OptionNetState.Auto)
                opts.Host = (OptionNetState)await Status.SuggestAsync();
            var cred = Credential;
            cred.State = (NetState)opts.Host;
            return cred.GetHelper();
        }

        public static UseregHelper GetUseregHelper(this VerbBase opts)
        {
            var cred = Credential;
            return cred.GetUseregHelper();
        }

        private static string ReadPassword()
        {
            StringBuilder builder = new StringBuilder();
            do
            {
                var c = Console.ReadKey(true);
                if (c.Key == ConsoleKey.Enter)
                    break;
                switch (c.Key)
                {
                    case ConsoleKey.Backspace:
                        builder.Remove(builder.Length - 1, 1);
                        break;
                    default:
                        builder.Append(c.KeyChar);
                        break;
                }
            }
            while (true);
            Console.WriteLine();
            return builder.ToString();
        }

        public static NetCredential ReadCredential()
        {
            Console.Write("请输入用户名：");
            var u = Console.ReadLine();
            Console.Write("请输入密码：");
            var p = ReadPassword();
            return new NetCredential() { Username = u, Password = p };
        }

        private class CredentialSettings
        {
            [JsonPropertyName("username")]
            public string Username { get; set; }
            [JsonPropertyName("password")]
            public string Password { get; set; }
        }

        public static NetCredential Credential
        {
            get
            {
                CredentialSettings settings = SettingsHelper.Helper.ReadSettings<CredentialSettings>();
                if (settings != null)
                {
                    NetCredential cred = new NetCredential
                    {
                        Username = settings.Username,
                        Password = Encoding.UTF8.GetString(Convert.FromBase64String(settings.Password))
                    };
                    return cred;
                }
                else
                {
                    return ReadCredential();
                }
            }
            set
            {
                CredentialSettings settings = new CredentialSettings()
                {
                    Username = value.Username ?? string.Empty,
                    Password = Convert.ToBase64String(Encoding.UTF8.GetBytes(value.Password ?? string.Empty))
                };
                SettingsHelper.Helper.WriteSettings(settings);
            }
        }
    }

    static class SettingsHelper
    {
        private const string SettingsFilename = "settings.json";
        private const string ProjectName = "TsinghuaNet.CLI";

        public readonly static SettingsFileHelper Helper = new SettingsFileHelper(ProjectName, SettingsFilename);
    }
}
