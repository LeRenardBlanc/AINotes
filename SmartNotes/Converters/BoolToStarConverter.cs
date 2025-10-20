using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace SmartNotes.Converters;

/// <summary>
/// Converter to display star emoji for favorite notes
/// </summary>
public class BoolToStarConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isFavorite && isFavorite)
        {
            return "⭐";
        }
        return string.Empty;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
