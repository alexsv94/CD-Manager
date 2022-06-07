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
    public partial class SCDateIntervalPicker : UserControl
    {
        public DateTime StartDate
        {
            get { return (DateTime)GetValue(StartDateProperty); }
            set { SetValue(StartDateProperty, value); }
        }
        public static readonly DependencyProperty StartDateProperty =
            DependencyProperty.Register("StartDate", typeof(DateTime), typeof(SCDateIntervalPicker), new PropertyMetadata(DateTime.MinValue));

        public DateTime EndDate
        {
            get { return (DateTime)GetValue(EndDateProperty); }
            set { SetValue(EndDateProperty, value); }
        }
        public static readonly DependencyProperty EndDateProperty =
            DependencyProperty.Register("EndDate", typeof(DateTime), typeof(SCDateIntervalPicker), new PropertyMetadata(DateTime.Now));

        public Action<DateTime, DateTime>? IntervalChanged;


        public SCDateIntervalPicker()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CalendarPopup.IsOpen = true;
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            StartDate = DateTime.MinValue;
            EndDate = DateTime.Now;

            RootButton.Content = "Интервал";
            IntervalChanged?.Invoke(StartDate, EndDate);
            CalendarPopup.IsOpen = false;
        }

        private void SetButton_Click(object sender, RoutedEventArgs e)
        {
            if (StartDate > EndDate)
            {
                SCMessageBox.ShowMsgBox("Начальная дата интервала не может быть позже конечной.",
                    "Ошибка выбора даты",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            
            RootButton.Content = $"<{StartDate.ToShortDateString()} - {EndDate.ToShortDateString()}>";
            IntervalChanged?.Invoke(StartDate, EndDate);

            CalendarPopup.IsOpen = false;
        }
    }
}
