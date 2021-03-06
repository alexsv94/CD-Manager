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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OrganizerWpf.StylizedControls.Controls
{
    /// <summary>
    /// Логика взаимодействия для WindowControlButton.xaml
    /// </summary>
    public partial class WindowControlButton : UserControl
    {
        public ImageSource Image
        {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }
        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(ImageSource), typeof(WindowControlButton), new PropertyMetadata(null));

        public Action Click
        {
            get { return (Action)GetValue(ClickProperty); }
            set { SetValue(ClickProperty, value); }
        }
        public static readonly DependencyProperty ClickProperty =
            DependencyProperty.Register("Click", typeof(Action), typeof(WindowControlButton), new PropertyMetadata(null));


        public bool IsCloseButton
        {
            get { return (bool)GetValue(IsCloseButtonProperty); }
            set { SetValue(IsCloseButtonProperty, value); }
        }
        public static readonly DependencyProperty IsCloseButtonProperty =
            DependencyProperty.Register("IsCloseButton", typeof(bool), typeof(WindowControlButton), new PropertyMetadata(false));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(WindowControlButton), new PropertyMetadata(default(CornerRadius)));

        private bool _isMousePressedOnThis = false;

        public WindowControlButton()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _isMousePressedOnThis = true;
            e.Handled = true;
        }


        private void Border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_isMousePressedOnThis)
            {
                Click?.Invoke();
                _isMousePressedOnThis = false;
            }
            e.Handled = true;
        }

        private void Border_MouseMove(object sender, MouseEventArgs e)
        {
            e.Handled = true;
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            _isMousePressedOnThis = false;
            BorderRoot.Background = new SolidColorBrush(Colors.Transparent);

            e.Handled = true;
        }
        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            if (IsCloseButton)
            {
                BorderRoot.Background = new SolidColorBrush(Color.FromRgb(219, 55, 55));
            }
            else
            {
                BorderRoot.Background = new SolidColorBrush(Color.FromRgb(43, 52, 58));
            }
            e.Handled = true;
        }
    }
}
