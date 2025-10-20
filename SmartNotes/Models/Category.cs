using System;

namespace SmartNotes.Models;

/// <summary>
/// Represents a category for organizing notes
/// </summary>
public class Category
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public string Icon { get; set; } = "📁";
    public string Color { get; set; } = "#0078D4";
}
