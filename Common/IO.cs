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
            Assembly asmb = Assembly.LoadFile(embedPath.Split('.')[0]);
            using(StreamReader sr=new StreamReader(asmb.GetManifestResourceStream(embedPath)))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                    yield return line;
            }
        }
    }
}
