using OrganizerWpf.Models;
using OrganizerWpf.Utilities;
using OrganizerWpf.ViewModels;
using OrganizerWpf.Windows.SettingsW;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OrganizerWpf.Windows.MainWindow
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Binding Props
        private ProductModel? _selectedProduct = null;
        public ProductModel? SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value!;
                OnSelectedCurrentProductChanged();
                OnPropertyChanged(nameof(SelectedProduct));
            }
        }

        private List<ProductModel>? _products = null;
        public List<ProductModel>? Products 
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
            }
        }
        #endregion

        #region Commands
        private RelayCommand? _openSettingsWindowCommand = null;
        public RelayCommand OpenSettingsWindowCommand
        {
            get
            {
                return _openSettingsWindowCommand ??= 
                    new RelayCommand(obj => new SettingsWindow().Show());
            }
        }

        private RelayCommand? _closeWindowCommand = null;
        public RelayCommand CloseWindowCommand
        {
            get
            {
                return _closeWindowCommand ??=
                    new RelayCommand(obj => _window!.Close());
            }
        }
        #endregion

        private string _workingDir = string.Empty;
        private readonly Window? _window = null;

        public MainWindowViewModel(Window window)
        {
            _window = window;

            SettingsLoader.Load();

            Settings.WorkingDirectoryChanged += OnWorkingDirectoryChanged;

            if (!string.IsNullOrEmpty(Settings.WorkingDirectoryPath))
                OnWorkingDirectoryChanged(Settings.WorkingDirectoryPath);
        }

        private void OnWorkingDirectoryChanged(string newPath)
        {
            _workingDir = newPath;
            UpdateProductList();
        }

        #region Handlers
        private void OnSelectedCurrentProductChanged()
        {
            if (SelectedProduct == null) return;

            string selectedProductDirectoryPath = SelectedProduct.ProductDirectoryPath!;
            Settings.CurrentProductDirectoryPath = selectedProductDirectoryPath;
        }
        private void UpdateProductList()
        {
            if (string.IsNullOrEmpty(_workingDir)) return;
            
            string[] paths = Directory.GetDirectories(_workingDir);

            DirectoryInfo[] dirs = new DirectoryInfo[paths.Length];

            for (int i = 0; i < paths.Length; i++)
            {
                dirs[i] = new DirectoryInfo(paths[i]);
            }

            var productsList = new List<ProductModel>();

            foreach (var dir in dirs)
            {
                ProductModel product = new ProductModel()
                {
                    ProductName = dir.Name,
                    ProductDirectoryPath = dir.FullName,
                };
                productsList.Add(product);
            }

            Products = productsList;
        }
        #endregion
    }
}
