using OrganizerWpf.Models;
using OrganizerWpf.Utilities;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace OrganizerWpf.Dialogs.ProductsChooseDialog
{
    /// <summary>
    /// Логика взаимодействия для ProductsChooseDialog.xaml
    /// </summary>
    public partial class ProductsChooseDialog : Window
    {
        public List<ProductModel>? FilteredProducts
        {
            get { return (List<ProductModel>)GetValue(FilteredProductsProperty); }
            set { SetValue(FilteredProductsProperty, value); }
        }
        public static readonly DependencyProperty FilteredProductsProperty =
            DependencyProperty.Register("FilteredProducts", typeof(List<ProductModel>), typeof(ProductsChooseDialog), new PropertyMetadata(null));

        public List<ProductModel> ExcludedProducts = new();
        public List<ProductModel> ChosenProducts = new();

        private List<ProductModel>? _products = null;
        private WindowEventsHelper _eventsHelper;

        public ProductsChooseDialog()
        {
            InitializeComponent();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _eventsHelper = new(this, false, false);
            GetProducts();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace((sender as TextBox)!.Text))
            {
                FilteredProducts = _products!.Where(x => x.Name!.ToLower().Contains((sender as TextBox)!.Text.ToLower())).ToList();
            }
            else
            {
                FilteredProducts = _products;
            }
        }

        private void GetProducts()
        {
            _products = FilteredProducts = FileSystemHelper.GetSerializedDirs<ProductModel>(Settings.WorkingDirectoryPath);

            _products = FilteredProducts = _products.Where(x => !ExcludedProducts.Any(y => y.Name == x.Name)).ToList();
            ExcludedProducts.Clear();
        }

        private void ButtonApply_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in ProductsDataGrid.SelectedItems)
            {
                ChosenProducts.Add((ProductModel)item);
            }
            DialogResult = true;
            Close();
        }
    }
}
