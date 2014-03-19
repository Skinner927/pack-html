using System;
using System.IO;
using HtmlAgilityPack;

namespace pack_html.packers
{
    internal class FaviconPacker : IPacker
    {
        public HtmlDocument Pack(HtmlDocument html, string currentDir)
        {
            // Determines if we should be nice and search the dir for a favicon
            bool iconFound = false;

            foreach (HtmlNode icon in Tools.SelectNodes(html, "//link[(@rel='icon' or @rel='shortcut icon') and @href]"))
            {
                // do we skip?
                if (icon.Attributes.Contains(Tools.SkipAttr))
                    continue;

                HtmlAttribute href = icon.Attributes["href"];
                using (var fr = new FileRetriever())
                {
                    string file = fr.Retrieve(Tools.GetFullPath(href.Value, currentDir));
                    if (file != null)
                    {
                        var c = new Base64Converter();
                        icon.SetAttributeValue("href", c.Convert(file));
                        iconFound = true;
                    }
                    else
                    {
                        Console.WriteLine("Icon: Error retrieving file: " + href.Value);
                    }
                }
            }


            if (iconFound)
                return html;

            // Search the root dir for a favicon, if it exists, be nice and append it to the head
            string favico = Path.Combine(currentDir, "favicon.ico");
            if (File.Exists(favico))
            {
                // get the 64 of the file
                var c = new Base64Converter();
                string base64 = c.Convert(favico);

                // Find the head
                HtmlNode head = html.DocumentNode.SelectSingleNode("/html/head");

                // Create the link element
                HtmlNode link = html.CreateElement("link");
                link.SetAttributeValue("rel", "shortcut icon");
                link.SetAttributeValue("type", "image/x-icon");
                link.SetAttributeValue("href", base64);

                // Apped the link element
                head.AppendChild(link);
            }

            return html;
        }
    }
}