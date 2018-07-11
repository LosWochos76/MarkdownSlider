using System.IO;

namespace MarkdownSlider
{
    public class Template
    {
        private string name;
        private string path;
        private string html;

        public Template(string name, string path)
        {
            this.name = name;
            this.path = path;
        }

        public string Name
        {
            get { return name; }
        }

        public string Path
        {
            get { return path; }
        }

        public string Html
        {
            get { return File.ReadAllText(path + System.IO.Path.DirectorySeparatorChar + "template.html"); }
        }

        public string Fill(Slide s)
        {
            return Html.Replace("{body}", s.ToHtml());
        }
    }
}
