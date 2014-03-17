using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace pack_html.packers
{
    class ImagePacker : IPacker
    {
        public HtmlDocument Pack(HtmlDocument html, string currentDir)
        {
            var c = new Base64Converter();

            // Look for all images
            foreach (var img in html.DocumentNode.SelectNodes("//img[@src]"))
            {
                // gets the img src
                var att = img.Attributes["src"];

                // Do the conversion
                att.Value = c.Convert(Tools.GetFullPath(att.Value, currentDir));
                img.Attributes["src"] = att;

            }

            return html;
        }
    }
}
