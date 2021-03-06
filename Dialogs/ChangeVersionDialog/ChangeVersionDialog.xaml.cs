using OrganizerWpf.Utilities;
using OrganizerWpf.Windows;
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
    public partial class ChangeVersionDialog : Window
    { 
        public ChangeVersionDialogViewModel? ViewModel { get; set; }

        private WindowEventsHelper? _eventsHelper;
        public ChangeVersionDialog()
        {
            InitializeComponent();
            SetupViewModel();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _eventsHelper ??= new(this, false, false);
        }

        public void SetupViewModel()
        {
            if (ViewModel == null)
            {
                ViewModel = new();

                documentPathRegion.DragEnter += ViewModel.OnDragEnter;
                documentPathRegion.DragLeave += ViewModel.OnDragLeave;
                documentPathRegion.Drop += ViewModel.OnDrop;
            }           

            DataContext = ViewModel;
        }
        private void DialogOK(object sender, RoutedEventArgs e)
        {
            if (ViewModel!.ApplyChanges())
                DialogResult = true;
        }
        private void DialogCancel(object sender, RoutedEventArgs e) 
        { 
            DialogResult = false; 
        }

    }
}
