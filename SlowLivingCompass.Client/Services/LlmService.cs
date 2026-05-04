using SlowLivingCompass.Client.Models;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;

namespace SlowLivingCompass.Client.Services;

public class LlmService
{
    private readonly HttpClient _http;
    private readonly string? _apiKey;

    public LlmService(HttpClient http, IConfiguration config)
    {
        _http = http;
        _apiKey = config["GeminiApiKey"];
    }

    public async Task<List<VibeMatchResult>> GetVibeMatchesAsync(string userMood, List<Place> candidates, GeolocationData? location = null)
    {
        if (string.IsNullOrWhiteSpace(_apiKey) || _apiKey == "YOUR_API_KEY_HERE")
        {
            // Fallback to mock logic if API key is not configured
            return await GetMockVibeMatchesAsync(userMood, candidates);
        }

        try
        {
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={_apiKey}";
            
            var locationInstruction = "";
            if (location != null)
            {
                locationInstruction = $@"
The user is currently located at GPS coordinates: Latitude {location.Latitude}, Longitude {location.Longitude}.
IMPORTANT: You MUST use your world knowledge to find exactly 3 real, specific, highly-rated places (e.g. independent cafes, quiet parks, bookstores, or hidden gems) that are physically near this exact location and match the user's mood perfectly. DO NOT invent fake places. Provide the real name of the place in PlaceName.";
            }
            else
            {
                locationInstruction = $@"
Here are {candidates.Count} possible places they could visit:
{JsonSerializer.Serialize(candidates)}
Select exactly 3 places from this list that best match their mood for a slow, relaxing experience.";
            }

            var prompt = $@"
You are a 'Slow Living' assistant. The user's current mood is: '{userMood}'.

{locationInstruction}

Return ONLY a valid JSON array of objects. Each object must have:
- PlaceName (string): The name of the place
- VibeReason (string): A short, warm, and comforting sentence in Traditional Chinese explaining why this place fits their mood.
- MatchScore (number): An integer between 85 and 99 indicating how well it matches.
Do not include markdown blocks like ```json . Just the raw JSON array.
";

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[] { new { text = prompt } }
                    }
                },
                generationConfig = new
                {
                    responseMimeType = "application/json"
                }
            };

            var response = await _http.PostAsJsonAsync(url, requestBody);
            response.EnsureSuccessStatusCode();

            var geminiResponse = await response.Content.ReadFromJsonAsync<GeminiResponse>();
            var jsonText = geminiResponse?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text;

            if (!string.IsNullOrWhiteSpace(jsonText))
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var results = JsonSerializer.Deserialize<List<VibeMatchResult>>(jsonText, options);
                if (results != null && results.Any())
                {
                    return results;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Gemini API error: {ex.Message}");
        }

        // If Gemini API fails, fallback to mock logic
        return await GetMockVibeMatchesAsync(userMood, candidates);
    }

    private async Task<List<VibeMatchResult>> GetMockVibeMatchesAsync(string userMood, List<Place> candidates)
    {
        await Task.Delay(1500); // Simulate network delay
        var random = new Random();
        var selectedPlaces = candidates.OrderBy(x => random.Next()).Take(3).ToList();
        
        var results = new List<VibeMatchResult>();
        var mockReasons = new List<string>
        {
            $"這裡安靜舒服的氛圍，正好可以接住你的「{userMood}」。給自己一點時間在這裡放空吧。",
            $"聽說這裡的步調很慢，當你覺得「{userMood}」時，最適合來這裡找回自己的節奏。",
            $"這間店有種魔力，能讓人暫時忘記日常的繁瑣。特別適合覺得「{userMood}」的你。"
        };

        for (int i = 0; i < selectedPlaces.Count; i++)
        {
            var place = selectedPlaces[i];
            results.Add(new VibeMatchResult
            {
                PlaceName = place.Name,
                VibeReason = mockReasons[i % mockReasons.Count],
                MatchScore = random.Next(85, 99)
            });
        }

        return results;
    }

    // Gemini API Response Models
    private class GeminiResponse
    {
        [JsonPropertyName("candidates")]
        public List<GeminiCandidate>? Candidates { get; set; }
    }

    private class GeminiCandidate
    {
        [JsonPropertyName("content")]
        public GeminiContent? Content { get; set; }
    }

    private class GeminiContent
    {
        [JsonPropertyName("parts")]
        public List<GeminiPart>? Parts { get; set; }
    }

    private class GeminiPart
    {
        [JsonPropertyName("text")]
        public string? Text { get; set; }
    }
}
