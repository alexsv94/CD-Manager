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
    public partial class SCBrowseTextBox : UserControl
    {
        public string Path
        {
            get { return (string)GetValue(PathProperty); }
            set { SetValue(PathProperty, value); }
        }
        public static readonly DependencyProperty PathProperty =
            DependencyProperty.Register("Path", typeof(string), typeof(SCBrowseTextBox), new PropertyMetadata(string.Empty));

        public ICommand BrowseButtonClick
        {
            get { return (ICommand)GetValue(BrowseButtonClickProperty); }
            set { SetValue(BrowseButtonClickProperty, value); }
        }
        public static readonly DependencyProperty BrowseButtonClickProperty =
            DependencyProperty.Register("BrowseButtonClick", typeof(ICommand), typeof(SCBrowseTextBox), new PropertyMetadata(null));

        public Action PathChanged
        {
            get { return (Action)GetValue(PathChangedProperty); }
            set { SetValue(PathChangedProperty, value); }
        }
        public static readonly DependencyProperty PathChangedProperty =
            DependencyProperty.Register("PathChanged", typeof(Action), typeof(SCBrowseTextBox), new PropertyMetadata(null));


        public SCBrowseTextBox()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PathChanged?.Invoke();
        }
    }
}
