using System;
using System.IO;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace pack_html.packers
{
    internal class CssPacker : IPacker
    {
        public HtmlDocument Pack(HtmlDocument html, string currentDir)
        {
            try
            {
                // Get all link tags and turn them into script tags
                foreach (HtmlNode link in html.DocumentNode.SelectNodes("//link[@rel='stylesheet']"))
                {
                    // get the file
                    HtmlAttribute href = link.Attributes["href"];
                    string contents = "";
                    using (var fr = new FileRetriever())
                    {
                        string file = fr.Retrieve(Tools.GetFullPath(href.Value, currentDir));
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
                    HtmlNode style = link.OwnerDocument.CreateElement("style");
                    style.SetAttributeValue(Tools.SkipAttr, "");

                    // Set the contents
                    style.AppendChild(style.OwnerDocument.CreateTextNode(contents));

                    // Replace the link element with the new style
                    link.ParentNode.ReplaceChild(style, link);
                }
            }
            catch (NullReferenceException)
            {
                // No Nodes
            }

            try
            {
                // Look for script tags
                foreach (HtmlNode style in html.DocumentNode.SelectNodes("//style"))
                {
                    // do we skip?
                    if (style.Attributes.Contains(Tools.SkipAttr))
                        continue;

                    // Get the style's contents and base64 all urls
                    string contents = style.InnerText;
                    contents = ReplaceUrls(contents, currentDir);

                    // Replace the contents
                    style.ChildNodes.Clear();
                    style.SetAttributeValue(Tools.SkipAttr, "");
                    style.AppendChild(style.OwnerDocument.CreateTextNode(contents));
                }
            }
            catch (NullReferenceException)
            {
                // No Nodes
            }

            try
            {
                // Get all elements with a style attr
                foreach (HtmlNode styleEl in html.DocumentNode.SelectNodes("//*[@style]"))
                {
                    if (styleEl.Attributes.Contains(Tools.SkipAttr))
                        continue;

                    // Get the style and replace urls
                    HtmlAttribute style = styleEl.Attributes["style"];
                    style.Value = ReplaceUrls(style.Value, currentDir);
                }
            }
            catch (NullReferenceException)
            {
                // No nodes
            }

            return html;
        }

        /// <summary>
        ///     Given a CSS string, will replace all URLs with their base64 equivalents
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string ReplaceUrls(string contents, string fileName)
        {

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
                    string encoded =
                        c.Convert(Tools.GetFullPath(url,
                                                    (File.Exists(fileName) ? Path.GetDirectoryName(fileName) : fileName)));

                    return "url(" + encoded + ")";
                }, RegexOptions.Multiline);
        }
    }
}