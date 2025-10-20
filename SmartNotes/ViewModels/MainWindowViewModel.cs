using System;
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

    public MainWindowViewModel()
    {
        _notesService = new NotesService();
        _aiService = new AIService();
        
        _ = InitializeAsync();
    }

    private async Task InitializeAsync()
    {
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
}
