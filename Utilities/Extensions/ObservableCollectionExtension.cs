using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizerWpf.Utilities.Extensions
{
    public static class ObservableCollectionExtension
    {
        public static void AddRange<T>(this ObservableCollection<T> target, IEnumerable<T> source)
        {
            foreach (var item in source)
            {
                target.Add(item);
            }
        }

        public static void ReplaceItems<T>(this ObservableCollection<T> target, IEnumerable<T> source)
        {
            target.Clear();
            target.AddRange(source);
        }

        public static void ReplaceItems<T>(this ObservableCollection<T> target, T source)
        {
            target.Clear();
            target.Add(source);
        }
    }
}
