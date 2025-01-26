using System.Text.RegularExpressions;

namespace Html_Serializer
{
    public class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; } = new List<string>();
        public Selector Parent { get; set; }
        public Selector Child { get; set; }

        public static Selector QueryToSelector(string query)
        {
            string[] arr = query.Split(' ');
            Selector root = new Selector();
            Selector current = root;

            string regex = @"(?<tag>\w+)(?:#(?<id>\w+))?(?:\.(?<class>\w+))*";

            foreach (string a in arr)
            {
                Match match = Regex.Match(a, regex);
                Selector newSelector = new Selector();

                newSelector.Id = match.Groups["id"].Value;

                var tag = match.Groups["tag"].Value;

                if (HtmlHelper.Instance.HtmlTags.Contains(tag) || HtmlHelper.Instance.HtmlVoidTags.Contains(tag))
                    newSelector.TagName = tag;

                var classes = match.Groups["class"].Captures;

                if (classes.Count > 0)
                {
                    foreach (Capture c in classes)
                    {
                        newSelector.Classes.Add(c.Value);
                    }
                }

                newSelector.Parent = current;
                current.Child = newSelector;
                current = newSelector;

            }
            return root.Child;
        }

    }
}
