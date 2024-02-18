using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Project_2
{
    internal class HtmlHelper
    {
        private readonly static HtmlHelper _instance = new HtmlHelper();
        public static HtmlHelper Instance = _instance;

        public string[] Tags { get; set; }
        public string[] TagsWithoutEnd { get; set; }
        private HtmlHelper()
        {
            string jsonTags = File.ReadAllText("JsonFiles/HtmlTags.json");
            string jsonTagsWithoutEnd = File.ReadAllText("JsonFiles/HtmlVoidTags.json");

            Tags = JsonSerializer.Deserialize<string[]>(jsonTags);
            TagsWithoutEnd = JsonSerializer.Deserialize<string[]>(jsonTagsWithoutEnd);
        }


    }
}
