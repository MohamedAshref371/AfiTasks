using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace AfiTasks
{
    public partial class MainWindow : Window
    {
        public static double FontSizeValue = 42;

        int idx; string[] elements;
        int colorState;

        public MainWindow(string[] elements, int colorState)
        {
            InitializeComponent();
            this.elements = elements;
            WindowCounter.Text = $"{idx}/{elements.Length - 1}";
            WindowText.Text = elements[idx];
            this.colorState = colorState;
            if (colorState == 1)
            {
                WindowBorder.Background = (Brush)new BrushConverter().ConvertFromString("#5000");
                WindowText.Stroke = Brushes.White;
            }
        }

        private void WindowTextFontSize(double fontSize)
        {
            double oldWidth = this.Width, oldHeight = this.Height;
            WindowText.FontSize = fontSize;

            Dispatcher.BeginInvoke(new Action(() =>
            {
                this.Left += (oldWidth - this.Width) / 2;
                this.Top += (oldHeight - this.Height) / 2;
            }), System.Windows.Threading.DispatcherPriority.Render);
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            WindowTextFontSize(FontSizeValue);

            if (colorState == 1)
            {
                TextColorOne.Color = Color.FromRgb(0, 0, 255);
                TextColorTwo.Color = Color.FromRgb(0, 90, 255);
                TextColorThree.Color = Color.FromRgb(0, 165, 255);
            }
            else
            {
                TextColorOne.Color = Color.FromRgb(255, 0, 0);
                TextColorTwo.Color = Color.FromRgb(255, 165, 0);
                TextColorThree.Color = Color.FromRgb(255, 255, 0);
            }
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            WindowTextFontSize(FontSizeValue - 8);

            TextColorOne.Color = Color.FromRgb(100, 100, 100);
            TextColorTwo.Color = Color.FromRgb(150, 150, 150);
            TextColorThree.Color = Color.FromRgb(200, 200, 200);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right && idx < elements.Length - 1)
                idx += 1;
            else if (e.ChangedButton == MouseButton.Middle && idx > 0)
                idx -= 1;
            else
                return;

            double oldWidth = this.Width, oldHeight = this.Height;
            WindowText.Text = elements[idx];
            WindowCounter.Text = $"{idx}/{elements.Length - 1}";

            Dispatcher.BeginInvoke(new Action(() =>
            {
                this.Left += (oldWidth - this.Width) / 2;
                this.Top += (oldHeight - this.Height) / 2;
            }), System.Windows.Threading.DispatcherPriority.Render);

            if (idx % 2 == 1)
            {
                WindowCounter.Fill = Brushes.White;
                WindowCounter.Stroke = Brushes.Black;
            }
            else
            {
                WindowCounter.Fill = Brushes.Black;
                WindowCounter.Stroke = Brushes.White;
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 && WindowNumber.Text.Length < 6)
            {
                int number = e.Key - Key.NumPad0;
                WindowNumber.Text += number.ToString();
            }
            else if (e.Key == Key.Add && FontSizeValue < 72)
            {
                FontSizeValue += 2;
                WindowTextFontSize(FontSizeValue);
            }
            else if (e.Key == Key.Subtract && FontSizeValue > 30)
            {
                FontSizeValue -= 2;
                WindowTextFontSize(FontSizeValue);
            }
            else if (e.Key == Key.Escape)
                this.Hide();
        }
    }
}
