using Project_2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Project_2
{
    internal class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; }
        public Selector Child { get; set; }
        public Selector Parent { get; set; }
        public Selector()
        {
            Classes = new List<string>();
        }

        public static Selector ParseToSelector(string query)
        {
            var queries = query.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            Selector root = new Selector();
            Selector currentSelector = root;
            foreach (var qr in queries)
            {
                //מפצל את המחרוזת לפי # ו- . ע''י רגקס
                 var divideQuery = Regex.Split(qr, @"(?<=[#.])");

                foreach (string str in divideQuery)
                { 
                    var s = str.Trim();
                    if (!s.StartsWith('#') && !s.StartsWith('.'))
                        if (HtmlHelper.Instance.Tags.Contains(s))
                            currentSelector.TagName =s;
                 
                    if (s.StartsWith("#"))
                    {
                        currentSelector.Id = s.Substring(1);
                    }
                    else if (s.StartsWith("."))
                    {                       
                        currentSelector.Classes.Add(s.Substring(1));
                    }
                }

                Selector child = new Selector();
                currentSelector.Child = child;
                child.Parent = currentSelector;
                currentSelector = child;

            }
            return root;
        }

    }
}

