using Avalonia.Controls;
using Avalonia.Input;
using SmartNotes.ViewModels;

namespace SmartNotes.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
        // Add keyboard shortcuts
        KeyDown += OnKeyDown;
    }

    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        var viewModel = DataContext as MainWindowViewModel;
        if (viewModel == null) return;

        // Ctrl+N - New Note
        if (e.Key == Key.N && e.KeyModifiers == KeyModifiers.Control)
        {
            viewModel.CreateNewNoteCommand.Execute(null);
            e.Handled = true;
        }
        // Ctrl+S - Save Note
        else if (e.Key == Key.S && e.KeyModifiers == KeyModifiers.Control)
        {
            viewModel.SaveCurrentNoteCommand.Execute(null);
            e.Handled = true;
        }
        // Ctrl+F - Focus Search
        else if (e.Key == Key.F && e.KeyModifiers == KeyModifiers.Control)
        {
            // Focus search box (would need named reference)
            e.Handled = true;
        }
        // Ctrl+D - Duplicate Note (future feature)
        else if (e.Key == Key.D && e.KeyModifiers == KeyModifiers.Control)
        {
            e.Handled = true;
        }
    }
}