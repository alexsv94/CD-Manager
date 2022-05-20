using OrganizerWpf.Dialogs.RenameDialog;
using OrganizerWpf.Models;
using OrganizerWpf.Utilities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OrganizerWpf.ViewModels
{
    public abstract class ExplorerViewModel : ViewModelBase
    {
        #region Commands
        private RelayCommand? _deleteCommand = null;
        public RelayCommand DeleteCommand =>
            _deleteCommand ??= new RelayCommand(obj => DeleteItem());

        private RelayCommand? _renameCommand = null;
        public RelayCommand RenameCommand =>
            _renameCommand ??= new RelayCommand(obj => RenameFile());
        #endregion

        #region Binding Props
        protected List<IFileSystemItem>? _items = null;
        public List<IFileSystemItem>? Items
        {
            get => _items;
            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        protected IFileSystemItem? _selectedItem = null;
        public IFileSystemItem? SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }
        #endregion

        public Label? UI_DropLabel;

        protected readonly string _targetDirectory = string.Empty;
        protected DirectoryInfo _rootDirectory;
        protected DirectoryInfo _currentDirectory;

        public ExplorerViewModel(string targetDir)
        {
            _targetDirectory = targetDir;
            Settings.CurrentProductDirectoryChanged += OnDirectoryChanged;
        }

        /// <summary>
        /// Update data of current type
        /// </summary>
        protected virtual void UpdateFileList()
        {
            if (_currentDirectory.FullName != _rootDirectory.FullName)
            {
                IFileSystemItem backDirItem = new DirectoryModel()
                {
                    Name = "<...>",
                    FullPath = _currentDirectory.Parent!.FullName,
                    Extension = "folder"
                };

                var list = new List<IFileSystemItem>
                {
                    backDirItem
                };
                list.AddRange(Items!);
                Items = list;
            }
        }

        #region Handlers
        private void OnDirectoryChanged(string newDir)
        {
            _currentDirectory = new(Path.Combine(newDir, _targetDirectory));
            _rootDirectory = new(_currentDirectory.FullName);
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

            if (Items != null && Items.Any(x => droppedFiles[0] == x.FullPath))
                return;

            var filePaths = FileSystemHelper.GetDroppedFilePaths(droppedFiles, true);
            FileSystemHelper.CopyItems(_currentDirectory.FullName, filePaths);

            UpdateFileList();
        }

        public void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.MouseDevice.LeftButton == MouseButtonState.Pressed && SelectedItem != null)
            {
                IDataObject dragObject = new DataObject(DataFormats.FileDrop, new string[] { SelectedItem.FullPath! });
                DragDrop.DoDragDrop(sender as DataGrid, dragObject, DragDropEffects.Copy);
            }
        }

        public void OnMouseDoubleClick()
        {
            if (SelectedItem is DirectoryModel dir)
            {
                _currentDirectory = new(dir.FullPath!);
                UpdateFileList();
            }
            else
            {
                FileSystemHelper.OpenFile(SelectedItem!.FullPath!);
            }
        }
        #endregion

        protected void DeleteItem()
        {
            if (SelectedItem == null) return;

            string typeOfItem = SelectedItem is DirectoryModel ? "Папку" : "Файл";

            if (MessageBox.Show($"Удалить {typeOfItem} {SelectedItem.Name} без возможности восстановления?",
                    "Удаление",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning,
                    MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                if (SelectedItem is DirectoryModel)
                    Directory.Delete(SelectedItem.FullPath!, true);
                else
                    File.Delete(SelectedItem.FullPath!);
            }

            UpdateFileList();
        }

        protected void RenameFile()
        {
            if (SelectedItem == null) return;

            RenameDialog renameDialog = new()
            {
                OldFileName = SelectedItem.Name!,
                NewFileName = SelectedItem.Name!
            };

            bool? result = renameDialog.ShowDialog();

            if ((bool)result!)
            {
                SelectedItem.Name = renameDialog.NewFileName;
                SelectedItem.FullPath = FileSystemHelper.RenameItem(SelectedItem.FullPath!, SelectedItem.Name);

                UpdateFileList();
            }
        }
    }
}
