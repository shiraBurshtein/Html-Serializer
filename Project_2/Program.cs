using Project_2;
using System.Text.RegularExpressions;

async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}
var html = await Load("222");
var cleanHtml = new Regex("\\s").Replace(html, "");
var htmlLines = new Regex("<(.*?)>").Split(cleanHtml).Where(l => l.Length > 0).ToString();



static HtmlElement BuildTreeOfElements(List<string> htmlLines)
{
    var root = new HtmlElement();
    var currentElement = root;
    foreach (var line in htmlLines)
    {
        var firstWord = line.Split(' ')[0];
        if (firstWord == "/Html")
            break;
        else if (firstWord[0].Equals("/"))
        {
            if (currentElement.Parent != null)
                currentElement = currentElement.Parent;
        }
       else if (HtmlHelper.Instance.Tags.Contains(firstWord))
        {
            var child = new HtmlElement { Name = firstWord, Parent = currentElement };
           //var attributes = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(line).ToList();
            var restOfString = line.Remove(0, firstWord.Length);
            var attributes = Regex.Matches(restOfString, "([a-zA-Z]+)=\\\"([^\\\"]*)\\\"")
                .Cast<Match>()
                .Select(m => $"{m.Groups[1].Value}=\"{m.Groups[2].Value}\"")
                .ToList();
            child.Attributes = attributes;
            foreach (var attribute in attributes)
            {
                if (attribute.StartsWith("class"))
                {
                    var classes = attribute.Split('=')[1].Trim('"').Split(' ');
                    child.Classes.AddRange(classes);
                }
                if (attribute.StartsWith("id"))
                    child.Id = attribute.Split('=')[1].Trim('"');
            }
            currentElement.Children.Add(child);

            if (!HtmlHelper.Instance.TagsWithoutEnd.Contains(firstWord) || line.EndsWith("/"))
                currentElement = child;
            else
                currentElement = child.Parent;
        }
        else
        {
            currentElement.InnerHtml = line;
        }
    }
    return root;
}