using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizerWpf.Utilities
{
    public static class Settings
    {
        public static string WorkingDirectoryPath { get; set; } = $"C:\\Users\\{Environment.UserName}\\Desktop\\TestFolder";
        
        private static string _currentProductDir = String.Empty;
        public static string CurrentProductDirectoryPath 
        { 
            get => _currentProductDir;
            set
            {
                _currentProductDir = value;
                CurrentProductDirectoryChanged?.Invoke(value);
            }
        }

        public static string EmployeeName { get; set; } = string.Empty;
        public static bool NeedToSpecifyVersionAfterCopy { get; set; } = false;

        #region Events
        public static event Action<string>? CurrentProductDirectoryChanged;
        #endregion
    }
}
