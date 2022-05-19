using OrganizerWpf.Utilities;
using System.Windows;
using System.Windows.Forms;

namespace OrganizerWpf.Windows.SettingsW
{
    public partial class SettingsWindow : Window, IView<SettingsWindowViewModel>
    {
        public SettingsWindowViewModel? ViewModel { get; set; }

        public SettingsWindow()
        {
            InitializeComponent();
            Loaded += SetupViewModel;
        }

        public void SetupViewModel(object sender, RoutedEventArgs e)
        {
            if (ViewModel == null)
                ViewModel = new();

            Closed += ViewModel.OnClose;

            DataContext = ViewModel;
        }
    }
}
