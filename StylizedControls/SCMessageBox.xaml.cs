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
using System.Windows.Shapes;

namespace OrganizerWpf.StylizedControls
{
    public partial class SCMessageBox : Window
    {
        private bool _isDragBeginInTitleBar = false;

        public SCMessageBox()
        {
            InitializeComponent();

            //Поиск ресурса
            //(<Type>)Application.Current.FindResource("<Resource Name>");
        }

        private void WindowDragMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && _isDragBeginInTitleBar)
            {
                DragMove();
            }
        }

        private void CloseWindow()
        {
            Close();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _isDragBeginInTitleBar = true;
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            _isDragBeginInTitleBar = false;
        }
    }
}
