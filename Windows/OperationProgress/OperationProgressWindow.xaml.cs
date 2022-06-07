using OrganizerWpf.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OrganizerWpf.Windows.OperationProgress
{
    public partial class OperationProgressWindow : Window
    {
        public AsyncOperation Operation
        {
            get { return (AsyncOperation)GetValue(OperationProperty); }
            set { SetValue(OperationProperty, value); }
        }
        public static readonly DependencyProperty OperationProperty =
            DependencyProperty.Register("Operation", typeof(AsyncOperation), typeof(OperationProgressWindow), new PropertyMetadata(null));

        private WindowEventsHelper _eventsHelper;

        public OperationProgressWindow()
        {
            InitializeComponent();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _eventsHelper = new(this, false, false);
        }

        public static void ShowProgressWindow(AsyncOperation operation, string title)
        {
            var window = new OperationProgressWindow();
            window.Operation = operation;
            window.Title = title;
            window.Show();
        }

        private async void ProgressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if ((sender as ProgressBar)!.Value >= 100)
            {
                await Task.Delay(100);
                Close();
            }
        }
    }
}
