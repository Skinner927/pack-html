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

            var jcp = new JavaScriptPacker();
            html = jcp.Pack(html, Path.GetDirectoryName(_file));


            // Add PACKED to the file name and save it
            var chop = _file.Split('.');
            var pathWithoutExtension = _file.Substring(0, _file.IndexOf("." + chop.Last()));
            var newFilename = pathWithoutExtension + "-PACKED." + chop.Last();


            Console.WriteLine(_file + " packed to: " + newFilename);
            html.Save(newFilename);
        }
    }
}
