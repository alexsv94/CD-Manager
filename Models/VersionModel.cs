using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizerWpf.Models
{
    [Serializable]
    public class VersionModel
    {
        public string Version { get; set; } = string.Empty;
        public DateTime? CreationTime { get; set; }
        public NoticeModel? NoticeFile { get; set; }

        public VersionModel(string version = "<Не указано>")
        {
            Version = version;
        }
    }
}