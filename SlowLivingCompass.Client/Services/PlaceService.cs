using SlowLivingCompass.Client.Models;

namespace SlowLivingCompass.Client.Services;

public class PlaceService
{
    private List<Place> _places = new();
    private bool _isInitialized = false;

    public List<Place> SharedCandidates { get; set; } = new();
    public double UserLat { get; set; } = 25.0422;
    public double UserLng { get; set; } = 121.5173;

    private Dictionary<string, List<string>> _themeMapping = new()
    {
        { "正餐", new() { "牛肉麵", "滷肉飯", "火鍋", "熱炒", "義大利麵", "拉麵", "壽司", "鵝肉", "咖哩飯", "定食", "排骨便當", "泰式料理", "韓式料理", "披薩" } },
        { "小吃/宵夜", new() { "臭豆腐", "滷味", "鹹酥雞", "雞排", "蚵仔煎", "燒烤", "串燒", "大腸包小腸", "地瓜球", "麵線", "水煎包", "章魚燒", "甜不辣", "肉圓" } },
        { "甜點/下午茶", new() { "咖啡", "甜點", "蛋糕", "鬆餅", "茶", "冰品", "豆花", "珍珠奶茶", "車輪餅", "肉桂捲", "下午茶", "書店" } }
    };

    public PlaceService()
    {
    }

    public async Task InitializeAsync(HttpClient http)
    {
        if (_isInitialized) return;
        
        var loadedPlaces = await System.Net.Http.Json.HttpClientJsonExtensions.GetFromJsonAsync<List<Place>>(http, "data/places.json");
        if (loadedPlaces != null)
        {
            _places = loadedPlaces;
            _isInitialized = true;
        }
    }

    public List<string> GetAllTags() => _places.SelectMany(p => p.Tags).Distinct().ToList();

    public List<string> GetThemes() => _themeMapping.Keys.ToList();
    
    public List<string> GetItemsByTheme(string theme)
    {
        if (_themeMapping.TryGetValue(theme, out var items))
        {
            var random = new Random();
            return items.OrderBy(x => random.Next()).Take(8).ToList(); // 每次隨機選 8 個，確保豐富且有新鮮感
        }
        return new List<string>();
    }

    public List<string> GetCommonIntents() => new() { "晚餐", "午餐", "火鍋", "甜點", "酒吧", "深夜", "散步", "書店" };

    public List<string> GetAvailableTagsInRange(double userLat, double userLng, int rangeMeters = 5000)
    {
        return _places
            .Where(p => CalculateDistance(userLat, userLng, p.Latitude, p.Longitude) <= rangeMeters)
            .SelectMany(p => p.Tags)
            .Distinct()
            .ToList();
    }

    public List<Place> GetMatchesByTag(string tag, double userLat, double userLng)
    {
        foreach (var place in _places)
        {
            place.DistanceInMeters = CalculateDistance(userLat, userLng, place.Latitude, place.Longitude);
        }

        return _places
            .Where(p => p.Tags.Contains(tag) && p.DistanceInMeters <= 5000) // 限制在 5 公里內
            .OrderBy(p => p.DistanceInMeters)
            .Take(3)
            .ToList();
    }

    public string GetMapSearchUrl(string query, double lat, double lng)
    {
        return $"https://www.google.com/maps/search/{Uri.EscapeDataString(query)}/@{lat},{lng},15z";
    }

    public List<Place> GetMatches(IEnumerable<string> selectedTags, string transportMode, double userLat, double userLng)
    {
        if (selectedTags == null || !selectedTags.Any()) 
            return new List<Place>();

        int maxDistance = transportMode == "Walking" ? 1000 : 4000;

        foreach (var place in _places)
        {
            place.DistanceInMeters = CalculateDistance(userLat, userLng, place.Latitude, place.Longitude);
        }

        return _places
            .Where(p => p.DistanceInMeters <= maxDistance)
            .OrderByDescending(p => p.Tags.Intersect(selectedTags).Count()) // 標籤最契合優先
            .ThenByDescending(p => p.IsFood) // 食物優先
            .ThenBy(p => p.DistanceInMeters) // 距離近優先
            .Take(3) // 取前 3 名
            .ToList();
    }

    private int CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        var dLat = (lat2 - lat1) * Math.PI / 180.0;
        var dLon = (lon2 - lon1) * Math.PI / 180.0;
        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(lat1 * Math.PI / 180.0) * Math.Cos(lat2 * Math.PI / 180.0) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return (int)(6371000 * c); // 地球半徑約 6371 公里
    }

    public List<Place> GetRandomPlaces(int count)
    {
        var random = new Random();
        return _places.OrderBy(x => random.Next()).Take(count).ToList();
    }
}
