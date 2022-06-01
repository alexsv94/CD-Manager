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

        public static Image Act => ResourceHelper.GetImageFromResource(_rootPath + "act.png");
        public static Image AddFile => ResourceHelper.GetImageFromResource(_rootPath + "addFile.png");
        public static Image ChangeVersion => ResourceHelper.GetImageFromResource(_rootPath + "changeVersion.png");
        public static Image Delete => ResourceHelper.GetImageFromResource(_rootPath + "delete.png");
        public static Image Edit => ResourceHelper.GetImageFromResource(_rootPath + "edit.png");
        public static Image Exit => ResourceHelper.GetImageFromResource(_rootPath + "exit.png");
        public static Image List => ResourceHelper.GetImageFromResource(_rootPath + "list.png");
        public static Image Notice => ResourceHelper.GetImageFromResource(_rootPath + "notice.png");
        public static Image Rename => ResourceHelper.GetImageFromResource(_rootPath + "rename.png");
        public static Image Settings => ResourceHelper.GetImageFromResource(_rootPath + "settings.png");
        public static Image VersionHistory => ResourceHelper.GetImageFromResource(_rootPath + "versionHistory.png");
    }
}