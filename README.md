# 📱 SmartNotes - Application de Prise de Notes Intelligente

![.NET](https://img.shields.io/badge/.NET-9.0-512BD4)
![Avalonia](https://img.shields.io/badge/Avalonia-11.3-8B44AC)
![License](https://img.shields.io/badge/license-MIT-green)

**SmartNotes** est votre compagnon d'écriture intelligent qui transforme la prise de notes en une expérience fluide et productive. Grâce à l'intelligence artificielle locale qui s'exécute directement sur votre ordinateur, SmartNotes vous aide à écrire plus vite, mieux structurer vos idées et réviser efficacement.

![SmartNotes Screenshot](docs/screenshot.png)

---

## ✨ Fonctionnalités Principales

### 📝 **Éditeur Intelligent**

- **Interface moderne et épurée** : Design Fluent qui s'adapte au thème clair ou sombre
- **Éditeur riche** : Écriture de texte avec support pour la mise en forme
- **Organisation intuitive** : Catégories, tags, couleurs personnalisables
- **Recherche instantanée** : Trouvez n'importe quelle note en quelques touches
- **Favoris** : Épinglez vos notes importantes pour un accès rapide

### 🤖 **Assistant IA en Temps Réel**

#### **Suggestions Automatiques**
- **Complétion intelligente** : L'IA suggère la suite de vos phrases pendant que vous écrivez
- **Correction d'abréviations** : Tapez "bcp" → l'IA propose "beaucoup"
- **Suggestions contextuelles** : L'IA comprend le sujet et propose des continuations pertinentes

#### **Génération de Contenu**
- **Développement d'idées** : Donnez un mot-clé, l'IA développe un paragraphe complet
- **Explications automatiques** : Demandez sur un sujet → texte détaillé généré
- **Reformulation** : Réécrivez un passage dans un style différent

#### **Aide à la Révision**
- **Résumés automatiques** : Créez un résumé de vos notes en un clic
- **Fiches d'étude** : L'IA extrait les points clés et génère des questions
- **Organisation intelligente** : Classement automatique par catégories

---

## 🚀 Installation

### Prérequis
- **.NET 9.0 SDK** ou supérieur
- **Windows 10/11**, **Linux** ou **macOS**

### Compilation depuis les sources

```bash
# Cloner le repository
git clone https://github.com/LeRenardBlanc/AINotes.git
cd AINotes/SmartNotes

# Restaurer les dépendances
dotnet restore

# Compiler l'application
dotnet build

# Lancer l'application
dotnet run
```

### Installation des dépendances

Le projet utilise les packages NuGet suivants :
- **Avalonia** (11.3.6) - Framework UI multiplateforme
- **Avalonia.AvaloniaEdit** (11.3.0) - Éditeur de texte avancé
- **CommunityToolkit.Mvvm** (8.2.1) - Architecture MVVM
- **Microsoft.ML.OnnxRuntime** (1.23.1) - Runtime IA local
- **Newtonsoft.Json** (13.0.4) - Sérialisation JSON

---

## 📖 Utilisation

### Créer une nouvelle note
1. Cliquez sur **"+ Nouvelle Note"** dans la barre supérieure
2. Donnez un titre à votre note
3. Commencez à écrire dans l'éditeur

### Utiliser l'Assistant IA
- **🤖 Générer Contenu** : Génère automatiquement du contenu basé sur un sujet
- **📝 Résumé** : Crée un résumé de votre note actuelle
- **🎓 Fiches** : Génère des fiches de révision avec questions/réponses

### Organiser vos notes
- Utilisez les **catégories** pour classer vos notes
- Ajoutez des **tags** pour faciliter la recherche
- Marquez vos notes en **favoris** (⭐) pour un accès rapide

### Rechercher
- Utilisez la barre de recherche en haut pour trouver vos notes
- La recherche s'effectue dans le titre, le contenu et les tags

---

## 🎯 Cas d'Usage

### **Pour les Lycéens / Étudiants**
✅ Prendre des notes de cours rapidement  
✅ Créer des fiches de révision automatiquement  
✅ Rédiger dissertations et exposés  
✅ Résumer des chapitres de manuels  
✅ Préparer des examens efficacement

### **Pour les Professionnels**
✅ Notes de réunions structurées  
✅ Documentation de projets  
✅ Rédaction de rapports et comptes-rendus  
✅ Brainstorming et mind mapping  
✅ Gestion de connaissances

### **Pour les Créatifs**
✅ Journal d'idées et inspiration  
✅ Développement de concepts  
✅ Scénarios et storylines  
✅ Planification de contenus

---

## 🏗️ Architecture

Le projet est structuré selon le pattern MVVM (Model-View-ViewModel) :

```
SmartNotes/
├── Models/              # Modèles de données (Note, Category, AppSettings)
├── ViewModels/          # Logique métier et état de l'application
├── Views/               # Interface utilisateur (XAML)
├── Services/            # Services (NotesService, AIService)
├── Assets/              # Ressources (icônes, images)
└── Program.cs           # Point d'entrée de l'application
```

### Composants Principaux

- **NotesService** : Gestion du stockage et de la récupération des notes (JSON local)
- **AIService** : Fonctionnalités d'intelligence artificielle (complétion, génération, résumé)
- **MainWindowViewModel** : État et logique de la fenêtre principale
- **MainWindow** : Interface utilisateur principale

---

## 🔒 Confidentialité

- **IA 100% locale** : Aucune donnée envoyée sur Internet
- **Pas de connexion requise** : Fonctionne entièrement hors ligne
- **Vos notes restent privées** : Stockées uniquement sur votre ordinateur dans :
  - Windows : `%LocalAppData%\SmartNotes`
  - Linux : `~/.local/share/SmartNotes`
  - macOS : `~/Library/Application Support/SmartNotes`

---

## 🛠️ Développement

### Structure des données

Les notes sont stockées au format JSON dans le dossier de données local :
- `notes.json` : Liste de toutes les notes
- `categories.json` : Catégories personnalisées

### Ajouter de nouvelles fonctionnalités

1. Créer un nouveau service dans `Services/`
2. Ajouter les commandes dans le ViewModel approprié
3. Créer l'interface utilisateur dans `Views/`
4. Lier les commandes avec les contrôles UI

---

## 🚧 Fonctionnalités à venir

- [ ] Export en PDF, Markdown et Word
- [ ] Support des images et tableaux dans l'éditeur
- [ ] Mode "tableau blanc" avec drag & drop
- [ ] Synchronisation cloud optionnelle
- [ ] Thèmes de couleurs personnalisés
- [ ] Mode sombre/clair
- [ ] Raccourcis clavier personnalisables
- [ ] Import de documents PDF/Word
- [ ] Notes vocales avec transcription
- [ ] Graphe de connaissances
- [ ] Amélioration de l'IA avec modèles locaux plus avancés

---

## 📝 Licence

Ce projet est sous licence MIT. Voir le fichier [LICENSE](LICENSE) pour plus de détails.

---

## 🤝 Contribution

Les contributions sont les bienvenues ! N'hésitez pas à :
1. Fork le projet
2. Créer une branche pour votre fonctionnalité (`git checkout -b feature/AmazingFeature`)
3. Commit vos changements (`git commit -m 'Add some AmazingFeature'`)
4. Push vers la branche (`git push origin feature/AmazingFeature`)
5. Ouvrir une Pull Request

---

## 📧 Contact

Pour toute question ou suggestion, n'hésitez pas à ouvrir une issue sur GitHub.

---

**SmartNotes : Votre cerveau augmenté pour écrire, apprendre et créer. 🧠✨**

