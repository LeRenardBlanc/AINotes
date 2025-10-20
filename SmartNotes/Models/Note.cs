using System;
using System.Collections.Generic;

namespace SmartNotes.Models;

/// <summary>
/// Represents a note with rich content
/// </summary>
public class Note
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = "Nouvelle Note";
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime ModifiedAt { get; set; } = DateTime.Now;
    public List<string> Tags { get; set; } = new();
    public string CategoryId { get; set; } = string.Empty;
    public string Color { get; set; } = "#FFFFFF";
    public bool IsFavorite { get; set; }
    public int WordCount { get; set; }
}
