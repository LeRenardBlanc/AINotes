using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SmartNotes.Models;
using Markdig;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using DocumentFormat.OpenXml.Packaging;
using OpenXmlDocument = DocumentFormat.OpenXml.Wordprocessing.Document;
using OpenXmlBody = DocumentFormat.OpenXml.Wordprocessing.Body;
using OpenXmlParagraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;
using OpenXmlRun = DocumentFormat.OpenXml.Wordprocessing.Run;
using OpenXmlText = DocumentFormat.OpenXml.Wordprocessing.Text;
using OpenXmlRunProperties = DocumentFormat.OpenXml.Wordprocessing.RunProperties;
using OpenXmlBold = DocumentFormat.OpenXml.Wordprocessing.Bold;
using OpenXmlItalic = DocumentFormat.OpenXml.Wordprocessing.Italic;
using OpenXmlFontSize = DocumentFormat.OpenXml.Wordprocessing.FontSize;
using OpenXmlColor = DocumentFormat.OpenXml.Wordprocessing.Color;

namespace SmartNotes.Services;

/// <summary>
/// Service for exporting notes to various formats (PDF, Markdown, Word, ODT)
/// </summary>
public class ExportService
{
    public ExportService()
    {
        // Configure QuestPDF license for community use
        QuestPDF.Settings.License = LicenseType.Community;
    }

