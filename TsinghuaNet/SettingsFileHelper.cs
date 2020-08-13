using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace TsinghuaNet
{
    public class SettingsFileHelper
    {
        private readonly static string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        public string FileFolderName { get; }
        public string FilePath { get; }

        private static string GetSettingsPath(string projName, string filename)
        {
            return Path.Combine(appDataPath, projName, filename);
        }

        private static void CreateSettingsFolder(string projName)
        {
            DirectoryInfo home = new DirectoryInfo(appDataPath);
            home.CreateSubdirectory(projName);
        }

        public SettingsFileHelper(string projName, string filename)
        {
            FileFolderName = projName;
            FilePath = GetSettingsPath(projName, filename);
        }

        public async ValueTask<T> ReadSettingsAsync<T>()
            where T : class
        {
            if (File.Exists(FilePath))
            {
                try
                {
                    using (var stream = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
                    {
                        return await JsonSerializer.DeserializeAsync<T>(stream);
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }
            return null;
        }

        public T ReadSettings<T>()
            where T : class
        {
            if (File.Exists(FilePath))
            {
                try
                {
                    return JsonSerializer.Deserialize<T>(File.ReadAllBytes(FilePath));
                }
                catch (Exception)
                {
                    return null;
                }
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
