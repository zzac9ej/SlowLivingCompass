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
        { "正餐", new() { "牛肉麵", "滷肉飯", "火鍋", "熱炒", "義大利麵", "拉麵", "壽司", "鵝肉", "咖哩飯", "定食", "排骨便當", "泰式料理", "韓式料理", "披薩", "水餃", "鐵板燒", "生魚片", "漢堡", "素食/蔬食", "早午餐", "茶餐廳", "丼飯", "烏龍麵", "烤肉飯", "健康餐", "越南河粉", "涼麵", "牛排", "石鍋拌飯", "印度咖哩", "北京烤鴨", "壽喜燒", "麻辣鍋", "酸菜白肉鍋", "羊肉爐", "薑母鴨", "海南雞飯", "肉骨茶", "炒飯", "炒麵", "便當", "豬排飯", "鰻魚飯", "蕎麥麵", "義式燉飯", "法式料理", "西班牙海鮮燉飯", "美式餐廳", "墨西哥捲餅", "沙拉", "夏威夷拌飯", "控肉飯", "雞肉飯", "燒臘", "小籠包", "蒸餃", "鍋貼", "自助餐", "牛肉湯", "客家小炒", "海鮮餐廳", "湯包", "螺螄粉", "酸辣粉", "烤魚", "川菜", "湘菜", "廣式燒臘", "溫體牛火鍋", "石頭火鍋", "花雕雞", "重慶烤魚", "港式飲茶", "生菜包肉", "部隊鍋", "銅盤烤肉", "日式串炸", "居酒屋", "釜飯", "湯咖哩", "文字燒", "大阪燒", "生魚片丼" } },
        { "小吃/宵夜", new() { "臭豆腐", "滷味", "鹹酥雞", "雞排", "蚵仔煎", "燒烤", "串燒", "大腸包小腸", "地瓜球", "麵線", "水煎包", "章魚燒", "甜不辣", "肉圓", "鹽水雞", "刈包", "蔥油餅", "烤香腸", "肉羹", "四神湯", "米糕", "生煎包", "炸雞", "烤玉米", "關東煮", "清粥小菜", "豬血糕", "肉燥飯", "地瓜薯條", "炸魷魚", "黑輪", "豬血湯", "貢丸湯", "蛤蜊湯", "白糖粿", "狀元糕", "雙胞胎", "炸鮮奶", "菱角酥", "胡椒餅", "韭菜盒", "宵夜豆漿", "飯糰", "燒餅油條", "蛋餅", "蘿蔔糕", "麻辣燙", "砂鍋粥", "蚵仔麵線", "大腸麵線", "筒仔米糕", "土虱", "當歸鴨", "藥燉排骨", "生炒花枝", "鱔魚意麵", "鼎邊銼", "蝦仁飯", "肉粽", "碗粿", "米粉湯", "黑白切", "魚丸湯", "排骨酥麵", "鴨肉飯", "豬腳飯", "鹹粥", "海鮮粥", "雞湯", "羊肉湯", "虱目魚湯", "生牛肉湯" } },
        { "甜點/下午茶", new() { "咖啡", "甜點", "蛋糕", "鬆餅", "茶", "冰品", "豆花", "珍珠奶茶", "車輪餅", "肉桂捲", "下午茶", "書店", "手搖飲", "千層蛋糕", "司康", "馬卡龍", "蛋塔", "布丁", "刨冰", "雪花冰", "舒芙蕾", "可麗露", "果汁", "熱仙草", "貓咪咖啡廳", "特色酒吧", "紅豆湯", "綠豆湯", "花生湯", "燒仙草", "地瓜湯", "水果冰", "芒果冰", "綿綿冰", "芋圓", "粉圓冰", "檸檬塔", "生乳捲", "可頌", "法式吐司", "蜜糖吐司", "雞蛋仔", "提拉米蘇", "布朗尼", "可可", "抹茶", "花草茶", "冰沙", "氣泡飲", "微醺特調", "精釀啤酒", "深夜甜點", "海景咖啡", "老宅咖啡", "桌遊店", "和菓子", "手工布丁", "達克瓦茲", "費南雪", "磅蛋糕", "乳酪蛋糕", "巴斯克", "戚風蛋糕", "布列塔尼", "可麗餅", "瑪德蓮", "檸檬蛋糕", "水果塔", "蒙布朗", "聖多諾黑", "法式軟糖", "生巧克力", "手工冰淇淋", "義式冰淇淋", "優格冰", "冰棒", "綠豆沙", "木瓜牛奶", "酪梨牛奶", "現打果汁", "冷泡茶", "手沖咖啡", "拿鐵", "西西里咖啡", "氣泡咖啡", "泰奶", "港式奶茶" } },
        { "異國/特色", new() { "印度料理", "土耳其烤肉", "西班牙餐酒館", "德式豬腳", "法式薄餅", "阿根廷烤肉", "俄羅斯餐廳", "古巴三明治", "馬來西亞料理", "印尼炒飯", "越式法國麵包", "新疆烤肉", "藏族料理", "中東料理", "希臘餐廳", "地中海料理", "摩洛哥料理", "瑞士起司鍋", "比利時鬆餅", "瑞典肉丸", "巴西窯烤", "秘魯料理", "葡萄牙餐廳", "牙買加烤雞", "烏克蘭羅宋湯", "南洋風味", "星馬料理", "尼泊爾料理" } }
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
