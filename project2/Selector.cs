using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace project2
{
    public class Selector
    {
        public string Tagname { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; } = new List<string>();
        public Selector Parent { get; set; }
        public Selector Child { get; set; }

        public static Selector Conversion(string selectorstr)
        {
            Selector selector = new Selector();
            Selector current = selector;
            string[] allString = selectorstr.Split(' ');
            foreach (var x in allString)
            {
                string[] selectors = new Regex("(?=[#\\.])").Split(x).Where(s => s.Length > 0).ToArray();
                foreach (var s in selectors)
                {
                    if (s.StartsWith('#'))
                        current.Id = s.Substring(1);
                    else if (s.StartsWith("."))
                        current.Classes.Add(s.Substring(1));
                    else if (HtmlHelper.Instance.Tag.Contains(s))
                        current.Tagname = s;
                    else Console.WriteLine($"not valid : {s}");
                }

                Selector newSelector = new Selector();
                current.Child = newSelector;
                newSelector.Parent = current; ;
                current = newSelector;
            }
            current.Parent.Child = null;

            return selector;
        }
        public override string ToString()
        {
            string str = "";
            if (Tagname != null) 
                str += "Name: " + Tagname;
            if (Id != null)
                str += " Id: " + Id;
            if (Classes.Count > 0)
            {
                str += " classes: ";
                foreach (var c in Classes)
                    str += c + " ";
            }
            return str;
        }
    }
}
