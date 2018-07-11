using Markdig;
using Markdig.SyntaxHighlighting;
using System;
using System.Collections.Generic;

namespace MarkdownSlider
{
    public class Slide
    {
        private List<string> markdown = new List<string>();
        private MarkdownPipeline pipeline;

        public Slide()
        {
            pipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .UseSyntaxHighlighting()
                .Build();
        }

        public void Add(string line)
        {
            markdown.Add(line);
        }

        public string ToHtml()
        {
            var all = String.Join("\n", markdown.ToArray());
            return Markdown.ToHtml(all, pipeline);
        }
    }
}
