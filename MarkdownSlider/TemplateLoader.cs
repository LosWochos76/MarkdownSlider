using System.Collections.Generic;
using System.IO;

namespace MarkdownSlider
{
    public class TemplateLoader
    {
        public static List<Template> Load()
        {
            var templates = new List<Template>();
            var currentPath = System.AppDomain.CurrentDomain.BaseDirectory;

            TryLoad(Path.Combine(currentPath, "Templates"), templates);
            TryLoad(Path.Combine(currentPath, "..\\..\\..\\Templates"), templates);
            TryLoad(Path.Combine(currentPath, "..\\..\\..\\..\\Templates"), templates);

            return templates;
        }

        private static void TryLoad(string path, List<Template> list)
        {
            if (!Directory.Exists(path))
                return;

            foreach (var dir in Directory.GetDirectories(path))
            {
                var di = new DirectoryInfo(dir);
                var t = new Template(di.Name, di.FullName);
                list.Add(t);
            }
        }
    }
}
