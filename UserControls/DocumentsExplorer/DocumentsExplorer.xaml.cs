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
                ViewModel = new();
                ViewModel.UI_DropLabel = dropLabel;
                ViewModel.UI_AdressPanel = adressPanel;

                rootContainer.DragEnter += ViewModel.OnDragEnter;
                rootContainer.DragLeave += ViewModel.OnDragLeave;
                rootContainer.Drop += ViewModel.OnDrop;

                rootDirectoryLink.MouseUp += (object sender, MouseButtonEventArgs e) => ViewModel.GoToRootDirectoty();
            }
                
            DataContext = ViewModel;
        }
        
        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ViewModel!.OnMouseDoubleClick(e);
        }

        private void MenuItem_СhangeVersion_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel!.SetMenuItemVisibility(sender);
        }

        private void DataGridRow_MouseMove(object sender, MouseEventArgs e)
        {
            ViewModel!.OnMouseMove(sender, e);
        }

        private void DataGridRow_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            ViewModel!.SetContextMenuVisibility(sender);
        }
    }
}
