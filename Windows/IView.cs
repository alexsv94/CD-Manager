using OrganizerWpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OrganizerWpf.Windows
{
    public interface IView<T>
    {
        T? ViewModel { get; set; }
        void SetupViewModel(object sender, RoutedEventArgs e);
    }
}
