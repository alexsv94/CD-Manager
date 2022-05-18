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
        private ProductInfo _selectedProduct;
        public ProductInfo? SelectedProduct 
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value!;
                OnSelectedCurrentProductChanged();
            }
        }                
        public List<ProductInfo> Products { get; set; } = new();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            UpdateProductList();
        }

        private void OnSelectedCurrentProductChanged()
        {
            string selectedProductDirectoryPath = SelectedProduct!.ProductDirectoryPath!;
            Settings.CurrentProductDirectoryPath = selectedProductDirectoryPath;
        }

        private void UpdateProductList()
        {
            string[] paths = Directory.GetDirectories(Settings.WorkingDirectoryPath);

            DirectoryInfo[] dirs = new DirectoryInfo[paths.Length];

            for (int i = 0; i < paths.Length; i++)
            {
                dirs[i] = new DirectoryInfo(paths[i]);
            }

            Products.Clear();

            foreach (var dir in dirs)
            {
                ProductInfo product = new ProductInfo()
                {
                    ProductName = dir.Name,
                    ProductDirectoryPath = dir.FullName,
                };
                Products.Add(product);
            }
        }

        private void MenuItem_Settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.Show();
        }
    }
}
