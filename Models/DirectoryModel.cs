using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizerWpf.Models
{
    public class DirectoryModel : IFileSystemItem
    {
        public string? Extension { get; set; }
        public string? Name { get; set; }
        public DateTime? CreationTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string? FullPath { get; set; }

        public void SetAllValues(DirectoryInfo dir)
        {
            Extension = "folder";
            FullPath = dir.FullName;
            Name = dir.Name;
            CreationTime = dir.CreationTime;
            UpdateTime = dir.LastWriteTime;
        }
    }
}