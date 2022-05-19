using System.Windows;

namespace OrganizerWpf.Windows.MainWindow
{
    public partial class MainWindow : Window, IView<MainWindowViewModel>
    {
        public MainWindowViewModel? ViewModel { get; set; } = null;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += SetupViewModel;
        }

        public void SetupViewModel(object sender, RoutedEventArgs e)
        {
            if (ViewModel == null)
                ViewModel = new(this);

            DataContext = ViewModel;
        }
    }
}
