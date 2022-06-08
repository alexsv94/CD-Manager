using OrganizerWpf.Dialogs.RenameDialog;
using OrganizerWpf.Models;
using OrganizerWpf.StylizedControls;
using OrganizerWpf.UserControls.DirLink;
using OrganizerWpf.Utilities;
using OrganizerWpf.Utilities.Extensions;
using OrganizerWpf.Utilities.Types;
using OrganizerWpf.Windows.OperationProgress;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
            _renameCommand ??= new RelayCommand(obj => RenameItem(obj as IFileSystemItem));
        #endregion

        #region Binding Props
        protected ObservableCollection<IFileSystemItem> _filteredItems = new();
        public ObservableCollection<IFileSystemItem> FilteredItems
        {
            get => _filteredItems;
            set
            {
                _filteredItems = value;
                OnPropertyChanged(nameof(FilteredItems));
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

        private DirectoryInfo? _rootDirectory = null;
        public DirectoryInfo? RootDirectory
        {
            get => _rootDirectory;
            set
            {
                _rootDirectory = value;
                OnPropertyChanged(nameof(RootDirectory));
            }
        }
        #endregion

        public Label? UI_DropLabel;
        public StackPanel? UI_AdressPanel;

        protected DirectoryInfo _currentDirectory;
        protected ObservableCollection<IFileSystemItem> _allItems = new();
        protected DateInterval _dateInterval;

        public ExplorerViewModel()
        {
            Settings.CurrentProductDirectoryChanged += OnRootDirectoryChanged;
            _dateInterval = new DateInterval();
        }

        public void GoToRootDirectoty()
        {
            if (RootDirectory != null)
                ChangeCurrentDirectory(RootDirectory.FullName);

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
            if (_currentDirectory.FullName != RootDirectory?.FullName)
            {
                IFileSystemItem backDirItem = new DirectoryModel()
                {
                    Name = "<...>",
                    FullPath = _currentDirectory.Parent!.FullName,
                    Extension = "folder"
                };
                
                _allItems.Insert(0, backDirItem);
            }

            var items = _allItems.Where(x => _dateInterval.Contains(x.CreationTime) || 
                                        x is DirectoryModel);
            FilteredItems.ReplaceItems(items);
        }

        #region Handlers
        protected virtual void OnRootDirectoryChanged(string newDir)
        {
            _currentDirectory = new(newDir);
            RootDirectory = new(_currentDirectory.FullName);
            GoToRootDirectoty();
        }

        public void OnIntervalChanged(DateInterval interval)
        {
            _dateInterval = interval;
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

        public async void OnDrop(object sender, DragEventArgs e)
        {
            UI_DropLabel!.Visibility = Visibility.Collapsed;

            string[] droppedFiles = (string[])e.Data.GetData(DataFormats.FileDrop);

            List<string> filePaths = FileSystemHelper.GetDroppedFilePaths(droppedFiles, true);

            AsyncOperation op = new(true, "Копирование") 
            { 
                TotalStepsCount = FileSystemHelper.GetDroppedFilePaths(droppedFiles, false).Count 
            };

            await FileSystemHelper.CopyItemsAsync(_currentDirectory.FullName, filePaths, op);
            UpdateFileList();
        }

        public void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.MouseDevice.LeftButton == MouseButtonState.Pressed && sender is DataGridRow row)
            {
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

            var result = SCMessageBox.ShowMsgBox($"Удалить {typeOfItem} {item.Name} без возможности восстановления?",
                                                "Удаление",
                                                MessageBoxButton.YesNo, MessageBoxImage.Question);
            
            if (result == SCMessageBoxResult.Yes)
            {
                if (item is DirectoryModel)
                    Directory.Delete(item.FullPath!, true);
                else
                    File.Delete(item.FullPath!);

                UpdateFileList();
            }           
        }

        protected void RenameItem(IFileSystemItem? item)
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

                if (item is DocumentModel model)
                {
                    RecentDocumentsStorage.ChangeDocumentParameters(model);
                }

                UpdateFileList();
            }
        }
    }
}
