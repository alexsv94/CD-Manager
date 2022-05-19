using OrganizerWpf.Dialogs.RenameDialog;
using OrganizerWpf.Models;
using OrganizerWpf.Utilities;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OrganizerWpf.ViewModels
{
    public abstract class ExplorerViewModel<T> : INotifyPropertyChanged 
        where T : SerializableModel<T>
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Commands
        private RelayCommand? _deleteCommand = null;
        public RelayCommand DeleteCommand =>
            _deleteCommand ??= new RelayCommand(obj => DeleteFile());

        private RelayCommand? _renameCommand = null;
        public RelayCommand RenameCommand =>
            _renameCommand ??= new RelayCommand(obj => RenameFile());
        #endregion

        #region Binding Props
        protected List<T>? _files = null;
        public List<T>? Files
        {
            get => _files;
            set
            {
                _files = value;
                OnPropertyChanged(nameof(Files));
            }
        }

        protected T? _selectedFile = null;
        public T? SelectedFile
        {
            get => _selectedFile;
            set
            {
                _selectedFile = value;
                OnPropertyChanged(nameof(SelectedFile));
            }
        }
        #endregion

        public Label? UI_DropLabel;

        protected readonly string _targetDirectory = string.Empty;
        protected readonly FilesystemHelper? _fsHelper = null;
        protected string _directoryPath = string.Empty;

        public ExplorerViewModel(string targetDir)
        {
            _fsHelper = new();
            _targetDirectory = targetDir;
            Settings.CurrentProductDirectoryChanged += OnDirectoryChanged;
        }

        /// <summary>
        /// Update data of current type
        /// </summary>
        protected virtual void UpdateFileList()
        {
            
        }

        #region Handlers
        private void OnDirectoryChanged(string newDir)
        {
            _directoryPath = Path.Combine(newDir, _targetDirectory);
            UpdateFileList();
        }

        public void OnDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
                UI_DropLabel!.Visibility = Visibility.Visible;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        public void OnDragLeave(object sender, DragEventArgs e)
        {
            UI_DropLabel!.Visibility = Visibility.Collapsed;
        }

        public void OnDrop(object sender, DragEventArgs e)
        {
            UI_DropLabel!.Visibility = Visibility.Collapsed;

            string[] droppedFiles = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (Files != null && Files.Any(x => droppedFiles[0] == x.FilePath))
                return;

            var filePaths = _fsHelper!.GetDroppedFiles(droppedFiles);
            _fsHelper.CopyFiles(_directoryPath, filePaths);

            UpdateFileList();
        }

        public void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.MouseDevice.LeftButton == MouseButtonState.Pressed)
            {
                IDataObject dragObject = new DataObject(DataFormats.FileDrop, new string[] { SelectedFile!.FilePath! });
                DragDrop.DoDragDrop(sender as DataGrid, dragObject, DragDropEffects.Copy);
            }
        }

        public void OnMouseDoubleClick()
        {
            _fsHelper!.OpenFile(SelectedFile!.FilePath!);
        }
        #endregion

        protected void DeleteFile()
        {
            if (SelectedFile == null) return;

            if (MessageBox.Show($"Удалить файл {SelectedFile.DocName} без возможности восстановления?",
                    "Удаление файла",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning,
                    MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                File.Delete(SelectedFile.FilePath!);
            }

            UpdateFileList();
        }

        protected void RenameFile()
        {
            if (SelectedFile == null) return;

            RenameDialog renameDialog = new()
            {
                OldFileName = SelectedFile.DocName!,
                NewFileName = SelectedFile.DocName!
            };

            bool? result = renameDialog.ShowDialog();

            if ((bool)result!)
            {
                SelectedFile.DocName = renameDialog.NewFileName;
                SelectedFile.FilePath = _fsHelper!.RenameFile(SelectedFile.FilePath!, SelectedFile.DocName);

                UpdateFileList();
            }
        }
    }
}
