using OrganizerWpf.Models;
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

namespace OrganizerWpf.Windows.VersionHistory
{
    public partial class VersionHistoryWindow : Window, IView<VersionHistoryWindowViewModel>
    {
        public DocumentModel? Document { get; set; }
        public VersionHistoryWindowViewModel? ViewModel { get; set; }

        public VersionHistoryWindow()
        {
            InitializeComponent();
            Loaded += SetupViewModel;
        }

        public void SetupViewModel(object sender, RoutedEventArgs e)
        {
            if (ViewModel == null)
            {
                ViewModel = new(Document!);
            }

            DataContext = ViewModel;
        }
        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ViewModel!.OnMouseDoubleClick(e);
        }
    }
}
