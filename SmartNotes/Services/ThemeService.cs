using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Styling;

namespace SmartNotes.Services;

/// <summary>
/// Service for managing application themes (light/dark mode)
/// </summary>
public class ThemeService
{
    private readonly string _settingsPath;
    private ThemeSettings _currentSettings;

    public ThemeService()
    {
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var smartNotesPath = Path.Combine(appDataPath, "SmartNotes");
        Directory.CreateDirectory(smartNotesPath);
        _settingsPath = Path.Combine(smartNotesPath, "theme_settings.json");
        
        _currentSettings = new ThemeSettings();
    }

    /// <summary>
    /// Load theme settings from disk
    /// </summary>
    public async Task<ThemeSettings> LoadThemeSettingsAsync()
    {
        try
        {
            if (File.Exists(_settingsPath))
            {
                var json = await File.ReadAllTextAsync(_settingsPath);
                _currentSettings = JsonSerializer.Deserialize<ThemeSettings>(json) ?? new ThemeSettings();
            }
        }
        catch
        {
            _currentSettings = new ThemeSettings();
        }

        return _currentSettings;
    }

    /// <summary>
    /// Save theme settings to disk
    /// </summary>
    public async Task SaveThemeSettingsAsync(ThemeSettings settings)
    {
        try
        {
            _currentSettings = settings;
            var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_settingsPath, json);
        }
        catch (Exception ex)
        {
            throw new Exception($"Erreur lors de la sauvegarde des paramètres de thème: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Apply theme to the application
    /// </summary>
    public void ApplyTheme(ThemeVariant theme)
    {
        if (Application.Current != null)
        {
            Application.Current.RequestedThemeVariant = theme;
        }
    }

    /// <summary>
    /// Get current theme variant
    /// </summary>
    public ThemeVariant GetCurrentTheme()
    {
        if (Application.Current != null)
        {
            return Application.Current.RequestedThemeVariant ?? ThemeVariant.Default;
        }
        return ThemeVariant.Default;
    }

    /// <summary>
    /// Toggle between light and dark themes
    /// </summary>
    public async Task<ThemeVariant> ToggleThemeAsync()
    {
        var currentTheme = GetCurrentTheme();
        var newTheme = currentTheme == ThemeVariant.Dark ? ThemeVariant.Light : ThemeVariant.Dark;
        
        ApplyTheme(newTheme);
        
        _currentSettings.CurrentTheme = newTheme == ThemeVariant.Dark ? "Dark" : "Light";
        await SaveThemeSettingsAsync(_currentSettings);
        
        return newTheme;
    }

    /// <summary>
    /// Set specific theme
    /// </summary>
    public async Task SetThemeAsync(string themeName)
    {
        var theme = themeName.ToLower() switch
        {
            "dark" => ThemeVariant.Dark,
            "light" => ThemeVariant.Light,
            _ => ThemeVariant.Default
        };

        ApplyTheme(theme);
        
        _currentSettings.CurrentTheme = themeName;
        await SaveThemeSettingsAsync(_currentSettings);
    }

    /// <summary>
    /// Initialize theme from saved settings
    /// </summary>
    public async Task InitializeThemeAsync()
    {
        var settings = await LoadThemeSettingsAsync();
        
        var theme = settings.CurrentTheme.ToLower() switch
        {
            "dark" => ThemeVariant.Dark,
            "light" => ThemeVariant.Light,
            _ => ThemeVariant.Default
        };

        ApplyTheme(theme);
    }
}

/// <summary>
/// Theme settings model
/// </summary>
public class ThemeSettings
{
    public string CurrentTheme { get; set; } = "Default";
    public string AccentColor { get; set; } = "#0078D4";
    public bool UseSystemTheme { get; set; } = true;
}
