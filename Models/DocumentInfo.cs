using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizerWpf.Models
{
    [Serializable]
    public class DocumentInfo : SerializableModel<DocumentInfo>, IFileInfo
    {
        public string? Extension { get; set; }
        public string? DocName { get; set; }
        public string? Version { get; set; }  
        public string? CreationDate { get; set; }
        public string? UpdateDate { get; set; }
        public string? FilePath { get; set; }
    }
}
