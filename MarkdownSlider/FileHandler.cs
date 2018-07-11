using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using uhttpsharp;
using uhttpsharp.Headers;

namespace MarkdownSlider
{
    public class FileHandler : IHttpRequestHandler
    {
        public static string DefaultMimeType { get; set; }
        public string HttpRootDirectory { get; set; }
        public static IDictionary<string, string> MimeTypes { get; private set; }

        static FileHandler()
        {
            DefaultMimeType = "text/plain";
            MimeTypes = new Dictionary<string, string>
                            {
                                {".css", "text/css"},
                                {".gif", "image/gif"},
                                {".htm", "text/html"},
                                {".html", "text/html"},
                                {".jpg", "image/jpeg"},
                                {".js", "application/javascript"},
                                {".png", "image/png"},
                                {".xml", "application/xml"},
                            };
        }

        private string GetContentType(string path)
        {
            var extension = Path.GetExtension(path) ?? string.Empty;
            if (MimeTypes.ContainsKey(extension))
                return MimeTypes[extension];
            return DefaultMimeType;
        }

        public async Task Handle(IHttpContext context, System.Func<Task> next)
        {
            var requestPath = context.Request.Uri.OriginalString.TrimStart('/');

            var httpRoot = Path.GetFullPath(HttpRootDirectory ?? ".");
            var path = Path.GetFullPath(Path.Combine(httpRoot, requestPath));

            if (!File.Exists(path))
            {
                await next().ConfigureAwait(false);

                return;
            }

            context.Response = new HttpResponse(GetContentType(path), File.OpenRead(path), context.Request.Headers.KeepAliveConnection());
        }
    }
}
