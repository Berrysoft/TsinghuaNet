using System;
using System.Collections.Generic;
using System.Text;
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

        public static NetCredential Credential
        {
            get
            {
                var settings = SettingsHelper.Helper.ReadDictionary();
                if (settings != null)
                {
                    NetCredential cred = new NetCredential
                    {
                        Username = settings["username"],
                        Password = Encoding.UTF8.GetString(Convert.FromBase64String(settings["password"]))
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
                var settings = new Dictionary<string, string>()
                {
                    ["username"] = value.Username ?? string.Empty,
                    ["password"] = Convert.ToBase64String(Encoding.UTF8.GetBytes(value.Password ?? string.Empty))
                };
                SettingsHelper.Helper.WriteDictionary(settings);
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
