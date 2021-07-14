using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Blog
{
    public class MarkdownExtensions
    {
        public static string ShowContentByMarkdown(string pathToFilesOfPost, string postContent)
        {

            var patterns = new List<string>()
            {
                "#i\\[(.+?)\\]",
                "#a\\[(.+?)\\]",
                "#v\\[(.+?)\\]",
                "#f\\[(.+?)\\]",
                "#ty\\[(.+?)\\]",
                "#tg\\[(.+?)\\]",
                "#tb\\[(.+?)\\]",
                "#tr\\[(.+?)\\]",
                "#ts\\[(.+?)\\]",
                "#c\\[(.+?)\\]",
                "#p\\[(.+?)\\]",
                "#tw\\[(.+?)\\]",
                "#ti\\[(.+?)\\]",
                "#nl",
                "#l"
            };

            foreach(var pattern in patterns)
            {
                string htmlTag;

                switch (pattern)
                {
                    // Image
                    case "#i\\[(.+?)\\]":
                        htmlTag = "<div class='row my-3'>" + "<div class='col-sm'>" + "<img src=  style='display:block; margin-left:auto; margin-right:auto; width:70%; height:auto;' >" + "</div>" + "</div>";
                        htmlTag = htmlTag.Insert(51, pathToFilesOfPost); // insert path to the file after <img src=
                        postContent = Regex.Replace(postContent, pattern, htmlTag);
                        break;
                    // Audio
                    case "#a\\[(.+?)\\]":
                        htmlTag = "<div class='row my-3 text-center'>" + "<div class='col-sm'>" + "<audio controls><source src=  type='audio/mp3'></audio>" + "</div>" + "</div>";
                        htmlTag = htmlTag.Insert(83, pathToFilesOfPost);
                        postContent = Regex.Replace(postContent, pattern, htmlTag);
                        break;
                    // Video
                    case "#v\\[(.+?)\\]":
                        htmlTag = "<div class='row my-3'>" + "<div class='col-sm'>" + "<video controls style='width:100%; height:auto;'><source src=' '  type='video/mp4'></video>" + "</div>" + "</div>";
                        htmlTag = htmlTag.Insert(104, pathToFilesOfPost);
                        postContent = Regex.Replace(postContent, pattern, htmlTag);
                        break;
                    // File
                    case "#f\\[(.+?)\\]":
                        htmlTag = "<div class='row my-3'>" + "<div class='col-sm'>" + "<object data=   style='width:100%; height:900px'></object>" + "</div>" + "</div>";
                        htmlTag = htmlTag.Insert(57, pathToFilesOfPost);
                        postContent = Regex.Replace(postContent, pattern, htmlTag);
                        break;
                    // Text yellow
                    case "#ty\\[(.+?)\\]":
                        htmlTag = "<span style='background-color:#ffffb3'>$1</span>";
                        postContent = Regex.Replace(postContent, pattern, htmlTag);
                        break;
                    // Text green
                    case "#tg\\[(.+?)\\]":
                        htmlTag = "<span style='background-color:#c6ffb3'>$1</span>";
                        postContent = Regex.Replace(postContent, pattern, htmlTag);
                        break;
                    // Text grey
                    case "#tb\\[(.+?)\\]":
                        htmlTag = "<span style='background-color:#f2f2f2'>$1</span>";
                        postContent = Regex.Replace(postContent, pattern, htmlTag);
                        break;
                    // Text red
                    case "#tr\\[(.+?)\\]":
                        htmlTag = "<span style='background-color:#ffb3ec'>$1</span>";
                        postContent = Regex.Replace(postContent, pattern, htmlTag);
                        break;
                    // New line
                    case "#nl":
                        htmlTag = "<br />";
                        postContent = Regex.Replace(postContent, pattern, htmlTag);
                        break;
                    // Horizontal rule
                    case "#l":
                        htmlTag = "<hr>";
                        postContent = Regex.Replace(postContent, pattern, htmlTag);
                        break;
                    // Strikethrough text
                    case "#ts\\[(.+?)\\]":
                        htmlTag = "<s>$1</s>";
                        postContent = Regex.Replace(postContent, pattern, htmlTag);
                        break;
                    // Code
                    case "#c\\[(.+?)\\]":
                        htmlTag = "<code style='color:#666666'>$1</code>";
                        postContent = Regex.Replace(postContent, pattern, htmlTag);
                        break;
                    // Paragraph
                    case "#p\\[(.+?)\\]":
                        htmlTag = "<p>$1</p>";
                        postContent = Regex.Replace(postContent, pattern, htmlTag);
                        break;
                    // Bold text
                    case "#tw\\[(.+?)\\]":
                        htmlTag = "<b>$1</b>";
                        postContent = Regex.Replace(postContent, pattern, htmlTag);
                        break;
                    // Italic text
                    case "#ti\\[(.+?)\\]":
                        htmlTag = "<i>$1</i>";
                        postContent = Regex.Replace(postContent, pattern, htmlTag);
                        break;
                }
            }

            return postContent;
        }
    }
}
