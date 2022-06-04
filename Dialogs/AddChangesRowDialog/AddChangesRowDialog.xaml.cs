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

namespace OrganizerWpf.Dialogs.AddChangesRowDialog
{
    public partial class AddChangesRowDialog : Window
    {
        public string DocName
        {
            get { return (string)GetValue(DocNameProperty); }
            set { SetValue(DocNameProperty, value); }
        }
        public static readonly DependencyProperty DocNameProperty =
            DependencyProperty.Register("DocName", typeof(string), typeof(AddChangesRowDialog), new PropertyMetadata(""));

        public string OldVersion
        {
            get { return (string)GetValue(OldVersionProperty); }
            set { SetValue(OldVersionProperty, value); }
        }
        public static readonly DependencyProperty OldVersionProperty =
            DependencyProperty.Register("OldVersion", typeof(string), typeof(AddChangesRowDialog), new PropertyMetadata(""));

        public string NewVersion
        {
            get { return (string)GetValue(NewVersionProperty); }
            set { SetValue(NewVersionProperty, value); }
        }
        public static readonly DependencyProperty NewVersionProperty =
            DependencyProperty.Register("NewVersion", typeof(string), typeof(AddChangesRowDialog), new PropertyMetadata(""));

        private WindowEventsHelper _eventsHelper;

        public AddChangesRowDialog()
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
            if (string.IsNullOrWhiteSpace(DocName) || 
                string.IsNullOrWhiteSpace(OldVersion) || 
                string.IsNullOrWhiteSpace(NewVersion))
            {
                SCMessageBox.ShowMsgBox("Заполните все поля", "Ошибка добавления строки", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DialogResult = true;
        }
    }
}
