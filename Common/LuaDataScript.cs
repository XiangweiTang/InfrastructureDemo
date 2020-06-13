using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Common
{
    public class LuaDataScript
    {      
        public LuaDataScript()
        {
            string folderPath = @"D:\Program Files (x86)\Steam\steamapps\common\Factorio\data\base\prototypes\recipe";
            ParseFolder(folderPath);
        }

        private void ParseFolder(string folderPath)
        {
            foreach(string filePath in Directory.EnumerateFiles(folderPath))
            {
                var list = File.ReadLines(filePath);
                string cleanS = RigidParseLevel0(ReadLuaScriptToString(list));
                RigidParseLevel1(cleanS);
            }
        }
        private string RigidParseLevel0(string rawContent)
        {
            // 11 here is to skip this header:
            // data:extend
            string primaryCleanup = rawContent.Substring(11).Trim('(', ')');
            return primaryCleanup.Substring(1, primaryCleanup.Length - 2);
        }
        private void RigidParseLevel1(string cleanContent)
        {            
            StringBuilder sb = new StringBuilder();
            int left = 0;
            foreach(char c in cleanContent)
            {
                if (c == '{')
                    left++;
                if (c == '}')
                    left--;
                if (left != 0)
                    sb.Append(c);
                else
                {
                    if (c == '}')
                        sb.Append(c);
                    if (c == ',')
                        continue;
                    string sub = sb.ToString();
                    if (sb.Length > 0)
                        RigidParseLevel2(sub);
                    sb.Clear();
                }
            }
        }
        private void RigidParseLevel2(string s)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            FillDict(s, dict);
            Console.WriteLine(dict["name"].Trim('"'));
            if (dict.ContainsKey("normal"))
            {
                FillDict(dict["normal"], dict);
            }
            double time = 0.5;
            if (dict.ContainsKey("energy_required"))
                time = double.Parse(dict["energy_required"]);
            string amount = "1";
            if (dict.ContainsKey("result_count"))
                amount = dict["result_count"];
            if (dict.ContainsKey("result"))
            {
                string ing = dict["ingredients"];
                SetIngredients(ing.Substring(1, ing.Length - 2));
                Console.WriteLine($"\t{time}sec(s)==>");
                Console.WriteLine($"\t{amount}\t{dict["result"].Trim('"')}");
            }
            else if (dict.ContainsKey("results"))
                Console.WriteLine("\tMore than one output");
            else
                throw new InfException("Invalid format");
        }  

        private void FillDict(string s, Dictionary<string,string> dict)
        {
            string content = s.Substring(1, s.Length - 2);
            StringBuilder sb = new StringBuilder();
            int left = 0;
            foreach(char c in content)
            {
                if (sb.Length == 0 && c == ',')
                    continue;
                if (c == '{')
                    left++;
                if (c == '}')
                    left--;
                if (c == ',')
                {
                    if (left == 0)
                    {
                        RigidSetDict2(sb, dict);
                        sb.Clear();
                        continue;
                    }
                }
                sb.Append(c);
            }
            RigidSetDict2(sb, dict);
        }
        
        private void RigidSetDict2(StringBuilder sb, Dictionary<string,string> dict)
        {
            string s = sb.ToString();
            int index = s.IndexOf('=');
            Sanity.Requires(index != -1, "Invalid format.");
            string key = s.Substring(0, index);
            string value = s.Substring(index + 1);
            dict.Add(key, value);
        }

        private string ReadLuaScriptToString(IEnumerable<string> luas)
        {
            StringBuilder sb = new StringBuilder();
            bool insideQuatation = false;
            bool preMinus = false;
            foreach(string s in luas)
            {
                foreach(char c in s)
                {
                    if (insideQuatation)
                    {
                        if (c == '"')
                            insideQuatation = false;
                        sb.Append(c);
                        continue;
                    }
                    if (c == '"')
                    {
                        insideQuatation = true;
                        sb.Append(c);
                        continue;
                    }
                    if (c == ' ')
                        continue;
                    if (preMinus)
                    {
                        if (c == '-')
                        {
                            preMinus = false;
                            break;
                        }
                        sb.Append('-');
                        sb.Append(c);
                        continue;
                    }
                    if (c == '-')
                    {
                        preMinus = true;
                        continue;
                    }
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        private void SetIngredients(string s)
        {
            int start = -1;
            for(int i = 0; i < s.Length; i++)
            {
                if (s[i] == '{')
                {
                    start = i;
                    continue;
                }
                if (s[i] == '}')
                {
                    string content = s.Substring(start + 1, i - start - 1);
                    SetIngredientsDetails(content);
                }
            }
        }

        private void SetIngredientsDetails(string s)
        {
            string type, value;
            var split = s.Split(',');
            if (s.Contains("type="))
            {
                type = split[1].Split('=')[1].Trim('"');
                value = split[2].Split('=')[1];
            }
            else
            {
                type = split[0].Trim('"');
                value = split[1];
            }
            Console.WriteLine($"\t{value}\t{type}");
        }
    }
}
