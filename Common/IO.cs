using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Net;

namespace Common
{
    /// <summary>
    /// The class for IO.
    /// </summary>
    public static class IO
    {
        /// <summary>
        /// Read the embedded file content as string sequence.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        /// <param name="asmbName">The assembly name string.</param>
        /// <returns>The content of the embedded file.</returns>
        public static IEnumerable<string> ReadEmbed(string path, string asmbName = "Common")
        {
            Assembly asmb = Assembly.Load(asmbName);
            using(StreamReader sr=new StreamReader(asmb.GetManifestResourceStream(path)))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                    yield return line;
            }
        }

        /// <summary>
        /// Read the embedded file content as string.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        /// <param name="asmbName">The assembly name string.</param>
        /// <returns>The content of the embedded file.</returns>
        public static string ReadEmbedAll(string path, string asmbName = "Common")
        {
            Assembly asmb = Assembly.Load(asmbName);
            using(StreamReader sr=new StreamReader(asmb.GetManifestResourceStream(path)))
            {
                return sr.ReadToEnd();
            }
        }

        /// <summary>
        /// Read web content as string from a certain uri.
        /// </summary>
        /// <param name="uri">The uri of the webpage.</param>
        /// <returns>The web content string.</returns>
        public static string ReadWebAll(string uri)
        {
            HttpWebRequest req = WebRequest.CreateHttp(uri);
            req.UseDefaultCredentials = true;
            using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
            {
                using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                    return sr.ReadToEnd();
            }
        }

        /// <summary>
        /// Read web content as a sequence of string from a certain uri.
        /// </summary>
        /// <param name="uri">The uri of the webpage.</param>
        /// <returns>The web content string sequence.</returns>
        public static IEnumerable<string> ReadWeb(string uri)
        {
            HttpWebRequest req = WebRequest.CreateHttp(uri);
            req.UseDefaultCredentials = true;
            using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
            {
                using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                        yield return line;
                }
            }
        }
    }
}
