using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizerWpf.Models
{
    public interface IFileSystemUnit
    {
        public string? Extension { get; set; }
        public string? Name { get; set; }
        public string? CreationDate { get; set; }
        public string? UpdateDate { get; set; }
        public string? FullPath { get; set; }
    }
}
