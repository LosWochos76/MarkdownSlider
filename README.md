# MarkdownSlider

A tool like [Deckset](https://www.deckset.com/) or [Marp](https://yhatt.github.io/marp/) to create 
presentation slides with [Markdown](https://github.com/adam-p/markdown-here/wiki/Markdown-Cheatsheet).

---
## Why using Markdown?

Due to my profession I create powerpoint slides a lot. It always bothered me that 
the content was burried in some proprietary format. Hence, it is difficult to reuse the slide-content
for other things like online ressources, books or handouts.

Writing slides in [Markdown](https://github.com/adam-p/markdown-here/wiki/Markdown-Cheatsheet) 
can be a good alternative. There are plenty of editors to write Markdown. Unfortunately, it is not quite easy to
create presentation slides with Markdown and present them in a classroom. 

Marp could be a great tool but it does not provide a presentation mode, so it is quite useless for my use-case.
On the other side Deckset is only available for Mac.

---
## Features

MarkdownSlider provides a GUI with a full-screen presentation mode for Markdown based presentations.

MarkdownSlider uses the horizontal ruler `---` to split Markdown into individual slides. It 
then uses [Markdig](https://github.com/lunet-io/markdig) to transform the Markdown into Html which gets 
inserted into a template file. Such templates can be created individually by using Html and CSS.

The result of that transformation is available via a local web-server using at 
[uHttpSharp](https://github.com/Code-Sharp/uHttpSharp). Each slide can be loaded from a 
specific URL, e.g. the first slide is available at ()[http://localhost:9876/slide0].

Furthermore, MarkdownSlider embeds a Chromium browser by using [CefSharp](https://github.com/cefsharp/CefSharp) 
to load each slide from the local web-server. Using keys like next, back or space one can flip 
through the presentation. With F11 one can switch to a full-screen presentation mode.

---
## ToDo

MarkdownSlider is currently work in progress. There are still a lot of things to do:
- Watch changes on the current Markdown-file and reload if necessary.
- Create some more templates and improve CSS.
- Add a toolbar.
- Find out, why syntax-highlighting does not work yet.
- Open a Markdown-file from a remote URL.

---
## Licenses

Copyright &copy; 2018 [Alexander Stuckenholz](https://github.com/LosWochos76).

This software released under the [MIT License](https://github.com/LosWochos76/MarkdownSlider/blob/master/LICENSE).