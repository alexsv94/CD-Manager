using OrganizerWpf.Commands;
using OrganizerWpf.Models;
using OrganizerWpf.Windows.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizerWpf.Windows.MainWindow
{
    public class MainWindowViewModel
    {
        private ProductInfo? _selectedProduct;
        public ProductInfo? SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnCurrentProductChanged();
            }
        }
        public List<ProductInfo> Products { get; set; } = new List<ProductInfo>();

        private string _kdDirectory = String.Empty;
        public string KDDirectory {
            get => _kdDirectory;
            set
            {
                _kdDirectory = value;
                KDDirectoryChanged?.Invoke(value);
            }
        }
        
        private string _prototypingDirectory = String.Empty;
        public string PrototypingDirectory 
        {
            get => _prototypingDirectory;
            set
            {
                _prototypingDirectory = value;
                PrototypingDirectoryChanged?.Invoke(value);
            }
        }

        private string _noticeDirectory = String.Empty;
        public string NoticeDirectory 
        {
            get => _noticeDirectory;
            set
            {
                _noticeDirectory = value;
                NoticeDirectoryChanged?.Invoke(value);
            }
        }

        #region Commands
        private RelayCommand? _openSettingsWindowCommand = null;
        public RelayCommand OpenSettingsWindowCommand
        {
            get => _openSettingsWindowCommand ??= new RelayCommand(obj =>
                {
                    SettingsWindow settingsWindow = new SettingsWindow();
                    settingsWindow.Show();
                });
        }
        #endregion

        #region
        public event Action<string>? KDDirectoryChanged;
        public event Action<string>? PrototypingDirectoryChanged;
        public event Action<string>? NoticeDirectoryChanged;
        #endregion

        public MainWindowViewModel()
        {
            if (!string.IsNullOrEmpty(ApplicationSettings.WorkingDirectoryPath))
            {
                UpdateProductList(ApplicationSettings.WorkingDirectoryPath);                
                if (Products.Count > 0) 
                    SelectedProduct = Products.First();
            }

            ApplicationSettings.WorkingDirectoryChanged += UpdateProductList;
        }

        private void OnCurrentProductChanged()
        {
            if (SelectedProduct == null) return;

            string selectedProductDirectoryPath = SelectedProduct.ProductDirectoryPath!;
            ApplicationSettings.CurrentProductDirectoryPath = selectedProductDirectoryPath;

            KDDirectory = Path.Combine(selectedProductDirectoryPath, "КД");
            PrototypingDirectory = Path.Combine(selectedProductDirectoryPath, "ЭМ");
            NoticeDirectory = Path.Combine(selectedProductDirectoryPath, "Извещения");
        }

        private void UpdateProductList(string workingDir)
        {
            Products.Clear();

            string[] paths = Directory.GetDirectories(workingDir);

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
                Products.Add(product);
            }
        }
    }
}
