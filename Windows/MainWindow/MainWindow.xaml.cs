using OrganizerWpf.StylizedControls;
using OrganizerWpf.Utilities;
using OrganizerWpf.Utilities.Types;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OrganizerWpf.Windows.MainWindow
{
    public partial class MainWindow : Window
    {
        public MainWindowViewModel? ViewModel { get; set; } = null;
        private WindowEventsHelper? _eventsHelper;

        public MainWindow()
        {
            InitializeComponent();
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            Loaded += SetupViewModel;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _eventsHelper ??= new(this, true, true);
        }

        public void SetupViewModel(object sender, RoutedEventArgs e)
        {
            if (ViewModel == null)
            {
                ViewModel = new(this);
                NoticeFilter.IntervalChanged = NoticeExplorer.ViewModel!.OnIntervalChanged;
            }

            DataContext = ViewModel;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel!.OnFilterValueChanged(sender);
        }
    }
}