    /// <summary>
    /// Export note to Markdown format
    /// </summary>
    public async Task<string> ExportToMarkdownAsync(Note note, string filePath)
    {
        try
        {
            var markdown = $"# {note.Title}\n\n";
            markdown += $"*Créé le: {note.CreatedAt:dd/MM/yyyy HH:mm}*\n";
            markdown += $"*Modifié le: {note.ModifiedAt:dd/MM/yyyy HH:mm}*\n\n";
            
            if (note.Tags.Any())
            {
                markdown += $"**Tags:** {string.Join(", ", note.Tags)}\n\n";
            }
            
            markdown += "---\n\n";
            markdown += note.Content;
            
            await File.WriteAllTextAsync(filePath, markdown);
            return filePath;
        }
        catch (Exception ex)
        {
            throw new Exception($"Erreur lors de l'export en Markdown: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Export note to PDF format
    /// </summary>
    public async Task<string> ExportToPdfAsync(Note note, string filePath)
    {
        try
        {
            await Task.Run(() =>
            {
                var document = QuestPDF.Fluent.Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(2, Unit.Centimetre);
                        page.PageColor(Colors.White);
                        page.DefaultTextStyle(x => x.FontSize(12).FontFamily("Arial"));

                        page.Header()
                            .Text(note.Title)
                            .FontSize(24)
                            .SemiBold()
                            .FontColor(Colors.Blue.Darken2);

                        page.Content()
                            .PaddingVertical(1, Unit.Centimetre)
                            .Column(column =>
                            {
                                column.Spacing(10);

                                // Metadata
                                column.Item().Row(row =>
                                {
                                    row.RelativeItem().Text($"Créé: {note.CreatedAt:dd/MM/yyyy HH:mm}").FontSize(10).Italic();
                                    row.RelativeItem().Text($"Modifié: {note.ModifiedAt:dd/MM/yyyy HH:mm}").FontSize(10).Italic();
                                });

                                // Tags
                                if (note.Tags.Any())
                                {
                                    column.Item().Text($"Tags: {string.Join(", ", note.Tags)}")
                                        .FontSize(10)
                                        .FontColor(Colors.Grey.Darken1);
                                }

                                column.Item().PaddingTop(0.5f, Unit.Centimetre);

                                // Content - split by paragraphs
                                var paragraphs = note.Content.Split(new[] { "\n\n", "\r\n\r\n" }, StringSplitOptions.None);
                                foreach (var paragraph in paragraphs)
                                {
                                    if (!string.IsNullOrWhiteSpace(paragraph))
                                    {
                                        column.Item().Text(paragraph.Trim())
                                            .FontSize(12)
                                            .LineHeight(1.5f);
                                    }
                                }
                            });

                        page.Footer()
                            .AlignCenter()
                            .Text(text =>
                            {
                                text.Span("Page ");
                                text.CurrentPageNumber();
                                text.Span(" / ");
                                text.TotalPages();
                            });
                    });
                });

                document.GeneratePdf(filePath);
            });

            return filePath;
        }
        catch (Exception ex)
        {
            throw new Exception($"Erreur lors de l'export en PDF: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Export note to Word (DOCX) format
    /// </summary>
    public async Task<string> ExportToWordAsync(Note note, string filePath)
    {
        try
        {
            await Task.Run(() =>
            {
                using var wordDocument = WordprocessingDocument.Create(filePath, DocumentFormat.OpenXml.WordprocessingDocumentType.Document);
                
                // Add main document part
                var mainPart = wordDocument.AddMainDocumentPart();
                mainPart.Document = new OpenXmlDocument();
                var body = mainPart.Document.AppendChild(new OpenXmlBody());

                // Add title
                var titleParagraph = body.AppendChild(new OpenXmlParagraph());
                var titleRun = titleParagraph.AppendChild(new OpenXmlRun());
                var titleRunProperties = titleRun.AppendChild(new OpenXmlRunProperties());
                titleRunProperties.AppendChild(new OpenXmlBold());
                titleRunProperties.AppendChild(new OpenXmlFontSize { Val = "48" }); // 24pt
                titleRunProperties.AppendChild(new OpenXmlColor { Val = "0078D4" });
                titleRun.AppendChild(new OpenXmlText(note.Title));

                // Add metadata
                var metaParagraph = body.AppendChild(new OpenXmlParagraph());
                var metaRun = metaParagraph.AppendChild(new OpenXmlRun());
                var metaRunProperties = metaRun.AppendChild(new OpenXmlRunProperties());
                metaRunProperties.AppendChild(new OpenXmlFontSize { Val = "18" }); // 9pt
                metaRunProperties.AppendChild(new OpenXmlItalic());
                metaRun.AppendChild(new OpenXmlText($"Créé: {note.CreatedAt:dd/MM/yyyy HH:mm} | Modifié: {note.ModifiedAt:dd/MM/yyyy HH:mm}"));

                // Add tags if any
                if (note.Tags.Any())
                {
                    var tagsParagraph = body.AppendChild(new OpenXmlParagraph());
                    var tagsRun = tagsParagraph.AppendChild(new OpenXmlRun());
                    var tagsRunProperties = tagsRun.AppendChild(new OpenXmlRunProperties());
                    tagsRunProperties.AppendChild(new OpenXmlFontSize { Val = "18" });
                    tagsRun.AppendChild(new OpenXmlText($"Tags: {string.Join(", ", note.Tags)}"));
                }

                // Add empty line
                body.AppendChild(new OpenXmlParagraph());

                // Add content paragraphs
                var paragraphs = note.Content.Split(new[] { "\n", "\r\n" }, StringSplitOptions.None);
                foreach (var para in paragraphs)
                {
                    var contentParagraph = body.AppendChild(new OpenXmlParagraph());
                    var contentRun = contentParagraph.AppendChild(new OpenXmlRun());
                    contentRun.AppendChild(new OpenXmlText(para));
                }

                mainPart.Document.Save();
            });

            return filePath;
        }
        catch (Exception ex)
        {
            throw new Exception($"Erreur lors de l'export en Word: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Export note to plain text format
    /// </summary>
    public async Task<string> ExportToTextAsync(Note note, string filePath)
    {
        try
        {
            var text = $"{note.Title}\n";
            text += new string('=', note.Title.Length) + "\n\n";
            text += $"Créé: {note.CreatedAt:dd/MM/yyyy HH:mm}\n";
            text += $"Modifié: {note.ModifiedAt:dd/MM/yyyy HH:mm}\n";
            
            if (note.Tags.Any())
            {
                text += $"Tags: {string.Join(", ", note.Tags)}\n";
            }
            
            text += "\n" + new string('-', 50) + "\n\n";
            text += note.Content;
            
            await File.WriteAllTextAsync(filePath, text);
            return filePath;
        }
        catch (Exception ex)
        {
            throw new Exception($"Erreur lors de l'export en texte: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Get suggested file path for export
    /// </summary>
    public string GetSuggestedFilePath(Note note, string extension)
    {
        var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        var smartNotesExportPath = Path.Combine(documentsPath, "SmartNotes", "Exports");
        
        // Create directory if it doesn't exist
        Directory.CreateDirectory(smartNotesExportPath);
        
        // Clean filename
        var fileName = string.Join("_", note.Title.Split(Path.GetInvalidFileNameChars()));
        if (fileName.Length > 50)
        {
            fileName = fileName.Substring(0, 50);
        }
        
        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        return Path.Combine(smartNotesExportPath, $"{fileName}_{timestamp}.{extension}");
    }
}
