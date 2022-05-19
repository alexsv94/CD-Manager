using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizerWpf.Utilities
{
    public static class Settings
    {
        private static string _workingDirectoryPath = string.Empty;            
        public static string WorkingDirectoryPath
        {
            get => _workingDirectoryPath;
            set
            {
                _workingDirectoryPath = value;
                WorkingDirectoryChanged?.Invoke(value);
            }
        }

        private static string _currentProductDir = string.Empty;
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
        public static event Action<string>? WorkingDirectoryChanged;
        #endregion
    }
}
