using OrganizerWpf.StylizedControls.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OrganizerWpf.Utilities
{
    public class WindowEventsHelper
    {
        private readonly Window _targetWindow;
        private bool _isDragBeginInTitleBar = false;

        public WindowEventsHelper(Window window, bool minimizeBtnVisible, bool maximizeBtnVisible)
        {
            _targetWindow = window;

            var test = window.Template.FindName("TitleBar", window);
            Border titleBar = (Border)window.Template.FindName("TitleBar", window);

            WindowControlButton closeBtn = (WindowControlButton)window.Template.FindName("CloseButton", window);
            WindowControlButton minimizeBtn = (WindowControlButton)window.Template.FindName("MinimizeButton", window);
            WindowControlButton maximizeBtn = (WindowControlButton)window.Template.FindName("MaximizeButton", window);

            titleBar.MouseMove += WindowDragMove;
            titleBar.MouseDown += BeginDrag;
            titleBar.MouseLeave += EndDrag;

            closeBtn.Click = CloseWindow;

            if (minimizeBtnVisible)
            {
                minimizeBtn.Click = MinimizeWindow;
            }
            else
            {
                minimizeBtn.Visibility = Visibility.Hidden;
            }

            if (minimizeBtnVisible)
            {
                maximizeBtn.Click = MaximizeOrRestoreWindow;
            }
            else
            {
                maximizeBtn.Visibility = Visibility.Hidden;
            }            
        }

        private void WindowDragMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && _isDragBeginInTitleBar)
            {
                _targetWindow?.DragMove();
            }
        }

        private void CloseWindow()
        {
            _targetWindow?.Close();
        }

        private void MaximizeOrRestoreWindow()
        {
            if (_targetWindow.WindowState == WindowState.Normal)
            {
                _targetWindow.WindowState = WindowState.Maximized;
            }
            else if (_targetWindow.WindowState == WindowState.Maximized)
            {
                _targetWindow.WindowState = WindowState.Normal;
            }
        }

        private void MinimizeWindow()
        {
            _targetWindow.WindowState = WindowState.Minimized;
        }

        private void BeginDrag(object sender, MouseButtonEventArgs e)
        {
            _isDragBeginInTitleBar = true;
        }

        private void EndDrag(object sender, MouseEventArgs e)
        {
            _isDragBeginInTitleBar = false;
        }
    }
}
