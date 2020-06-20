using System;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.Net;

namespace Common
{
    public static class IO
    {
        public static byte[] ReadBytes(string path, int count)
        {
            FileInfo file = new FileInfo(path);
            Sanity.Requires(file.Length <= int.MaxValue, "File is larger than 2 GiB.");
            count = Math.Min((int)file.Length, count);
            using(BinaryReader br=new BinaryReader(file.OpenRead()))
            {
                return br.ReadBytes(count);
            }
        }
        public static string ReadAllPage(string uri)
        {
            HttpWebRequest req = WebRequest.CreateHttp(uri);
            using(HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
            {
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                    {
                        return sr.ReadToEnd();
                    }
                }
                else
                    throw new InfException("Fail to reach the page.");
            }
        }
        public static IEnumerable<string> ReadPage(string uri)
        {
            HttpWebRequest req = WebRequest.CreateHttp(uri);
            using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
            {
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                            yield return line;
                    }
                }
                else
                    throw new InfException("Fail to reach the page.");
            }
        }
        public static IEnumerable<string> ReadEmbed(string embedPath)
        {
            Assembly asmb = Assembly.Load(embedPath.Split('.')[0]);
            using(StreamReader sr=new StreamReader(asmb.GetManifestResourceStream(embedPath)))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                    yield return line;
            }
        }
        public static IEnumerable<string[]> ReadEmbedClean(string embedPath)
        {
            foreach(string s in ReadEmbed(embedPath))
            {
                if (s[0] != '/' && s[1] != '/')
                    yield return s.Split('\t');
            }
        }
    }
}
