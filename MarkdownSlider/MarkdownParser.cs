using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownSlider
{
    class MarkdownParser
    {
        public static List<Slide> Parse(string filename)
        {
            if (!File.Exists(filename))
                return new List<Slide>();

            var slides = new List<Slide>();
            var lines = File.ReadAllLines(filename);
            var s = new Slide();
           
            foreach (var line in lines)
            {
                if (line.StartsWith("---"))
                {
                    slides.Add(s);
                    s = new Slide();
                }
                else
                {
                    s.Add(line);
                }
            }

            slides.Add(s);
            return slides;
        }
    }
}
