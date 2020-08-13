using System;
using System.Text;
using System.Threading.Tasks;
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
            var settings = Settings;
            var cred = new NetCredential { Username = settings.Username, Password = settings.Password };
            cred.State = (NetState)opts.Host;
            cred.UseProxy = opts.UseProxy;
            return cred.GetHelper(settings);
        }

        public static IUsereg GetUseregHelper(this WebVerbBase opts)
        {
            var settings = Settings;
            var cred = new NetCredential { Username = settings.Username, Password = settings.Password };
            cred.UseProxy = opts.UseProxy;
            return cred.GetUseregHelper();
        }

        private static string ReadPassword()
        {
            StringBuilder builder = new StringBuilder();
            while (true)
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
            Console.WriteLine();
            return builder.ToString();
        }

        public static NetCLISettings ReadCredential()
        {
            Console.Write("请输入用户名：");
            var u = Console.ReadLine();
            Console.Write("请输入密码：");
            var p = ReadPassword();
            return new NetCLISettings() { Username = u, Password = p };
        }

        private static NetCLISettings settingsCache;

        public static NetCLISettings Settings
        {
            get => settingsCache ??= (SettingsHelper.Helper.ReadSettings<NetCLISettings>() ?? ReadCredential());
            set => SettingsHelper.Helper.WriteSettings(value);
        }

        public static void SaveSettings()
        {
            Settings = settingsCache;
        }
    }

    static class SettingsHelper
    {
        private const string SettingsFilename = "settings.json";
        private const string ProjectName = "TsinghuaNet.CLI";

        public readonly static SettingsFileHelper Helper = new SettingsFileHelper(ProjectName, SettingsFilename);
    }
}
