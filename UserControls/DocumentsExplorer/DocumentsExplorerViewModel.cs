using OrganizerWpf.Commands;
using OrganizerWpf.Dialogs.ChangeVersionDialog;
using OrganizerWpf.Dialogs.RenameDialog;
using OrganizerWpf.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OrganizerWpf.UserControls.DocumentsExplorer
{
    public class DocumentsExplorerViewModel
    {
        
        public string DirectoryPath { get; set; } = string.Empty;

        public DocumentInfo SelectedDocument { get; set; }
        public List<DocumentInfo> Documents { get; set; }

        #region Commands
        private RelayCommand? _deleteDocumentCommand = null;
        public RelayCommand DeleteDocumentCommand
        {
            get => _deleteDocumentCommand ??= new RelayCommand(obj => DeleteDocument());
        }

        private RelayCommand? _renameDocumentCommand = null;
        public RelayCommand RenameDocumentCommand
        {
            get => _renameDocumentCommand ??= new RelayCommand(obj => RenameDocument());
        }

        private RelayCommand? _changeDocumentVersionCommand = null;
        public RelayCommand ChangeDocumentVersionCommand
        {
            get => _changeDocumentVersionCommand ??= new RelayCommand(obj => ChangeDocumentVersion());
        }        
        #endregion

        private bool _initCompleted;
        private FilesystemHelper? _fsHelper;


        public void UpdateFileList(string newDir)
        {
            if (!_initCompleted)
                Initialize();

            DirectoryPath = newDir;
            Documents = _fsHelper!.GetDocuments(newDir);
        }

        private void Initialize()
        {
            Documents = new();
            _fsHelper = new FilesystemHelper();
            _initCompleted = true;
        }

        public void OnDrop(DragEventArgs e)
        {
            string[] droppedFiles = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (Documents != null && Documents.Any(x => droppedFiles[0] == x.FilePath))
                return;

            var filePaths = _fsHelper!.GetDroppedFiles(droppedFiles);
            _fsHelper.CopyFiles(DirectoryPath, filePaths);

            UpdateFileList(DirectoryPath);
        }

        public void OnMouseMove(DependencyObject dragSource, MouseEventArgs e)
        {
            if (e.MouseDevice.LeftButton == MouseButtonState.Pressed && SelectedDocument != null)
            {                
                IDataObject dragObject = new DataObject(DataFormats.FileDrop, new string[] { SelectedDocument!.FilePath! });
                DragDrop.DoDragDrop(dragSource, dragObject, DragDropEffects.Copy);
            }
        }

        public void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            _fsHelper!.OpenFile(SelectedDocument!.FilePath!);
        }

        public void RenameDocument()
        {
            RenameDialog renameDialog = new RenameDialog();

            renameDialog.OldFileName = renameDialog.NewFileName = SelectedDocument!.DocName!;

            bool? result = renameDialog.ShowDialog();

            if ((bool)result!)
            {
                SelectedDocument!.DocName = renameDialog.NewFileName;
                SelectedDocument!.FilePath = _fsHelper!.RenameFile(SelectedDocument!.FilePath!, SelectedDocument!.DocName);

                UpdateFileList(DirectoryPath);
            }
        }

        public void DeleteDocument()
        { 
            if (MessageBox.Show($"Удалить файл {SelectedDocument!.DocName} без возможности восстановления?",
                    "Удаление файла",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning,
                    MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                File.Delete(SelectedDocument!.FilePath!);
            }

            UpdateFileList(DirectoryPath);
        }

        private void ChangeDocumentVersion()
        {
            ChangeVersionDialog versionDialog = new();

            versionDialog.OldVersion = versionDialog.NewVersion = SelectedDocument!.Version!;

            bool? result = versionDialog.ShowDialog();

            if ((bool)result!)
            {
                SelectedDocument!.Version = versionDialog.NewVersion;
                _fsHelper!.SetFileMetadata(SelectedDocument!);

                UpdateFileList(DirectoryPath);
            }
        }
    }
}
