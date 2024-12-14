using System;
using System.Collections.Generic
using System.IO
using System.Linq

 namespace ScriptureMemorization
 {
    public class Verse
    {
        public string Text { get; private set; }
        public bool IsHidden { get; private set; }

        public verse(string Text)
        {
            this.Text = Text;
            this.IsHidden = false;
        }
        public void Hide() => IsHidden = true;
        public void Reveal() => IsHidden = false;
        public override string ToString() => IsHidden ? new string('_', Text.Length) : Text;
         
    }
// Represents the reference (e.g., "John 3:16")
    public class ScriptureReference
    {
        public string Book { get; private set; }
        public string Chapter { get; private set; }
        public string VerseRange { get; private set; }

        public ScriptureReference(string book, string chapter, string verseRange)
        {
            Book = book;
            Chapter = chapter;
            VerseRange = verseRange;
        }

        public override string ToString() => $"{Book} {Chapter}:{VerseRange}";
    }

    // Represents the scripture text and handles operations
    public class Scripture
    {
        public ScriptureReference Reference { get; private set; }
        public List<Word> Words { get; private set; }

        public Scripture(ScriptureReference reference, string text)
        {
            Reference = reference;
            Words = text.Split(' ').Select(w => new Word(w)).ToList();
        }

        // Displays the scripture with hidden/visible words
        public string GetDisplayText()
        {
            return $"{Reference}\n{string.Join(" ", Words)}";
        }

        // Hides a specified number of random words
        public void HideRandomWords(int count)
        {
            var visibleWords = Words.Where(w => !w.IsHidden).ToList();
            var random = new Random();

            for (int i = 0; i < count && visibleWords.Count > 0; i++)
            {
                var wordToHide = visibleWords[random.Next(visibleWords.Count)];
                wordToHide.Hide();
                visibleWords.Remove(wordToHide);
            }
        }

        // Checks if all words are hidden
        public bool AreAllWordsHidden()
        {
            return Words.All(w => w.IsHidden);
        }
    }

    // Main program
    public class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("Welcome to Scripture Memorization!");
            Console.WriteLine("Loading scriptures...");
            
            // Load scriptures from a file
            var scriptures = LoadScripturesFromFile("scriptures.txt");
            if (scriptures.Count == 0)
            {
                Console.WriteLine("No scriptures found in the file. Exiting.");
                return;
            }

            // Allow user to select or randomize scripture
            Console.WriteLine("Choose a scripture to memorize:");
            for (int i = 0; i < scriptures.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {scriptures[i].Reference}");
            }
            Console.WriteLine("Enter the number of your choice or type 'random':");
            var choice = Console.ReadLine();

            Scripture selectedScripture;
            if (choice.ToLower() == "random")
            {
                var random = new Random();
                selectedScripture = scriptures[random.Next(scriptures.Count)];
            }
            else if (int.TryParse(choice, out int index) && index >= 1 && index <= scriptures.Count)
            {
                selectedScripture = scriptures[index - 1];
            }
            else
            {
                Console.WriteLine("Invalid choice. Exiting.");
                return;
            }

            // Memorization loop
            while (true)
            {
                Console.Clear();
                Console.WriteLine(selectedScripture.GetDisplayText());
                if (selectedScripture.AreAllWordsHidden())
                {
                    Console.WriteLine("\nCongratulations! You have successfully memorized the scripture.");
                    break;
                }

                Console.WriteLine("\nPress ENTER to hide more words or type 'quit' to exit.");
                var input = Console.ReadLine();
                if (input.ToLower() == "quit")
                    break;

                selectedScripture.HideRandomWords(3);
            }
        }

        // Loads scriptures from a file
        static List<Scripture> LoadScripturesFromFile(string filePath)
        {
            var scriptures = new List<Scripture>();

            if (!File.Exists(filePath))
                return scriptures;

            foreach (var line in File.ReadAllLines(filePath))
            {
                var parts = line.Split('|');
                if (parts.Length == 3)
                {
                    var reference = new ScriptureReference(parts[0], parts[1], parts[2]);
                    var text = string.Join("|", parts.Skip(3)); // Remaining part as scripture text
                    scriptures.Add(new Scripture(reference, text));
                }
            }

            return scriptures;
        }
    }
}