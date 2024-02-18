using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace project2
{
    public class HtmlHelper//מחלקה שבה יש את כל התגיות
    {
        private readonly static HtmlHelper _Helper = new HtmlHelper();
        public static HtmlHelper Instance => _Helper;
        public List<string> Tag { get; set; }
        public List<string> Tag2 { get; set; }
        private HtmlHelper()
        {
            var content = File.ReadAllText("seed/html.txt");
            Tag = JsonSerializer.Deserialize<List<string>>(content);
            content = File.ReadAllText("seed/OneTag.txt");
            Tag2 = JsonSerializer.Deserialize<List<string>>(content);
        }

    }

}
