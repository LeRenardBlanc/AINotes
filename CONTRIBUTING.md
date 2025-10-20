# Contributing to SmartNotes

🎉 Thank you for considering contributing to SmartNotes! We're building the best intelligent note-taking application with local AI.

## 🌟 Ways to Contribute

There are many ways to contribute to SmartNotes:

- 🐛 **Report bugs** and issues
- 💡 **Suggest new features** and improvements
- 📝 **Improve documentation**
- 🌍 **Translate** to other languages
- 🎨 **Design** UI/UX improvements
- 💻 **Write code** for new features
- 🧪 **Write tests** to improve quality
- 📣 **Spread the word** about SmartNotes

## 🚀 Getting Started

### Prerequisites

- **.NET 9.0 SDK** or later
- **Git** for version control
- **Visual Studio 2022**, **VS Code**, or **JetBrains Rider**
- Basic knowledge of **C#** and **XAML**

### Setting Up Development Environment

1. **Fork the repository**
   ```bash
   # Click "Fork" on GitHub
   ```

2. **Clone your fork**
   ```bash
   git clone https://github.com/YOUR_USERNAME/AINotes.git
   cd AINotes/SmartNotes
   ```

3. **Restore dependencies**
   ```bash
   dotnet restore
   ```

4. **Build the project**
   ```bash
   dotnet build
   ```

5. **Run the application**
   ```bash
   dotnet run
   ```

### Project Structure

```
SmartNotes/
├── Models/          # Data models (Note, Category, etc.)
├── ViewModels/      # MVVM ViewModels
├── Views/           # UI (XAML files)
├── Services/        # Business logic (NotesService, AIService)
├── Converters/      # Value converters for data binding
└── Assets/          # Resources (icons, images)
```

## 📋 Development Workflow

### 1. Create a Branch

Always create a new branch for your work:

```bash
git checkout -b feature/your-feature-name
# or
git checkout -b fix/your-bug-fix
```

Branch naming conventions:
- `feature/` - New features
- `fix/` - Bug fixes
- `docs/` - Documentation changes
- `refactor/` - Code refactoring
- `test/` - Adding tests
- `style/` - UI/UX changes

### 2. Make Your Changes

- Write clean, readable code
- Follow C# coding conventions
- Add comments for complex logic
- Use meaningful variable names
- Keep methods small and focused

### 3. Test Your Changes

```bash
# Build to check for compilation errors
dotnet build

# Run the application to test functionality
dotnet run
```

**Manual Testing Checklist**:
- [ ] Create a new note
- [ ] Edit and save a note
- [ ] Search for notes
- [ ] Use AI features
- [ ] Test keyboard shortcuts
- [ ] Verify UI responsiveness

### 4. Commit Your Changes

Use clear and descriptive commit messages:

```bash
git add .
git commit -m "feat: Add dark theme support"
```

**Commit Message Format**:
```
<type>: <subject>

<body (optional)>

<footer (optional)>
```

**Types**:
- `feat`: New feature
- `fix`: Bug fix
- `docs`: Documentation
- `style`: Formatting, UI changes
- `refactor`: Code restructuring
- `test`: Adding tests
- `chore`: Maintenance tasks

**Examples**:
```
feat: Add export to PDF functionality
fix: Resolve search crash with empty query
docs: Update installation instructions
style: Improve button styling in toolbar
refactor: Extract AI service interface
```

### 5. Push to Your Fork

```bash
git push origin feature/your-feature-name
```

### 6. Create a Pull Request

