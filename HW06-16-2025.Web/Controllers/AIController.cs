using HW06_16_2025.Web.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Text.Json;
using System.Text;
using System.Text.Json.Serialization;
using AngleSharp.Html.Parser;
using HW06_16_2025.Web.Services;

namespace HW06_16_2025.Web.Controllers
{
    public class OllamaResponse
    {
        [JsonPropertyName("response")]
        public string Response { get; set; }
    } 

    public class SummaryResponse
    {
        public string Summary { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class AIController : ControllerBase
    {
        [HttpPost("generate")]
        public SummaryResponse Generate(URLModel request)
        {
            var service = new ArticleService();
            string text = service.Summary(request.Url);
            var prompt = $"Write a short summary based on this article {text}.";

            var ollamaRequest = new
            {
                model = "llama3",
                prompt = prompt,
                stream = false
            };

            var json = JsonSerializer.Serialize(ollamaRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var client = new HttpClient();
            var response = client.PostAsync("https://api.lit-ai-demo.com/api/generate", content).Result;

            var result = response.Content.ReadFromJsonAsync<OllamaResponse>().Result;

            return new SummaryResponse { Summary = result.Response };
        }

        
    }
}
