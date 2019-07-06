using System;
using System.IO;
using System.Text.Json;

namespace TsinghuaNet.Helpers
{
    public class SettingsFileHelper
    {
        private const string confName = ".config";

        public string FileFolderName { get; }
        public string FilePath { get; }

        private static string GetSettingsPath(string projName, string filename)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), confName, projName, filename);
        }

        private static void CreateSettingsFolder(string projName)
        {
            DirectoryInfo home = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
            home.CreateSubdirectory(Path.Combine(confName, projName));
        }

        public SettingsFileHelper(string projName, string filename)
        {
            FileFolderName = projName;
            FilePath = GetSettingsPath(projName, filename);
        }

        public T ReadSettings<T>()
            where T : class
        {
            if (File.Exists(FilePath))
            {
                byte[] json = File.ReadAllBytes(FilePath);
                return JsonSerializer.Deserialize<T>(json);
            }
            return null;
        }

        public void WriteSettings<T>(T settings)
            where T : class
        {
            CreateSettingsFolder(FileFolderName);
            File.WriteAllBytes(FilePath, JsonSerializer.SerializeToUtf8Bytes(settings));
        }

        public bool DeleteSettings()
        {
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
                return true;
            }
            return false;
        }
    }
}
