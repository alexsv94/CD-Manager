using OrganizerWpf.Models;
using OrganizerWpf.Utilities;
using OrganizerWpf.Utilities.Extensions;
using OrganizerWpf.ViewModels;
using System.IO;

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
            Items.ReplaceItems(FileSystemHelper.GetItems<NoticeModel>(_currentDirectory.FullName));
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