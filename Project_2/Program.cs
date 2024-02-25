using Project_2;
using System.Text.RegularExpressions;

Serializer serializer = new Serializer();

var html = await serializer.Load("https://netfree.link/");

HtmlElement element = serializer.Serialize(html);