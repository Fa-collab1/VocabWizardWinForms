using OfficeOpenXml;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace VocabWizardWinForms
{
    public static class ExerciseLoader
    {
        // Laddar alla övningar från Excel-filer i en angiven katalog
        public static List<ExerciseData> LoadAllExercises(string directoryPath)
        {
            var exercises = new List<ExerciseData>();
            // Hämta alla Excel-filer i katalogen som inte är temporära och inte dolda (när man redigerar i Excel så kan det skapas en del underliga filer)
            string[] excelFiles = Directory.GetFiles(directoryPath, "*.xlsx")
                .Where(file => !Path.GetFileName(file).StartsWith('~') &&
                               (File.GetAttributes(file) & FileAttributes.Hidden) == 0)
                .ToArray();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Licensinställning för att använda EPPlus

            foreach (string file in excelFiles)
            {
                try
                {
                    using var package = new ExcelPackage(new FileInfo(file)); // Öppna Excel-filen
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Hämta det första bladet i filen

                    // Hoppa över filen om första bladet är tomt
                    if (worksheet.Dimension == null)
                    {
                        continue;
                    }

                    // Gå igenom raderna, börja på rad 2 för att hoppa över rubrikraden
                    for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                    {
                        var dirtyCellValue = worksheet.Cells[row, 12].Text.Trim();

                        // Skapa ett ExerciseData-objekt med värden från Excel-raden
                        var exercise = new ExerciseData
                        {
                            RowNumber = row,
                            Language = worksheet.Cells[row, 1].Text,
                            WordClass = worksheet.Cells[row, 2].Text,
                            Chapter = worksheet.Cells[row, 3].Text,
                            Translation = worksheet.Cells[row, 4].Text,
                            Original = worksheet.Cells[row, 5].Text,
                            FileName = Path.GetFileName(file),
                            UpdateDate = DateTime.TryParse(worksheet.Cells[row, 6].Text, out var updateDate) ? updateDate : DateTime.MinValue, // Om datumet inte kan läsas in så sätts det till DateTime.MinValue
                            LatestPoint = double.TryParse(worksheet.Cells[row, 7].Text, out var latestPoint) ? latestPoint : 0, // Om poängen inte kan läsas in så sätts den till 0
                            SecondLatestPoint = double.TryParse(worksheet.Cells[row, 8].Text, out var secondPoint) ? secondPoint : 0,
                            ThirdLatestPoint = double.TryParse(worksheet.Cells[row, 9].Text, out var thirdPoint) ? thirdPoint : 0,
                            FourthLatestPoint = double.TryParse(worksheet.Cells[row, 10].Text, out var fourthPoint) ? fourthPoint : 0,
                            FifthLatestPoint = double.TryParse(worksheet.Cells[row, 11].Text, out var fifthPoint) ? fifthPoint : 0,
                            Dirty = !string.IsNullOrEmpty(dirtyCellValue) && (dirtyCellValue.Trim() == "1" || dirtyCellValue.Trim().ToLower() == "true"), // Om cellen är tom eller innehåller någoting annat än "1" eller "true" så sätts Dirty till false
                        };

                        exercises.Add(exercise); // Lägger till övningen i listan
                    }
                }
                catch (System.IO.InvalidDataException ex) // Fångar upp fel som kan uppstå vid inläsning av filer
                {
                    // Visar ett felmeddelande med filens namn om något går fel med en fil
                    MessageBox.Show($"Error processing file {file}: {ex.Message}");
                }
            }
            return exercises; // Returnera listan med alla inlästa övningar
        }

        // Sorterar en lista med siffror och strängar separat
        public static List<string> SortNumbersAndStrings(IEnumerable<string> items) // IEnumerable är en generisk typ som kan användas för att iterera över en samling objekt
        {
            var numbers = items.Where(item => int.TryParse(item, out _)).Select(item => int.Parse(item)).OrderBy(n => n).Select(n => n.ToString());
            var strings = items.Where(item => !int.TryParse(item, out _)).OrderBy(s => s);
            return numbers.Concat(strings).ToList(); // Kombinera och returnera de sorterade listorna
        }

        // Filtrerar övningar baserat på valda filterkriterier
        public static List<ExerciseData> GetFilteredExercises( // Returnerar en lista med övningar som matchar de valda filterkriterierna
            List<ExerciseData> exercises, FilterCriteria selectedFilters)
        {
            return exercises.Where(exercise =>
                (selectedFilters.Languages.Count == 0 || selectedFilters.Languages.Contains(exercise.Language)) && // Om inga språk är valda eller om övningens språk finns i listan med valda språk om 0 språk är valda så filtreras ingenting bort baserat på språk
                (selectedFilters.WordClasses.Count == 0 || selectedFilters.WordClasses.Contains(exercise.WordClass)) && // Samma som ovan fast för ordklasser
                (selectedFilters.Chapters.Count == 0 || selectedFilters.Chapters.Contains(exercise.Chapter)) && // Samma som ovan fast för kapitel
                (selectedFilters.FileNames.Count == 0 || selectedFilters.FileNames.Contains(exercise.FileName)) && // Samma som ovan fast för filnamn
                (selectedFilters.AveragePoints.Count == 0 || selectedFilters.AveragePoints.Contains(exercise.AveragePoint)) && // Samma som ovan fast för genomsnittspoäng
                (selectedFilters.DirtyStatuses.Count == 0 || selectedFilters.DirtyStatuses.Contains(exercise.DirtyStatus)) // Samma som ovan fast för dirty-info
            ).ToList(); // Returnera filtrerade övningar
        }
    }
}
