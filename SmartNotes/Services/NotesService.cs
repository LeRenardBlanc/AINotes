using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using SmartNotes.Models;

namespace SmartNotes.Services;

/// <summary>
/// Service for managing notes storage and retrieval
/// </summary>
public class NotesService
{
    private readonly string _dataPath;
    private readonly string _notesFile;
    private readonly string _categoriesFile;
    private List<Note> _notes = new();
    private List<Category> _categories = new();

    public NotesService()
    {
        _dataPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "SmartNotes"
        );
        
        _notesFile = Path.Combine(_dataPath, "notes.json");
        _categoriesFile = Path.Combine(_dataPath, "categories.json");
        
        Directory.CreateDirectory(_dataPath);
        LoadData();
    }

    public async Task<List<Note>> GetAllNotesAsync()
    {
        return await Task.FromResult(_notes.OrderByDescending(n => n.ModifiedAt).ToList());
    }

    public async Task<Note?> GetNoteByIdAsync(Guid id)
    {
        return await Task.FromResult(_notes.FirstOrDefault(n => n.Id == id));
    }

    public async Task<Note> SaveNoteAsync(Note note)
    {
        note.ModifiedAt = DateTime.Now;
        note.WordCount = CountWords(note.Content);
        
        var existingNote = _notes.FirstOrDefault(n => n.Id == note.Id);
        if (existingNote != null)
        {
            _notes.Remove(existingNote);
        }
        
        _notes.Add(note);
        await SaveDataAsync();
        return note;
    }

    public async Task DeleteNoteAsync(Guid id)
    {
        var note = _notes.FirstOrDefault(n => n.Id == id);
        if (note != null)
        {
            _notes.Remove(note);
            await SaveDataAsync();
        }
    }

    public async Task<List<Note>> SearchNotesAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return await GetAllNotesAsync();

        var lowerQuery = query.ToLower();
        return await Task.FromResult(
            _notes.Where(n => 
                n.Title.ToLower().Contains(lowerQuery) ||
                n.Content.ToLower().Contains(lowerQuery) ||
                n.Tags.Any(t => t.ToLower().Contains(lowerQuery))
            ).OrderByDescending(n => n.ModifiedAt).ToList()
        );
    }

    public async Task<List<Category>> GetCategoriesAsync()
    {
        return await Task.FromResult(_categories);
    }

    public async Task<Category> SaveCategoryAsync(Category category)
    {
        var existing = _categories.FirstOrDefault(c => c.Id == category.Id);
        if (existing != null)
        {
            _categories.Remove(existing);
        }
        
        _categories.Add(category);
        await SaveDataAsync();
        return category;
    }

    private void LoadData()
    {
        try
        {
            if (File.Exists(_notesFile))
            {
                var json = File.ReadAllText(_notesFile);
                _notes = JsonSerializer.Deserialize<List<Note>>(json) ?? new List<Note>();
            }
            
            if (File.Exists(_categoriesFile))
            {
                var json = File.ReadAllText(_categoriesFile);
                _categories = JsonSerializer.Deserialize<List<Category>>(json) ?? new List<Category>();
            }

            // Add default categories if none exist
            if (_categories.Count == 0)
            {
                _categories.AddRange(new[]
                {
                    new Category { Name = "Général", Icon = "📝", Color = "#0078D4" },
                    new Category { Name = "Travail", Icon = "💼", Color = "#107C10" },
                    new Category { Name = "Personnel", Icon = "🏠", Color = "#D83B01" },
                    new Category { Name = "Études", Icon = "📚", Color = "#8764B8" },
                });
            }
        }
        catch
        {
            // If loading fails, start with empty lists
            _notes = new List<Note>();
            _categories = new List<Category>();
        }
    }

    private async Task SaveDataAsync()
    {
        try
        {
            var notesJson = JsonSerializer.Serialize(_notes, new JsonSerializerOptions 
            { 
                WriteIndented = true 
            });
            await File.WriteAllTextAsync(_notesFile, notesJson);
            
            var categoriesJson = JsonSerializer.Serialize(_categories, new JsonSerializerOptions 
            { 
                WriteIndented = true 
            });
            await File.WriteAllTextAsync(_categoriesFile, categoriesJson);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving data: {ex.Message}");
        }
    }

    private int CountWords(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return 0;

        return text.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
    }
}
