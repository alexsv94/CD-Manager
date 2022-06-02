using OrganizerWpf.StylizedControls;
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

namespace OrganizerWpf.Dialogs.AddExtendToRowDialog
{
    public partial class AddExtendToRowDialog : Window
    {
        public string ProductName
        {
            get { return (string)GetValue(ProductNameProperty); }
            set { SetValue(ProductNameProperty, value); }
        }
        public static readonly DependencyProperty ProductNameProperty =
            DependencyProperty.Register("ProductName", typeof(string), typeof(AddExtendToRowDialog), new PropertyMetadata(""));

        public string DecNumber
        {
            get { return (string)GetValue(DecNumberProperty); }
            set { SetValue(DecNumberProperty, value); }
        }
        public static readonly DependencyProperty DecNumberProperty =
            DependencyProperty.Register("DecNumber", typeof(string), typeof(AddExtendToRowDialog), new PropertyMetadata(""));

        private WindowEventsHelper _eventsHelper;

        public AddExtendToRowDialog()
        {
            InitializeComponent();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _eventsHelper = new(this, false, false);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ProductName) || string.IsNullOrWhiteSpace(DecNumber))
            {
                SCMessageBox.ShowMsgBox("Заполните все поля", "Ошибка добавления строки", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DialogResult = true;
        }
    }
}
