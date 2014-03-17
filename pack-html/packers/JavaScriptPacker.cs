using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace pack_html.packers
{
    class JavaScriptPacker : IPacker
    {
        public HtmlDocument Pack(HtmlDocument html, string currentDir)
        {
  
            // Get all script tags
            foreach (var script in html.DocumentNode.SelectNodes("//script[@src]"))
            {
                // do we skip?
                if (script.Attributes.Contains(Tools.SkipAttr))
                    continue;

                // Get the file
                var src = script.Attributes["src"];
                var contents = "";
                using (var fr = new FileRetriever())
                {
                    var file = fr.Retrieve(Tools.GetFullPath(src.Value, currentDir));
                    if(file != null)
                        contents = File.ReadAllText(file);
                    else
                        Console.WriteLine("JS: Error retrieving file: " + src.Value);
                }

                // Remove the src tag as we're now going inline
                script.Attributes.Remove("src");

                // Set the contents to what the file was
                script.AppendChild(script.OwnerDocument.CreateTextNode(contents.Trim()));
            }

            // Send it back
            return html;
        }
    }
}
