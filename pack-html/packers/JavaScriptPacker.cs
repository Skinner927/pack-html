using System;
using System.IO;
using HtmlAgilityPack;

namespace pack_html.packers
{
    internal class JavaScriptPacker : IPacker
    {
        public HtmlDocument Pack(HtmlDocument html, string currentDir)
        {
            try
            {
                // Get all script tags
                foreach (HtmlNode script in html.DocumentNode.SelectNodes("//script[@src]"))
                {
                    // do we skip?
                    if (script.Attributes.Contains(Tools.SkipAttr))
                        continue;

                    // Get the file
                    HtmlAttribute src = script.Attributes["src"];
                    string contents = "";
                    using (var fr = new FileRetriever())
                    {
                        string file = fr.Retrieve(Tools.GetFullPath(src.Value, currentDir));
                        if (file != null)
                            contents = File.ReadAllText(file);
                        else
                            Console.WriteLine("JS: Error retrieving file: " + src.Value);
                    }

                    // Remove the src tag as we're now going inline
                    script.Attributes.Remove("src");

                    // Set the contents to what the file was
                    script.AppendChild(script.OwnerDocument.CreateTextNode(contents.Trim()));
                }
            }
            catch (NullReferenceException)
            {
                // No Nodes
            }

            // Send it back
            return html;
        }
    }
}