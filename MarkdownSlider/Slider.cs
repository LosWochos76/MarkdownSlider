using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    public class Slider : INotifyPropertyChanged
    {
        private string filename;
        private List<Slide> slides;
        private List<Template> templates;
        private int current_slide = 0;
        private int current_template = 0;

        private HttpServer httpServer = null;
        private SlideHandler slideHandler = new SlideHandler();
        private FileHandler templateFileHandler = new FileHandler();
        private FileHandler markdownFileHandler = new FileHandler();
 
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<MenuItem> TemplateMenuItems { get; set; }
        public ICommand NextSlideCommand { get; set; }
        public ICommand LastSlideCommand { get; set; }
        public string Status { get; set; }
        public string Url { get; set; }

        public Slider()
        {
            TemplateMenuItems = new ObservableCollection<MenuItem>();
            NextSlideCommand = new RelayCommand(new Action(NextSlide), new Func<bool>(HasNextSlide));
            LastSlideCommand = new RelayCommand(new Action(LastSlide), new Func<bool>(HasLastSlide));

            templates = TemplateLoader.Load();
            CreateTemplateMenuItems();
            StartWebServer();

            if (File.Exists("Readme.md"))
                OpenFile("Readme.md");
        }

        private void StartWebServer()
        {
            slideHandler.Template = templates[current_template];

            httpServer = new HttpServer(new HttpRequestProvider());
            httpServer.Use(new TcpListenerAdapter(new TcpListener(IPAddress.Loopback, 9876)));
            httpServer.Use(slideHandler);
            httpServer.Use(templateFileHandler);
            httpServer.Use(markdownFileHandler);
            httpServer.Start();
        }

        private void CreateTemplateMenuItems()
        {
            for (int i = 0; i < templates.Count; i++)
            {
                var mi = new MenuItem();
                mi.Header = templates[i].Name;
                mi.IsCheckable = true;
                mi.Tag = i;
                mi.Checked += Template_Checked;

                if (i == 0)
                    mi.IsChecked = true;

                TemplateMenuItems.Add(mi);
            }
        }

        private void Template_Checked(object sender, RoutedEventArgs e)
        {
            var mi = sender as MenuItem;

            foreach (var item in TemplateMenuItems)
            {
                if (item != mi)
                    item.IsChecked = false;
                else
                    item.IsChecked = true;
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

            Url = "http://localhost:9876/";
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Url"));

            Url = "http://localhost:9876/slide" + current_slide;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Url"));

            Status = "[" + (current_slide+1) + "/" + slides.Count + "]";
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Status"));
        }

        public void OpenFile(string filename)
        {
            this.filename = filename;
            slides = MarkdownParser.Parse(filename);

            var fi = new FileInfo(filename);
            markdownFileHandler.HttpRootDirectory = fi.Directory.ToString();
            slideHandler.Slides = slides;

            ShowCurrentSlide();
        }

        public void Reload()
        {
            OpenFile(filename);
        }

        public void NextSlide()
        {
            if (HasNextSlide())
            {
                current_slide++;
                ShowCurrentSlide();
            }
        }

        private bool HasNextSlide()
        {
            return (slides != null && current_slide < slides.Count - 1);
        }

        public void LastSlide()
        {
            if (HasLastSlide())
            {
                current_slide--;
                ShowCurrentSlide();
            }
        }

        private bool HasLastSlide()
        {
            return (slides != null && current_slide > 0);
        }
    }
}
