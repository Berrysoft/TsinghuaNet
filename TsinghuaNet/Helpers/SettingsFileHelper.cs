using System;
using System.IO;
using Newtonsoft.Json.Linq;

namespace TsinghuaNet.Helpers
{
    public static class SettingsFileHelper
    {
        private const string confName = ".config";

        public static string GetSettingsPath(string projName, string filename)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), confName, projName, filename);
        }

        public static void CreateSettingsFolder(string projName)
        {
            DirectoryInfo home = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
            home.CreateSubdirectory(Path.Combine(confName, projName));
        }

        public static bool DeleteSettings(string projName, string filename)
        {
            var p = GetSettingsPath(projName, filename);
            if (File.Exists(p))
            {
                File.Delete(p);
                return true;
            }
            return false;
        }

        public static JToken GetSettings(JObject json, string key, JToken def)
        {
            if (json.ContainsKey(key))
                return json[key];
            else
                return def;
        }
    }
}
