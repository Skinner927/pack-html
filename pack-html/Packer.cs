using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace pack_html
{
    class Packer
    {
        private string _file;

        /// <summary>
        /// Creates a packer. Run Pack() on it.
        /// </summary>
        /// <param name="file">Absolute path of file to pack</param>
        public Packer(string file)
        {
            _file = file;
        }

        public void Pack()
        {
            
            var doc = new HtmlDocument();
            doc.Load(Path.GetFullPath(path: _file));

            foreach (HtmlNode img in doc.DocumentNode.SelectNodes("//img[@src]"))
            {
                var att = img.Attributes["src"];
                att.Value =
                    "http://trendwallpaper.com/wp-content/uploads/2013/12/Disney-Cartoon-Animation-Wallpaper.jpg";
                img.Attributes["src"] = att;
                    
            }


            doc.Save("test.html");
        }
    }
}
