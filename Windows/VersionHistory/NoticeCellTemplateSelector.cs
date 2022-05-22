using OrganizerWpf.Models;
using System.Windows;
using System.Windows.Controls;

namespace OrganizerWpf.Windows.VersionHistory
{
    public class NoticeCellTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? DOCTemplate { get; set; }
        public DataTemplate? EmptyTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var doc = item as VersionModel;

            if (doc?.NoticeFile == null)
                return EmptyTemplate;

            return DOCTemplate;
        }
    }
}
