using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizerWpf.Models
{
    public interface IFileSystemItem
    {
        public string? Extension { get; set; }
        public string? Name { get; set; }
        public DateTime? CreationTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string? FullPath { get; set; }
    }
}
