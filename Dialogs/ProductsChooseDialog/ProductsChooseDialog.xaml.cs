using OrganizerWpf.Models;
using OrganizerWpf.Utilities;
using OrganizerWpf.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ObservableCollection<ProductModel> FilteredProducts
        {
            get { return (ObservableCollection<ProductModel>)GetValue(FilteredProductsProperty); }
            set { SetValue(FilteredProductsProperty, value); }
        }
        public static readonly DependencyProperty FilteredProductsProperty =
            DependencyProperty.Register("FilteredProducts", 
                typeof(ObservableCollection<ProductModel>), 
                typeof(ProductsChooseDialog), 
                new PropertyMetadata(new ObservableCollection<ProductModel>()));

        public List<ProductModel> ExcludedProducts = new();
        public List<ProductModel> ChosenProducts = new();

        private List<ProductModel> _allProducts = new();
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
                FilteredProducts.ReplaceItems(_allProducts!.Where(x => x.ShortName.ToLower().Contains((sender as TextBox)!.Text.ToLower())));
            }
            else
            {
                FilteredProducts.ReplaceItems(_allProducts);
            }
        }

        private void GetProducts()
        {
            _allProducts = FileSystemHelper.GetSerializedDirs<ProductModel>(Settings.WorkingDirectoryPath);
            _allProducts = _allProducts.Where(x => !ExcludedProducts.Any(y => y.ShortName == x.ShortName)).ToList();

            var noticeFolder = _allProducts.FirstOrDefault(x => x.Name == "Извещения");
            if (noticeFolder != null) _allProducts.Remove(noticeFolder);

            FilteredProducts.ReplaceItems(_allProducts);

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
