using System.Text.Json;
namespace Html_Serializer
{
    public class HtmlHelper
    {
        private readonly static HtmlHelper _instance = new HtmlHelper();
        public static HtmlHelper Instance => _instance;
        public string[] HtmlVoidTags { get; set; }
        public string[] HtmlTags { get; set; }
        private HtmlHelper()
        {
            string HtmlTagsJson = File.ReadAllText("Json/HtmlTags.json");
            HtmlTags = JsonSerializer.Deserialize<string[]>(HtmlTagsJson);

            string HtmlVoidTagsJson = File.ReadAllText("Json/HtmlVoidTags.json");
            HtmlVoidTags = JsonSerializer.Deserialize<string[]>(HtmlVoidTagsJson);
        }
    }
}
