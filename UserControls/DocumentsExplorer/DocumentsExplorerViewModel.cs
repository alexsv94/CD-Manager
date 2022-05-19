using OrganizerWpf.Dialogs.ChangeVersionDialog;
using OrganizerWpf.Models;
using OrganizerWpf.Utilities;
using OrganizerWpf.ViewModels;
using System;
using System.IO;
using System.Linq;

namespace OrganizerWpf.UserControls.DocumentsExplorer
{
    public class DocumentsExplorerViewModel : ExplorerViewModel<DocumentModel>
    {
        public DocumentsExplorerViewModel(string targetDir) : base(targetDir) { }

        #region Commands
        private RelayCommand? _changeVersionCommand= null;
        public RelayCommand ChangeVersionCommand => 
            _changeVersionCommand ??= new RelayCommand(obj => ChangeDocumentVersion());
        #endregion

        protected override void UpdateFileList()
        {
            Files = FileSystemHelper.GetFiles<DocumentModel>(_directoryPath);
        }

        private void ChangeDocumentVersion()
        {
            if (SelectedFile == null) return;

            ChangeVersionDialog versionDialog = new()
            {
                OldVersion = SelectedFile.Version,
                NewVersion = SelectedFile.Version
            };
            bool? result = versionDialog.ShowDialog();

            if (!(bool)result!) return;

            var noticeInfo = new FileInfo(versionDialog.NoticeFilePath!);
            var newVersionObject = new VersionModel()
            {
                Version = SelectedFile.Version,
                CreationDate = string.IsNullOrEmpty(versionDialog.NoticeFilePath) 
                    ? DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") 
                    : noticeInfo.CreationTime.ToString("dd.MM.yyyy HH:mm:ss"),
                NoticeFilePath = versionDialog.NoticeFilePath
            };

            SelectedFile.Version = versionDialog.NewVersion;

            if (SelectedFile.VersionHistory == null)
            {
                SelectedFile.VersionHistory = new VersionModel[] { newVersionObject };
            }
            else
            {
                var list = SelectedFile.VersionHistory.ToList();
                list.Add(newVersionObject);
                SelectedFile.VersionHistory = list.ToArray();
            }

            FileSystemHelper.SetFileMetadata(SelectedFile);

            UpdateFileList();
        }
    }
}
