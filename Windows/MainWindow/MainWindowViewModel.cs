using OrganizerWpf.Models;
using OrganizerWpf.StylizedControls;
using OrganizerWpf.Utilities;
using OrganizerWpf.Utilities.Extensions;
using OrganizerWpf.ViewModels;
using OrganizerWpf.Windows.NoticeCreate;
using OrganizerWpf.Windows.SettingsW;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        private ObservableCollection<ProductModel> _filteredProducts = new();
        public ObservableCollection<ProductModel> FilteredProducts
        {
            get => _filteredProducts;
            set
            {
                _filteredProducts = value;
                OnPropertyChanged(nameof(FilteredProducts));
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

        private RelayCommand? _createNoticeCommand = null;
        public RelayCommand CreateNoticeCommand
        {
            get
            {
                return _createNoticeCommand ??=
                    new RelayCommand(obj => OpenCreateNoticeWindow());
            }
        }


        private RelayCommand? _createActCommand = null;
        public RelayCommand CreateActCommand
        {
            get
            {
                return _createActCommand ??=
                    new RelayCommand(obj => _window!.Close());
            }
        }
        #endregion

        private List<ProductModel> _products = new();
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
        private void OpenCreateNoticeWindow()
        {
            new NoticeCreateForm().Show();
        }

        private void OnWorkingDirectoryChanged(string newPath)
        {
            _workingDir = newPath;

            if (!new DirectoryInfo(newPath).GetDirectories().Any(x => x.Name == "Извещения"))
            {
                var result = SCMessageBox.ShowMsgBox("Создать каталог \"Извещения\"? Это необходимо для корректной работы программы.",
                    "Каталог \"Извещения\" не найден",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == SCMessageBoxResult.Yes)
                {
                    Directory.CreateDirectory(Path.Combine(newPath, "Извещения"));
                }
            }

            UpdateProductList();
        }

        #region Handlers
        private void OnSelectedCurrentProductChanged()
        {
            if (SelectedProduct == null) return;

            string selectedProductDirectoryPath = SelectedProduct.FullPath!;
            Settings.CurrentProductDirectoryPath = selectedProductDirectoryPath;
        }
        private void UpdateProductList()
        {
            if (string.IsNullOrEmpty(_workingDir)) return;
            _products = FileSystemHelper.GetSerializedDirs<ProductModel>(_workingDir);
            _products.Remove(_products.FirstOrDefault(x => x.Name == "Извещения")!);
            FilteredProducts.ReplaceItems(_products);
        }

        public void OnFilterValueChanged(object sender)
        {
            if (!string.IsNullOrWhiteSpace((sender as TextBox)!.Text))
            {
                FilteredProducts.ReplaceItems(_products!.Where(x => x.ShortName.ToLower().Contains((sender as TextBox)!.Text.ToLower())));
            }
            else
            {
                FilteredProducts.ReplaceItems(_products);
            }            
        }
        #endregion
    }
}
