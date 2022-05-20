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
        public string? CreationDate { get; set; }
        public string? UpdateDate { get; set; }
        public string? FullPath { get; set; }

        public void SetAllValues(DirectoryInfo dir)
        {
            Extension = "folder";
            FullPath = dir.FullName;
            Name = dir.Name;
            CreationDate = dir.CreationTime.ToString("dd.MM.yyyy HH:mm:ss");
            UpdateDate = dir.LastWriteTime.ToString("dd.MM.yyyy HH:mm:ss");
        }
    }
}