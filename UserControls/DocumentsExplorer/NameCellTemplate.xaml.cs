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

namespace OrganizerWpf.UserControls.DocumentsExplorer
{
    /// <summary>
    /// Логика взаимодействия для NameCellTemplate.xaml
    /// </summary>
    public partial class NameCellTemplate : UserControl
    {
        public string DocName
        {
            get { return (string)GetValue(DocNameProperty); }
            set { SetValue(DocNameProperty, value); }
        }
        public static readonly DependencyProperty DocNameProperty =
            DependencyProperty.Register("DocName", typeof(string), typeof(NameCellTemplate), new PropertyMetadata(""));

        public ImageSource? Image
        {
            get { return (ImageSource?)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }
        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(ImageSource), typeof(NameCellTemplate), new PropertyMetadata(null));

        public NameCellTemplate()
        {
            InitializeComponent();
        }
    }
}
