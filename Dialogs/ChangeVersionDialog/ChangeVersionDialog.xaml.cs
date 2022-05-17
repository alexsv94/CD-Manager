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

namespace OrganizerWpf.Dialogs.ChangeVersionDialog
{
    /// <summary>
    /// Логика взаимодействия для ChangeVersionDialog.xaml
    /// </summary>
    public partial class ChangeVersionDialog : Window
    {
        public string? OldVersion { get; set; }

        public string NewVersion
        {
            get { return (string)GetValue(NewVersionProperty); }
            set { SetValue(NewVersionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NewFileName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NewVersionProperty =
            DependencyProperty.Register("NewVersion", typeof(string), typeof(ChangeVersionDialog), new PropertyMetadata(""));

        public ChangeVersionDialog()
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
