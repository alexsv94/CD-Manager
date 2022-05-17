using OrganizerWpf.Dialogs.RenameDialog;
using OrganizerWpf.Models;
using OrganizerWpf.Windows.Settings;
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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {             
        public MainWindow()
        {
            InitializeComponent();
            var viewModel = new MainWindowViewModel();

            viewModel.KDDirectoryChanged += kdExplorer.ViewModel!.UpdateFileList;
            viewModel.PrototypingDirectoryChanged += prototypingExplorer.ViewModel!.UpdateFileList;
            viewModel.NoticeDirectoryChanged += noticeExplorer.UpdateFileList;

            DataContext = viewModel;            
        }
    }
}
