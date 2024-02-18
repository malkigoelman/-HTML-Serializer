using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project2
{
    public class HtmlElement
    {
        public string Id;
        public string Name { get; set; }
        public Dictionary<string,string> Attributes { get; set; } = new Dictionary<string,string>();
        public List<string> Classes { get; set; } = new List<string>();
        public string InnerHtml { get; set; } = "";
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; } = new List<HtmlElement>();
        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> queue = new Queue<HtmlElement>();
            queue.Enqueue(this);
            while (queue.Count > 0)
            {
                HtmlElement element = queue.Dequeue();
                if(this!=element)
                yield return element;
                foreach(var child in element.Children)
                    queue.Enqueue(child);
            }
        }
        public IEnumerable<HtmlElement> Ancestors()
        {
            HtmlElement element = Parent;
            //מחזירה את כל האבות של העץ שעליו היא מופעלת
            while (element!= null)
            {
                yield return element;
                element = element.Parent;
            }
        }
        public IEnumerable<HtmlElement> FindAll(Selector selector)
        {
            HashSet<HtmlElement> result = new HashSet<HtmlElement>();

            foreach (var x in Descendants())
                x.FindElement(selector, result);
            return result;
        }
        private void FindElement(Selector s, HashSet<HtmlElement> result)
        {
            if (IsEqual(s))
            {
                if (s.Child == null)
                    result.Add(this);
                else
                {
                    foreach (var x in Descendants())
                        x.FindElement(s.Child, result);
                }
            }
            return;
        }
        private bool IsEqual(Selector s)
        {
            return ((s.Id == null||s.Id.Equals(Id)) &&
                (s.Tagname == null||Name.Equals(s.Tagname))
                && (s.Classes.Intersect(Classes).Count() == s.Classes.Count));

        }

        public override string ToString()
        {
            string str = "";
            if (Name != null)
                str += "Name: " + Name;
            if (Id != null)
                str += " Id: " + Id;
            if (Classes.Count > 0)
            {
                str += " Classes: ";
                foreach (var c in Classes)
                    str += c + " ";
            }
            return str;
        }
    }
}
