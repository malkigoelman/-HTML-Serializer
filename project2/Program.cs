using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using project2;


internal class Program
{
    private static async Task Main(string[] args)
    {
        var html = await Load("https://mail.google.com/mail/u/0/#inbox");
        html = new Regex("[\\r\\n\\t]").Replace(new Regex("\\s{2,}").Replace(html, ""), "");
        var htmlLines = new Regex("<(.*?)>").Split(html).Where(s => s.Length > 0).ToArray();

        HtmlElement root = BuildChild(htmlLines[1].Split(' ')[0], null, htmlLines[1]);
        Build(root, htmlLines.Skip(2).ToList());

        Console.WriteLine("begin");
        PrintTree(root, "");

        var x1 = root.FindAll(Selector.Conversion("div"));
        var x2 = root.FindAll(Selector.Conversion("from div"));
        //var x3 = root.FindAll(Selector.Conversion("div"));
        Console.ReadLine();

        //var list = root.FindAll(Selector.Conversion("form div div.YhhY8"));


        static HtmlElement Build(HtmlElement root, List<string> lines)
        {
            HtmlElement Tree = root;

            foreach (var line in lines)
            {
                if (line.StartsWith("/html"))
                    break;
                if (line.StartsWith("/"))
                {
                    Tree = Tree.Parent;
                    continue;
                }
                string tag = line.Split(' ')[0];
                if (!HtmlHelper.Instance.Tag.Contains(tag))
                {
                    Tree.InnerHtml += line;
                    continue;
                }
                HtmlElement child = BuildChild(tag, Tree, line);
                Tree.Children.Add(child);

                if (!HtmlHelper.Instance.Tag2.Contains(tag) && !line.EndsWith("/"))
                    Tree = child;
            }
            return root;

        }
        static HtmlElement BuildChild(string tag, HtmlElement currentP, string line)
        {
            HtmlElement child = new HtmlElement { Name = tag, Parent = currentP };

            var x = new Regex("([^\\s]*?)=\"(,*?)\"").Matches(line);
            foreach (var m in x)
            {
                string name = m.ToString().Split('=')[0];
                string value = m.ToString().Split('=')[1].Replace("\"", "");

                if (name.ToLower() == "class")
                    child.Classes.AddRange(value.Split(' '));
                else if (name.ToLower() == "id")
                    child.Id = value;
                else child.Attributes.Add(name, value);
            }
            return child;
        }


        static void PrintTree(HtmlElement element, string why)
        {
            Console.WriteLine($"{why}{element}");
            foreach (var x in element.Children)
                PrintTree(x, why + " ");
        }

        async Task<string> Load(string url)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(url);
            var html = await response.Content.ReadAsStringAsync();
            return html;
        }
    }
}