using Microsoft.Win32;
using System.Windows;
using System.Windows.Input;

namespace MarkdownSlider
{
    public partial class MainWindow : Window
    {
        private bool fullscreen = false;
        private Slider slider;

        public MainWindow()
        {
            InitializeComponent();

            slider = new Slider();
            DataContext = slider;
        }

        private void ToggleFullscreen()
        {
            if (!fullscreen)
            {
                WindowStyle = WindowStyle.None;
                WindowState = WindowState.Maximized;
                menu.Visibility = Visibility.Collapsed;
                statusBar.Visibility = Visibility.Collapsed;
                fullscreen = true;
            }
            else
            {
                WindowStyle = WindowStyle.SingleBorderWindow;
                WindowState = WindowState.Normal;
                menu.Visibility = Visibility.Visible;
                statusBar.Visibility = Visibility.Visible;
                fullscreen = false;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F11)
                ToggleFullscreen();

            if (e.Key == Key.Escape && fullscreen)
                ToggleFullscreen();
        }

        private void Help_Clicked(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/LosWochos76/MarkdownSlider");
        }

        private void Open_Clicked(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Markdown Files|*.md";
            openFileDialog.Title = "Select a Markdown File";
            var result = openFileDialog.ShowDialog();

            if (!result.HasValue || result.Value == false)
                return;

            slider.OpenFile(openFileDialog.FileName);
        }

        private void Exit_Clicked(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void F11_Clicked(object sender, RoutedEventArgs e)
        {
            ToggleFullscreen();
        }

        private void Reload_Clicked(object sender, RoutedEventArgs e)
        {
            slider.Reload();
        }
    }
}