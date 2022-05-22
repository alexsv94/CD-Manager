using OrganizerWpf.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OrganizerWpf.Icons
{
    public class IconSelector
    {        
        public static MenuItems MenuItems { get; set; } = new MenuItems();
    }

    public class MenuItems
    {
        private const string _rootPath = "Icons\\MenuItems\\";

        public static Image ChangeVersion => ResourceHelper.GetImageFromResource(_rootPath + "changeVersion.png");
        public static Image Delete => ResourceHelper.GetImageFromResource(_rootPath + "delete.png");
        public static Image Edit => ResourceHelper.GetImageFromResource(_rootPath + "edit.png");
        public static Image Exit => ResourceHelper.GetImageFromResource(_rootPath + "exit.png");
        public static Image Rename => ResourceHelper.GetImageFromResource(_rootPath + "rename.png");
        public static Image Settings => ResourceHelper.GetImageFromResource(_rootPath + "settings.png");
        public static Image VersionHistory => ResourceHelper.GetImageFromResource(_rootPath + "versionHistory.png");
    }
}
