using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;

namespace ColorAiApp
{
    public class ColorService
    {
        private readonly HttpClient _http;

        public ColorService(string apiKey)
        {
            _http = new HttpClient();
            _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
        }
         
        public async Task<List<string>> GetColors(string description)
        {
            var prompt = $"Give exactly 3 hex color codes for: {description}. Only hex, each on new line.";

            var body = new
            {
                model = "gpt-5.5",
                messages = new[]
                {
                new { role = "user", content = prompt }
            }
            };

            var json = JsonSerializer.Serialize(body);

            var response = await _http.PostAsync(
                "https://api.openai.com/v1/chat/completions",
                new StringContent(json, Encoding.UTF8, "application/json")
            );

            var result = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(result);
            var content = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return content.Split('\n')
                          .Select(x => x.Trim())
                          .Where(x => x.StartsWith("#"))
                          .Take(3)
                          .ToList();
        }
    }
}