using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GYM.Infrastructure
{
    public class AIService : IAiService
    {
        private readonly string _modelId;
        private readonly string _apiKey;
        private readonly string _baseUrl;
        private readonly HttpClient _httpClient;
        private readonly List<ChatMessage> _chatHistory;

        public AIService(IConfiguration config, HttpClient httpClient)
        {
            _apiKey = config["OpenRouter:ApiKey"] ?? "";
            _modelId = config["OpenRouter:ModelId"] ?? "openrouter/free";
            _baseUrl = config["OpenRouter:BaseUrl"] ?? "https://openrouter.ai/api/v1/chat/completions";

            _httpClient = httpClient;
            _chatHistory = new List<ChatMessage>();
        }

        public async Task<string> GetResponseAsync(string prompt)
        {
            if (string.IsNullOrWhiteSpace(_apiKey))
                return "AI service API key is missing. Please contact admin.";

            if (string.IsNullOrWhiteSpace(_modelId))
                return "AI model is not configured. Please contact admin.";

            try
            {
                var requestBody = new
                {
                    model = _modelId,
                    messages = new[]
                    {
                        new ChatMessage
                        {
                            Role = "system",
                            Content = GetFitnessCoachSystemPrompt()
                        },
                        new ChatMessage
                        {
                            Role = "user",
                            Content = prompt
                        }
                    },
                    max_tokens = 700,
                    temperature = 0.5
                };

                var json = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                using var request = new HttpRequestMessage(HttpMethod.Post, _baseUrl);

                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", _apiKey);

                request.Headers.TryAddWithoutValidation("HTTP-Referer", "http://localhost");
                request.Headers.TryAddWithoutValidation("X-Title", "GYM Smart Fitness Coach");

                request.Content = content;

                var response = await _httpClient.SendAsync(request);
                var responseString = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    return "AI coach quota has been exceeded. Please try again later or contact admin.";
                }

                if (!response.IsSuccessStatusCode)
                {
                    return "Sorry, the AI coach is currently unavailable. Please try again later.";
                }

                using var doc = JsonDocument.Parse(responseString);

                if (!doc.RootElement.TryGetProperty("choices", out var choices) ||
                    choices.GetArrayLength() == 0)
                {
                    return "Sorry, the AI coach did not return a valid response.";
                }

                var aiResponse = choices[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();

                return string.IsNullOrWhiteSpace(aiResponse)
                    ? "No response"
                    : aiResponse;
            }
            catch
            {
                return "Sorry, there was a problem connecting to your AI coach. Please try again later.";
            }
        }

        public async Task<string> ChatAsync(string userMessage)
        {
            if (string.IsNullOrWhiteSpace(_apiKey))
                return "AI service API key is missing. Please contact admin.";

            if (string.IsNullOrWhiteSpace(_modelId))
                return "AI model is not configured. Please contact admin.";

            try
            {
                _chatHistory.Add(new ChatMessage
                {
                    Role = "user",
                    Content = userMessage
                });

                var messages = new List<ChatMessage>
                {
                    new ChatMessage
                    {
                        Role = "system",
                        Content = GetFitnessCoachSystemPrompt()
                    }
                };

                messages.AddRange(_chatHistory);

                var requestBody = new
                {
                    model = _modelId,
                    messages = messages,
                    max_tokens = 700,
                    temperature = 0.5
                };

                var json = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                using var request = new HttpRequestMessage(HttpMethod.Post, _baseUrl);

                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", _apiKey);

                request.Headers.TryAddWithoutValidation("HTTP-Referer", "http://localhost");
                request.Headers.TryAddWithoutValidation("X-Title", "GYM Smart Fitness Coach");

                request.Content = content;

                var response = await _httpClient.SendAsync(request);
                var responseString = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    return "AI coach quota has been exceeded. Please try again later or contact admin.";
                }

                if (!response.IsSuccessStatusCode)
                {
                    return "Sorry, the AI coach is currently unavailable. Please try again later.";
                }

                using var doc = JsonDocument.Parse(responseString);

                if (!doc.RootElement.TryGetProperty("choices", out var choices) ||
                    choices.GetArrayLength() == 0)
                {
                    return "Sorry, the AI coach did not return a valid response.";
                }

                var aiResponse = choices[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString() ?? "No response";

                _chatHistory.Add(new ChatMessage
                {
                    Role = "assistant",
                    Content = aiResponse
                });

                return aiResponse;
            }
            catch
            {
                return "Sorry, there was a problem connecting to your AI coach. Please try again later.";
            }
        }

        private string GetFitnessCoachSystemPrompt()
        {
            return @"
You are a professional Smart Fitness Coach inside a Gym Management System.

Main responsibility:
Provide safe, practical, personalized fitness guidance. Always follow safety-first behavior.

Language rules:
- The user may write in English, Bangla, or Banglish.
- Understand the meaning, not only exact words.
- Reply in the same language style used by the user.
- If the user writes Bangla, reply in Bangla.
- If the user writes Banglish, reply in Banglish or simple Bangla.
- If the user writes English, reply in English.

Formatting rules:
- Do not use raw markdown symbols like **, ###, ___, or code blocks.
- Use clean plain text headings.
- Use short paragraphs and bullet points.
- Keep the response organized and professional.
- Do not make the answer robotic.
- Keep the answer practical and easy to follow.

Safety rules:
- You are not a doctor.
- Do not diagnose disease.
- Do not prescribe medicine.
- Do not give medicine dosage.
- Do not suggest supplements, fat burners, detox drinks, or extreme diets.
- If medical advice is needed, suggest consulting a doctor or qualified health professional.
- If the user is a child or teenager, avoid aggressive weight loss, heavy lifting, fasting, strict diet, or extreme training.

Health-condition priority:
Before giving a workout plan, check whether the user is sick, injured, weak, or unsafe to exercise.

If the user mentions any of these conditions, do not give a normal workout plan:
- diarrhea, loose motion, patla paikhana, পাতলা পায়খানা, pet kharap, পেট খারাপ
- vomiting, bomi, বমি
- fever, jor, জ্বর
- dizziness, matha ghura, মাথা ঘোরা
- chest pain, buk betha, বুকে ব্যথা
- breathing problem, shashkosto, শ্বাসকষ্ট
- fainting, অজ্ঞান
- severe weakness, khub durbol, খুব দুর্বল
- blood in stool, পায়খানায় রক্ত
- injury, pain, sprain, ব্যথা, মচকানো

If the user has minor sickness:
Use this format:
Health Note:
Exercise Decision:
What You Should Do Now:
Food and Hydration:
When to See a Doctor:
Reminder:

If the user has serious symptoms such as chest pain, breathing problem, fainting, blood in stool, severe dehydration, repeated vomiting, high fever, severe injury, or extreme weakness:
- Tell the user not to exercise.
- Advise urgent medical help.
- Keep the answer short, serious, and safe.

Normal workout plan rules:
If the user is healthy and asks for a plan, use this format:
Quick Note:
Today's Plan:
Workout:
Food Suggestion:
Reminder:

Today's plan and previous workout rule:
If the user asks for today's plan but previous workout history is missing, politely ask for recent workout details such as:
- What exercises they did in the last 1 to 3 days
- Sets and reps
- Difficulty level
- Soreness or pain
- Current energy level
- Available time today

However, do not stop the response completely. Give a beginner-safe general plan and mention that a more personalized plan requires previous workout details.

Exercise technique mode:
If the user asks how to do an exercise, such as push-up, squat, plank, bench press, or any movement, do not give a full workout plan. Explain the technique like a gym trainer.

Use this format:
Exercise Name:
Target Area:
Step-by-Step Technique:
Breathing:
Common Mistakes:
Beginner Version:
Advanced Version:
Safety Tips:

Do not claim that you can see the user's posture or body movement. Instead, give self-check cues such as:
- Keep your body straight
- Do not let your lower back drop
- Keep the movement controlled
- Stop if you feel sharp pain

Profile validation:
If age, height, weight, or goal seems unrealistic or unsafe, politely ask the user to recheck the profile information. Still provide only safe general guidance.

Tone:
Be professional, supportive, and clear. Sound like a real fitness trainer, but without claiming to physically observe the user.
";
        }
    }

    public class ChatMessage
    {
        [JsonPropertyName("role")]
        public string Role { get; set; } = "";

        [JsonPropertyName("content")]
        public string Content { get; set; } = "";
    }
}