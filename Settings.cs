using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizerWpf
{
    public static class Settings
    {
        public static string WorkingDirectoryPath { get; set; } = $"C:\\Users\\{Environment.UserName}\\Desktop\\TestFolder";
        public static string CurrentProductDirectoryPath { get; set; } = string.Empty;

        public static string EmployeeName { get; set; } = string.Empty;
        public static bool NeedToSpecifyVersionAfterCopy { get; set; } = false;
    }
}
