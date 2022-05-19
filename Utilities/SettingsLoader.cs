using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;
using System.Windows;

namespace OrganizerWpf.Utilities
{
    [Serializable]
    public class SettingsLoader
    {
        public string WorkingDirectoryPath { get; set; } = string.Empty;
        public string CurrentProductDirectoryPath { get; set; } = string.Empty;
        public string EmployeeName { get; set; } = string.Empty;
        public bool NeedToSpecifyVersionAfterCopy { get; set; }
        
        public static SettingsLoader Load()
        {
            if (!File.Exists("Settings.json")) return new();
            
            string jsonString = File.ReadAllText("Settings.json");
            var settings = JsonConvert.DeserializeObject<SettingsLoader>(jsonString);
            
            if (settings == null) return new();
            
            settings.UpdateSettings();
            return settings;
        }
        public void ReadValuesFromSettings()
        {
            foreach (PropertyInfo property in typeof(Settings).GetProperties(BindingFlags.Static | BindingFlags.Public))
            {
                GetType().GetProperty(property.Name)!
                        .SetValue(this, property.GetValue(null));
            }
        }
        public void UpdateSettings()
        {
            Type settings = typeof(Settings);

            foreach (PropertyInfo property in GetType().GetProperties())
            {
                settings.GetProperty(property.Name, BindingFlags.Static | BindingFlags.Public)!
                        .SetValue(null, property.GetValue(this));
            }
        }

        public void Save()
        {
            if (File.Exists("Settings.json"))
            {
                File.Delete("Settings.json");
            }

            File.WriteAllText("Settings.json", JsonConvert.SerializeObject(this));
            UpdateSettings();
        }
    }
}
