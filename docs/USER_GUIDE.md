# SmartNotes - Guide de l'Utilisateur

## 🎯 Présentation

SmartNotes est une application de prise de notes intelligente avec intelligence artificielle intégrée. Elle combine un éditeur de texte moderne avec des fonctionnalités d'IA pour vous aider à écrire plus vite et mieux organiser vos pensées.

## 🚀 Démarrage Rapide

### Première utilisation

1. **Lancez l'application** : Double-cliquez sur SmartNotes.exe (Windows) ou exécutez `dotnet run` depuis le dossier du projet
2. **Note de bienvenue** : Une note d'accueil est créée automatiquement pour vous guider
3. **Créez votre première note** : Cliquez sur le bouton "+ Nouvelle Note"

### Interface

L'interface est divisée en trois zones principales :

1. **Barre supérieure** : Création de notes et recherche
2. **Panneau latéral gauche** : Liste des catégories et notes
3. **Zone d'édition** : Éditeur de texte avec barre d'outils IA

## 📝 Gestion des Notes

### Créer une note

- Cliquez sur **"+ Nouvelle Note"** dans la barre supérieure
- Ou utilisez le raccourci **Ctrl+N**
- Une nouvelle note vierge s'ouvre immédiatement

### Éditer une note

1. Cliquez sur une note dans la liste de gauche
2. Modifiez le titre en haut de l'éditeur
3. Écrivez votre contenu dans la zone de texte
4. La note est **sauvegardée automatiquement** après chaque modification

### Supprimer une note

- Cliquez sur le bouton **"🗑️ Supprimer"** dans la barre d'outils
- La note est supprimée définitivement (pas de corbeille pour le moment)

### Marquer en favori

- Cliquez sur le bouton **"⭐ Favori"**
- Les notes favorites sont marquées d'une étoile dans la liste

## 🔍 Recherche

### Recherche instantanée

1. Cliquez dans la barre de recherche en haut
2. Tapez votre recherche
3. Appuyez sur **Entrée**
4. Les résultats apparaissent dans la liste de gauche

La recherche s'effectue dans :
- Les titres de notes
- Le contenu complet
- Les tags associés

Pour afficher toutes les notes, effacez la recherche et appuyez sur Entrée.

## 🤖 Fonctionnalités IA

### 1. Générer du Contenu

**Comment l'utiliser** :
- Cliquez sur **"🤖 Générer Contenu"**
- L'IA génère automatiquement du texte sur un sujet prédéfini
- Le contenu est ajouté à votre note

**Exemples de sujets reconnus** :
- "photosynthèse" → Explication détaillée de la photosynthèse
- "révolution française" → Texte historique sur la Révolution française
- "intelligence artificielle" → Description de l'IA

### 2. Résumer une Note

**Comment l'utiliser** :
1. Écrivez ou collez du texte dans votre note
2. Cliquez sur **"📝 Résumé"**
3. Un résumé est ajouté au début de votre note

**Idéal pour** :
- Condenser des notes de cours longues
- Créer des synthèses de réunions
- Extraire l'essentiel d'un document

### 3. Générer des Fiches de Révision

**Comment l'utiliser** :
1. Écrivez vos notes de cours ou de lecture
2. Cliquez sur **"🎓 Fiches"**
3. Une nouvelle note est créée avec des questions/réponses

**Résultat** :
- Questions de révision automatiques
- Réponses basées sur votre contenu
- Format idéal pour la mémorisation

## 📂 Organisation

### Catégories

Les catégories vous permettent de classer vos notes par thème :

**Catégories par défaut** :
- 📝 Général
- 💼 Travail
- 🏠 Personnel
- 📚 Études

### Tags

*(Fonctionnalité à venir)*

Les tags permettront d'ajouter des mots-clés à vos notes pour une organisation encore plus fine.

## ⚙️ Paramètres

### Stockage des données

Vos notes sont stockées localement dans :
- **Windows** : `C:\Users\[VotreNom]\AppData\Local\SmartNotes`
- **Linux** : `~/.local/share/SmartNotes`
- **macOS** : `~/Library/Application Support/SmartNotes`

Deux fichiers sont créés :
- `notes.json` : Toutes vos notes
- `categories.json` : Vos catégories personnalisées

### Sauvegarde automatique

Les notes sont sauvegardées automatiquement :
- **2 secondes** après chaque modification
- Lors de la fermeture de l'application
- Lors du changement de note

### Confidentialité

✅ **100% local** : Aucune donnée n'est envoyée sur Internet  
✅ **Pas de compte** : Pas besoin de créer un compte  
✅ **Pas de tracking** : Aucune analytique ou suivi  
✅ **Open Source** : Le code est ouvert et vérifiable  

## 💡 Astuces et Conseils

### Pour les étudiants

1. **Prenez des notes pendant le cours** de manière brève
2. **Utilisez "Générer Contenu"** pour développer les points importants
3. **Créez des fiches** avant chaque examen
4. **Organisez par matière** avec les catégories

### Pour les professionnels

1. **Notes de réunion** : Écrivez les points clés, laissez l'IA structurer
2. **Brainstorming** : Notez vos idées, utilisez "Générer" pour les développer
3. **Comptes-rendus** : Résumez vos notes longues en quelques lignes
4. **Documentation** : Organisez par projet avec les catégories

### Pour la créativité

1. **Journal d'idées** : Notez toutes vos inspirations
2. **Développement** : Laissez l'IA vous aider à développer vos concepts
3. **Organisation** : Catégorisez par projet ou thème
4. **Révision** : Relisez vos anciennes notes pour trouver l'inspiration

## ❓ FAQ

### L'IA a besoin d'Internet ?

Non ! L'IA fonctionne **100% en local** sur votre ordinateur. Aucune connexion Internet n'est requise.

### Mes notes sont-elles privées ?

Oui, absolument. Toutes vos notes restent sur votre ordinateur et ne sont jamais envoyées ailleurs.

### Comment sauvegarder mes notes ?

Vos notes sont automatiquement sauvegardées. Pour une sauvegarde manuelle, copiez simplement le dossier `SmartNotes` de votre dossier de données.

### Puis-je exporter mes notes ?

*(Fonctionnalité à venir)* L'export en PDF, Markdown et Word sera disponible dans une prochaine version.

### L'application fonctionne sur Mac/Linux ?

Oui ! SmartNotes utilise Avalonia et fonctionne sur Windows, macOS et Linux.

## 🐛 Problèmes Connus

- L'export de notes n'est pas encore implémenté
- Les thèmes personnalisés ne sont pas encore disponibles
- La synchronisation cloud n'est pas encore supportée

## 📧 Support

Pour toute question ou problème :
1. Consultez ce guide
2. Ouvrez une issue sur GitHub
3. Consultez le README du projet

---

**Bon travail avec SmartNotes ! ✨**
