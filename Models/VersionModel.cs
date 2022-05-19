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
        public string? Version { get; set; }
        public string? CreationDate { get; set; }
        public string? NoticeFilePath { get; set; } 
    }
}