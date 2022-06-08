using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OrganizerWpf.UserControls.DocumentsExplorer
{
    public partial class DocumentsExplorer : UserControl
    {
        public DocumentsExplorerViewModel? ViewModel { get; private set; } = null;

        public DocumentsExplorer()
        {
            InitializeComponent();
            Loaded += SetupViewModel;
        }

        private void SetupViewModel(object sender, RoutedEventArgs e)
        {
            if (ViewModel == null)
            {
                ViewModel = new()
                {
                    UI_DropLabel = dropLabel,
                    UI_AdressPanel = adressPanel
                };

                rootContainer.DragEnter += ViewModel.OnContainerDragEnter;
                rootContainer.DragLeave += ViewModel.OnContainerDragLeave;
                rootContainer.Drop += ViewModel.OnContainerDrop;

                rootDirectoryLink.MouseUp += (object sender, MouseButtonEventArgs e) => ViewModel.GoToRootDirectoty();
            }
                
            DataContext = ViewModel;
        }
        
        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ViewModel!.OnDataGridRowDoubleClick(e);
        }

        private void MenuItem_СhangeVersion_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel!.SetMenuItemVisibility(sender);
        }

        private void DataGridRow_MouseMove(object sender, MouseEventArgs e)
        {
            ViewModel!.OnDataGridRowMouseMove(sender, e);
        }

        private void DataGridRow_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            ViewModel!.SetContextMenuVisibility(sender);
        }

        private void DataGridRow_Drop(object sender, DragEventArgs e)
        {
            ViewModel!.OnDatagridRowDrop(sender, e);
        }
    }
}
