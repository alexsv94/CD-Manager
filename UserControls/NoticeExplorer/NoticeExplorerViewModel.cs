using OrganizerWpf.Models;
using OrganizerWpf.Utilities;
using OrganizerWpf.Utilities.Extensions;
using OrganizerWpf.ViewModels;

namespace OrganizerWpf.UserControls.NoticeExplorer
{
    public class NoticeExplorerViewModel : ExplorerViewModel
    {
        public NoticeExplorerViewModel(string targetDir) : base(targetDir) { }

        protected override void UpdateFileList()
        {
            Items.ReplaceItems(FileSystemHelper.GetItems<NoticeModel>(_currentDirectory.FullName));
        }
    }
}
