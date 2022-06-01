using OrganizerWpf.Models;
using OrganizerWpf.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OrganizerWpf.Dialogs.RecentDocumentsDialog
{
    /// <summary>
    /// Логика взаимодействия для RecentDocumentsDialog.xaml
    /// </summary>
    public partial class RecentDocumentsDialog : Window
    {
        public List<DocumentModel> RecentDocs
        {
            get { return (List<DocumentModel>)GetValue(RecentDocsProperty); }
            set { SetValue(RecentDocsProperty, value); }
        }
        public static readonly DependencyProperty RecentDocsProperty =
            DependencyProperty.Register("RecentDocs", typeof(List<DocumentModel>), typeof(RecentDocumentsDialog), new PropertyMetadata(new List<DocumentModel>()));

        private WindowEventsHelper _eventsHelper;

        public RecentDocumentsDialog()
        {
            InitializeComponent();
            RecentDocs.Add(new DocumentModel() { Name = "TestDoc1", Version = new("ФИАШ.789456.123"), PreviousVersion = new("ФИАШ.789456.123.01") });
            RecentDocs.Add(new DocumentModel() { Name = "TestDoc2", Version = new("ФИАШ.789456.123"), PreviousVersion = new("ФИАШ.789456.123.01") });
            RecentDocs.Add(new DocumentModel() { Name = "TestDoc3", Version = new("ФИАШ.789456.123"), PreviousVersion = new("ФИАШ.789456.123.01") });
            RecentDocs.Add(new DocumentModel() { Name = "TestDoc4", Version = new("ФИАШ.789456.123"), PreviousVersion = new("ФИАШ.789456.123.01") });
            RecentDocs.Add(new DocumentModel() { Name = "TestDoc1", Version = new("ФИАШ.789456.123"), PreviousVersion = new("ФИАШ.789456.123.01") });
            RecentDocs.Add(new DocumentModel() { Name = "TestDoc2", Version = new("ФИАШ.789456.123"), PreviousVersion = new("ФИАШ.789456.123.01") });
            RecentDocs.Add(new DocumentModel() { Name = "TestDoc3", Version = new("ФИАШ.789456.123"), PreviousVersion = new("ФИАШ.789456.123.01") });
            RecentDocs.Add(new DocumentModel() { Name = "TestDoc4", Version = new("ФИАШ.789456.123"), PreviousVersion = new("ФИАШ.789456.123.01") });
            RecentDocs.Add(new DocumentModel() { Name = "TestDoc1", Version = new("ФИАШ.789456.123"), PreviousVersion = new("ФИАШ.789456.123.01") });
            RecentDocs.Add(new DocumentModel() { Name = "TestDoc2", Version = new("ФИАШ.789456.123"), PreviousVersion = new("ФИАШ.789456.123.01") });
            RecentDocs.Add(new DocumentModel() { Name = "TestDoc3", Version = new("ФИАШ.789456.123"), PreviousVersion = new("ФИАШ.789456.123.01") });
            RecentDocs.Add(new DocumentModel() { Name = "TestDoc4", Version = new("ФИАШ.789456.123"), PreviousVersion = new("ФИАШ.789456.123.01") });
            RecentDocs.Add(new DocumentModel() { Name = "TestDoc1", Version = new("ФИАШ.789456.123"), PreviousVersion = new("ФИАШ.789456.123.01") });
            RecentDocs.Add(new DocumentModel() { Name = "TestDoc2", Version = new("ФИАШ.789456.123"), PreviousVersion = new("ФИАШ.789456.123.01") });
            RecentDocs.Add(new DocumentModel() { Name = "TestDoc3", Version = new("ФИАШ.789456.123"), PreviousVersion = new("ФИАШ.789456.123.01") });
            RecentDocs.Add(new DocumentModel() { Name = "TestDoc4", Version = new("ФИАШ.789456.123"), PreviousVersion = new("ФИАШ.789456.123.01") });
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _eventsHelper = new(this, false, false);
        }
    }
}
