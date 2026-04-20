using Microsoft.JSInterop;
using System.Text.Json;

namespace SlowLivingCompass.Client.Services;

public class VisitRecord
{
    public string PlaceName { get; set; } = "";
    public string Tag { get; set; } = "";
    public DateTime VisitedAt { get; set; } = DateTime.Now;
}

public class JourneyService
{
    private readonly IJSRuntime _jsRuntime;
    private const string VISITS_KEY = "CompassVisitsCount";
    private const string HISTORY_KEY = "CompassVisitHistory";

    public JourneyService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task<int> GetVisitCountAsync()
    {
        var history = await GetVisitHistoryAsync();
        return history.Count;
    }

    public async Task<List<VisitRecord>> GetVisitHistoryAsync()
    {
        try
        {
            var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", HISTORY_KEY);
            if (string.IsNullOrEmpty(json)) return new List<VisitRecord>();
            return JsonSerializer.Deserialize<List<VisitRecord>>(json) ?? new List<VisitRecord>();
        }
        catch
        {
            return new List<VisitRecord>();
        }
    }

    public async Task AddVisitAsync(string placeName = "未知的角落", string tag = "探索")
    {
        var history = await GetVisitHistoryAsync();
        history.Add(new VisitRecord
        {
            PlaceName = placeName,
            Tag = tag,
            VisitedAt = DateTime.Now
        });
        var json = JsonSerializer.Serialize(history);
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", HISTORY_KEY, json);
    }

    public (string Badge, string Title, string Description) GetLevel(int visitCount)
    {
        return visitCount switch
        {
            0 => ("🌱", "初心者", "尚未開始探索，地圖正在等待你的第一步。"),
            < 3 => ("🌿", "微風漫步者", "你已經開始感受慢活的節奏了。"),
            < 6 => ("🍃", "巷弄探索家", "隱藏的角落開始為你展開面貌。"),
            < 10 => ("🌳", "在地靈魂", "你對這座城市的質感已有自己的見解。"),
            _ => ("🏔️", "慢活大師", "你已與城市的靈魂完全共鳴，傳說級探索者。")
        };
    }
}
