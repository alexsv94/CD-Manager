using OrganizerWpf.Utilities;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;

namespace OrganizerWpf.Windows.SettingsW
{
    public partial class SettingsWindow : Window, IView<SettingsWindowViewModel>
    {
        public SettingsWindowViewModel? ViewModel { get; set; }
        private WindowEventsHelper _eventsHelper;

        public SettingsWindow()
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
                ViewModel = new();

            DataContext = ViewModel;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            ViewModel?.OnClose(e);
        }
    }
}
