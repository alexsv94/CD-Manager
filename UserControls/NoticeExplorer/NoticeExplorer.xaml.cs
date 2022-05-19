using OrganizerWpf.Models;
using OrganizerWpf.UserControls.DocumentsExplorer;
using OrganizerWpf.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;

namespace OrganizerWpf.UserControls.NoticeExplorer
{
    public partial class NoticeExplorer : UserControl
    {
        public string TargetDirectory { get; set; } = string.Empty;
        public NoticeExplorerViewModel? ViewModel { get; private set; } = null;

        public NoticeExplorer()
        {
            InitializeComponent();
            Loaded += SetupViewModel;
        }

        private void SetupViewModel(object sender, RoutedEventArgs e)
        {
            if (ViewModel == null)
            {
                ViewModel = new(TargetDirectory);
                ViewModel.UI_DropLabel = dropLabel;

                rootContainer.DragEnter += ViewModel.OnDragEnter;
                rootContainer.DragLeave += ViewModel.OnDragLeave;
                rootContainer.Drop += ViewModel.OnDrop;

                dataGrid.MouseMove += ViewModel.OnMouseMove;
            }

            DataContext = ViewModel;
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ViewModel!.OnMouseDoubleClick();
        }
    }
}
