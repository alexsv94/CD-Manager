using Newtonsoft.Json;
using System;
using System.Reflection;

namespace OrganizerWpf.Utilities
{
    [Serializable]
    public class SettingsLoader
    {
        public string WorkingDirectoryPath { get; set; } = string.Empty;
        public string CurrentProductDirectoryPath { get; set; } = string.Empty;
        public string EmployeeName { get; set; } = string.Empty;
        public bool NeedToSpecifyVersionAfterCopy { get; set; }
        
        public void ReadValuesFromSettings()
        {
            foreach (PropertyInfo property in typeof(Settings).GetProperties(BindingFlags.Static | BindingFlags.Public))
            {
                GetType().GetProperty(property.Name)!
                        .SetValue(this, property.GetValue(this));
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

        public static SettingsLoader? FromJson(string jsonString)
        {
            return JsonConvert.DeserializeObject<SettingsLoader>(jsonString);
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
