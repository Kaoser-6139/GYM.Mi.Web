using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;

namespace GYM.Infrastructure
{
    public class GeminiService : IGeminiService
    {
        private readonly string _modelId;
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;
        private readonly List<ChatMessage> _chatHistory;

        public GeminiService(IConfiguration config, HttpClient httpClient)
        {
            _apiKey = config["Gemini:ApiKey"];
            _modelId = config["Gemini:ModelId"]; 
            _httpClient = httpClient;
            _chatHistory = new List<ChatMessage>();
        }

        public async Task<string> GetResponseAsync(string prompt)
        {
            try
            {
                
                var url = $"https://generativelanguage.googleapis.com/v1beta/models/{_modelId}:generateContent?key={_apiKey}";

                var requestBody = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new { text = prompt }
                            }
                        }
                    },
                    generationConfig = new
                    {
                        maxOutputTokens = 500,
                        temperature = 0.7,
                        topP = 0.8,
                        topK = 40
                    }
                };

                var json = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("X-goog-api-key", _apiKey);

                var response = await _httpClient.PostAsync(url, content);
                var responseString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return $"Error: {response.StatusCode} - {responseString}";

                using var doc = JsonDocument.Parse(responseString);

               
                return doc.RootElement
                          .GetProperty("candidates")[0]
                          .GetProperty("content")
                          .GetProperty("parts")[0]
                          .GetProperty("text")
                          .GetString() ?? "No response";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        public async Task<string> ChatAsync(string userMessage)
        {
            try
            {
                
                _chatHistory.Add(new ChatMessage { Role = "user", Parts = new[] { new Part { Text = userMessage } } });

                var url = $"https://generativelanguage.googleapis.com/v1beta/models/{_modelId}:generateContent?key={_apiKey}";

                var requestBody = new
                {
                    contents = _chatHistory,
                    generationConfig = new
                    {
                        maxOutputTokens = 500,
                        temperature = 0.7,
                        topP = 0.8,
                        topK = 40
                    }
                };

                var json = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);
                var responseString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return $"Error: {response.StatusCode} - {responseString}";

                using var doc = JsonDocument.Parse(responseString);
                var aiResponse = doc.RootElement
                                  .GetProperty("candidates")[0]
                                  .GetProperty("content")
                                  .GetProperty("parts")[0]
                                  .GetProperty("text")
                                  .GetString() ?? "No response";

            
                _chatHistory.Add(new ChatMessage { Role = "model", Parts = new[] { new Part { Text = aiResponse } } });

                return aiResponse;
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
    }

   
    public class ChatMessage
    {
        public string Role { get; set; } 
        public Part[] Parts { get; set; }
    }

    public class Part
    {
        public string Text { get; set; }
    }
}
