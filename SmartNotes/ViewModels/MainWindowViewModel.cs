using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartNotes.Models;
using SmartNotes.Services;

namespace SmartNotes.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly NotesService _notesService;
    private readonly AIService _aiService;
    private readonly ExportService _exportService;
    private readonly ThemeService _themeService;
    private readonly FormattingService _formattingService;
    private readonly KeyboardShortcutsService _shortcutsService;

    [ObservableProperty]
    private ObservableCollection<Note> _notes = new();

    [ObservableProperty]
    private ObservableCollection<Category> _categories = new();

    [ObservableProperty]
    private Note? _selectedNote;

    [ObservableProperty]
    private string _searchQuery = string.Empty;

    [ObservableProperty]
    private string _editorContent = string.Empty;

    [ObservableProperty]
    private string _noteTitle = string.Empty;

    [ObservableProperty]
    private bool _isEditing;

    [ObservableProperty]
    private ObservableCollection<string> _aiSuggestions = new();

    [ObservableProperty]
    private string _statusMessage = "Prêt";

    [ObservableProperty]
    private string _currentTheme = "Clair";

    public MainWindowViewModel()
    {
        _notesService = new NotesService();
        _aiService = new AIService();
        _exportService = new ExportService();
        _themeService = new ThemeService();
        _formattingService = new FormattingService();
        _shortcutsService = new KeyboardShortcutsService();
        
        _ = InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        await _themeService.InitializeThemeAsync();
        UpdateThemeDisplay();
        await LoadNotesAsync();
        await LoadCategoriesAsync();
    }

    [RelayCommand]
    private async Task LoadNotesAsync()
    {
        var notes = await _notesService.GetAllNotesAsync();
        Notes.Clear();
        foreach (var note in notes)
        {
            Notes.Add(note);
        }
        
        if (Notes.Count == 0)
        {
            // Create a welcome note
            var welcomeNote = new Note
            {
                Title = "Bienvenue dans SmartNotes ! 🎉",
                Content = "SmartNotes est votre compagnon d'écriture intelligent.\n\n" +
                         "✨ Fonctionnalités principales :\n" +
                         "• Éditeur de texte riche\n" +
                         "• Organisation avec catégories et tags\n" +
                         "• Recherche instantanée\n" +
                         "• Assistant IA intégré\n" +
                         "• Sauvegarde automatique\n\n" +
                         "Commencez à écrire et laissez l'IA vous assister !",
                IsFavorite = true
            };
            await _notesService.SaveNoteAsync(welcomeNote);
            Notes.Add(welcomeNote);
        }
    }

    [RelayCommand]
    private async Task LoadCategoriesAsync()
    {
        var categories = await _notesService.GetCategoriesAsync();
        Categories.Clear();
        foreach (var category in categories)
        {
            Categories.Add(category);
        }
    }

    [RelayCommand]
    private void CreateNewNote()
    {
        var newNote = new Note();
        Notes.Insert(0, newNote);
        SelectedNote = newNote;
        NoteTitle = newNote.Title;
        EditorContent = newNote.Content;
        IsEditing = true;
    }

    [RelayCommand]
    private void SelectNote(Note? note)
    {
        if (note != null)
        {
            SelectedNote = note;
        }
    }

    [RelayCommand]
    private async Task SaveCurrentNoteAsync()
    {
        if (SelectedNote != null)
        {
            SelectedNote.Title = NoteTitle;
            SelectedNote.Content = EditorContent;
            await _notesService.SaveNoteAsync(SelectedNote);
            StatusMessage = $"Note sauvegardée à {DateTime.Now:HH:mm}";
        }
    }

    [RelayCommand]
    private async Task DeleteNoteAsync(Note? note)
    {
        if (note != null)
        {
            await _notesService.DeleteNoteAsync(note.Id);
            Notes.Remove(note);
            if (SelectedNote?.Id == note.Id)
            {
                SelectedNote = null;
                IsEditing = false;
            }
        }
    }

    [RelayCommand]
    private async Task SearchNotesAsync()
    {
        var results = await _notesService.SearchNotesAsync(SearchQuery);
        Notes.Clear();
        foreach (var note in results)
        {
            Notes.Add(note);
        }
    }

    [RelayCommand]
    private async Task ToggleFavoriteAsync(Note? note)
    {
        if (note != null)
        {
            note.IsFavorite = !note.IsFavorite;
            await _notesService.SaveNoteAsync(note);
        }
    }

    [RelayCommand]
    private async Task GenerateAIContentAsync(string prompt)
    {
        try
        {
            StatusMessage = "Génération IA en cours...";
            var content = await _aiService.GenerateContentAsync(prompt);
            EditorContent += "\n\n" + content;
            StatusMessage = "Contenu généré !";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Erreur: {ex.Message}";
        }
    }

    [RelayCommand]
    private async Task SummarizeNoteAsync()
    {
        if (string.IsNullOrWhiteSpace(EditorContent))
            return;

        try
        {
            StatusMessage = "Création du résumé...";
            var summary = await _aiService.SummarizeAsync(EditorContent);
            EditorContent = summary + "\n\n---\n\n" + EditorContent;
            StatusMessage = "Résumé créé !";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Erreur: {ex.Message}";
        }
    }

    [RelayCommand]
    private async Task GenerateFlashcardsAsync()
    {
        if (string.IsNullOrWhiteSpace(EditorContent))
            return;

        try
        {
            StatusMessage = "Génération des fiches...";
            var flashcards = await _aiService.GenerateFlashcardsAsync(EditorContent);
            
            var flashcardNote = new Note
            {
                Title = $"Fiches de révision - {NoteTitle}",
                Content = "# Fiches de Révision\n\n" + 
                         string.Join("\n\n", flashcards.Select((f, i) => 
                             $"## Question {i + 1}\n**Q:** {f.Question}\n**R:** {f.Answer}"))
            };
            
            await _notesService.SaveNoteAsync(flashcardNote);
            Notes.Insert(0, flashcardNote);
            StatusMessage = "Fiches créées !";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Erreur: {ex.Message}";
        }
    }

    partial void OnSelectedNoteChanged(Note? value)
    {
        if (value != null)
        {
            NoteTitle = value.Title;
            EditorContent = value.Content;
            IsEditing = true;
        }
        else
        {
            IsEditing = false;
        }
    }

    partial void OnEditorContentChanged(string value)
    {
        // Auto-save after typing (debounced in real implementation)
        if (SelectedNote != null)
        {
            _ = Task.Run(async () =>
            {
                await Task.Delay(2000); // Simple debounce
                await SaveCurrentNoteAsync();
            });
        }
    }

    [RelayCommand]
    private async Task ExportNotePdfAsync()
    {
        if (SelectedNote == null)
            return;

        try
        {
            StatusMessage = "Export PDF en cours...";
            var filePath = _exportService.GetSuggestedFilePath(SelectedNote, "pdf");
            await _exportService.ExportToPdfAsync(SelectedNote, filePath);
            StatusMessage = $"PDF exporté: {filePath}";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Erreur export PDF: {ex.Message}";
        }
    }

    [RelayCommand]
    private async Task ExportNoteMarkdownAsync()
    {
        if (SelectedNote == null)
            return;

        try
        {
            StatusMessage = "Export Markdown en cours...";
            var filePath = _exportService.GetSuggestedFilePath(SelectedNote, "md");
            await _exportService.ExportToMarkdownAsync(SelectedNote, filePath);
            StatusMessage = $"Markdown exporté: {filePath}";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Erreur export Markdown: {ex.Message}";
        }
    }

    [RelayCommand]
    private async Task ExportNoteWordAsync()
    {
        if (SelectedNote == null)
            return;

        try
        {
            StatusMessage = "Export Word en cours...";
            var filePath = _exportService.GetSuggestedFilePath(SelectedNote, "docx");
            await _exportService.ExportToWordAsync(SelectedNote, filePath);
            StatusMessage = $"Word exporté: {filePath}";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Erreur export Word: {ex.Message}";
        }
    }

    [RelayCommand]
    private async Task ToggleThemeAsync()
    {
        try
        {
            var newTheme = await _themeService.ToggleThemeAsync();
            UpdateThemeDisplay();
            StatusMessage = $"Thème changé: {CurrentTheme}";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Erreur changement thème: {ex.Message}";
        }
    }

    private void UpdateThemeDisplay()
    {
        var theme = _themeService.GetCurrentTheme();
        CurrentTheme = theme.Key switch
        {
            "Dark" => "Sombre",
            "Light" => "Clair",
            _ => "Système"
        };
    }

    [RelayCommand]
    private async Task ImproveTextAsync()
    {
        if (string.IsNullOrWhiteSpace(EditorContent))
            return;

        try
        {
            StatusMessage = "Amélioration du texte...";
            var improved = await _aiService.ImproveTextQualityAsync(EditorContent);
            EditorContent = improved;
            StatusMessage = "Texte amélioré !";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Erreur: {ex.Message}";
        }
    }

    [RelayCommand]
    private async Task AnalyzeFormattingAsync()
    {
        if (string.IsNullOrWhiteSpace(EditorContent))
            return;

        try
        {
            StatusMessage = "Analyse de la mise en forme...";
            var suggestions = await _aiService.SuggestFormattingAsync(EditorContent);
            
            if (suggestions.Any())
            {
                var suggestionText = string.Join("\n", suggestions);
                StatusMessage = suggestionText;
            }
            else
            {
                StatusMessage = "✅ Mise en forme correcte !";
            }
        }
        catch (Exception ex)
        {
            StatusMessage = $"Erreur: {ex.Message}";
        }
    }

    [RelayCommand]
    private void ApplyBoldFormatting()
    {
        // Note: This is a simplified implementation that applies formatting to the last word.
        // Full text selection support would require integration with the TextBox's SelectionStart/SelectionEnd properties.
        if (!string.IsNullOrWhiteSpace(EditorContent))
        {
            var lines = EditorContent.Split('\n');
            if (lines.Length > 0)
            {
                var lastLine = lines[^1];
                var words = lastLine.Split(' ');
                if (words.Length > 0)
                {
                    words[^1] = _formattingService.ApplyBold(words[^1]);
                    lines[^1] = string.Join(' ', words);
                    EditorContent = string.Join('\n', lines);
                }
            }
        }
    }

    [RelayCommand]
    private void ApplyItalicFormatting()
    {
        // Note: This is a simplified implementation. See ApplyBoldFormatting comment.
        if (!string.IsNullOrWhiteSpace(EditorContent))
        {
            var lines = EditorContent.Split('\n');
            if (lines.Length > 0)
            {
                var lastLine = lines[^1];
                var words = lastLine.Split(' ');
                if (words.Length > 0)
                {
                    words[^1] = _formattingService.ApplyItalic(words[^1]);
                    lines[^1] = string.Join(' ', words);
                    EditorContent = string.Join('\n', lines);
                }
            }
        }
    }

    [RelayCommand]
    private void InsertHeading(string level)
    {
        if (int.TryParse(level, out var headingLevel))
        {
            var heading = _formattingService.ApplyHeading("Titre", headingLevel);
            EditorContent += $"\n{heading}\n";
        }
    }

    [RelayCommand]
    private void InsertBulletList()
    {
        var list = _formattingService.CreateBulletList(new List<string> { "Item 1", "Item 2", "Item 3" });
        EditorContent += $"\n{list}\n";
    }

    [RelayCommand]
    private void InsertTable()
    {
        var headers = new List<string> { "Colonne 1", "Colonne 2", "Colonne 3" };
        var rows = new List<List<string>>
        {
            new() { "Donnée 1", "Donnée 2", "Donnée 3" },
            new() { "Donnée 4", "Donnée 5", "Donnée 6" }
        };
        var table = _formattingService.CreateTable(headers, rows);
        EditorContent += $"\n{table}\n";
    }

    [RelayCommand]
    private void InsertHorizontalRule()
    {
        EditorContent += _formattingService.InsertHorizontalRule();
    }

    [RelayCommand]
    private void InsertCodeBlock()
    {
        var code = _formattingService.CreateCodeBlock("// Votre code ici", "csharp");
        EditorContent += $"\n{code}\n";
    }
}
