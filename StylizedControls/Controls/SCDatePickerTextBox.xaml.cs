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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OrganizerWpf.StylizedControls.Controls
{
    /// <summary>
    /// Логика взаимодействия для SCDatePickerTextBox.xaml
    /// </summary>
    public partial class SCDatePickerTextBox : UserControl
    {
        public DateTime Date
        {
            get { return (DateTime)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }
        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register("Date", typeof(DateTime), typeof(SCDatePickerTextBox), new PropertyMetadata(null));



        public SCDatePickerTextBox()
        {
            InitializeComponent();
            Date = DateTime.Now;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CalendarPopup.IsOpen = !CalendarPopup.IsOpen;
        }

        private void DateCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            Date = DateCalendar.SelectedDates[0];
        }
    }
}
