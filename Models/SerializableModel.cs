using Newtonsoft.Json;
using System;
using System.IO;

namespace OrganizerWpf.Models
{
    public class SerializableModel<T> : IFileSystemItem
    {
        public string? Extension { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ShortName
        {
            get
            {
                int extSplitter = Name.LastIndexOf('.');
                return extSplitter > 0 ? Name[..extSplitter] : Name;
            }
        }
        public DateTime? CreationTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string? FullPath { get; set; }

        public virtual void SetDefaultValues(FileInfo file)
        {
            Extension = file.Extension;
            FullPath = file.FullName;
            Name = file.Name;
            CreationTime = file.CreationTime;
            UpdateTime = file.LastWriteTime;            
        }

        public virtual void SetDefaultValues(DirectoryInfo dir)
        {
            FullPath = dir.FullName;
            Name = dir.Name;
            CreationTime = dir.CreationTime;
            UpdateTime = dir.LastWriteTime;
        }

        public virtual void SetValuesFromMetadata(T? fileMetaData)
        {
            throw new NotImplementedException("Method 'SetValuesFromMetadata()' is not implemented");
        }

        public static T? FromJson(string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
