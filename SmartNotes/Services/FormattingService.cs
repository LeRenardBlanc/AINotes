using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartNotes.Services;

/// <summary>
/// Service for text formatting operations
/// Provides methods to apply and detect various text formatting styles
/// </summary>
public class FormattingService
{
    /// <summary>
    /// Apply bold formatting to selected text
    /// Note: Current implementation applies to the last word. 
    /// Full text selection support requires cursor position tracking.
    /// </summary>
    public string ApplyBold(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return text;
        
        // Check if already bold to avoid double-formatting
        if (text.StartsWith("**") && text.EndsWith("**"))
            return text;
        
        return $"**{text}**";
    }

    /// <summary>
    /// Apply italic formatting to selected text
    /// Note: Current implementation applies to the last word.
    /// Full text selection support requires cursor position tracking.
    /// </summary>
    public string ApplyItalic(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return text;
        
        // Check if already italic to avoid double-formatting
        if (text.StartsWith("*") && text.EndsWith("*") && !text.StartsWith("**"))
            return text;
        
        return $"*{text}*";
    }

    /// <summary>
    /// Apply underline formatting to selected text
    /// </summary>
    public string ApplyUnderline(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return text;
        
        return $"<u>{text}</u>";
    }

    /// <summary>
    /// Apply highlight formatting to selected text
    /// </summary>
    public string ApplyHighlight(string text, string color = "yellow")
    {
        if (string.IsNullOrWhiteSpace(text))
            return text;
        
        return $"=={text}==";
    }

    /// <summary>
    /// Apply heading formatting (H1-H6)
    /// </summary>
    public string ApplyHeading(string text, int level)
    {
        if (string.IsNullOrWhiteSpace(text))
            return text;
        
        level = Math.Clamp(level, 1, 6);
        var prefix = new string('#', level);
        return $"{prefix} {text}";
    }

    /// <summary>
    /// Create a bullet list from lines
    /// </summary>
    public string CreateBulletList(List<string> items)
    {
        if (items == null || items.Count == 0)
            return string.Empty;

        var sb = new StringBuilder();
        foreach (var item in items)
        {
            if (!string.IsNullOrWhiteSpace(item))
            {
                sb.AppendLine($"- {item.Trim()}");
            }
        }
        return sb.ToString();
    }

    /// <summary>
    /// Create a numbered list from lines
    /// </summary>
    public string CreateNumberedList(List<string> items)
    {
        if (items == null || items.Count == 0)
            return string.Empty;

        var sb = new StringBuilder();
        for (int i = 0; i < items.Count; i++)
        {
            if (!string.IsNullOrWhiteSpace(items[i]))
            {
                sb.AppendLine($"{i + 1}. {items[i].Trim()}");
            }
        }
        return sb.ToString();
    }

    /// <summary>
    /// Create a quote block
    /// </summary>
    public string CreateQuote(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return text;

        var lines = text.Split('\n');
        var sb = new StringBuilder();
        foreach (var line in lines)
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                sb.AppendLine($"> {line.Trim()}");
            }
        }
        return sb.ToString();
    }

    /// <summary>
    /// Create a code block
    /// </summary>
    public string CreateCodeBlock(string code, string language = "")
    {
        if (string.IsNullOrWhiteSpace(code))
            return code;

        return $"```{language}\n{code}\n```";
    }

    /// <summary>
    /// Insert a horizontal rule
    /// </summary>
    public string InsertHorizontalRule()
    {
        return "\n---\n";
    }

    /// <summary>
    /// Create a table in markdown format
    /// </summary>
    public string CreateTable(List<string> headers, List<List<string>> rows)
    {
        if (headers == null || headers.Count == 0)
            return string.Empty;

        var sb = new StringBuilder();
        
        // Headers
        sb.AppendLine("| " + string.Join(" | ", headers) + " |");
        
        // Separator
        sb.AppendLine("|" + string.Join("|", headers.Select(header => "---")) + "|");
        
        // Rows
        if (rows != null)
        {
            foreach (var row in rows)
            {
                sb.AppendLine("| " + string.Join(" | ", row) + " |");
            }
        }
        
        return sb.ToString();
    }

    /// <summary>
    /// Insert a link
    /// </summary>
    public string CreateLink(string text, string url)
    {
        if (string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(url))
            return text;
        
        return $"[{text}]({url})";
    }

    /// <summary>
    /// Insert an image reference
    /// </summary>
    public string CreateImage(string altText, string imageUrl)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
            return string.Empty;
        
        altText = string.IsNullOrWhiteSpace(altText) ? "Image" : altText;
        return $"![{altText}]({imageUrl})";
    }

    /// <summary>
    /// Apply color to text (using HTML)
    /// </summary>
    public string ApplyColor(string text, string color)
    {
        if (string.IsNullOrWhiteSpace(text))
            return text;
        
        return $"<span style=\"color:{color}\">{text}</span>";
    }

    /// <summary>
    /// Remove all formatting from text
    /// </summary>
    public string RemoveFormatting(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return text;

        // Remove markdown formatting
        text = System.Text.RegularExpressions.Regex.Replace(text, @"\*\*(.+?)\*\*", "$1"); // Bold
        text = System.Text.RegularExpressions.Regex.Replace(text, @"\*(.+?)\*", "$1"); // Italic
        text = System.Text.RegularExpressions.Regex.Replace(text, @"==(.+?)==", "$1"); // Highlight
        text = System.Text.RegularExpressions.Regex.Replace(text, @"<u>(.+?)</u>", "$1"); // Underline
        text = System.Text.RegularExpressions.Regex.Replace(text, @"^#{1,6}\s+", ""); // Headers
        text = System.Text.RegularExpressions.Regex.Replace(text, @"^\>\s+", ""); // Quotes
        text = System.Text.RegularExpressions.Regex.Replace(text, @"^\-\s+", ""); // Lists
        text = System.Text.RegularExpressions.Regex.Replace(text, @"^\d+\.\s+", ""); // Numbered lists
        
        return text;
    }

    /// <summary>
    /// Detect the formatting style of text
    /// </summary>
    public Dictionary<string, bool> DetectFormatting(string text)
    {
        var formatting = new Dictionary<string, bool>
        {
            ["Bold"] = text.Contains("**"),
            ["Italic"] = text.Contains("*") && !text.Contains("**"),
            ["Underline"] = text.Contains("<u>"),
            ["Highlight"] = text.Contains("=="),
            ["Heading"] = text.TrimStart().StartsWith("#"),
            ["Quote"] = text.TrimStart().StartsWith(">"),
            ["List"] = text.TrimStart().StartsWith("-") || text.TrimStart().StartsWith("*"),
            ["NumberedList"] = System.Text.RegularExpressions.Regex.IsMatch(text.TrimStart(), @"^\d+\."),
            ["Code"] = text.Contains("```"),
            ["Link"] = text.Contains("[") && text.Contains("]") && text.Contains("("),
            ["Image"] = text.Contains("!["),
        };

        return formatting;
    }
}
