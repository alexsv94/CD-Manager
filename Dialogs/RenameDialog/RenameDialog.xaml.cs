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

namespace OrganizerWpf.Dialogs.RenameDialog
{
    /// <summary>
    /// Логика взаимодействия для RenameDialog.xaml
    /// </summary>
    public partial class RenameDialog : Window
    {
        public string OldFileName { get; set; }

        public string NewFileName
        {
            get { return (string)GetValue(NewFileNameProperty); }
            set { SetValue(NewFileNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NewFileName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NewFileNameProperty =
            DependencyProperty.Register("NewFileName", typeof(string), typeof(RenameDialog), new PropertyMetadata(""));


        public RenameDialog()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
