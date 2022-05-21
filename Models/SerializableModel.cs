using Newtonsoft.Json;
using System;
using System.IO;

namespace OrganizerWpf.Models
{
    public class SerializableModel<T> : IFileSystemItem
    {
        public string? Extension { get; set; }
        public string? Name { get; set; }
        public string? CreationDate { get; set; }
        public string? UpdateDate { get; set; }
        public string? FullPath { get; set; }

        public virtual void SetDefaultValues(FileInfo file)
        {
            Extension = file.Extension;
            FullPath = file.FullName;
            Name = file.Name;
            CreationDate = file.CreationTime.ToString("dd.MM.yyyy HH:mm:ss");
            UpdateDate = file.LastWriteTime.ToString("dd.MM.yyyy HH:mm:ss");            
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
            return JsonConvert.SerializeObject(this);
        }
    }
}
