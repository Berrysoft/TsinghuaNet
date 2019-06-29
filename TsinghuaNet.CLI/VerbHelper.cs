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
                if (File.Exists(SettingsHelper.SettingsPath))
                {
                    NetCredential cred = new NetCredential();
                    using (StreamReader stream = new StreamReader(SettingsHelper.SettingsPath))
                    using (JsonTextReader reader = new JsonTextReader(stream))
                    {
                        var json = JObject.Load(reader);
                        cred.Username = (string)SettingsHelper.GetSettings(json, "username", string.Empty);
                        cred.Password = Encoding.UTF8.GetString(Convert.FromBase64String((string)SettingsHelper.GetSettings(json, "password", string.Empty)));
                        return cred;
                    }
                }
                else
                    return ReadCredential();
            }
            set
            {
                JObject json = new JObject();
                json["username"] = value.Username ?? string.Empty;
                json["password"] = Convert.ToBase64String(Encoding.UTF8.GetBytes(value.Password ?? string.Empty));
                SettingsHelper.CreateSettingsFolder();
                using (StreamWriter stream = new StreamWriter(SettingsHelper.SettingsPath))
                using (JsonTextWriter writer = new JsonTextWriter(stream))
                {
                    json.WriteTo(writer);
                }
            }
        }
    }

    static class SettingsHelper
    {
        private const string settingsFilename = "settings.json";
        public static string SettingsPath
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".config", "TsinghuaNet.CLI", settingsFilename);
            }
        }

        public static JToken GetSettings(JObject json, string key, JToken def)
        {
            if (json.ContainsKey(key))
                return json[key];
            else
                return def;
        }

        public static void CreateSettingsFolder()
        {
            DirectoryInfo home = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
            home.CreateSubdirectory(Path.Combine(".config", "TsinghuaNet.CLI"));
        }

        public static void DeleteSettings()
        {
            var p = SettingsPath;
            if (File.Exists(p))
            {
                Console.Write("是否要删除设置文件？[y/N]");
                var de = Console.ReadLine();
                if (string.Equals(de, "y", StringComparison.OrdinalIgnoreCase))
                {
                    File.Delete(p);
                    Console.WriteLine("已删除");
                }
            }
        }
    }
}
