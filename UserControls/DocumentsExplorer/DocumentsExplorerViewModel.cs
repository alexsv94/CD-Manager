using OrganizerWpf.Dialogs.ChangeVersionDialog;
using OrganizerWpf.Models;
using OrganizerWpf.Utilities;
using OrganizerWpf.Utilities.Extensions;
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
        public DocumentsExplorerViewModel() : base() { }

        #region Commands
        private RelayCommand? _changeVersionCommand= null;
        public RelayCommand ChangeVersionCommand => 
            _changeVersionCommand ??= new RelayCommand(obj => ChangeDocumentVersion(obj as DocumentModel));

        private RelayCommand? _showVersionHistoryCommand = null;
        public RelayCommand ShowVersionHistoryCommand =>
            _showVersionHistoryCommand ??= new RelayCommand(obj => ShowVersionHistory(obj as DocumentModel));
        #endregion

        protected override void RefreshItems()
        {
            _allItems.ReplaceItems(FileSystemHelper.GetItems<DocumentModel>(_currentDirectory.FullName));
            base.RefreshItems();
        }        

        private void ChangeDocumentVersion(DocumentModel? document)
        {
            if (document == null) return;

            ChangeVersionDialog versionDialog = new();
            versionDialog.ViewModel!.NewVersion = document.Version.Version;
            versionDialog.ViewModel!.OldDocument = document;
            versionDialog.ViewModel!.OldVersion = document.Version.Version;
            versionDialog.ViewModel!.NewDocumentRequired = true;

            bool? result = versionDialog.ShowDialog();

            if (!(bool)result!) return;

            RefreshItems();
        }

        private void ShowVersionHistory(DocumentModel? document)
        {
            if (document == null) return;

            VersionHistoryWindow historyWindow = new();
            historyWindow.Document = document;

            historyWindow.ShowDialog();

            RefreshItems();
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
