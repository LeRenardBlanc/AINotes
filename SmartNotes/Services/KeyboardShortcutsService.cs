using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartNotes.Services;

/// <summary>
/// Service for managing customizable keyboard shortcuts
/// </summary>
public class KeyboardShortcutsService
{
    private readonly string _settingsPath;
    private Dictionary<string, string> _shortcuts;

    public KeyboardShortcutsService()
    {
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var smartNotesPath = Path.Combine(appDataPath, "SmartNotes");
        Directory.CreateDirectory(smartNotesPath);
        _settingsPath = Path.Combine(smartNotesPath, "shortcuts.json");
        
        _shortcuts = GetDefaultShortcuts();
    }

    /// <summary>
    /// Get default keyboard shortcuts
    /// </summary>
    private Dictionary<string, string> GetDefaultShortcuts()
    {
        return new Dictionary<string, string>
        {
            // File operations
            ["NewNote"] = "Ctrl+N",
            ["SaveNote"] = "Ctrl+S",
            ["DeleteNote"] = "Delete",
            ["ExportNote"] = "Ctrl+E",
            
            // Editing
            ["Bold"] = "Ctrl+B",
            ["Italic"] = "Ctrl+I",
            ["Underline"] = "Ctrl+U",
            ["Undo"] = "Ctrl+Z",
            ["Redo"] = "Ctrl+Y",
            ["Copy"] = "Ctrl+C",
            ["Cut"] = "Ctrl+X",
            ["Paste"] = "Ctrl+V",
            ["SelectAll"] = "Ctrl+A",
            
            // Formatting
            ["Heading1"] = "Ctrl+1",
            ["Heading2"] = "Ctrl+2",
            ["Heading3"] = "Ctrl+3",
            ["BulletList"] = "Ctrl+L",
            ["NumberedList"] = "Ctrl+Shift+L",
            ["Quote"] = "Ctrl+Q",
            ["CodeBlock"] = "Ctrl+K",
            
            // Search and navigation
            ["Search"] = "Ctrl+F",
            ["NextNote"] = "Ctrl+Down",
            ["PreviousNote"] = "Ctrl+Up",
            
            // AI features
            ["AIGenerate"] = "Ctrl+G",
            ["AISummarize"] = "Ctrl+Shift+S",
            ["AIImprove"] = "Ctrl+Shift+I",
            
            // View
            ["ToggleTheme"] = "Ctrl+T",
            ["FocusMode"] = "F11",
            ["Settings"] = "Ctrl+,",
        };
    }

    /// <summary>
    /// Load shortcuts from disk
    /// </summary>
    public async Task<Dictionary<string, string>> LoadShortcutsAsync()
    {
        try
        {
            if (File.Exists(_settingsPath))
            {
                var json = await File.ReadAllTextAsync(_settingsPath);
                var loaded = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                if (loaded != null && loaded.Count > 0)
                {
                    _shortcuts = loaded;
                }
            }
        }
        catch (Exception)
        {
            // Use defaults on error - silent fallback is intentional for first-time users
            _shortcuts = GetDefaultShortcuts();
        }

        return _shortcuts;
    }

    /// <summary>
    /// Save shortcuts to disk
    /// </summary>
    public async Task SaveShortcutsAsync(Dictionary<string, string> shortcuts)
    {
        try
        {
            _shortcuts = shortcuts;
            var json = JsonSerializer.Serialize(shortcuts, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_settingsPath, json);
        }
        catch (Exception ex)
        {
            throw new Exception($"Erreur lors de la sauvegarde des raccourcis: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Get shortcut for an action
    /// </summary>
    public string GetShortcut(string action)
    {
        return _shortcuts.TryGetValue(action, out var shortcut) ? shortcut : string.Empty;
    }

    /// <summary>
    /// Set custom shortcut for an action
    /// </summary>
    public async Task SetShortcutAsync(string action, string shortcut)
    {
        _shortcuts[action] = shortcut;
        await SaveShortcutsAsync(_shortcuts);
    }

    /// <summary>
    /// Reset shortcuts to defaults
    /// </summary>
    public async Task ResetToDefaultsAsync()
    {
        _shortcuts = GetDefaultShortcuts();
        await SaveShortcutsAsync(_shortcuts);
    }

    /// <summary>
    /// Get all shortcuts
    /// </summary>
    public Dictionary<string, string> GetAllShortcuts()
    {
        return new Dictionary<string, string>(_shortcuts);
    }

    /// <summary>
    /// Validate if a shortcut is already in use
    /// </summary>
    public bool IsShortcutInUse(string shortcut, string excludeAction = "")
    {
        foreach (var kvp in _shortcuts)
        {
            if (kvp.Key != excludeAction && kvp.Value.Equals(shortcut, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Get action name from shortcut
    /// </summary>
    public string? GetActionForShortcut(string shortcut)
    {
        foreach (var kvp in _shortcuts)
        {
            if (kvp.Value.Equals(shortcut, StringComparison.OrdinalIgnoreCase))
            {
                return kvp.Key;
            }
        }
        return null;
    }
}
