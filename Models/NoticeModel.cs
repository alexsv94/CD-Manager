using System;
using System.IO;

namespace OrganizerWpf.Models
{
    [Serializable]
    public class NoticeModel : SerializableModel<NoticeModel>
    {  
        public string? Reason { get; set; }

        public override void SetDefaultValues(FileInfo file)
        {
            base.SetDefaultValues(file);
            Reason = "<Не указано>";
        }

        public override void SetValuesFromMetadata(NoticeModel? fileMetaData)
        {
            if (fileMetaData != null)
            {
                Reason = fileMetaData.Reason;
            }                
        }
    }
}
