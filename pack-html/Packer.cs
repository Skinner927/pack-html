using System;
using System.IO;
using System.Linq;
using HtmlAgilityPack;
using pack_html.packers;

namespace pack_html
{
    internal class Packer
    {
        private readonly string _file;

        /// <summary>
        ///     Creates a packer. Run Pack() on it.
        /// </summary>
        /// <param name="file">Absolute path of file to pack</param>
        public Packer(string file)
        {
            _file = file;
        }

        /// <summary>
        ///     This is essentially Main()
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

            var cp = new CssPacker();
            html = cp.Pack(html, Path.GetDirectoryName(_file));

            var ico = new FaviconPacker();
            html = ico.Pack(html, Path.GetDirectoryName(_file));


            // Add PACKED to the file name and save it
            string[] chop = _file.Split('.');
            string pathWithoutExtension = _file.Substring(0, _file.IndexOf("." + chop.Last(), StringComparison.Ordinal));
            string newFilename = pathWithoutExtension + "-PACKED." + chop.Last();


            Console.WriteLine(_file + " packed to: " + newFilename);
            html.Save(newFilename);
        }
    }
}