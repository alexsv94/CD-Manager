using System;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace OrganizerWpf.Utilities
{
    public static class ResourceHelper
    {
        public static Image GetImageFromResource(string pathInApplication)
        {
            var assembly = Assembly.GetCallingAssembly();

            if (pathInApplication[0] == '/')
            {
                pathInApplication = pathInApplication[1..];
            }

            return new Image()
            {
                Source = new BitmapImage(new Uri(@"pack://application:,,,/" + assembly.GetName().Name + ";component/" + pathInApplication, UriKind.Absolute))
            };
        }
    }
}
