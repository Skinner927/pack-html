using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace pack_html
{
    class Base64Converter
    {

        /// <summary>
        /// Retrieves the passed file and returns the proper base64 encoding for direct entry in a HTML page.
        /// </summary>
        /// <param name="fileName">Absolute path to file</param>
        /// <returns>Base64 encoded file</returns>
        public string Convert(string fileName)
        {

            using (var fr = new FileRetriever())
            {
                // retrieve our file
                var dlFile = fr.Retrieve(fileName);

                // Errors?
                if (dlFile == null)
                    return "";

                // Do the base64 conversion
                string base64;
                string extension;
                using (var reader = new FileStream(dlFile, FileMode.Open))
                {
                    // Base64 conversion
                    var buffer = new byte[reader.Length];
                    reader.Read(buffer, 0, (int)reader.Length);
                    base64 = System.Convert.ToBase64String(buffer);

                    // Resolve MIME Type
                    /* http://stackoverflow.com/questions/58510/using-net-how-can-you-find-the-mime-type-of-a-file-based-on-the-file-signature */
                    UInt32 mimeType;
                    FindMimeFromData(0, null, buffer, 256, null, 0, out mimeType, 0);
                    var mimeTypePtr = new IntPtr(mimeType);
                    extension = Marshal.PtrToStringUni(mimeTypePtr);
                }

                // return it
                return "data:" + extension + ";base64," + base64;
            }

        }

        
        [DllImport(@"urlmon.dll", CharSet = CharSet.Auto)]
        private extern static System.UInt32 FindMimeFromData(
            System.UInt32 pBC,
            [MarshalAs(UnmanagedType.LPStr)] System.String pwzUrl,
            [MarshalAs(UnmanagedType.LPArray)] byte[] pBuffer,
            System.UInt32 cbSize,
            [MarshalAs(UnmanagedType.LPStr)] System.String pwzMimeProposed,
            System.UInt32 dwMimeFlags,
            out System.UInt32 ppwzMimeOut,
            System.UInt32 dwReserverd
        );

    }
}
