using OrganizerWpf.Utilities.Types;
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
        public DateInterval Interval
        {
            get { return (DateInterval)GetValue(IntervalProperty); }
            set { SetValue(IntervalProperty, value); }
        }
        public static readonly DependencyProperty IntervalProperty =
            DependencyProperty.Register("Interval", typeof(DateInterval), typeof(SCDateIntervalPicker), new PropertyMetadata(new DateInterval()));

        public Action<DateInterval>? IntervalChanged
        {
            get { return (Action<DateInterval>?)GetValue(IntervalChangedProperty); }
            set { SetValue(IntervalChangedProperty, value); }
        }
        public static readonly DependencyProperty IntervalChangedProperty =
            DependencyProperty.Register("IntervalChanged", typeof(Action<DateInterval>), typeof(SCDateIntervalPicker), new PropertyMetadata(null));


        public SCDateIntervalPicker()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CalendarPopup.IsOpen = !CalendarPopup.IsOpen;
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            Interval.SetDefault();

            RootButton.Content = "Интервал";
            IntervalChanged?.Invoke(Interval);
            CalendarPopup.IsOpen = false;
        }

        private void SetButton_Click(object sender, RoutedEventArgs e)
        {
            if (Interval.StartDate > Interval.EndDate)
            {
                SCMessageBox.ShowMsgBox("Начальная дата интервала не может быть позже конечной.",
                    "Ошибка выбора даты",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            
            RootButton.Content = $"<{Interval.StartDate.ToShortDateString()} - {Interval.EndDate.ToShortDateString()}>";
            IntervalChanged?.Invoke(Interval);

            CalendarPopup.IsOpen = false;
        }
    }    
}