using System.Collections.Generic;
using System.IO;

namespace MarkdownSlider
{
    class TemplateLoader
    {
        private List<Template> templates = new List<Template>();

        public List<Template> Load()
        {
            var currentPath = System.AppDomain.CurrentDomain.BaseDirectory;
            TryLoad(Path.Combine(currentPath, "Templates"));
            TryLoad(Path.Combine(currentPath, "..\\..\\..\\Templates"));
            TryLoad(Path.Combine(currentPath, "..\\..\\..\\..\\Templates"));

            return templates;
        }

        private void TryLoad(string path)
        {
            if (!Directory.Exists(path))
                return;

            foreach (var dir in Directory.GetDirectories(path))
            {
                var di = new DirectoryInfo(dir);
                var t = new Template(di.Name, di.FullName);
                templates.Add(t);
            }
        }
    }
}
