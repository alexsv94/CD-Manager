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
using System.Collections.Specialized;
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

        private RelayCommand? _copyToClipBoardCommand = null;
        public RelayCommand CopyToClipBoardCommand =>
            _copyToClipBoardCommand ??= new RelayCommand(obj => CopyItemToClipBoard(obj as IFileSystemItem));

        private RelayCommand? _refreshCommand = null;
        public RelayCommand RefreshCommand =>
            _refreshCommand ??= new RelayCommand(obj => RefreshItems());

        private RelayCommand? _pasteToContainerCommand = null;
        public RelayCommand PasteToContainerCommand =>
            _pasteToContainerCommand ??= new RelayCommand(obj => PasteToContainerFromClipBoard());

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

        private DataObject? _dragData = null;

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
            RefreshItems();
        }
        
        protected virtual void RefreshItems()
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
            RefreshItems();
        }

        public void OnContainerDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop) && e.Data != _dragData)
            {
                e.Effects = DragDropEffects.Copy;
                UI_DropLabel!.Visibility = Visibility.Visible;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        public void OnContainerDragLeave(object sender, DragEventArgs e)
        {
            UI_DropLabel!.Visibility = Visibility.Collapsed;
        }

        public void OnContainerDrop(object sender, DragEventArgs e)
        {
            UI_DropLabel!.Visibility = Visibility.Collapsed;

            if (e.Data == _dragData) return;

            ProcessFileDrop(_currentDirectory.FullName, e, FileOperation.Copy);
        }

        public void OnDatagridRowDrop(object sender, DragEventArgs e)
        {
            UI_DropLabel!.Visibility = Visibility.Collapsed;

            if (sender is not DataGridRow row) return;
            if (row.DataContext is not DirectoryModel dir) return;
            if ((e.Data.GetData(DataFormats.FileDrop) as string[])!.Contains(dir.FullPath)) return;

            FileOperation opType = _dragData == null ? FileOperation.Copy : FileOperation.Move; 

            ProcessFileDrop(dir.FullPath!, e, opType);
            e.Handled = true;
        }

        private async void ProcessFileDrop(string targetDir, DragEventArgs e, FileOperation operationType)
        {
            string[] droppedFiles = (string[])e.Data.GetData(DataFormats.FileDrop);

            List<string> filePaths = FileSystemHelper.GetDroppedFilePaths(droppedFiles, true);

            AsyncOperation op = new(true, "Копирование")
            {
                TotalStepsCount = FileSystemHelper.GetDroppedFilePaths(droppedFiles, false).Count
            };

            await FileSystemHelper.CopyItemsAsync(targetDir, filePaths, operationType, op);
            RefreshItems();

            _dragData = null;
        }

        public void OnDataGridRowMouseMove(object sender, MouseEventArgs e)
        {
            if (sender is not DataGridRow row) return;
            if (row.DataContext is not IFileSystemItem item) return;
            row.IsSelected = true;

            if (e.MouseDevice.LeftButton == MouseButtonState.Pressed && item.Name != "<...>")
            {               
                string dragFilePath = item.FullPath!;
                _dragData = new DataObject(DataFormats.FileDrop, new string[] { dragFilePath });
                DragDrop.DoDragDrop(row, _dragData, DragDropEffects.Copy);
            }
        }

        public void OnDataGridRowDoubleClick(MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed) return;
            
            if (SelectedItem is DirectoryModel dir)
            {
                _currentDirectory = new(dir.FullPath!);

                if (UI_AdressPanel != null)
                {
                    if (dir.Name != "<...>")
                    {
                        DirLink dirLink = new()
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

                RefreshItems();
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

                RefreshItems();
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

                RefreshItems();
            }
        }
        protected void CopyItemToClipBoard(IFileSystemItem? item)
        {
            if (item == null) return;

            StringCollection files = new();
            files.Add(item.FullPath!);

            Clipboard.SetFileDropList(files);
        }

        public void CheckClipboardData(object sender)
        {
            StringCollection items = Clipboard.GetFileDropList();

            if (items.Count == 0)
                (sender as FrameworkElement)!.IsEnabled = false;
        }

        private async void PasteToContainerFromClipBoard()
        {
            StringCollection items = Clipboard.GetFileDropList();
            string[] paths = new string[items.Count];

            for (int i = 0; i < items.Count; i++)
            {
                paths[i] = items[i]!;
            }

            List<string> filePaths = FileSystemHelper.GetDroppedFilePaths(paths, true);

            AsyncOperation op = new(true, "Копирование")
            {
                TotalStepsCount = FileSystemHelper.GetDroppedFilePaths(paths, false).Count
            };

            await FileSystemHelper.CopyItemsAsync(_currentDirectory.FullName, filePaths, FileOperation.Copy, op);
            RefreshItems();
        }
    }    
}
