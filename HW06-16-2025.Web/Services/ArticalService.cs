using AngleSharp.Html.Parser;

namespace HW06_16_2025.Web.Services
{
    public class ArticleService
    {
        
            public string Summary(string u) 
            {
                var x = GetHtml(u);
                var parser = new HtmlParser();
                var document = parser.ParseDocument(x);


                foreach (var node in document.QuerySelectorAll("style, script, nav, header, footer, aside"))
                {
                    node.Remove();
                }

                var selectors = new[] { ".post-content", ".entry-content", ".article-content", ".main-content", "article" };

                foreach (var selector in selectors)
                {
                    var element = document.QuerySelector(selector);
                    if (element != null)
                    {
                        var text = element.TextContent.Trim();
                        if (!string.IsNullOrWhiteSpace(text) && text.Length > 200)
                        {
                            return text;
                        }
                    }
                }

                var candidates = document.Body.QuerySelectorAll("div, section, p")
                    .Select(e => new { Element = e, Text = e.TextContent?.Trim() ?? "" })
                    .Where(e => e.Text.Length > 100)
                    .OrderByDescending(e => e.Text.Length);

                return candidates.FirstOrDefault()?.Text ?? string.Empty;
            }

            private string GetHtml(string url)
            {
                using var client = new HttpClient();
                return client.GetStringAsync($"{url}").Result;
            }

        }
    
}
