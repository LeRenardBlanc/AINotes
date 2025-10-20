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
    /// Enhanced version with more topics and better context awareness
    /// </summary>
    public Task<string> GenerateContentAsync(string prompt)
    {
        if (string.IsNullOrWhiteSpace(prompt))
            return Task.FromResult(string.Empty);

        // Enhanced AI content generation with more topics
        var responses = new Dictionary<string, string>
        {
            ["photosynthèse"] = "La photosynthèse est le processus biologique par lequel les plantes convertissent la lumière solaire en énergie chimique. Ce mécanisme fondamental permet aux végétaux de produire du glucose à partir de dioxyde de carbone et d'eau, tout en libérant de l'oxygène dans l'atmosphère. Les chloroplastes, contenant la chlorophylle, sont les organites responsables de cette réaction vitale pour l'ensemble des écosystèmes terrestres. Ce processus se déroule en deux phases principales : les réactions photochimiques (phase claire) et le cycle de Calvin (phase sombre).",
            
            ["révolution française"] = "La Révolution française (1789-1799) fut une période de bouleversements politiques et sociaux majeurs qui transforma profondément la France et influença l'Europe entière. Débutant avec la prise de la Bastille le 14 juillet 1789, elle marqua la fin de l'Ancien Régime et de la monarchie absolue. Les idéaux révolutionnaires de liberté, égalité et fraternité conduisirent à l'établissement de la Première République et à la Déclaration des Droits de l'Homme et du Citoyen. Cette période vit l'abolition des privilèges féodaux, la nationalisation des biens du clergé, et l'émergence d'une nouvelle organisation politique et sociale.",
            
            ["intelligence artificielle"] = "L'intelligence artificielle (IA) représente l'ensemble des techniques permettant à des machines d'imiter certaines capacités cognitives humaines. Les systèmes d'IA modernes utilisent des algorithmes d'apprentissage automatique pour analyser des données, reconnaître des modèles et prendre des décisions. Les applications de l'IA sont vastes : reconnaissance vocale, vision par ordinateur, traduction automatique, et assistants virtuels qui transforment notre quotidien. Les récentes avancées en deep learning et en traitement du langage naturel ont permis des progrès spectaculaires dans des domaines comme la génération de texte, la création d'images et l'analyse prédictive.",
            
            ["changement climatique"] = "Le changement climatique désigne les modifications durables des températures et des conditions météorologiques à l'échelle planétaire. Depuis le milieu du XXe siècle, les activités humaines, notamment la combustion de combustibles fossiles, sont devenues le principal moteur du réchauffement climatique. Les conséquences incluent la fonte des glaciers, l'élévation du niveau des mers, l'intensification des événements météorologiques extrêmes, et des perturbations des écosystèmes. La limitation du réchauffement à 1,5°C nécessite des actions urgentes et coordonnées à l'échelle mondiale.",
            
            ["quantum"] = "L'informatique quantique exploite les propriétés de la mécanique quantique pour effectuer des calculs d'une puissance inégalée. Contrairement aux ordinateurs classiques qui utilisent des bits (0 ou 1), les ordinateurs quantiques utilisent des qubits qui peuvent exister dans une superposition d'états. Ce phénomène, combiné à l'intrication quantique, permet de résoudre certains problèmes complexes exponentiellement plus rapidement que les ordinateurs traditionnels. Les applications potentielles incluent la cryptographie, la simulation moléculaire, et l'optimisation de systèmes complexes.",
            
            ["blockchain"] = "La blockchain est une technologie de stockage et de transmission d'informations fonctionnant sans organe central de contrôle. Elle consiste en une base de données distribuée, sécurisée et transparente, contenant l'historique de tous les échanges effectués. Chaque bloc est lié cryptographiquement au précédent, formant une chaîne inaltérable. Au-delà des cryptomonnaies, la blockchain trouve des applications dans la traçabilité des produits, les contrats intelligents, la gestion de l'identité numérique, et la certification de documents.",
        };

        var promptLower = prompt.ToLower();
        foreach (var key in responses.Keys)
        {
            if (promptLower.Contains(key))
            {
                return Task.FromResult(responses[key]);
            }
        }

        // Context-aware generic expansion with better structure
        var genericContent = $"# {prompt}\n\n" +
            $"## Introduction\n\n" +
            $"{prompt} est un sujet d'une importance capitale qui mérite une attention particulière dans le contexte actuel. " +
            $"Ce concept englobe différents aspects et dimensions qu'il convient d'analyser en profondeur pour en saisir toute la complexité.\n\n" +
            $"## Aspects Clés\n\n" +
            $"Les recherches dans ce domaine montrent que la compréhension de {prompt} nécessite une approche multidisciplinaire. " +
            $"Il est essentiel de considérer à la fois les dimensions théoriques et pratiques de ce sujet.\n\n" +
            $"## Implications\n\n" +
            $"Les implications de {prompt} sont multiples et touchent différents secteurs. " +
            $"Une réflexion critique sur ses applications et ses conséquences s'avère nécessaire pour en tirer le meilleur parti.";
        
        return Task.FromResult(genericContent);
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

    /// <summary>
    /// Suggest better formatting for the text
    /// </summary>
    public Task<List<string>> SuggestFormattingAsync(string text)
    {
        var suggestions = new List<string>();
        
        if (string.IsNullOrWhiteSpace(text))
            return Task.FromResult(suggestions);

        // Check for missing structure
        if (!text.Contains("\n\n") && text.Length > 200)
        {
            suggestions.Add("💡 Ajoutez des paragraphes pour améliorer la lisibilité");
        }

        // Check for headings
        if (text.Length > 500 && !text.Contains("#") && !text.Contains("titre", StringComparison.OrdinalIgnoreCase))
        {
            suggestions.Add("📑 Structurez votre texte avec des titres et sous-titres");
        }

        // Check for lists
        if ((text.Contains("premièrement") || text.Contains("deuxièmement") || text.Contains("ensuite")) 
            && !text.Contains("•") && !text.Contains("-"))
        {
            suggestions.Add("📝 Utilisez des listes à puces pour énumérer vos points");
        }

        // Check for emphasis
        if (text.Length > 300 && !text.Contains("**") && !text.Contains("important", StringComparison.OrdinalIgnoreCase))
        {
            suggestions.Add("⭐ Mettez en valeur les concepts clés avec du texte en gras");
        }

        return Task.FromResult(suggestions);
    }

    /// <summary>
    /// Detect sentiment and suggest appropriate tone adjustments
    /// </summary>
    public Task<string> AnalyzeToneAsync(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return Task.FromResult("Neutre");

        var textLower = text.ToLower();
        
        // Simple sentiment analysis
        var positiveWords = new[] { "excellent", "super", "génial", "formidable", "merveilleux", "parfait" };
        var negativeWords = new[] { "mauvais", "problème", "difficile", "erreur", "échec", "impossible" };
        var formalWords = new[] { "convient", "nécessite", "constitue", "s'avère", "demeure" };
        
        var positiveCount = positiveWords.Count(w => textLower.Contains(w));
        var negativeCount = negativeWords.Count(w => textLower.Contains(w));
        var formalCount = formalWords.Count(w => textLower.Contains(w));

        if (formalCount >= 2)
            return Task.FromResult("Formel et professionnel");
        if (positiveCount > negativeCount)
            return Task.FromResult("Positif et optimiste");
        if (negativeCount > positiveCount)
            return Task.FromResult("Critique - considérez un ton plus constructif");
        
        return Task.FromResult("Neutre et équilibré");
    }

    /// <summary>
    /// Generate contextual completion based on the current writing context
    /// </summary>
    public Task<string> GetSmartCompletionAsync(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return Task.FromResult(string.Empty);

        var textLower = text.ToLower().TrimEnd();
        
        // Context-aware completions
        if (textLower.EndsWith("par exemple"))
            return Task.FromResult(", on peut citer");
        
        if (textLower.EndsWith("en conclusion"))
            return Task.FromResult(", il est essentiel de retenir que");
        
        if (textLower.EndsWith("tout d'abord"))
            return Task.FromResult(", il convient de noter que");
        
        if (textLower.EndsWith("de plus"))
            return Task.FromResult(", il est important de souligner que");
        
        if (textLower.EndsWith("cependant"))
            return Task.FromResult(", il faut considérer que");
        
        if (textLower.EndsWith("ainsi"))
            return Task.FromResult(", nous pouvons observer que");

        // Generic helpful continuation
        return Task.FromResult(" [Appuyez sur Tab pour continuer avec l'IA]");
    }

    /// <summary>
    /// Improve text quality with better vocabulary and structure
    /// </summary>
    public Task<string> ImproveTextQualityAsync(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return Task.FromResult(string.Empty);

        // Replace common informal expressions with more polished alternatives
        var improvements = new Dictionary<string, string>
        {
            { "il y a", "il existe" },
            { "il faut", "il convient de" },
            { "on peut", "il est possible de" },
            { "très important", "essentiel" },
            { "beaucoup de", "de nombreux" },
            { "assez", "relativement" },
            { "pas mal", "considérable" },
            { "un peu", "quelque peu" },
        };

        var improvedText = text;
        foreach (var (oldPhrase, newPhrase) in improvements)
        {
            improvedText = System.Text.RegularExpressions.Regex.Replace(
                improvedText, 
                $@"\b{oldPhrase}\b", 
                newPhrase, 
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
            );
        }

        return Task.FromResult(improvedText);
    }
}
