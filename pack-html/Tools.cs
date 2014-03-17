using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pack_html
{
    class Tools
    {
        // This is the string an attribute must have if the block will not be packed
        public static readonly string SkipAttr = "data-no-pack";

        public static bool IsUrl(string url)
        {
            // Basically checks if the path starts with // or [protocol]:// it's a URL
            return System.Text.RegularExpressions.Regex.IsMatch(url, "^[A-z]*:?//");
        }

        /// <summary>
        /// Returns the full path of a file if not a URL. It's safe to pass URLs through here.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="currentDir"></param>
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
    }
}
