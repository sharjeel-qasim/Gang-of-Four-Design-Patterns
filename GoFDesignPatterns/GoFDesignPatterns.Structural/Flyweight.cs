namespace GoFDesignPatterns.Structural;

/// <summary>
/// Flyweight: Contains intrinsic immutable state that can be shared across thousands of objects.
/// </summary>
public class MapMarkerIcon(string markerType, string iconImageName, string categoryColor)
{
    public string MarkerType { get; } = markerType;
    public string IconImageName { get; } = iconImageName;
    public string CategoryColor { get; } = categoryColor;

    // Extrinsic state (latitude, longitude, label) is passed in as arguments
    public string Render(double latitude, double longitude, string label) =>
        $"[Marker: {MarkerType}] '{label}' at ({latitude:F4}, {longitude:F4}) using shared icon '{IconImageName}' (Color: {CategoryColor})";
}

/// <summary>
/// Flyweight Factory: Creates and caches flyweight objects to ensure maximum memory reuse.
/// </summary>
public class MapMarkerFactory
{
    private readonly Dictionary<string, MapMarkerIcon> _flyweights = [];

    public MapMarkerIcon GetMarkerIcon(string markerType, string iconImageName, string categoryColor)
    {
        var key = $"{markerType}_{iconImageName}_{categoryColor}".ToLowerInvariant();

        if (!_flyweights.TryGetValue(key, out var icon))
        {
            icon = new MapMarkerIcon(markerType, iconImageName, categoryColor);
            _flyweights[key] = icon;
        }

        return icon;
    }

    public int TotalSharedFlyweightsCount => _flyweights.Count;
}

/// <summary>
/// Context Object holding extrinsic state (coordinates, label) and a reference to the shared flyweight.
/// </summary>
public class MapMarker(double latitude, double longitude, string label, MapMarkerIcon icon)
{
    public double Latitude { get; } = latitude;
    public double Longitude { get; } = longitude;
    public string Label { get; } = label;
    public MapMarkerIcon Icon { get; } = icon;

    public string Draw() => Icon.Render(Latitude, Longitude, Label);
}
