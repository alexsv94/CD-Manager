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
        public NoticeExplorerViewModel? ViewModel { get; private set; } = null;

        public NoticeExplorer()
        {
            InitializeComponent();
            //Loaded += SetupViewModel;
            if (ViewModel == null)
            {
                ViewModel = new();
                ViewModel.UI_DropLabel = dropLabel;

                rootContainer.DragEnter += ViewModel.OnContainerDragEnter;
                rootContainer.DragLeave += ViewModel.OnContainerDragLeave;
                rootContainer.Drop += ViewModel.OnContainerDrop;

                dataGrid.MouseMove += ViewModel.OnDataGridRowMouseMove;
            }

            DataContext = ViewModel;
        }

        private void SetupViewModel(object sender, RoutedEventArgs e)
        {
            if (ViewModel == null)
            {
                ViewModel = new();
                ViewModel.UI_DropLabel = dropLabel;

                rootContainer.DragEnter += ViewModel.OnContainerDragEnter;
                rootContainer.DragLeave += ViewModel.OnContainerDragLeave;
                rootContainer.Drop += ViewModel.OnContainerDrop;

                dataGrid.MouseMove += ViewModel.OnDataGridRowMouseMove;
            }

            DataContext = ViewModel;
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ViewModel!.OnDataGridRowDoubleClick(e);
        }
    }
}
