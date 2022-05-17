using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.IO;
using OrganizerWpf.Models;
using OrganizerWpf.Dialogs.RenameDialog;
using OrganizerWpf.Dialogs.ChangeVersionDialog;

namespace OrganizerWpf.UserControls.DocumentsExplorer
{
    /// <summary>
    /// Логика взаимодействия для DocumentsExplorer.xaml
    /// </summary>
    public partial class DocumentsExplorer : UserControl
    {
        public DocumentsExplorerViewModel? ViewModel { get; private set; }

        public DocumentsExplorer()
        {            
            InitializeComponent();
            ViewModel = new DocumentsExplorerViewModel();
            DataContext = ViewModel;
            dataGrid.ItemsSource = ViewModel.Documents;
            MessageBox.Show(dataGrid.ItemsSource.ToString());
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
            ViewModel!.OnDrop(e);
        }

        private void dataGrid_MouseMove(object sender, MouseEventArgs e)
        {
            ViewModel!.OnMouseMove(dataGrid, e);
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ViewModel!.OnMouseDoubleClick(e);
        }

    }
}
