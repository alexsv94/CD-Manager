using OrganizerWpf.Models;
using OrganizerWpf.Utilities;
using OrganizerWpf.ViewModels;

namespace OrganizerWpf.UserControls.NoticeExplorer
{
    public class NoticeExplorerViewModel : ExplorerViewModel<NoticeModel>
    {
        public NoticeExplorerViewModel(string targetDir) : base(targetDir) { }

        protected override void UpdateFileList()
        {
            Files = FileSystemHelper.GetFiles<NoticeModel>(_directoryPath);
        }
    }
}
