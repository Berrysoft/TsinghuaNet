using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
                var path = SettingsFileHelper.GetSettingsPath(SettingsHelper.ProjectName, SettingsHelper.SettingsFilename);
                if (File.Exists(path))
                {
                    NetCredential cred = new NetCredential();
                    using (StreamReader stream = new StreamReader(path))
                    using (JsonTextReader reader = new JsonTextReader(stream))
                    {
                        var json = JObject.Load(reader);
                        cred.Username = (string)SettingsFileHelper.GetSettings(json, "username", string.Empty);
                        cred.Password = Encoding.UTF8.GetString(Convert.FromBase64String((string)SettingsFileHelper.GetSettings(json, "password", string.Empty)));
                        return cred;
                    }
                }
                else
                    return ReadCredential();
            }
            set
            {
                JObject json = new JObject
                {
                    ["username"] = value.Username ?? string.Empty,
                    ["password"] = Convert.ToBase64String(Encoding.UTF8.GetBytes(value.Password ?? string.Empty))
                };
                SettingsFileHelper.CreateSettingsFolder(SettingsHelper.ProjectName);
                using (StreamWriter stream = new StreamWriter(SettingsFileHelper.GetSettingsPath(SettingsHelper.ProjectName, SettingsHelper.SettingsFilename)))
                using (JsonTextWriter writer = new JsonTextWriter(stream))
                {
                    json.WriteTo(writer);
                }
            }
        }
    }

    static class SettingsHelper
    {
        public const string SettingsFilename = "settings.json";
        public const string ProjectName = "TsinghuaNet.CLI";
    }
}
