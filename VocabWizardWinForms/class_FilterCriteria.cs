namespace VocabWizardWinForms
{
    // Klass som representerar filterkriterier för att filtrera övningar.
    public class FilterCriteria
    {
        // Listor som lagrar valda alternativ för olika filtertyper.
        public List<string> Languages { get; set; } = []; // Valda språk
        public List<string> WordClasses { get; set; } = []; // Valda ordklasser
        public List<string> Chapters { get; set; } = []; // Valda kapitel
        public List<string> FileNames { get; set; } = []; // Valda filnamn
        public List<string> AveragePoints { get; set; } = []; // Valda genomsnittspoäng
        public List<string> DirtyStatuses { get; set; } = []; // Valda "dirty"-statusar

        // Metod för att hämta valda alternativ baserat på den angivna filtertypen.
        public List<string> GetSelectedOptions(string optionType)
        {
            switch (optionType)
            {
                case "Language":
                    return Languages;
                case "WordClass":
                    return WordClasses;
                case "Chapter":
                    return Chapters;
                case "FileName":
                    return FileNames;
                case "AveragePoint":
                    return AveragePoints;
                case "DirtyStatus":
                    return DirtyStatuses;
                default:
                    // Visar ett popup-meddelande om filtertypen är ogiltig.
                    MessageBox.Show("Ogiltig filtertyp angiven.");
                    return new List<string>(); // Returnera en tom lista som fallback.
            }
        }
    }
}