using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace OrganizerWpf.Models
{
    [Serializable]
    public class DocumentModel : SerializableModel<DocumentModel>
    {
        public VersionModel Version => VersionHistory.Count > 0 ? VersionHistory[0] : new();
        public VersionModel? PreviousVersion => VersionHistory.Count > 1 ? VersionHistory[1] : null;
        public List<VersionModel> VersionHistory { get; set; } = new();

        public override void SetDefaultValues(FileInfo file)
        {
            base.SetDefaultValues(file);
            VersionHistory = new() { new VersionModel() { CreationTime = CreationTime } };
        }

        public override void SetValuesFromMetadata(DocumentModel? fileMetaData)
        {
            if (fileMetaData != null)
            {
                VersionHistory = fileMetaData.VersionHistory;
            }
        }
    }
}
