using OrganizerWpf.Dialogs.ChangeVersionDialog;
using OrganizerWpf.Icons;
using OrganizerWpf.Models;
using OrganizerWpf.Utilities;
using OrganizerWpf.ViewModels;
using System;
using System.Collections.Generic;
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
        protected List<VersionModel>? _items = null;
        public List<VersionModel>? Items
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
            Items = _document.VersionHistory.ToList();
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

            FileSystemHelper.SaveFileMetadata(_document);

            UpdateVersionsList();
        }

        private void DeleteVersion(VersionModel version)
        {
            List<VersionModel> versionsList = _document.VersionHistory.ToList();

            versionsList.Remove(version);

            if (versionsList.Count == 0)
                versionsList.Add(new VersionModel() { CreationTime = _document.CreationTime });

            _document.VersionHistory = versionsList.ToArray();
            _document.Version = GetLatestVersion(versionsList);
            
            if (_document.VersionHistory.Length > 1)
                _document.PreviousVersion = _document.VersionHistory[1];

            FileSystemHelper.SaveFileMetadata(_document);
            UpdateVersionsList();
        }

        private VersionModel GetLatestVersion(List<VersionModel> versionsList)
        {
            if (versionsList.Count == 0)
            {
                var newVersionObj = new VersionModel();
                versionsList.Add(newVersionObj);
            };
            
            List<DateTime?> dateTimeList = new();

            foreach (var v in versionsList)
            {
                dateTimeList.Add(v.CreationTime);
            }

            DateTime? latestDate = dateTimeList.First();

            foreach (var dateTime in dateTimeList)
            {
                if (dateTime > latestDate)
                    latestDate = dateTime;
            }

            return versionsList.FirstOrDefault(x => x.CreationTime! == latestDate)!;
        }
    }
}
