using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace pack_html.packers
{
    class CssPacker : IPacker
    {
        public HtmlDocument Pack(HtmlDocument html, string currentDir)
        {
            // Get all link tags and turn them into script tags
            foreach (var link in html.DocumentNode.SelectNodes("//link[@rel='stylesheet']"))
            {
                

                // get the file
                var href = link.Attributes["href"];
                var contents = "";
                using (var fr = new FileRetriever())
                {
                    var file = fr.Retrieve(Tools.GetFullPath(href.Value, currentDir));
                    if (file != null)
                    {
                        contents = File.ReadAllText(file);
                        // Check the css for dependencies
                        contents = ReplaceUrls(contents.Trim(), file);
                    }
                    else
                    {
                        Console.WriteLine("CSS: Error retrieving file: " + href.Value);
                    }
                }

                // Create a new style element and set it to be skipped (so it's not read in the next step)
                var style = link.OwnerDocument.CreateElement("style");
                style.SetAttributeValue(Tools.SkipAttr, "");

                // Set the contents
                style.AppendChild(style.OwnerDocument.CreateTextNode(contents));

                // Replace the link element with the new style
                link.ParentNode.ReplaceChild(style, link);
            }

            foreach (var style in html.DocumentNode.SelectNodes("//style")) 
            {
                // do we skip?
                if (style.Attributes.Contains(Tools.SkipAttr))
                    continue;

                // Get the style's contents and base64 all urls
                var contents = style.InnerText;
                contents = ReplaceUrls(contents, currentDir);

                // Replace the contents
                style.ChildNodes.Clear();
                style.AppendChild(style.OwnerDocument.CreateTextNode(contents));
            }

            return html;
        }

        /// <summary>
        /// Given a CSS string, will replace all URLs with their base64 equivalents
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string ReplaceUrls(string contents, string fileName)
        {
            var regex = new Regex(@"", RegexOptions.Multiline);

            // Replace each url tag with the base64 equivalent
            return Regex.Replace(contents, @"url[ ]*\(((?!data:)[^\n]+?)\)", delegate(Match match)
                {
                    var c = new Base64Converter();

                    // Get the clean URL from the match
                    string url = match.Groups[1].ToString();

                    // Crazy sanitation
                    url = url.Trim();
                    url = url.Trim('\'', '\"');
                    url = url.Trim();

                    // Get our base64
                    var encoded = c.Convert(Tools.GetFullPath(url, (File.Exists(fileName) ? Path.GetDirectoryName(fileName) : fileName) ));

                    return "url(" + encoded + ")";
                }, RegexOptions.Multiline);
        }
    }
}
