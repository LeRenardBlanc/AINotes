using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartNotes.Services;

/// <summary>
/// Service for AI-powered features (text completion, generation, summarization)
/// This is a mock implementation that demonstrates the AI capabilities
/// In production, this would use ONNX Runtime with a local language model
/// </summary>
public class AIService
{
    private readonly Random _random = new();
    
    // Common French words and phrases for autocomplete
    private readonly Dictionary<string, List<string>> _completions = new()
    {
        { "la", new() { "la Révolution française", "la photosynthèse", "la société", "la technologie" } },
        { "le", new() { "le développement", "le projet", "le système", "le problème" } },
        { "pour", new() { "pour conclure", "pour résumer", "pour commencer", "pour cette raison" } },
        { "il", new() { "il est important de", "il faut noter que", "il convient de", "il est essentiel de" } },
        { "en", new() { "en conclusion", "en effet", "en résumé", "en particulier" } },
        { "bcp", new() { "beaucoup" } },
        { "svp", new() { "s'il vous plaît" } },
        { "tjrs", new() { "toujours" } },
        { "pr", new() { "pour" } },
    };

    /// <summary>
    /// Get AI suggestions for text completion
    /// </summary>
    public Task<List<string>> GetCompletionsAsync(string text, int maxSuggestions = 3)
    {
        var suggestions = new List<string>();
        
        if (string.IsNullOrWhiteSpace(text))
            return Task.FromResult(suggestions);

        var lastWord = GetLastWord(text).ToLower();
        
        // Check for abbreviations
        if (_completions.ContainsKey(lastWord))
        {
            suggestions.AddRange(_completions[lastWord].Take(maxSuggestions));
        }
        else
        {
            // Generate contextual suggestions
            suggestions.AddRange(GenerateContextualSuggestions(text, maxSuggestions));
        }
        
        return Task.FromResult(suggestions);
    }

    /// <summary>
    /// Generate expanded content from a keyword or phrase
    /// </summary>
    public Task<string> GenerateContentAsync(string prompt)
    {
        if (string.IsNullOrWhiteSpace(prompt))
            return Task.FromResult(string.Empty);

        // Simulate AI content generation
        var responses = new Dictionary<string, string>
        {
            ["photosynthèse"] = "La photosynthèse est le processus biologique par lequel les plantes convertissent la lumière solaire en énergie chimique. Ce mécanisme fondamental permet aux végétaux de produire du glucose à partir de dioxyde de carbone et d'eau, tout en libérant de l'oxygène dans l'atmosphère. Les chloroplastes, contenant la chlorophylle, sont les organites responsables de cette réaction vitale pour l'ensemble des écosystèmes terrestres.",
            
            ["révolution française"] = "La Révolution française (1789-1799) fut une période de bouleversements politiques et sociaux majeurs qui transforma profondément la France et influença l'Europe entière. Débutant avec la prise de la Bastille le 14 juillet 1789, elle marqua la fin de l'Ancien Régime et de la monarchie absolue. Les idéaux révolutionnaires de liberté, égalité et fraternité conduisirent à l'établissement de la Première République et à la Déclaration des Droits de l'Homme et du Citoyen.",
            
            ["intelligence artificielle"] = "L'intelligence artificielle (IA) représente l'ensemble des techniques permettant à des machines d'imiter certaines capacités cognitives humaines. Les systèmes d'IA modernes utilisent des algorithmes d'apprentissage automatique pour analyser des données, reconnaître des modèles et prendre des décisions. Les applications de l'IA sont vastes : reconnaissance vocale, vision par ordinateur, traduction automatique, et assistants virtuels qui transforment notre quotidien.",
        };

        var promptLower = prompt.ToLower();
        foreach (var key in responses.Keys)
        {
            if (promptLower.Contains(key))
            {
                return Task.FromResult(responses[key]);
            }
        }

        // Generic expansion
        return Task.FromResult($"{prompt} est un sujet important qui mérite une attention particulière. Ce concept englobe différents aspects et dimensions qu'il convient d'analyser en profondeur. Les recherches dans ce domaine montrent que la compréhension de {prompt} nécessite une approche multidisciplinaire et une réflexion critique sur ses implications pratiques et théoriques.");
    }

    /// <summary>
    /// Summarize text content
    /// </summary>
    public Task<string> SummarizeAsync(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return Task.FromResult(string.Empty);

        var sentences = text.Split(new[] { '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Trim())
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .ToList();

        if (sentences.Count == 0)
            return Task.FromResult("Aucun contenu à résumer.");

        // Take first few sentences as summary
        var summaryCount = Math.Min(3, sentences.Count);
        var summary = string.Join(". ", sentences.Take(summaryCount)) + ".";
        
        return Task.FromResult($"Résumé : {summary}");
    }

    /// <summary>
    /// Generate study flashcards from content
    /// </summary>
    public Task<List<(string Question, string Answer)>> GenerateFlashcardsAsync(string content)
    {
        var flashcards = new List<(string, string)>();
        
        if (string.IsNullOrWhiteSpace(content))
            return Task.FromResult(flashcards);

        // Extract key concepts (simplified)
        var sentences = content.Split(new[] { '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Trim())
            .Where(s => !string.IsNullOrWhiteSpace(s) && s.Length > 20)
            .Take(5)
            .ToList();

        foreach (var sentence in sentences)
        {
            var words = sentence.Split(' ');
            if (words.Length > 5)
            {
                var question = $"Qu'est-ce que {words[0].ToLower()} {words[1].ToLower()}... ?";
                flashcards.Add((question, sentence));
            }
        }

        if (flashcards.Count == 0)
        {
            flashcards.Add(("Question exemple", "Réponse basée sur votre contenu"));
        }

        return Task.FromResult(flashcards);
    }

    /// <summary>
    /// Reformulate text in different styles
    /// </summary>
    public Task<string> ReformulateAsync(string text, string style)
    {
        if (string.IsNullOrWhiteSpace(text))
            return Task.FromResult(string.Empty);

        var result = style.ToLower() switch
        {
            "formel" => $"Dans le cadre de nos travaux, il convient de noter que {text}",
            "simple" => $"En termes simples : {text}",
            "concis" => text.Length > 100 ? text[..100] + "..." : text,
            _ => text
        };

        return Task.FromResult(result);
    }

    /// <summary>
    /// Extract key points from text
    /// </summary>
    public Task<List<string>> ExtractKeyPointsAsync(string text)
    {
        var points = new List<string>();
        
        if (string.IsNullOrWhiteSpace(text))
            return Task.FromResult(points);

        var sentences = text.Split(new[] { '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Trim())
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Take(5)
            .ToList();

        foreach (var sentence in sentences)
        {
            points.Add($"• {sentence}");
        }

        return Task.FromResult(points);
    }

    private string GetLastWord(string text)
    {
        var trimmed = text.TrimEnd();
        var lastSpace = trimmed.LastIndexOf(' ');
        return lastSpace >= 0 ? trimmed[(lastSpace + 1)..] : trimmed;
    }

    private List<string> GenerateContextualSuggestions(string text, int count)
    {
        var suggestions = new List<string>
        {
            "est un aspect important",
            "représente une opportunité",
            "nécessite une analyse approfondie",
            "constitue un élément clé",
            "demeure un sujet d'actualité"
        };

        return suggestions.Take(count).ToList();
    }
}
