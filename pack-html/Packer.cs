using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using pack_html.packers;

namespace pack_html
{
    class Packer
    {
        private readonly string _file;

        /// <summary>
        /// Creates a packer. Run Pack() on it.
        /// </summary>
        /// <param name="file">Absolute path of file to pack</param>
        public Packer(string file)
        {
            _file = file;
        }

        /// <summary>
        /// This is essentailly Main()
        /// </summary>
        public void Pack()
        {
            // Load the HTML file
            var html = new HtmlDocument();
            html.Load(Path.GetFullPath(path: _file));

            //TODO: Turn this into reflection

            var ip = new ImagePacker();
            html = ip.Pack(html, Path.GetDirectoryName(_file));


            


            html.Save("test.html");
        }
    }
}
