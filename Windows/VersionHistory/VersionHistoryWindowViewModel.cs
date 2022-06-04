using OrganizerWpf.Dialogs.ChangeVersionDialog;
using OrganizerWpf.Icons;
using OrganizerWpf.Models;
using OrganizerWpf.Utilities;
using OrganizerWpf.Utilities.Extensions;
using OrganizerWpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OrganizerWpf.Windows.VersionHistory
{
    public class VersionHistoryWindowViewModel : ViewModelBase
    {
        #region Commands
        private RelayCommand? _deleteVersionCommand = null;
        public RelayCommand DeleteVersionCommand =>
            _deleteVersionCommand ??= new RelayCommand(obj => DeleteVersion((obj as VersionModel)!));

        private RelayCommand? _editVersionCommand = null;
        public RelayCommand EditVersionCommand =>
            _editVersionCommand ??= new RelayCommand(obj => EditVersion((obj as VersionModel)!));

        private RelayCommand? _addNewVersionCommand = null;
        public RelayCommand AddNewVersionCommand =>
            _addNewVersionCommand ??= new RelayCommand(obj => CreateDocumentVersion());
        #endregion

        #region Binding Props
        protected ObservableCollection<VersionModel>? _items = new();
        public ObservableCollection<VersionModel> Items
        {
            get => _items;
            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        protected VersionModel? _selectedItem = null;
        public VersionModel? SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }
        #endregion

        private readonly DocumentModel _document;

        public VersionHistoryWindowViewModel(DocumentModel document)
        {
            _document = document;
            UpdateVersionsList();
        }

        private void UpdateVersionsList()
        {
            Items.ReplaceItems(_document.VersionHistory);
        }

        #region Handlers
        public void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed || SelectedItem!.NoticeFile == null) return;

            FileSystemHelper.OpenFile(SelectedItem!.NoticeFile!.FullPath!);
        }
        #endregion

        private void CreateDocumentVersion()
        {
            if (_document == null) return;

            ChangeVersionDialog versionDialog = new();
            versionDialog.ViewModel!.OldDocument = _document;
            versionDialog.ViewModel!.NewDocumentRequired = true;

            bool? result = versionDialog.ShowDialog();

            if (!(bool)result!) return;

            UpdateVersionsList();
        }

        private void EditVersion(VersionModel version)
        {
            if (_document == null) return;

            ChangeVersionDialog versionDialog = new();
            versionDialog.ViewModel!.NewVersion = _document.Version.Version;
            versionDialog.ViewModel!.OldDocument = _document;
            versionDialog.ViewModel!.NewDocument = new(_document.FullPath!);
            versionDialog.ViewModel!.OldVersion = _document.Version.Version;
            versionDialog.ViewModel!.IsEditMode = true;
            versionDialog.ViewModel!.NewDocumentRequired = true;

            bool? result = versionDialog.ShowDialog();

            if (!(bool)result!) return;

            RecentDocumentsStorage.ChangeDocumentParameters(_document);
            FileSystemHelper.SaveFileMetadata(_document);

            UpdateVersionsList();
        }

        private void DeleteVersion(VersionModel version)
        {
            _document.VersionHistory.Remove(version);

            if (_document.VersionHistory.Count == 0)
                _document.VersionHistory.Add(new VersionModel() { CreationTime = _document.CreationTime });

            RecentDocumentsStorage.ChangeDocumentParameters(_document);

            FileSystemHelper.SaveFileMetadata(_document);
            UpdateVersionsList();
        }
    }
}
