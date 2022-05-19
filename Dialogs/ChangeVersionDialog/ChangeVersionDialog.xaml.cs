using OrganizerWpf.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OrganizerWpf.Dialogs.ChangeVersionDialog
{
    /// <summary>
    /// Логика взаимодействия для ChangeVersionDialog.xaml
    /// </summary>
    public partial class ChangeVersionDialog : Window
    {
        public string? OldVersion { get; set; }
        public string? NewVersion
        {
            get { return (string)GetValue(NewVersionProperty); }
            set { SetValue(NewVersionProperty, value); }
        }
        public static readonly DependencyProperty NewVersionProperty =
            DependencyProperty.Register("NewVersion", typeof(string), typeof(ChangeVersionDialog), new PropertyMetadata(""));

        public string? NoticeFilePath
        {
            get { return (string)GetValue(NoticeFilePathProperty); }
            set { SetValue(NoticeFilePathProperty, value); }
        }
        public static readonly DependencyProperty NoticeFilePathProperty =
            DependencyProperty.Register("NoticeFilePath", typeof(string), typeof(ChangeVersionDialog), new PropertyMetadata(""));

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

        private void noticeBrowseBtn_Click(object sender, RoutedEventArgs e)
        {
            using var dialog = new OpenFileDialog();
            dialog.InitialDirectory = Path.Combine(Settings.CurrentProductDirectoryPath, "Извещения");

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                NoticeFilePath = dialog.FileName;
            }
        }
    }
}
