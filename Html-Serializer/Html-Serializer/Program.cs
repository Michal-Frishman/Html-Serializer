using Html_Serializer;
using System.Text.RegularExpressions;

//var html = await Load("https://www.jumbomail.me/l/he/home");
var html = await Load("https://hebrewbooks.org/");

var cleanHtml = new Regex("[\\r\\n\\t]").Replace(new Regex("\\s{2,}").Replace(html, ""), "");

var htmlLines = new Regex("<(.*?)>").Split(cleanHtml).Where(s => s.Length > 0);

var root = Serialize(htmlLines);


static HtmlElement Serialize(IEnumerable<string> htmlLines)
{

    HtmlElement root = new HtmlElement();
    HtmlElement current = root;
    foreach (var line in htmlLines)
    {
        string[] arr = line.Split(' ');
        if (arr[0] != "/html")
        {
            if (arr[0].StartsWith("/"))
            {
                current = current.Parent;

            }
            else if (HtmlHelper.Instance.HtmlTags.Contains(arr[0]) || HtmlHelper.Instance.HtmlVoidTags.Contains(arr[0]))
            {
                HtmlElement newElement = new HtmlElement();
                newElement.Parent = current;
                current.Children.Add(newElement);
                newElement.Name = arr[0];
                current = newElement;
                if (line.IndexOf(' ') != -1)
                {
                    var attributes = new Regex("([^\\s]*?)=\".(.*?)\"").Matches(line);
                    foreach (var attr in attributes)
                    {
                        string[] split = attr.ToString().Split("=");
                        if (split[0] == "id")
                        {
                            current.Id = split[1];
                        }
                        else if (split[0] == "class")
                        {
                            current.Classes = split[1].Split(' ').ToList();
                        }
                        else
                        {
                            current.Attributes.Add(split[0], split[1]);
                        }
                    }
                }
                if (HtmlHelper.Instance.HtmlVoidTags.Contains(arr[0]))
                {
                    current = current.Parent;
                }

            }
            else
            {
                current.InnerHtml = line;
            }
        }
    }
    return root;
}

async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}

Console.WriteLine("----print the tree----");
PrintTree(root);


void PrintTree(HtmlElement root)
{
    if (root == null)
        return;
    Console.WriteLine(root.ToString());
    for (int i = 0; i < root.Children.Count; i++) { PrintTree(root.Children[i]); }
}



void Check(string s, HtmlElement dom)
{
    Selector selector = Selector.QueryToSelector(s);
    var result = dom.FindElements(selector);
    result.ToList().ForEach(element => { Console.WriteLine(element); });
}



HtmlElement dom = Serialize(htmlLines);

Console.WriteLine("--------div img---------");
Check("div img", dom);


Console.WriteLine();
