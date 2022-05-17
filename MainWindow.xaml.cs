using OrganizerWpf.Dialogs.RenameDialog;
using OrganizerWpf.Models;
using OrganizerWpf.Windows.SettingsWindow;
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

namespace OrganizerWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string KDDirectory
        {
            get { return (string)GetValue(KDDirectoryProperty); }
            set { SetValue(KDDirectoryProperty, value); }
        }

        // Using a DependencyProperty as the backing store for KDDirectory.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty KDDirectoryProperty =
            DependencyProperty.Register("KDDirectory", typeof(string), typeof(MainWindow), new PropertyMetadata(null));


        public string PrototypingDirectory
        {
            get { return (string)GetValue(PrototypingDirectoryProperty); }
            set { SetValue(PrototypingDirectoryProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PrototypingDirectory.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PrototypingDirectoryProperty =
            DependencyProperty.Register("PrototypingDirectory", typeof(string), typeof(MainWindow), new PropertyMetadata(null));


        public string NoticeDirectory
        {
            get { return (string)GetValue(NoticeDirectoryProperty); }
            set { SetValue(NoticeDirectoryProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NoticeDirectory.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NoticeDirectoryProperty =
            DependencyProperty.Register("NoticeDirectory", typeof(string), typeof(MainWindow), new PropertyMetadata(null));



        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            if (!string.IsNullOrEmpty(Settings.WorkingDirectoryPath))
                UpdateProductList();

            productList.SelectedItem = productList.Items[0];            
        }

        private void productList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChangeCurrentProduct();
        }

        private void ChangeCurrentProduct()
        {
            string selectedProductDirectoryPath = ((ProductInfo)productList.SelectedItem).ProductDirectoryPath!;
            Settings.CurrentProductDirectoryPath = selectedProductDirectoryPath;

            KDDirectory = Path.Combine(selectedProductDirectoryPath, "КД");
            PrototypingDirectory = Path.Combine(selectedProductDirectoryPath, "ЭМ");
            NoticeDirectory = Path.Combine(selectedProductDirectoryPath, "Извещения");

            kdExplorer.UpdateFileList();
            prototypingExplorer.UpdateFileList();
            noticeExplorer.UpdateFileList();
        }

        private void UpdateProductList()
        {
            string[] paths = Directory.GetDirectories(Settings.WorkingDirectoryPath);

            DirectoryInfo[] dirs = new DirectoryInfo[paths.Length];

            for (int i = 0; i < paths.Length; i++)
            {
                dirs[i] = new DirectoryInfo(paths[i]);
            }

            foreach (var dir in dirs)
            {
                ProductInfo product = new ProductInfo()
                {
                    ProductName = dir.Name,
                    ProductDirectoryPath = dir.FullName,
                };
                productList.Items.Add(product);
            }
        }

        private void MenuItem_Settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.Show();
        }
    }
}
