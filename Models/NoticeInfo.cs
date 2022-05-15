using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OrganizerWpf.Models
{
    [Serializable]
    public class NoticeInfo : SerializableModel<NoticeInfo>, IFileInfo
    {        
        public string? Extension { get; set; }
        public string? DocName { get; set; }
        public string? CreationDate { get; set; }
        public string? FilePath { get; set; }
        public string? Reason { get; set; }
    }
}
