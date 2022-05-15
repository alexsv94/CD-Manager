using OrganizerWpf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace OrganizerWpf.UserControls.DocumentsExplorer
{
    public class IconTemplateSelector : DataTemplateSelector
    {              
        public DataTemplate DOCTemplate { get; set; }
        public DataTemplate CDRTemplate { get; set; }
        public DataTemplate CDWTemplate { get; set; }
        public DataTemplate DCHTemplate { get; set; }
        public DataTemplate DWGTemplate { get; set; }
        public DataTemplate PDFTemplate { get; set; }
        public DataTemplate XLSTemplate { get; set; }
        public DataTemplate ZIPTemplate { get; set; }
        public DataTemplate TXTTemplate { get; set; }
        public DataTemplate DefaultTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var doc = item as IFileInfo;

            if (doc == null)
                return base.SelectTemplate(item, container);

            return doc.Extension switch
            {
                ".doc" or ".docx" => DOCTemplate,
                ".cdr" => CDRTemplate,
                ".cdw" => CDWTemplate,
                ".dch" => DCHTemplate,
                ".dwg" => DWGTemplate,
                ".pdf" => PDFTemplate,
                ".xls" or ".xlsx" => XLSTemplate,
                ".zip" => ZIPTemplate,
                ".txt" => TXTTemplate,
                _ => DefaultTemplate,
            };
        }

    }
}
