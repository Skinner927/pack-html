using System;
using System.IO;
using System.Net;

namespace pack_html
{
    /// <summary>
    ///     File Retriever is used to retrieve files from either a URL or local path.
    ///     Please wrap in a using clause so files can be removed when done.
    /// </summary>
    internal class FileRetriever : IDisposable
    {
        private readonly string _tempDir;

        public FileRetriever()
        {
            _tempDir = GetTemporaryDirectory();
        }

        /// <summary>
        ///     Retrieves a file from the web or locally.
        /// </summary>
        /// <param name="file">Absolute file path or URL</param>
        /// <returns>Local absolute filepath of where to the file is or null on failure</returns>
        public string Retrieve(string file)
        {
            // Do we have to download the file or is it local?
            string dlFile = file;

            if (Tools.IsUrl(file))
            {
                // generate a temp file name
                do
                {
                    dlFile = Path.Combine(_tempDir, Path.GetRandomFileName());
                } while (File.Exists(dlFile));

                // Download the file
                using (var wc = new WebClient())
                {
                    try
                    {
                        wc.DownloadFile(Tools.SafeUrl(file), dlFile);
                    }
                    catch (WebException)
                    {
                        // File couldn't download, maybe it doesn't exist
                        return null;
                    }
                }
            }

            // Verify file exists, and return it or else null because error
            return File.Exists(dlFile) ? dlFile : null;
        }

        private static string GetTemporaryDirectory()
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }

        #region Disposal

        public void Dispose()
        {
            DisposeMe();
            GC.SuppressFinalize(this);
        }

        ~FileRetriever()
        {
            DisposeMe();
        }

        private void DisposeMe()
        {
            // Cleanup temp dir
            try
            {
                if (Directory.Exists(_tempDir))
                    Directory.Delete(_tempDir, true);
            }
            catch
            {
                /* eat it, whatever it's a temp dir */
            }
        }

        #endregion
    }
}