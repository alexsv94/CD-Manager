using OrganizerWpf.Dialogs.ChangeVersionDialog;
using OrganizerWpf.Dialogs.RenameDialog;
using OrganizerWpf.Models;
using OrganizerWpf.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OrganizerWpf.UserControls.DocumentsExplorer
{
    public class DocumentsExplorerViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private List<DocumentInfo> _documents;
        public List<DocumentInfo> Documents 
        {
            get => _documents;
            set
            {
                _documents = value;
                OnPropertyChanged(nameof(Documents));
            }
        }

        private DocumentInfo _selectedDocument;
        public DocumentInfo SelectedDocument
        {
            get => _selectedDocument;
            set
            {
                _selectedDocument = value;
                OnPropertyChanged(nameof(SelectedDocument));
            }
        }

        private readonly string _targetDirectory = string.Empty;
        private readonly FilesystemHelper? _fsHelper;
        private string _directoryPath = string.Empty;

        public DocumentsExplorerViewModel(string targetDir)
        {
            _fsHelper = new();
            _targetDirectory = targetDir;
            Settings.CurrentProductDirectoryChanged += OnDirectoryChanged;
        }
        private void UpdateFileList()
        {
            Documents = _fsHelper!.GetDocuments(_directoryPath);
        }

        private void OnDirectoryChanged(string newDir)
        {
            _directoryPath = Path.Combine(newDir, _targetDirectory);
            UpdateFileList();
        }

        public void OnDrop(DragEventArgs e)
        {
            string[] droppedFiles = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (Documents != null && Documents.Any(x => droppedFiles[0] == x.FilePath))
                return;

            var filePaths = _fsHelper!.GetDroppedFiles(droppedFiles);
            _fsHelper.CopyFiles(_directoryPath, filePaths);

            UpdateFileList();
        }

        public void OnMouseMove(DataGrid sender, MouseEventArgs e)
        {
            if (e.MouseDevice.LeftButton == MouseButtonState.Pressed)
            {
                IDataObject dragObject = new DataObject(DataFormats.FileDrop, new string[] { SelectedDocument!.FilePath! });
                DragDrop.DoDragDrop(sender, dragObject, DragDropEffects.Copy);
            }
        }

        public void OnMouseDoubleClick()
        {
            _fsHelper!.OpenFile(SelectedDocument!.FilePath!);
        }

        public void DeleteDocument()
        {
            if (SelectedDocument == null) return;

            if (MessageBox.Show($"Удалить файл {SelectedDocument.DocName} без возможности восстановления?",
                    "Удаление файла",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning,
                    MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                File.Delete(SelectedDocument.FilePath!);
            }

            UpdateFileList();
        }

        public void RenameDocument()
        {
            if (SelectedDocument == null) return;

            RenameDialog renameDialog = new RenameDialog();
            renameDialog.OldFileName = renameDialog.NewFileName = SelectedDocument.DocName!;

            bool? result = renameDialog.ShowDialog();

            if ((bool)result!)
            {
                SelectedDocument.DocName = renameDialog.NewFileName;
                SelectedDocument.FilePath = _fsHelper!.RenameFile(SelectedDocument.FilePath!, SelectedDocument.DocName);

                UpdateFileList();
            }
        }

        public void ChangeDocumentVersion()
        {
            if (SelectedDocument == null) return;

            ChangeVersionDialog versionDialog = new()
            {
                OldVersion = SelectedDocument.Version,
                NewVersion = SelectedDocument.Version
            };
            bool? result = versionDialog.ShowDialog();

            if (!(bool)result!) return;

            var noticeInfo = new FileInfo(versionDialog.NoticeFilePath!);
            var newVersionObject = new VersionInfo()
            {
                Version = SelectedDocument.Version,
                CreationDate = string.IsNullOrEmpty(versionDialog.NoticeFilePath) 
                    ? DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") 
                    : noticeInfo.CreationTime.ToString("dd.MM.yyyy HH:mm:ss"),
                NoticeFilePath = versionDialog.NoticeFilePath
            };

            SelectedDocument.Version = versionDialog.NewVersion;

            if (SelectedDocument.VersionHistory == null)
            {
                SelectedDocument.VersionHistory = new VersionInfo[] { newVersionObject };
            }
            else
            {
                var list = SelectedDocument.VersionHistory.ToList();
                list.Add(newVersionObject);
                SelectedDocument.VersionHistory = list.ToArray();
            }

            _fsHelper!.SetFileMetadata(SelectedDocument);

            UpdateFileList();
        }
    }
}
