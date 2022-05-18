using OrganizerWpf.Utilities;
using System.Windows;
using System.Windows.Forms;

namespace OrganizerWpf.Windows.SettingsW
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void workDirBrowseBtn_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Settings.WorkingDirectoryPath = dialog.SelectedPath;
                    workDirTextBox.Text = dialog.SelectedPath;
                }
            }
        }
    }
}
