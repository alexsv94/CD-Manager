using OrganizerWpf.Models;
using OrganizerWpf.ViewModels;

namespace OrganizerWpf.UserControls.NoticeExplorer
{
    public class NoticeExplorerViewModel : ExplorerViewModel<NoticeModel>
    {
        public NoticeExplorerViewModel(string targetDir) : base(targetDir) { }

        protected override void UpdateFileList()
        {
            Files = _fsHelper!.GetFiles<NoticeModel>(_directoryPath);
        }
    }
}
