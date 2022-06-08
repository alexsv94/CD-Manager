using OrganizerWpf.Models;
using OrganizerWpf.Utilities;
using OrganizerWpf.Utilities.Extensions;
using OrganizerWpf.ViewModels;
using System;
using System.IO;
using System.Linq;

namespace OrganizerWpf.UserControls.NoticeExplorer
{
    public class NoticeExplorerViewModel : ExplorerViewModel
    {
        public NoticeExplorerViewModel()
        {
            Settings.WorkingDirectoryChanged += OnRootDirectoryChanged;
        }

        protected override void UpdateFileList()
        {
            if (_currentDirectory == null) return;
            
            _allItems.ReplaceItems(FileSystemHelper.GetItems<NoticeModel>(_currentDirectory.FullName));

            var items = _allItems.Where(x => _dateInterval.Contains((DateTime)x.CreationTime!) ||
                                       x is DirectoryModel);
            FilteredItems.ReplaceItems(items);
        }

        protected override void OnRootDirectoryChanged(string newDir)
        {
            OnCurrentProductDirectoryChanged(Settings.CurrentProductDirectoryPath);
        }

        private void OnCurrentProductDirectoryChanged(string newDir)
        {
            if (string.IsNullOrWhiteSpace(newDir)) return;

            var currentProductDirInfo = new DirectoryInfo(newDir);
            _currentDirectory = new(Path.Combine(Settings.WorkingDirectoryPath, "Извещения", currentProductDirInfo.Name));
            RootDirectory = new(_currentDirectory.FullName);
            GoToRootDirectoty();
        }
    }
}