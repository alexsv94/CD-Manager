using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OrganizerWpf.UserControls.DocumentsExplorer
{
    public partial class DocumentsExplorer : UserControl
    {         
        public string TargetDirectory { get; set; }

        private DocumentsExplorerViewModel _viewModel;

        public DocumentsExplorer()
        {
            InitializeComponent();
            Loaded += SetupViewModel;            
        }

        private void SetupViewModel(object sender, RoutedEventArgs e)
        {
            if (_viewModel == null)
                _viewModel = new DocumentsExplorerViewModel(TargetDirectory);
            DataContext = _viewModel;
        }

        private void Border_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
                dropLabel.Visibility = Visibility.Visible;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }            
        }

        private void Border_DragLeave(object sender, DragEventArgs e)
        {
            dropLabel.Visibility = Visibility.Collapsed;
        }

        private void Border_Drop(object sender, DragEventArgs e)
        {
            dropLabel.Visibility = Visibility.Collapsed;
            _viewModel.OnDrop(e);       
        }

        private void dataGrid_MouseMove(object sender, MouseEventArgs e)
        {
            _viewModel.OnMouseMove(dataGrid, e);
        }

        private void MenuItem_Delete_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.DeleteDocument();
        }        

        private void MenuItem_Rename_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.RenameDocument();
        }

        private void MenuItem_ChangeVersion_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.ChangeDocumentVersion();
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _viewModel.OnMouseDoubleClick();
        }
    }
}
