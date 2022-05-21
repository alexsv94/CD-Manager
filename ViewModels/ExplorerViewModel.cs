using OrganizerWpf.Dialogs.RenameDialog;
using OrganizerWpf.Models;
using OrganizerWpf.UserControls.DirLink;
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
            _deleteCommand ??= new RelayCommand(obj => DeleteItem(obj as IFileSystemItem));

        private RelayCommand? _renameCommand = null;
        public RelayCommand RenameCommand =>
            _renameCommand ??= new RelayCommand(obj => RenameFile(obj as IFileSystemItem));
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
        public StackPanel? UI_AdressPanel;

        protected readonly string _targetDirectory = string.Empty;
        protected DirectoryInfo _rootDirectory;
        protected DirectoryInfo _currentDirectory;

        public ExplorerViewModel(string targetDir)
        {
            _targetDirectory = targetDir;
            Settings.CurrentProductDirectoryChanged += OnRootDirectoryChanged;
        }

        public void GoToRootDirectoty()
        {
            ChangeCurrentDirectory(_rootDirectory.FullName);

            if (UI_AdressPanel != null)
            {
                UI_AdressPanel.Children.RemoveRange(1, UI_AdressPanel.Children.Count - 1);
            }
        }

        public void ChangeCurrentDirectory(string newDir)
        {
            _currentDirectory = new(newDir);
            UpdateFileList();
        }
        
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
        private void OnRootDirectoryChanged(string newDir)
        {
            _currentDirectory = new(Path.Combine(newDir, _targetDirectory));
            _rootDirectory = new(_currentDirectory.FullName);
            GoToRootDirectoty();
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
            if (e.MouseDevice.LeftButton == MouseButtonState.Pressed)
            {
                DataGridRow row = (DataGridRow)sender;
                string dragFilePath = (row.DataContext as IFileSystemItem)!.FullPath!;
                IDataObject dragObject = new DataObject(DataFormats.FileDrop, new string[] { dragFilePath });
                DragDrop.DoDragDrop(row, dragObject, DragDropEffects.Copy);
            }
        }

        public void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed) return;
            
            if (SelectedItem is DirectoryModel dir)
            {
                _currentDirectory = new(dir.FullPath!);

                if (UI_AdressPanel != null)
                {
                    if (dir.Name != "<...>")
                    {
                        DirLink dirLink = new DirLink()
                        {
                            Text = dir.Name!,
                            DirPath = dir.FullPath!,
                            ClickCallback = ChangeCurrentDirectory
                        };
                        UI_AdressPanel.Children.Add(dirLink);
                    }
                    else
                    {
                        UI_AdressPanel.Children.RemoveAt(UI_AdressPanel.Children.Count - 1);
                    }                    
                }                

                UpdateFileList();
            }
            else
            {
                FileSystemHelper.OpenFile(SelectedItem!.FullPath!);
            }
        }
        #endregion

        protected void DeleteItem(IFileSystemItem? item)
        {
            if (item == null) return;

            string typeOfItem = item is DirectoryModel ? "папку" : "файл";

            if (MessageBox.Show($"Удалить {typeOfItem} {item.Name} без возможности восстановления?",
                    "Удаление",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning,
                    MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                if (item is DirectoryModel)
                    Directory.Delete(item.FullPath!, true);
                else
                    File.Delete(item.FullPath!);

                UpdateFileList();
            }            
        }

        protected void RenameFile(IFileSystemItem? item)
        {
            if (item == null) return;

            RenameDialog renameDialog = new()
            {
                OldFileName = item.Name!,
                NewFileName = item.Name!
            };

            bool? result = renameDialog.ShowDialog();

            if ((bool)result!)
            {
                item.Name = renameDialog.NewFileName;
                item.FullPath = FileSystemHelper.RenameItem(item.FullPath!, item.Name);

                UpdateFileList();
            }
        }
    }
}
