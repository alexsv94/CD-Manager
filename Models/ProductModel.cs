using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizerWpf.Models
{
    public class ProductModel
    {
        public string? ProductName { get; set; }
        public string? ProductDirectoryPath { get; set; }

        public override string? ToString()
        {
            return ProductName;
        }
    }
}
