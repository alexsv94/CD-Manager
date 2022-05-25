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

        #region Dep Props
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(SCMessageBox), new PropertyMetadata(""));

        public string Caption
        {
            get { return (string)GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }
        public static readonly DependencyProperty CaptionProperty =
            DependencyProperty.Register("Caption", typeof(string), typeof(SCMessageBox), new PropertyMetadata(""));
        #endregion

        private bool _isDragBeginInTitleBar = false;
        public MessageBoxResult Result = MessageBoxResult.None;

        public SCMessageBox()
        {
            InitializeComponent();
            DataContext = this;
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
            Result = MessageBoxResult.None;
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

        #region Results
        
        #endregion

        #region Static
        private static Button GetButton(SCMessageBox owner, ButtonConfig config)
        {
            Button button = new();
            button.Style = (Style)Application.Current.FindResource("SCButtonStyle");
            button.Content = config.Content;
            button.Width = 80;
            button.Height = 30;

            button.Click += (object sender, RoutedEventArgs e) =>
            {
                owner.Result = config.Result;
                owner.Close();
            };

            return button;
        }

        private static List<Button> GetButtons(SCMessageBox owner, params ButtonConfig[] configs)
        {
            List<Button> buttons = new();

            for (int i = 0; i < configs.Length; i++)
            {
                Button btn = GetButton(owner, configs[i]);

                if (i < configs.Length - 1)
                {
                    btn.Margin = new Thickness(0, 0, 10, 0);
                }

                buttons.Add(btn);
            }

            return buttons;
        }

        public static MessageBoxResult ShowMsgBox(string text)
        {
            var window = new SCMessageBox();
            window.Text = text;
            
            Button buttonOK = GetButton(window, new ButtonConfig("OK", MessageBoxResult.OK));             
            window.buttonsContainer.Children.Add(buttonOK);

            window.ShowDialog();
            return window.Result;
        }

        public static MessageBoxResult ShowMsgBox(string text, string caption)
        {
            var window = new SCMessageBox();
            window.Text = text;
            window.Caption = caption;

            Button buttonOK = GetButton(window, new ButtonConfig("OK", MessageBoxResult.OK));
            window.buttonsContainer.Children.Add(buttonOK);

            window.ShowDialog();
            return window.Result;
        }

        public static MessageBoxResult ShowMsgBox(string text, string caption, MessageBoxButton buttons, MessageBoxResult defaultResult = MessageBoxResult.None)
        {            
            if (buttons == MessageBoxButton.OK)
                return ShowMsgBox(text, caption);

            var window = new SCMessageBox();
            window.Text = text;
            window.Caption = caption;
            window.Result = defaultResult;

            switch (buttons)
            {
                case MessageBoxButton.YesNo:
                    {
                        foreach (var btn in GetButtons(window, 
                            new ButtonConfig("Да", MessageBoxResult.Yes), 
                            new ButtonConfig("Нет", MessageBoxResult.No)))
                        {
                            window.buttonsContainer.Children.Add(btn);
                        }
                    }
                    break;
                case MessageBoxButton.YesNoCancel:
                    {
                        foreach (var btn in GetButtons(window,
                            new ButtonConfig("Да", MessageBoxResult.Yes),
                            new ButtonConfig("Нет", MessageBoxResult.No),
                            new ButtonConfig("Отмена", MessageBoxResult.Cancel)))
                        {
                            window.buttonsContainer.Children.Add(btn);
                        }
                    }
                    break;
                case MessageBoxButton.OKCancel:
                    {
                        foreach (var btn in GetButtons(window,
                            new ButtonConfig("ОК", MessageBoxResult.Yes),
                            new ButtonConfig("Отмена", MessageBoxResult.Cancel)))
                        {
                            window.buttonsContainer.Children.Add(btn);
                        }
                    }
                    break;
            }

            window.ShowDialog();
            return window.Result;
        }
        #endregion
    }

    record ButtonConfig(string Content, MessageBoxResult Result);
    public enum MessageBoxResult
    {
        OK,
        Cancel,
        Yes,
        No,
        None
    }
}
