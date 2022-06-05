using OrganizerWpf.Dialogs.RecentDocumentsDialog;
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

namespace OrganizerWpf.Windows.NoticeCreate
{
    /// <summary>
    /// Логика взаимодействия для NoticeCreateForm.xaml
    /// </summary>
    public partial class NoticeCreateForm : Window, IView<NoticeCreateFormViewModel>
    {
        public NoticeCreateFormViewModel? ViewModel { get; set; }

        private WindowEventsHelper _eventsHelper;
        public NoticeCreateForm()
        {
            InitializeComponent();
            Loaded += SetupViewModel;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _eventsHelper = new(this, false, false);
        }

        public void SetupViewModel(object sender, RoutedEventArgs e)
        {
            if (ViewModel == null)
            {
                ViewModel = new(progressBar);
                DataContext = ViewModel;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Title = "Извещение " + (sender as TextBox)!.Text;
        }

        private void ExtendToItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ViewModel!.OnExtendToItemMouseDoubleClick(sender);
        }

        private void ChangesListItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ViewModel!.OnChangesListItemMouseDoubleClick(sender);
        }
    }
}
