using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Project_2
{
    internal class HtmlElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Attributes { get; set; }
        public List<string> Classes { get; set; }
        public string InnerHtml { get; set; }
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; }
        public HtmlElement()
        {
            Attributes = new List<string>();
            Classes = new List<string>();
            Children = new List<HtmlElement>();
        }
        public IEnumerable<HtmlElement> Descendants()
        {

            Queue<HtmlElement> queue = new Queue<HtmlElement>();
            queue.Enqueue(this);
            while (queue.Count > 0)
            {
                var tmp= queue.Dequeue();
                foreach( var child in tmp.Children)
                {
                    queue.Enqueue(child);
                }
                yield return tmp;

            }
        }
        public IEnumerable<HtmlElement> Ancestors()
        {
            var parent=this;
            while (parent!=null)
            {
                yield return parent;
                parent = parent.Parent;

            }
        }
        public static IEnumerable<HtmlElement> FindElementsBySelector( this HtmlElement htmlElement , Selector selector)
        {
            HashSet<HtmlElement> result = new HashSet<HtmlElement>();
            var descendants = htmlElement.Descendants();
            foreach( var descendant in descendants)
            {
               FindElementsRecursive(descendant, selector, result);
            }

            return result;
        }
        private static IEnumerable<HtmlElement> FindElementsRecursive(HtmlElement element, Selector selector, HashSet<HtmlElement> result)
        {
            if (selector == null)
                return result;

            if (MatchesSelector(element, selector))
            {
                result.Add(element);
            }

            FindElementsRecursive(element, selector.Child, result);
            foreach (var child in element.Children)
            {
                FindElementsRecursive(child, selector, result);
            }
            return null;
        }
        private static bool MatchesSelector(HtmlElement element, Selector selector)
        {
            if (selector == null)
                return true;

            if (!string.IsNullOrEmpty(selector.TagName) && !string.Equals(element.Name, selector.TagName, StringComparison.OrdinalIgnoreCase))
                return false;

            if (!string.IsNullOrEmpty(selector.Id) && !string.Equals(element.Id, selector.Id, StringComparison.OrdinalIgnoreCase))
                return false;

            if (selector.Classes != null && selector.Classes.Count > 0)
            {
                foreach (var cssClass in selector.Classes)
                {
                    if (!element.Classes.Contains(cssClass, StringComparer.OrdinalIgnoreCase))
                        return false;
                }
            }

            return true;
        }


    }
}
