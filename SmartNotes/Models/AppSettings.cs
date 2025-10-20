namespace SmartNotes.Models;

/// <summary>
/// Application settings and preferences
/// </summary>
public class AppSettings
{
    public string Theme { get; set; } = "Default"; // Default, Light, Dark
    public string AccentColor { get; set; } = "Blue"; // Blue, Green, Ivory, Peach, Rose, Lavender
    public bool EnableAI { get; set; } = true;
    public bool AutoSave { get; set; } = true;
    public int AutoSaveInterval { get; set; } = 30; // seconds
    public string DefaultCategory { get; set; } = string.Empty;
}
