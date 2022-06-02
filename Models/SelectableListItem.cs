using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizerWpf.Models
{
    public class SelectableListItem<T> 
        where T: class, new()
    {
        public T? Data { get; private set; }
        public bool Selected;

        public SelectableListItem(T item)
        {
            Data = item;
        }
    }
}
