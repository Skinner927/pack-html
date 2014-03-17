using System.IO;
using System.Text.RegularExpressions;

namespace pack_html
{
    internal class Tools
    {
        // This is the string an attribute must have if the block will not be packed
        public static readonly string SkipAttr = "data-no-pack";

        public static bool IsUrl(string url)
        {
            // Basically checks if the path starts with // or [protocol]:// it's a URL
            return Regex.IsMatch(url, "^[A-z]*:?//");
        }

        /// <summary>
        ///     Returns the full path of a file if not a URL. It's safe to pass URLs through here.
        /// </summary>
        /// <param name="filePath">Path of file or URL</param>
        /// <param name="currentDir">Current dir to look in for files. Optional if file is a full path or URL</param>
        /// <returns></returns>
        public static string GetFullPath(string filePath, string currentDir)
        {
            // Get the full path if it's not a URL
            if (!IsUrl(filePath))
            {
                if (!Path.IsPathRooted(filePath))
                {
                    filePath = Path.Combine(currentDir, filePath);
                }
            }

            return filePath;
        }

        /// <summary>
        ///     It seems MS hasn't complied with RFC 3986: "Uniform Resource Identifier (URI): Generic Syntax", Section 4.2
        ///     http://www.ietf.org/rfc/rfc3986.txt
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string SafeUrl(string url)
        {
            return url.StartsWith("//") ? "http:" + url : url;
        }
    }
}