using OrganizerWpf.Dialogs.ChangeVersionDialog;
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
        private RelayCommand? _deleteCommand = null;
        public RelayCommand DeleteCommand =>
            _deleteCommand ??= new RelayCommand(obj => DeleteVersion((obj as VersionModel)!));

        private RelayCommand? _editVersionCommand = null;
        public RelayCommand EditVersionCommand =>
            _editVersionCommand ??= new RelayCommand(obj => EditVersion((obj as VersionModel)!));

        private RelayCommand? _changeVersionCommand = null;
        public RelayCommand ChangeVersionCommand =>
            _changeVersionCommand ??= new RelayCommand(obj => ChangeDocumentVersion((obj as DocumentModel)!));
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
            Items = _document.VersionHistory?.ToList();
        }

        #region Handlers
        public void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed || SelectedItem!.NoticeFile == null) return;

            FileSystemHelper.OpenFile(SelectedItem!.NoticeFile!.FullPath!);
        }
        #endregion

        private void ChangeDocumentVersion(DocumentModel document)
        {
            if (document == null) return;

            ChangeVersionDialog versionDialog = new()
            {
                OldVersion = document.Version,
                NewVersion = document.Version
            };
            bool? result = versionDialog.ShowDialog();

            if (!(bool)result!) return;

            var noticeInfo = FileSystemHelper.GetFileMetadata<NoticeModel>(versionDialog.NoticeFilePath);
            var newVersionObject = new VersionModel()
            {
                Version = document.Version,
                CreationTime = string.IsNullOrEmpty(versionDialog.NoticeFilePath)
                    ? DateTime.Now
                    : noticeInfo != null ? noticeInfo.CreationTime! : DateTime.Now,
                NoticeFile = noticeInfo,
            };

            document.Version = versionDialog.NewVersion;

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

            UpdateVersionsList();
        }

        private void EditVersion(VersionModel version)
        {
            ChangeVersionDialog versionDialog = new()
            {
                OldVersion = _document.Version,
                NewVersion = _document.Version,
                NoticeFilePath = version.NoticeFile?.FullPath!
            };
            bool? result = versionDialog.ShowDialog();

            if (!(bool)result!) return;

            var noticeInfo = FileSystemHelper.GetFileMetadata<NoticeModel>(versionDialog.NoticeFilePath);
            
            var versionObject = _document.VersionHistory!.FirstOrDefault(x => x.Version == versionDialog.OldVersion)!;

            versionObject.Version = versionDialog.NewVersion;
            versionObject.CreationTime = string.IsNullOrEmpty(versionDialog.NoticeFilePath)
                    ? DateTime.Now
                    : noticeInfo != null ? noticeInfo.CreationTime! : DateTime.Now;
            versionObject.NoticeFile = noticeInfo;

            FileSystemHelper.SaveFileMetadata(_document);

            UpdateVersionsList();
        }

        private void DeleteVersion(VersionModel version)
        {
            List<VersionModel>? versionsList = _document.VersionHistory?.ToList();

            if (versionsList == null) return;

            versionsList.Remove(version);
            _document.VersionHistory = versionsList.ToArray();
            _document.Version = GetLatestVersion(versionsList).Version;

            FileSystemHelper.SaveFileMetadata(_document);
            UpdateVersionsList();
        }

        private VersionModel GetLatestVersion(List<VersionModel> versionsList)
        {
            List<DateTime> dateTimeList = new();

            foreach (var v in versionsList)
            {
                dateTimeList.Add((DateTime)(v.CreationTime!));
            }

            DateTime latestDate = dateTimeList.First();

            foreach (var dateTime in dateTimeList)
            {
                if (dateTime > latestDate)
                    latestDate = dateTime;
            }

            return versionsList.FirstOrDefault(x => x.CreationTime! == latestDate)!;
        }
    }
}
