using OrganizerWpf.Dialogs.ChangeVersionDialog;
using OrganizerWpf.Models;
using OrganizerWpf.Utilities;
using OrganizerWpf.ViewModels;
using OrganizerWpf.Windows.VersionHistory;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace OrganizerWpf.UserControls.DocumentsExplorer
{
    public class DocumentsExplorerViewModel : ExplorerViewModel
    {
        public DocumentsExplorerViewModel(string targetDir) : base(targetDir) { }

        #region Commands
        private RelayCommand? _changeVersionCommand= null;
        public RelayCommand ChangeVersionCommand => 
            _changeVersionCommand ??= new RelayCommand(obj => ChangeDocumentVersion(obj as DocumentModel));

        private RelayCommand? _showVersionHistoryCommand = null;
        public RelayCommand ShowVersionHistoryCommand =>
            _showVersionHistoryCommand ??= new RelayCommand(obj => ShowVersionHistory(obj as DocumentModel));
        #endregion

        protected override void UpdateFileList()
        {
            Items = FileSystemHelper.GetItems<DocumentModel>(_currentDirectory.FullName);
            base.UpdateFileList();
        }        

        private void ChangeDocumentVersion(DocumentModel? document)
        {
            if (document == null) return;

            ChangeVersionDialog versionDialog = new()
            {
                OldVersion = document.Version,
                NewVersion = document.Version
            };
            bool? result = versionDialog.ShowDialog();

            if (!(bool)result!) return;

            document.Version = versionDialog.NewVersion;

            var noticeInfo = FileSystemHelper.GetFileMetadata<NoticeModel>(versionDialog.NoticeFilePath);
            var newVersionObject = new VersionModel()
            {
                Version = document.Version,
                CreationDate = string.IsNullOrEmpty(versionDialog.NoticeFilePath)
                    ? DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")
                    : noticeInfo != null ? noticeInfo.CreationDate! : DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"),
                NoticeFile = noticeInfo ?? FileSystemHelper.GetFile<NoticeModel>(versionDialog.NoticeFilePath) as NoticeModel,
            };            

            if (document.VersionHistory == null)
            {
                document.VersionHistory = new VersionModel[] { newVersionObject };
            }
            else
            {
                var list = document.VersionHistory.ToList();
                list.Add(newVersionObject);
                document.VersionHistory = list.ToArray();
            }

            FileSystemHelper.SaveFileMetadata(document);

            UpdateFileList();
        }

        private void ShowVersionHistory(DocumentModel? document)
        {
            if (document == null) return;

            VersionHistoryWindow historyWindow = new();
            historyWindow.Document = document;

            historyWindow.Show();
        }

        #region Handlers
        public void SetMenuItemVisibility(object sender)
        {
            MenuItem menuItem = (sender as MenuItem)!;
            ContextMenu contextMenu = (menuItem.Parent as ContextMenu)!;
            object context = (contextMenu.PlacementTarget as FrameworkElement)!.DataContext;

            if (context is not DocumentModel)
                menuItem!.Visibility = Visibility.Collapsed;
            else
                menuItem!.Visibility = Visibility.Visible;
        }

        public void SetContextMenuVisibility(object sender)
        {
            var row = sender as FrameworkElement;
            var contextMenu = row!.ContextMenu;
            
            if (row.DataContext is DirectoryModel model && model.Name == "<...>")
                contextMenu!.Visibility = Visibility.Collapsed;
            else
                contextMenu!.Visibility = Visibility.Visible;
        }
        #endregion
    }
}
