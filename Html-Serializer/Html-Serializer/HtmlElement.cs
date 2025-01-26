using System.Text;
using System.Xml.Linq;

namespace Html_Serializer
{
    public class HtmlElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Dictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();
        public List<string> Classes { get; set; } = new List<string>();
        public string InnerHtml { get; set; }
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; } = new List<HtmlElement>();
        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> queue = new Queue<HtmlElement>();
            HtmlElement element;
            queue.Enqueue(this);
            while (queue.Count > 0)
            {
                element = queue.Dequeue();
                yield return element;
                foreach (var child in element.Children)
                {
                    queue.Enqueue(child);
                }
            }
        }
        public IEnumerable<HtmlElement> Ancestors()
        {
            HtmlElement element = this;
            while (element != null)
            {
                yield return element;
                element = element.Parent;
            }
        }
        public IEnumerable<HtmlElement> FindElements(Selector selector)
        {
            var set = new HashSet<HtmlElement>();
            FindElementBySelector(selector, set, this.Descendants());
            return set;
        }
        public void FindElementBySelector(Selector selector, HashSet<HtmlElement> set, IEnumerable<HtmlElement> elements)
        {
            if (selector == null)
                return;

            foreach (var element in elements)
            {
                if (Compare(element, selector))
                {
                    if (selector.Child == null)
                        set.Add(element);
                    FindElementBySelector(selector.Child, set, element.Descendants());
                }
            }
        }
        public bool Compare(HtmlElement element, Selector selector)
        {
            if (selector == null || element == null)
                return false;

            var s = "\"" + selector.Id + "\"";

            if (selector.Id != "" && !s.Equals(element.Id))
                return false;

            if (element.Name != selector.TagName)
                return false;

            foreach (var c in selector.Classes)
                if (element.Classes.Count > 0 && !element.Classes.Contains(c))
                    return false;

            return true;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"Id: {Id}");
            stringBuilder.AppendLine($"Name: {Name}");
            stringBuilder.AppendLine("Attributes:");

            foreach (var attribute in Attributes)
            {
                stringBuilder.AppendLine($"   {attribute.Key}: {attribute.Value}");
            }

            stringBuilder.AppendLine("Classes:");

            foreach (var className in Classes)
            {
                stringBuilder.AppendLine($"   {className}");
            }

            stringBuilder.AppendLine($"InnerHtml: {InnerHtml}");
            stringBuilder.AppendLine($"Parent: {Parent?.Id ?? Parent?.Name ?? "null"}");
            stringBuilder.AppendLine("Children:");

            foreach (var child in Children)
            {
                stringBuilder.AppendLine($"   {child.Name ?? child.Id}");
            }

            return stringBuilder.ToString();
        }

    }

}
