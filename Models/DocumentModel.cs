using System;
using System.IO;

namespace OrganizerWpf.Models
{
    [Serializable]
    public class DocumentModel : SerializableModel<DocumentModel>
    {
        public VersionModel Version { get; set; } = new();
        public VersionModel[] VersionHistory { get; set; } = new VersionModel[0];

        public override void SetDefaultValues(FileInfo file)
        {
            base.SetDefaultValues(file);
            VersionHistory = new VersionModel[] { new VersionModel() { CreationTime = CreationTime } };
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
