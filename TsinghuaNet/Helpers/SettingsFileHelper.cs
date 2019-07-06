﻿using System;
using System.Collections.Generic;
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

        public Dictionary<string, string> ReadDictionary()
        {
            if (File.Exists(FilePath))
            {
                var dict = new Dictionary<string, string>();
                var json = JsonDocument.Parse(File.ReadAllBytes(FilePath));
                foreach (var obj in json.RootElement.EnumerateObject())
                {
                    dict.Add(obj.Name, obj.Value.GetString());
                }
                return dict;
            }
            else
            {
                return null;
            }
        }

        public void WriteDictionary(Dictionary<string, string> dict)
        {
            using (var stream = new FileStream(FilePath, FileMode.CreateNew, FileAccess.Write))
            using (var json = new Utf8JsonWriter(stream))
            {
                json.WriteStartObject();
                foreach (var pair in dict)
                {
                    json.WriteString(pair.Key, pair.Value);
                }
                json.WriteEndObject();
                json.Flush();
            }
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
