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

        public List<DocumentModel> ExcludeList = new();
        public List<DocumentModel> ChosenRecentDocs = new();

        private WindowEventsHelper _eventsHelper;

        public RecentDocumentsDialog()
        {
            InitializeComponent();
            RecentDocumentsStorage.Load();
        }

        private void FilterDocList()
        {
            RecentDocs.Clear();
            RecentDocs.AddRange(RecentDocumentsStorage.Instance.RecentDocuments);
            RecentDocs = RecentDocs.Where(x => !ExcludeList.Any(y => y.ShortName == x.ShortName)).ToList();
            ExcludeList.Clear();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _eventsHelper = new(this, false, false);
            FilterDocList();
        }

        private void ButtonApply_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in RecentDocsDataGrid.SelectedItems)
            {
                ChosenRecentDocs.Add((DocumentModel)item);
            }
            DialogResult = true;
            Close();
        }
    }
}
