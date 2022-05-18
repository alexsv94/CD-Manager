using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;
using OrganizerWpf.Models;
using OrganizerWpf.Dialogs.RenameDialog;
using OrganizerWpf.Dialogs.ChangeVersionDialog;
using System;
using OrganizerWpf.Utilities;

namespace OrganizerWpf.UserControls.DocumentsExplorer
{
    /// <summary>
    /// Логика взаимодействия для DocumentsExplorer.xaml
    /// </summary>
    public partial class DocumentsExplorer : UserControl
    {  
        #region Dependency Props       
        public List<DocumentInfo> Documents
        {
            get { return (List<DocumentInfo>)GetValue(DocumentsProperty); }
            set { SetValue(DocumentsProperty, value); }
        }        
        public static readonly DependencyProperty DocumentsProperty =
            DependencyProperty.Register("Documents", typeof(List<DocumentInfo>), typeof(DocumentsExplorer), new PropertyMetadata(null));

        public string TargetDirectory
        {
            get { return (string)GetValue(TargetDirectoryProperty); }
            set { SetValue(TargetDirectoryProperty, value); }
        }
        public static readonly DependencyProperty TargetDirectoryProperty =
            DependencyProperty.Register("TargetDirectory", typeof(string), typeof(DocumentsExplorer), new PropertyMetadata(""));
        #endregion

        //public List<DocumentInfo> Documents { get; set; } = new();

        private string _directoryPath = string.Empty;
        private bool _initCompleted;
        private FilesystemHelper? _fsHelper;

        public DocumentsExplorer()
        {
            InitializeComponent();
            dataGrid.DataContext = this;

            Settings.CurrentProductDirectoryChanged += OnDirectoryChanged;
        }

        private void OnDirectoryChanged(string newDir)
        {
            _directoryPath = Path.Combine(newDir, TargetDirectory);
            UpdateFileList();
        }

        private void UpdateFileList()
        {
            if (!_initCompleted)
                Initialize();

            Documents = _fsHelper!.GetDocuments(_directoryPath);
        }

        private void Initialize()
        {
            _fsHelper = new FilesystemHelper();
            _initCompleted = true;           
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

            string[] droppedFiles = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (Documents != null && Documents.Any(x => droppedFiles[0] == x.FilePath))
                return;

            var filePaths = _fsHelper!.GetDroppedFiles(droppedFiles);
            _fsHelper.CopyFiles(_directoryPath, filePaths);

            UpdateFileList();
        }

        private void dataGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.MouseDevice.LeftButton == MouseButtonState.Pressed && dataGrid.SelectedItem != null)
            {
                DocumentInfo itemInfo = (DocumentInfo)dataGrid.SelectedItem;

                IDataObject dragObject = new DataObject(DataFormats.FileDrop, new string[] { itemInfo.FilePath! });
                DragDrop.DoDragDrop(dataGrid, dragObject, DragDropEffects.Copy);
            }
        }

        private void MenuItem_Delete_Click(object sender, RoutedEventArgs e)
        {
            DocumentInfo selectedFile = (DocumentInfo)dataGrid.SelectedItem;

            if (MessageBox.Show($"Удалить файл {selectedFile.DocName} без возможности восстановления?",
                    "Удаление файла",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning,
                    MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                File.Delete(selectedFile.FilePath!);
            }

            UpdateFileList();
        }        

        private void MenuItem_Rename_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem == null) return;

            DocumentInfo selectedFile = (DocumentInfo)dataGrid.SelectedItem;
            RenameDialog renameDialog = new RenameDialog();

            renameDialog.OldFileName = renameDialog.NewFileName = selectedFile.DocName!;

            bool? result = renameDialog.ShowDialog();            
            
            if ((bool)result!)
            {
                selectedFile.DocName = renameDialog.NewFileName;
                selectedFile.FilePath = _fsHelper!.RenameFile(selectedFile.FilePath!, selectedFile.DocName);

                UpdateFileList();
            }
        }

        private void MenuItem_ChangeVersion_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem == null) return;

            DocumentInfo selectedFile = (DocumentInfo)dataGrid.SelectedItem;
            ChangeVersionDialog versionDialog = new();

            versionDialog.OldVersion = versionDialog.NewVersion = selectedFile.Version!;

            bool? result = versionDialog.ShowDialog();

            if ((bool)result!)
            {
                selectedFile.Version = versionDialog.NewVersion;
                _fsHelper!.SetFileMetadata(selectedFile);

                UpdateFileList();
            }
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                _fsHelper!.OpenFile(((DocumentInfo)dataGrid.SelectedItem).FilePath!);
            }
        }

    }
}
