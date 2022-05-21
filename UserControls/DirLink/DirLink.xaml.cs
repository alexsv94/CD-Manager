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

namespace OrganizerWpf.UserControls.DirLink
{    
    public partial class DirLink : UserControl
    {
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(DirLink), new PropertyMetadata(string.Empty));

        public string DirPath { get; set; } = string.Empty;
        public Action<string>? ClickCallback { get; set; }

        public DirLink()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Label_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ClickCallback?.Invoke(DirPath);
            StackPanel parent = (VisualTreeHelper.GetParent(this) as StackPanel)!;

            int childIndex = parent.Children.IndexOf(this);
            parent.Children.RemoveRange(childIndex + 1, parent.Children.Count - childIndex);

            /*while (parent.Children.Count > childIndex + 1)
            {
                parent.Children.RemoveAt(parent.Children.Count - 1);
            }*/
        }
    }
}
