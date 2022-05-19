using System;
using System.IO;

namespace OrganizerWpf.Models
{
    [Serializable]
    public class DocumentModel : SerializableModel<DocumentModel>
    {       
        public string? Version { get; set; }         
        public VersionModel[]? VersionHistory { get; set; }

        public override void SetDefaultValues(FileInfo file)
        {
            base.SetDefaultValues(file);
            Version = "<Не указано>";
        }

        public override void SetValuesFromMetadata(DocumentModel? fileMetaData)
        {
            if (fileMetaData != null)
            {
                Version = fileMetaData.Version;
                VersionHistory = fileMetaData.VersionHistory;
            }
        }
    }
}
