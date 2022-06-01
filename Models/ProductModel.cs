using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizerWpf.Models
{
    public class ProductModel : SerializableModel<ProductModel>
    {
        public string DecNumber { get; set; } = string.Empty;        
        
        public override void SetValuesFromMetadata(ProductModel? fileMetaData)
        {
            if (fileMetaData != null)
            {
                FullPath = fileMetaData.FullPath;
                Name = fileMetaData.Name;
                DecNumber = fileMetaData.DecNumber;
            }
        }

        public override string? ToString()
        {
            return Name;
        }
    }
}
