using OrganizerWpf.Models;
using OrganizerWpf.Utilities;
using OrganizerWpf.ViewModels;

namespace OrganizerWpf.UserControls.NoticeExplorer
{
    public class NoticeExplorerViewModel : ExplorerViewModel
    {
        public NoticeExplorerViewModel(string targetDir) : base(targetDir) { }

        protected override void UpdateFileList()
        {
            Items = FileSystemHelper.GetItems<NoticeModel>(_currentDirectory.FullName);
        }
    }
}
