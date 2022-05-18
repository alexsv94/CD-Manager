using OrganizerWpf.Models;
using OrganizerWpf.UserControls.DocumentsExplorer;
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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace OrganizerWpf.UserControls.NoticeExplorer
{
    /// <summary>
    /// Логика взаимодействия для NoticeExplorer.xaml
    /// </summary>
    public partial class NoticeExplorer : UserControl
    {
        #region Dependency Props
        public string TargetDirectory
        {
            get { return (string)GetValue(TargetDirectoryProperty); }
            set { SetValue(TargetDirectoryProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TargetDirectory.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TargetDirectoryProperty =
            DependencyProperty.Register("TargetDirectory", typeof(string), typeof(NoticeExplorer), new PropertyMetadata(""));

        public List<NoticeInfo> Notices
        {
            get { return (List<NoticeInfo>)GetValue(DocumentsProperty); }
            set { SetValue(DocumentsProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Documents.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DocumentsProperty =
            DependencyProperty.Register("Notices", typeof(List<NoticeInfo>), typeof(NoticeExplorer), new PropertyMetadata(null));
        #endregion

        
        private bool _initCompleted;
        private FilesystemHelper? _fsHelper;
        private string _directoryPath = string.Empty;

        public NoticeExplorer()
        {
            InitializeComponent();
            dataGrid.DataContext = this;

            Settings.CurrentProductDirectoryChanged += OnDirectoryChanged;
        }

        private void OnDirectoryChanged(string newDir)
        {
            _directoryPath = Path.Combine(newDir, TargetDirectory);
            UpdateFileList();
        }

        private void UpdateFileList()
        {
            if (!_initCompleted)
                Initialize();

            Notices = _fsHelper!.GetNotices(_directoryPath);
        }

        private void Initialize()
        {
            _fsHelper = new FilesystemHelper();
            _initCompleted = true;
        }

        private void Border_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
                dropLabel.Visibility = Visibility.Visible;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void Border_DragLeave(object sender, DragEventArgs e)
        {
            dropLabel.Visibility = Visibility.Collapsed;
        }

        private void Border_Drop(object sender, DragEventArgs e)
        {
            dropLabel.Visibility = Visibility.Collapsed;

            string[] droppedFiles = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (Notices != null && Notices.Any(x => droppedFiles[0] == x.FilePath))
                return;

            var filePaths = _fsHelper!.GetDroppedFiles(droppedFiles);
            _fsHelper.CopyFiles(_directoryPath, filePaths);

            UpdateFileList();
        }

        private void dataGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.MouseDevice.LeftButton == MouseButtonState.Pressed && dataGrid.SelectedItem != null)
            {
                NoticeInfo itemInfo = (NoticeInfo)dataGrid.SelectedItem;

                IDataObject dragObject = new DataObject(DataFormats.FileDrop, new string[] { itemInfo.FilePath! });
                DragDrop.DoDragDrop(dataGrid, dragObject, DragDropEffects.Copy);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            NoticeInfo selectedFile = (NoticeInfo)dataGrid.SelectedItem;

            if (MessageBox.Show($"Удалить файл {selectedFile.DocName} без возможности восстановления?",
                    "Удаление файла",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning,
                    MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                File.Delete(selectedFile.FilePath!);
            }

            UpdateFileList();
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                _fsHelper!.OpenFile(((NoticeInfo)dataGrid.SelectedItem).FilePath!);
            }
        }
    }
}
