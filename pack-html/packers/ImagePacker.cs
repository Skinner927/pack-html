using System;
using HtmlAgilityPack;

namespace pack_html.packers
{
    internal class ImagePacker : IPacker
    {
        public HtmlDocument Pack(HtmlDocument html, string currentDir)
        {
            var c = new Base64Converter();

            try
            {
                // Look for all images
                foreach (HtmlNode img in html.DocumentNode.SelectNodes("//img[@src]"))
                {
                    // do we skip this?
                    if (img.Attributes.Contains(Tools.SkipAttr))
                        continue;

                    // gets the img src
                    HtmlAttribute att = img.Attributes["src"];

                    // Do the conversion
                    att.Value = c.Convert(Tools.GetFullPath(att.Value, currentDir));
                    img.Attributes["src"] = att;
                }
            }
            catch (NullReferenceException)
            {
                // No Nodes
            }

            return html;
        }
    }
}