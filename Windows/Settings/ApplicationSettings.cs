using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizerWpf.Windows.Settings
{
    public static class ApplicationSettings
    {
        private static string _wkDirPath = $"C:\\Users\\{Environment.UserName}\\Desktop\\TestFolder";
        public static string WorkingDirectoryPath {
            get => _wkDirPath;
            set
            { 
                _wkDirPath = value;
                WorkingDirectoryChanged?.Invoke(_wkDirPath);
            }
        }
        public static string CurrentProductDirectoryPath { get; set; } = string.Empty;

        public static string EmployeeName { get; set; } = string.Empty;
        public static bool NeedToSpecifyVersionAfterCopy { get; set; } = false;

        #region Events
        public static event Action<string>? WorkingDirectoryChanged;

        #endregion
    }
}
