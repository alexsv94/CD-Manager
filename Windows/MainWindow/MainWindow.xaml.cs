using OrganizerWpf.Dialogs.RenameDialog;
using OrganizerWpf.Models;
using OrganizerWpf.Utilities;
using OrganizerWpf.Windows.SettingsW;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace OrganizerWpf.Windows.MainWindow
{    
    public partial class MainWindow : Window
    {
        public MainWindowViewModel? ViewModel { get; set; } = null;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += SetupViewModel;
        }

        private void SetupViewModel(object sender, RoutedEventArgs e)
        {
            if (ViewModel == null)
                ViewModel = new(this);

            DataContext = ViewModel;
        }
    }
}
