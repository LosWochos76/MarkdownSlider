using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using uhttpsharp;

namespace MarkdownSlider
{
    class SlideHandler : IHttpRequestHandler
    {
        public List<Slide> Slides { get; set; }
        public Template Template { get; set; }

        public async Task Handle(IHttpContext context, Func<Task> next)
        {
            var requestPath = context.Request.Uri.OriginalString.TrimStart('/');

            if (requestPath.StartsWith("slide") && Slides != null && Template != null)
            {
                int slide_number = Convert.ToInt32(requestPath.Substring(5));
                var slide = Slides[slide_number];
                var html = Template.Fill(slide);

                context.Response = new HttpResponse("text/html", GenerateStreamFromString(html), false);
            }
            else
            {
                await next().ConfigureAwait(false);
            }
        }

        public Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