1. Go to the [original repository](https://github.com/LeRenardBlanc/AINotes)
2. Click "New Pull Request"
3. Select your branch
4. Fill in the PR template:
   - **Title**: Clear description of changes
   - **Description**: What, why, and how
   - **Screenshots**: For UI changes
   - **Related issues**: Reference any related issues

## 📝 Coding Guidelines

### C# Style

Follow standard C# conventions:

```csharp
// ✅ Good
public class NotesService
{
    private readonly string _dataPath;
    
    public async Task<Note> GetNoteAsync(Guid id)
    {
        // Implementation
    }
}

// ❌ Bad
public class notesservice
{
    private string dataPath;
    
    public Note getnote(Guid id)
    {
        // Implementation
    }
}
```

### XAML Style

```xaml
<!-- ✅ Good: Clear hierarchy and formatting -->
<Button Content="Save Note"
        Command="{Binding SaveCommand}"
        Background="#0078D4"
        Padding="12,8"
        CornerRadius="4"/>

<!-- ❌ Bad: All on one line -->
<Button Content="Save Note" Command="{Binding SaveCommand}" Background="#0078D4" Padding="12,8" CornerRadius="4"/>
```

### MVVM Pattern

Always follow MVVM principles:

```csharp
// ✅ Good: ViewModel with proper commands
public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _noteTitle;
    
    [RelayCommand]
    private async Task SaveNoteAsync()
    {
        // Implementation
    }
}

// ❌ Bad: Code in View code-behind
public partial class MainWindow : Window
{
    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        // Don't put business logic here!
    }
}
```

### Async/Await

Use async/await for I/O operations:

```csharp
// ✅ Good
public async Task<List<Note>> GetNotesAsync()
{
    var json = await File.ReadAllTextAsync(_notesFile);
    return JsonSerializer.Deserialize<List<Note>>(json);
}

// ❌ Bad
public List<Note> GetNotes()
{
    var json = File.ReadAllText(_notesFile); // Blocking!
    return JsonSerializer.Deserialize<List<Note>>(json);
}
```

## 🐛 Reporting Bugs

### Before Reporting

1. **Search existing issues** to avoid duplicates
2. **Update to latest version** to see if bug is fixed
3. **Reproduce the bug** consistently

### Bug Report Template

```markdown
**Describe the bug**
A clear and concise description of what the bug is.

**To Reproduce**
Steps to reproduce the behavior:
1. Go to '...'
2. Click on '....'
3. Scroll down to '....'
4. See error

**Expected behavior**
What you expected to happen.

**Screenshots**
If applicable, add screenshots.

**Environment:**
 - OS: [e.g. Windows 11, Ubuntu 22.04]
 - Version: [e.g. 1.0.0]
 - .NET Version: [e.g. 9.0]

**Additional context**
Any other context about the problem.
```

## 💡 Suggesting Features

### Feature Request Template

```markdown
**Is your feature request related to a problem?**
A clear description of the problem.

**Describe the solution you'd like**
What you want to happen.

**Describe alternatives you've considered**
Other solutions you've thought about.

**Additional context**
Mockups, examples, or other context.

**Priority**
How important is this feature to you?
- [ ] Critical - Can't use app without it
- [ ] High - Would significantly improve experience
- [ ] Medium - Nice to have
- [ ] Low - Minor improvement
```

## 🧪 Testing Guidelines

### Manual Testing

For now, we focus on manual testing. When testing your changes:

1. **Create test notes** with various content
2. **Test edge cases** (empty notes, very long notes, special characters)
3. **Test error scenarios** (invalid data, corrupted files)
4. **Test on different platforms** if possible

### Future: Automated Tests

We plan to add automated tests. Here's what they might look like:

```csharp
[Fact]
public async Task CreateNote_ShouldAddToCollection()
{
    // Arrange
    var service = new NotesService();
    var note = new Note { Title = "Test Note" };
    
    // Act
    await service.SaveNoteAsync(note);
    var notes = await service.GetAllNotesAsync();
    
    // Assert
    Assert.Contains(note, notes);
}
```

## 📚 Documentation Guidelines

### Code Comments

```csharp
/// <summary>
/// Saves a note to local storage
/// </summary>
/// <param name="note">The note to save</param>
/// <returns>The saved note with updated metadata</returns>
public async Task<Note> SaveNoteAsync(Note note)
{
    // Update modification timestamp
    note.ModifiedAt = DateTime.Now;
    
    // Calculate word count
    note.WordCount = CountWords(note.Content);
    
    // Save to storage
    await SaveDataAsync();
    
    return note;
}
```

### Documentation Files

When updating documentation:
- Use **clear, simple language**
- Include **examples** where possible
- Add **screenshots** for UI features
- Keep **consistent formatting**
- Translate to **French and English**

## 🎨 Design Guidelines

### UI/UX Principles

1. **Simplicity**: Keep interface clean and uncluttered
2. **Consistency**: Use same patterns throughout app
3. **Feedback**: Provide visual feedback for user actions
4. **Accessibility**: Consider all users, including those with disabilities

### Color Palette

```
Primary: #0078D4 (Blue)
Success: #107C10 (Green)
Warning: #FFB900 (Yellow)
Error: #D13438 (Red)
Info: #8764B8 (Purple)
```

### Spacing

Use multiples of 4:
- Small: 4px, 8px
- Medium: 12px, 16px
- Large: 20px, 24px
- Extra Large: 32px, 40px

## 🌍 Translation Guidelines

We welcome translations! To add a new language:

1. Create translation file (future):
   ```
   Resources/
   ├── Strings.en.json
   ├── Strings.fr.json
   └── Strings.es.json  # New language
   ```

2. Translate all strings
3. Test in application
4. Submit PR

## 🏆 Recognition

Contributors will be:
- Listed in **CONTRIBUTORS.md**
- Mentioned in **release notes**
- Credited in **About dialog** (future)

## ❓ Questions?

- **GitHub Discussions**: Ask questions
- **GitHub Issues**: For specific problems
- **Email**: support@smartnotes.app (future)

## 📜 Code of Conduct

### Our Pledge

We pledge to make participation in our project a harassment-free experience for everyone, regardless of age, body size, disability, ethnicity, gender identity, level of experience, nationality, personal appearance, race, religion, or sexual identity and orientation.

### Our Standards

**Positive behavior**:
- Using welcoming and inclusive language
- Being respectful of differing viewpoints
- Gracefully accepting constructive criticism
- Focusing on what is best for the community
- Showing empathy towards other community members

**Unacceptable behavior**:
- Trolling, insulting/derogatory comments, personal or political attacks
- Public or private harassment
- Publishing others' private information
- Other conduct which could reasonably be considered inappropriate

## 📄 License

By contributing, you agree that your contributions will be licensed under the MIT License.

---

Thank you for contributing to SmartNotes! Together, we're building something amazing. 🚀✨
