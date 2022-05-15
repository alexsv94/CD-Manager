using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizerWpf.Models
{
    public interface IFileInfo
    {
        public string? Extension { get; set; }
        public string? DocName { get; set; }
        public string? CreationDate { get; set; }
        public string? FilePath { get; set; }
    }
}
