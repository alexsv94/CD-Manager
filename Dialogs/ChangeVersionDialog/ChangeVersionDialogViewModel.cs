using OrganizerWpf.Models;
using OrganizerWpf.StylizedControls;
using OrganizerWpf.Utilities;
using OrganizerWpf.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OrganizerWpf.Dialogs.ChangeVersionDialog
{
    public class ChangeVersionDialogViewModel : ViewModelBase
    {
        #region Commands
        private RelayCommand? _chooseDocumentFileCommand;
        public RelayCommand ChooseDocumentFileCommand =>
            _chooseDocumentFileCommand ??= new(obj => ChooseDocumentFile());

        private RelayCommand? _chooseNoticeFileCommand;
        public RelayCommand ChooseNoticeFileCommand =>
            _chooseNoticeFileCommand ??= new(obj => ChooseNoticeFile());
        #endregion

        #region Binding Props
        private string _newVersion = string.Empty;
        public string NewVersion 
        { 
            get => _newVersion;
            set
            {
                _newVersion = value;
                OnPropertyChanged(nameof(NewVersion));
            }
        }

        private FileInfo? _newDocument = null;
        public FileInfo? NewDocument
        {
            get => _newDocument;
            set
            {
                _newDocument = value;
                OnPropertyChanged(nameof(NewDocument));
            }
        }

        private NoticeModel? _notice = null;
        public NoticeModel? Notice
        {
            get => _notice;
            set
            {
                _notice = value;
                OnPropertyChanged(nameof(Notice));
            }
        }
        #endregion

        public DocumentModel? OldDocument { get; set; }
        public bool NewDocumentRequired { get; set; }
        public bool IsEditMode { get; set; }
        public string OldVersion { get; set; }

        public ChangeVersionDialogViewModel()
        {

        }

        public bool ApplyChanges()
        {
            if (string.IsNullOrEmpty(NewVersion))
            {
                ShowErrorMessage("Необходимо указать версию документа",
                    "Ошибка сохранения версии");
                return false;
            }

            if (NewDocumentRequired && NewDocument == null)
            {
                ShowErrorMessage("Необходимо указать путь к файлу документа", 
                    "Ошибка сохранения версии");                
                return false;
            }

            if (IsEditMode)
            {
                if (OldDocument!.VersionHistory.Length > 0 && 
                    OldDocument!.VersionHistory.Any(x => x.Version == NewVersion) && 
                    NewVersion != OldVersion)
                {
                    ShowErrorMessage("Версия уже существует",
                    "Ошибка сохранения версии");
                    return false;
                }
            }
            else if (OldDocument!.VersionHistory.Length > 0 && 
                     OldDocument!.VersionHistory.Any(x => x.Version == NewVersion))
            {
                ShowErrorMessage("Версия уже существует",
                    "Ошибка сохранения версии");
                return false;
            }

            if (NewDocument != null && NewDocument.FullName != OldDocument.FullPath)
            {
                FileSystemHelper.ReplaceFile(NewDocument.FullName, OldDocument?.FullPath!);               
            }            

            VersionModel versionObj;

            if (IsEditMode)
            {
                versionObj = OldDocument!.VersionHistory.FirstOrDefault(x => x.Version == OldVersion)!;

                versionObj.Version = NewVersion;
                versionObj.CreationTime = Notice == null ? DateTime.Now : Notice.CreationTime;
                versionObj.NoticeFile = Notice;
            }
            else
            {
                versionObj = new VersionModel(NewVersion)
                {
                    CreationTime = Notice == null ? DateTime.Now : Notice.CreationTime,
                    NoticeFile = Notice,
                };

                var list = OldDocument!.VersionHistory.ToList();
                list.Insert(0, versionObj);
                OldDocument.VersionHistory = list.ToArray();
                
                if (OldDocument.VersionHistory.Length > 1)
                {
                    OldDocument.PreviousVersion = OldDocument.VersionHistory[1];
                }

                OldDocument!.Version.Version = NewVersion;

                RecentDocumentsStorage.AddDocument(OldDocument);
            }           

            FileSystemHelper.SaveFileMetadata(OldDocument);

            return true;
        }

        private void ChooseDocumentFile()
        {
            using var dialog = new System.Windows.Forms.OpenFileDialog();

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var doc = new FileInfo(dialog.FileName);

                if (doc != null && doc.Name == OldDocument?.Name)
                {
                    NewDocument = doc;
                }
                else
                {
                    ShowErrorMessage("Название файла должно совпадать с названием документа",
                        "Неверный файл");
                }
            }
        }

        private void ChooseNoticeFile()
        {
            using var dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.InitialDirectory = Path.Combine(Settings.CurrentProductDirectoryPath, "Извещения");

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var notice = FileSystemHelper.GetFile<NoticeModel>(dialog.FileName)!;

                if (notice.Name!.Contains("Извещение"))
                    Notice = notice;
                else
                    ShowErrorMessage("Название файла должно содержать слово 'Извещение'",
                        "Неверный файл извещения");
            }
        }

        private void ShowErrorMessage(string text, string caption)
        {
            SCMessageBox.ShowMsgBox(text, caption,
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
        }

        #region Handlers
        public void OnDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
                //UI_DropLabel!.Visibility = Visibility.Visible;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        public void OnDragLeave(object sender, DragEventArgs e)
        {
            //UI_DropLabel!.Visibility = Visibility.Collapsed;
        }

        public void OnDrop(object sender, DragEventArgs e)
        {
            //UI_DropLabel!.Visibility = Visibility.Collapsed;

            string[] droppedFiles = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (droppedFiles.Length > 1)
            {
                ShowErrorMessage("Необходимо указать один файл", "Ошибка указания файла");
            }
            else
            {
                var doc = new FileInfo(droppedFiles[0]);

                if (doc != null && doc.Name == OldDocument?.Name)
                {
                    NewDocument = doc;
                }
                else
                {
                    ShowErrorMessage("Название файла должно совпадать с названием документа",
                        "Неверный файл");
                }
            }
        }

        public void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.MouseDevice.LeftButton == MouseButtonState.Pressed)
            {
                DataGridRow row = (DataGridRow)sender;
                string dragFilePath = (row.DataContext as IFileSystemItem)!.FullPath!;
                IDataObject dragObject = new DataObject(DataFormats.FileDrop, new string[] { dragFilePath });
                DragDrop.DoDragDrop(row, dragObject, DragDropEffects.Copy);
            }
        }
        #endregion
    }
}
