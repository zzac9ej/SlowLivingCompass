namespace SlowLivingCompass.Client.Models;

public class Place
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = new();
    public int DistanceInMeters { get; set; }
    public bool IsFood { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
