using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using uhttpsharp;
using uhttpsharp.Listeners;
using uhttpsharp.RequestProviders;

namespace MarkdownSlider
{
    public partial class MainWindow : Window
    {
        private List<Slide> slides;
        private List<Template> templates;
        private bool fullscreen = false;
        private int current_slide = 0;
        private int current_template = 0;
        private HttpServer httpServer = null;
        private SlideHandler slideHandler = new SlideHandler();
        private FileHandler templateFileHandler = new FileHandler();
        private FileHandler markdownFileHandler = new FileHandler();

        public MainWindow()
        {
            InitializeComponent();
            LoadTemplates();

            httpServer = new HttpServer(new HttpRequestProvider());
            httpServer.Use(new TcpListenerAdapter(new TcpListener(IPAddress.Loopback, 9876)));
            httpServer.Use(slideHandler);
            httpServer.Use(templateFileHandler);
            httpServer.Use(markdownFileHandler);
            httpServer.Start();

            slideHandler.Template = templates[current_template];

            if (File.Exists("Readme.md"))
                ShowFile("Readme.md");
        }

        private void LoadTemplates()
        {
            var tl = new TemplateLoader();
            templates = tl.Load();

            for (int i=0; i<templates.Count; i++)
            {
                var mi = new MenuItem();
                mi.Header = templates[i].Name;
                mi.Visibility = Visibility.Visible;
                mi.Tag = i;
                mi.IsCheckable = true;
                mi.Checked += Template_Checked;

                if (i == 0)
                    mi.IsChecked = true;

                templateMenu.Items.Add(mi);
            }
        }

        private void Template_Checked(object sender, RoutedEventArgs e)
        {
            var mi = sender as MenuItem;
            if (!mi.IsChecked)
                mi.IsChecked = true;

            foreach (var item in templateMenu.Items)
            {
                var ci = item as MenuItem;
                if (ci != mi)
                    ci.IsChecked = false;
            }

            current_template = (int)mi.Tag;
            templateFileHandler.HttpRootDirectory = templates[current_template].Path;
            slideHandler.Template = templates[current_template];
                
            ShowCurrentSlide();
        }

        private void ShowCurrentSlide()
        {
            if (slides == null || current_slide > slides.Count)
                return;

            browser.Address = "http://localhost:9876/slide" + current_slide;
        }

        private void ToggleFullscreen()
        {
            if (!fullscreen)
            {
                WindowStyle = WindowStyle.None;
                WindowState = WindowState.Maximized;
                menu.Visibility = Visibility.Hidden;
                fullscreen = true;
            }
            else
            {
                WindowStyle = WindowStyle.SingleBorderWindow;
                WindowState = WindowState.Normal;
                menu.Visibility = Visibility.Visible;
                fullscreen = false;
            }
        }

        private void ShowNextSlide()
        {
            if (slides != null && current_slide < slides.Count - 1)
            {
                current_slide++;
                ShowCurrentSlide();
            }
        }

        private void ShowLastSlide()
        {
            if (slides != null && current_slide > 0)
            {
                current_slide--;
                ShowCurrentSlide();
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F11)
                ToggleFullscreen();

            if (e.Key == Key.Right || e.Key == Key.Space)
                ShowNextSlide();

            if (e.Key == Key.Left)
                ShowLastSlide();

            if (e.Key == Key.Escape && fullscreen)
                ToggleFullscreen();
        }

        private void Help_Clicked(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/LosWochos76/MarkdownSlider");
        }

        private void ShowFile(string filename)
        {
            slides = MarkdownParser.Parse(filename);

            var fi = new FileInfo(filename);
            markdownFileHandler.HttpRootDirectory = fi.Directory.ToString();
            slideHandler.Slides = slides;

            ShowCurrentSlide();
        }

        private void Open_Clicked(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Markdown Files|*.md";
            openFileDialog.Title = "Select a Markdown File";
            var result = openFileDialog.ShowDialog();

            if (!result.HasValue || result.Value == false)
                return;

            ShowFile(openFileDialog.FileName);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}