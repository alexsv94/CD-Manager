using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OrganizerWpf.UserControls.DocumentsExplorer
{
    public partial class DocumentsExplorer : UserControl
    {         
        public string TargetDirectory { get; set; } = string.Empty;
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

        private void MenuItem_СhangeVersion_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel!.SetMenuItemVisibility(sender);
        }
    }
}
