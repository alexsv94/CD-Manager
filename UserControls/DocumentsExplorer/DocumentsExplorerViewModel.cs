using OrganizerWpf.Dialogs.ChangeVersionDialog;
using OrganizerWpf.Models;
using OrganizerWpf.Utilities;
using OrganizerWpf.ViewModels;
using System;
using System.IO;
using System.Linq;
using System.Windows;

namespace OrganizerWpf.UserControls.DocumentsExplorer
{
    public class DocumentsExplorerViewModel : ExplorerViewModel
    {
        public DocumentsExplorerViewModel(string targetDir) : base(targetDir) { }

        #region Commands
        private RelayCommand? _changeVersionCommand= null;
        public RelayCommand ChangeVersionCommand => 
            _changeVersionCommand ??= new RelayCommand(obj => ChangeDocumentVersion());
        #endregion

        protected override void UpdateFileList()
        {
            Items = FileSystemHelper.GetItems<DocumentModel>(_currentDirectory.FullName);
            base.UpdateFileList();
        }        

        private void ChangeDocumentVersion()
        {
            if (SelectedItem == null || SelectedItem is not DocumentModel doc) return;

            ChangeVersionDialog versionDialog = new()
            {
                OldVersion = doc.Version,
                NewVersion = doc.Version
            };
            bool? result = versionDialog.ShowDialog();

            if (!(bool)result!) return;

            var noticeInfo = new FileInfo(versionDialog.NoticeFilePath!);
            var newVersionObject = new VersionModel()
            {
                Version = doc.Version,
                CreationDate = string.IsNullOrEmpty(versionDialog.NoticeFilePath) 
                    ? DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") 
                    : noticeInfo.CreationTime.ToString("dd.MM.yyyy HH:mm:ss"),
                NoticeFilePath = versionDialog.NoticeFilePath
            };

            doc.Version = versionDialog.NewVersion;

            if (doc.VersionHistory == null)
            {
                doc.VersionHistory = new VersionModel[] { newVersionObject };
            }
            else
            {
                var list = doc.VersionHistory.ToList();
                list.Add(newVersionObject);
                doc.VersionHistory = list.ToArray();
            }

            FileSystemHelper.SetFileMetadata(doc);

            UpdateFileList();
        }

        #region Handlers
        public void SetMenuItemVisibility(object sender)
        {
            if (SelectedItem is not DocumentModel)
                (sender as FrameworkElement)!.Visibility = Visibility.Collapsed;
            else
                (sender as FrameworkElement)!.Visibility = Visibility.Visible;
        }
        #endregion
    }
}
