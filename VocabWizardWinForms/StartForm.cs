using OfficeOpenXml;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace VocabWizardWinForms
{
    public partial class StartForm : Form
    {
        private List<ExerciseData> _allExercises = []; // Lista som lagrar alla övningar
        private FilterCriteria _selectedFilters = new(); // Filterkriterier för att filtrera övningar
        private FileSystemWatcher? _fileWatcher; // Filövervakare för att upptäcka ändringar i Excel-filerna som innehåller övningar
        private bool filesInUse = false; // Flagga för att förhindra att filer som håller på att uppdateras inte läses in innan användaren är klar


        public StartForm()
        {
            InitializeComponent(); // Initialisera komponenter
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Sätt licenskontext för att undvika licensvarningar
            InitializeFileWatcher(); // Initialisera filövervakaren
            LoadExercisesFromFolder(); // Ladda in övningar från mappen
            PopulateFilterOptions(); // Fyll filteralternativ
            oneCardRadionButton.Checked = false; // Avmarkera radioknappen för en övning
            fiveCardRadionButton.Checked = true; // Markera radioknappen för fem övningar
        }


        private void toggle_RadioButton_Click(object sender, EventArgs e) // Metod för att byta mellan att öva på en övning eller fem övningar har satt denna som acceptknapp för att kunna använda enter för att byta mellan att öva på en eller fem övningar (använder en knapp som knappt syns för detta)    
        {
            if (oneCardRadionButton.Checked == true)
            {
                oneCardRadionButton.Checked = false;
                fiveCardRadionButton.Checked = true;
            }
            else
            {
                oneCardRadionButton.Checked = true;
                fiveCardRadionButton.Checked = false;
            }

        }


        private void QuitButton_Click(object sender, EventArgs e) // Metod för att avsluta programmet när quit-knappen klickas
        {
            Application.Exit(); // Avsluta programmet
        }


        private void InitializeFileWatcher() // Metod för att initialisera filövervakaren
        {
            string directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "excel"); // Skapa sökväg till mappen med Excel-filer

            if (!Directory.Exists(directoryPath)) // Om mappen inte finns
            {
                Directory.CreateDirectory(directoryPath); // Skapa mappen
            }

            _fileWatcher = new FileSystemWatcher(directoryPath, "*.xlsx") // Skapa en ny filövervakare för mappen med Excel-filer
            {
                EnableRaisingEvents = true, // Aktivera händelserbevakare
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite // Aktivera händelsebevakare för filnamn och senaste skrivning
            };

            _fileWatcher.Created += OnFileChanged; // Lägg till ett event för när en fil skapas
            _fileWatcher.Changed += OnFileChanged; // Lägg till ett event för när en fil ändras
            _fileWatcher.Deleted += OnFileChanged; // Lägg till ett event för när en fil tas bort
            _fileWatcher.Renamed += OnFileRenamed; // Lägg till ett event för när en fil byter namn


#if DEBUG // Om programmet körs i debug-läge
            string devFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Excel"); // Skapa sökväg till utvecklingsmappen med Excel-filer

            if (Directory.Exists(devFolderPath)) // Om utvecklingsmappen finns
            {
                foreach (var sourceFilePath in Directory.GetFiles(devFolderPath, "*.xlsx")) // Loopa igenom alla Excel-filer i utvecklingsmappen
                {
                    var destFilePath = Path.Combine(directoryPath, Path.GetFileName(sourceFilePath)); // Skapa sökväg till destinationsmappen

                    if (File.Exists(destFilePath)) // Om filen redan finns i destinationsmappen
                    {
                        DateTime sourceLastWriteTime = File.GetLastWriteTime(sourceFilePath); // Hämta senaste skrivningstid för källfilen
                        DateTime destLastWriteTime = File.GetLastWriteTime(destFilePath); // Hämta senaste skrivningstid för destinationsfilen

                        if (sourceLastWriteTime > destLastWriteTime) // Om källfilen är nyare än destinationsfilen
                        {
                            File.Copy(sourceFilePath, destFilePath, true); // Kopiera filen
                        }
                    }
                    else // Om filen inte finns i destinationsmappen
                    {
                        File.Copy(sourceFilePath, destFilePath, true); // Kopiera filen
                    }
                }
            }
#endif
        }

        private void OnFileChanged(object sender, FileSystemEventArgs e) // Metod för att hantera filändringar
        {
            if (filesInUse == false) // om fil används, gör inget
            {

                if (InvokeRequired) // Om anropet inte sker på rätt tråd
                {
                    Invoke(new Action(() => OnFileChanged(sender, e))); // Anropa metoden på rätt tråd
                    return; // Returnera
                }

                ReLoad(e); // Ladda om filen

            }
        }

        private void ReLoad(FileSystemEventArgs e)
        {
            try
            {
                // Kontroll om filen är tillgänglig
                using (var stream = new FileStream(e.FullPath, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    // Kontroll om filen är tillgänglig
                    stream.Close(); // Stäng filen så att den kan användas av följande processer
                }

                LoadExercisesFromFolder(); // Ladda in övningar från mappen
                PopulateFilterOptions(); // Fyll filteralternativ
                UpdateFilteredCount(); // Uppdatera räkneverket

            }
            catch (IOException) // Om det uppstår ett IO-fel
            {
                MessageBox.Show($"Unable to access the file '{e.Name}'. It might be open in another application.", "File Access Error"); // Visa ett felmeddelande
            }
        }

        private void OnFileRenamed(object sender, RenamedEventArgs e)
        {
            if (filesInUse == false) // om fil används, gör inget
            {
                if (InvokeRequired) // Kontrollera om metoden anropas från en annan tråd än huvudtråden
                {
                    // Om anropet sker från en annan tråd än huvudtråden, kör metoden igen på huvudtråden
                    Invoke(new Action(() => OnFileRenamed(sender, e))); // Flytta anropet till huvudtråden för att säkert uppdatera UI
                    return; // Avbryt metoden så att den inte fortsätter att köras på fel tråd
                }

                ReLoad(e); // Ladda om filen
            }
        }


        private void Practice(bool intoLanguage) // Metod för att öva på övningar
        {
            filesInUse = true; // Sätt filesInUse till true för att förhindra att filtret uppdateras medan vi övar så det inte laddar om datat varje gång vi sparar poäng efter varje ord/mening vi övar på, det segar ner programmet otroligt mycket då


            var storedFilters = GetSelectedFilters(); // Spara nuvarande filterstatus och markeringar för att kunna återställa dem senare
            var selectedCheckboxes = GetSelectedCheckboxes(); // Spara markerade checkboxar för varje grupp för att kunna återställa dem senare

            var filteredExercises = ExerciseLoader.GetFilteredExercises( // Använd ExerciseLoader för att hämta de filtrerade övningarna
                _allExercises, storedFilters); // Använd alla övningar och de valda filtren

            if (filteredExercises.Count == 0) // Om inga övningar matchade filtret
            {
                MessageBox.Show("No exercises matched your filter criteria. Please adjust your selections.", "No Results"); // Visa ett meddelande om att inga övningar matchade filtret, ingen idé att öva om det inte finns några övningar att öva på... så vi avbryter här
                filesInUse = false; // Återställ filesInUse-flaggan
                return; // Avbryt metoden så att inget mer körs
            }

            var practiceSet = filteredExercises.Select(e => new ExerciseData // Skapa en ny lista med övningar att öva på
            {
                RowNumber = e.RowNumber,
                UpdateDate = e.UpdateDate,
                LatestPoint = e.LatestPoint,
                SecondLatestPoint = e.SecondLatestPoint,
                ThirdLatestPoint = e.ThirdLatestPoint,
                FourthLatestPoint = e.FourthLatestPoint,
                FifthLatestPoint = e.FifthLatestPoint,
                Language = e.Language,
                WordClass = e.WordClass,
                Chapter = e.Chapter,
                FileName = e.FileName,
                Original = intoLanguage ? e.Translation : e.Original, // baserat på flaggan, byt plats (eller inte) på översättning och original
                Translation = intoLanguage ? e.Original : e.Translation, // baserat på flaggan, byt plats (eller inte) på översättning och original 
                Dirty = e.Dirty
            }).OrderBy(x => new Random().Next()).ToList();

            var flashcardForm = new FlashCardForm(practiceSet, oneCardRadionButton.Checked); // Skapa en ny instans av FlashCardForm med övningarna ovan att öva på och bool-värdet på radioknappen för en övning eller fem övningar skickas med
            flashcardForm.ShowDialog(); // Visa FlashCardForm som en dialogruta
            filesInUse = false; // Återställ filesInUse-flaggan efter att övningen är klar

            // Ladda om uppdaterad data från mappen och återställ filter och markeringar
            LoadExercisesFromFolder(); // Ladda in övningar från mappen
            PopulateFilterOptions(); // Fyll filteralternativ
            UpdateFilteredCount(); // Uppdatera räkneverket

            // Återställ filter och markeringar
            _selectedFilters = storedFilters; // Återställ filterkriterierna
            RestoreCheckboxState(selectedCheckboxes); // Återställ markerade kryssrutor
            ApplyDynamicFilterUpdates();  // Uppdatera kryssrutornas tillgänglighet baserat på filtren
        }




        private void ClearScoresForSelectedExercises()
        {
            // Sätt filesInUse till true för att förhindra att filtret uppdateras medan vi gör ändringar
            filesInUse = true;

            // Spara nuvarande filterstatus och markeringar för att återställa senare
            FilterCriteria storedFilters = GetSelectedFilters();


            // Spara markerade checkboxar för varje grupp
            Dictionary<string, List<string>> selectedCheckboxes = GetSelectedCheckboxes();

            // Filtrera och hitta alla övningar som matchar nuvarande filter
            var filteredExercises = ExerciseLoader.GetFilteredExercises( // Använd ExerciseLoader för att hämta de filtrerade övningarna
                _allExercises, GetSelectedFilters()); // Använd alla övningar och de valda filtren

            if (filteredExercises.Count == 0) // Om inga övningar matchade filtret
            {
                MessageBox.Show("Inga övningar matchade ditt urval.", "Inga matchningar"); // Visa ett meddelande
                filesInUse = false;
                return;
            }

            // Nollställ poäng för varje övning i det valda urvalet och spara de påverkade filerna
            var affectedFiles = new HashSet<string>();
            foreach (var exercise in filteredExercises)
            {
                exercise.LatestPoint = 0;
                exercise.SecondLatestPoint = 0;
                exercise.ThirdLatestPoint = 0;
                exercise.FourthLatestPoint = 0;
                exercise.FifthLatestPoint = 0;
                exercise.UpdateDate = DateTime.Now;// Uppdatera datumet för senaste ändringen till nuvarande tidpunkt
                affectedFiles.Add(exercise.FileName);
            }

            // Skriv tillbaka nollställda poäng till de påverkade Excel-filerna
            foreach (var fileName in affectedFiles)
            {
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "excel", fileName);
                using var package = new ExcelPackage(new FileInfo(filePath));
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Hämta första arket i Excel-filen
                foreach (var exercise in filteredExercises.Where(e => e.FileName == fileName))
                {
                    int row = exercise.RowNumber;
                    worksheet.Cells[row, 6].Value = exercise.UpdateDate;
                    worksheet.Cells[row, 7].Value = exercise.LatestPoint;
                    worksheet.Cells[row, 8].Value = exercise.SecondLatestPoint;
                    worksheet.Cells[row, 9].Value = exercise.ThirdLatestPoint;
                    worksheet.Cells[row, 10].Value = exercise.FourthLatestPoint;
                    worksheet.Cells[row, 11].Value = exercise.FifthLatestPoint;
                }
                package.Save(); // Spara Excel-filen
            }

            // Ladda om uppdaterad data från mappen och återställ filter och markeringar
            LoadExercisesFromFolder();

            _selectedFilters = storedFilters;// Återställ filterkriterierna
            RestoreCheckboxState(selectedCheckboxes); // Återställ markerade kryssrutor

            // Uppdatera räkneverk och filterdynamik efter att ha återställt filter och markeringar
            ApplyDynamicFilterUpdates(); // Uppdatera kryssrutornas tillgänglighet baserat på filtren
            UpdateFilteredCount(); // Uppdatera räkneverket 

            // Återställ filesInUse-flaggan
            filesInUse = false;

        }


        private void LoadExercisesFromFolder() // Metod för att ladda in övningar från mappen
        {
            string directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "excel"); // Skapa sökväg till mappen med Excel-filer
            _allExercises = ExerciseLoader.LoadAllExercises(directoryPath); // Ladda in alla övningar från mappen
            UpdateFilteredCount(); // Uppdatera räkneverket
        }



        private FilterCriteria GetSelectedFilters() // Metod för att hämta valda filter
        {
            return new FilterCriteria // Returnera en ny instans av FilterCriteria med valda filter
            {
                Languages = new List<string>(_selectedFilters.Languages), // Kopiera listan med valda språk
                WordClasses = new List<string>(_selectedFilters.WordClasses), // Kopiera listan med valda ordklasser
                Chapters = new List<string>(_selectedFilters.Chapters), // Kopiera listan med valda kapitel
                FileNames = new List<string>(_selectedFilters.FileNames), // Kopiera listan med valda filnamn
                AveragePoints = new List<string>(_selectedFilters.AveragePoints), // Kopiera listan med valda genomsnittspoäng
                DirtyStatuses = new List<string>(_selectedFilters.DirtyStatuses) // Kopiera listan med valda "dirty"-statusar
            };
        }

        private Dictionary<string, List<string>> GetSelectedCheckboxes() // Metod för att hämta valda kryssrutor
        {
            var selectedCheckboxes = new Dictionary<string, List<string>> // Skapa en ny dictionary för att lagra valda kryssrutor
            {
                ["Languages"] = languageBox.Controls.OfType<CheckBox>() // Hämta alla kryssrutor i språkgruppen
                            .Where(cb => cb.Checked)
                            .Select(cb => cb.Text).ToList(),
                ["WordClasses"] = wordClassBox.Controls.OfType<CheckBox>() // Hämta alla kryssrutor i ordklassgruppen
                            .Where(cb => cb.Checked)
                            .Select(cb => cb.Text).ToList(),
                ["Chapters"] = chapterBox.Controls.OfType<CheckBox>() // Hämta alla kryssrutor i kapitelgruppen
                            .Where(cb => cb.Checked)
                            .Select(cb => cb.Text).ToList(),
                ["FileNames"] = fileNameBox.Controls.OfType<CheckBox>() // Hämta alla kryssrutor i filnamnsgruppen
                            .Where(cb => cb.Checked)
                            .Select(cb => cb.Text).ToList(),
                ["AveragePoints"] = averagePointsBox.Controls.OfType<CheckBox>() // Hämta alla kryssrutor i genomsnittspoängsgruppen
                            .Where(cb => cb.Checked)
                            .Select(cb => cb.Text).ToList(),
                ["DirtyStatuses"] = dirtyStatusBox.Controls.OfType<CheckBox>() // Hämta alla kryssrutor i dirtyStatus-gruppen
                            .Where(cb => cb.Checked)
                            .Select(cb => cb.Text).ToList()
            };



            return selectedCheckboxes; // Returnera dictionaryn med valda kryssrutor
        }

        private void ApplyDynamicFilterUpdates() // Metod för att uppdatera filterdynamik
        {
            var availableLanguages = GetAvailableOptions(e => e.Language, e => MatchesFilter(e, _selectedFilters.WordClasses, e.WordClass, _selectedFilters.Chapters, e.Chapter, _selectedFilters.FileNames, e.FileName, _selectedFilters.AveragePoints, e.AveragePoint, _selectedFilters.DirtyStatuses, e.DirtyStatus)); // Hämta tillgängliga språk
            var availableWordClasses = GetAvailableOptions(e => e.WordClass, e => MatchesFilter(e, _selectedFilters.Languages, e.Language, _selectedFilters.Chapters, e.Chapter, _selectedFilters.FileNames, e.FileName, _selectedFilters.AveragePoints, e.AveragePoint, _selectedFilters.DirtyStatuses, e.DirtyStatus)); // Hämta tillgängliga ordklasser
            var availableChapters = GetAvailableOptions(e => e.Chapter, e => MatchesFilter(e, _selectedFilters.Languages, e.Language, _selectedFilters.WordClasses, e.WordClass, _selectedFilters.FileNames, e.FileName, _selectedFilters.AveragePoints, e.AveragePoint, _selectedFilters.DirtyStatuses, e.DirtyStatus)); // Hämta tillgängliga kapitel
            var availableFileNames = GetAvailableOptions(e => e.FileName, e => MatchesFilter(e, _selectedFilters.Languages, e.Language, _selectedFilters.WordClasses, e.WordClass, _selectedFilters.Chapters, e.Chapter, _selectedFilters.AveragePoints, e.AveragePoint, _selectedFilters.DirtyStatuses, e.DirtyStatus)); // Hämta tillgängliga filnamn
            var availableAveragePoints = GetAvailableOptions(e => e.AveragePoint, e => MatchesFilter(e, _selectedFilters.Languages, e.Language, _selectedFilters.WordClasses, e.WordClass, _selectedFilters.Chapters, e.Chapter, _selectedFilters.FileNames, e.FileName, _selectedFilters.DirtyStatuses, e.DirtyStatus)); // Hämta tillgängliga genomsnittspoäng
            var availableDirtyStatus = GetAvailableOptions(e => e.DirtyStatus, e => MatchesFilter(e, _selectedFilters.Languages, e.Language, _selectedFilters.WordClasses, e.WordClass, _selectedFilters.Chapters, e.Chapter, _selectedFilters.FileNames, e.FileName, _selectedFilters.AveragePoints, e.AveragePoint)); // Hämta tillgängliga "dirty"-statusar

            // Uppdatera kryssrutornas tillgänglighet baserat på filtren
            UpdateCheckboxAvailability(languageBox, _selectedFilters.Languages, availableLanguages);// Uppdatera språkkryssrutornas tillgänglighet
            UpdateCheckboxAvailability(wordClassBox, _selectedFilters.WordClasses, availableWordClasses); // Uppdatera ordklasskryssrutornas tillgänglighet
            UpdateCheckboxAvailability(chapterBox, _selectedFilters.Chapters, availableChapters); // Uppdatera kapitelkryssrutornas tillgänglighet
            UpdateCheckboxAvailability(fileNameBox, _selectedFilters.FileNames, availableFileNames); // Uppdatera filnamnskryssrutornas tillgänglighet
            UpdateCheckboxAvailability(averagePointsBox, _selectedFilters.AveragePoints, availableAveragePoints); // Uppdatera genomsnittspoängskryssrutornas tillgänglighet
            UpdateCheckboxAvailability(dirtyStatusBox, _selectedFilters.DirtyStatuses, availableDirtyStatus); // Uppdatera "dirty"-statuskryssrutornas tillgänglighet

            ApplyColorToCheckBoxes(languageBox, availableLanguages);
            ApplyColorToCheckBoxes(wordClassBox, availableWordClasses);
            ApplyColorToCheckBoxes(chapterBox, availableChapters);
            ApplyColorToCheckBoxes(fileNameBox, availableFileNames);
            ApplyColorToCheckBoxes(averagePointsBox, availableAveragePoints);
            ApplyColorToCheckBoxes(dirtyStatusBox, availableDirtyStatus);

        }


        // Metod för att färglägga kryssrutorna baserat på tillgängliga alternativ
        private static void ApplyColorToCheckBoxes(GroupBox groupBox, List<string> availableOptions)
        {
            foreach (CheckBox checkBox in groupBox.Controls.OfType<CheckBox>())
            {
                // Om kryssrutan är markerad och inte finns i listan över tillgängliga alternativ, markera den visuellt
                if (checkBox.Checked && !availableOptions.Contains(checkBox.Text))
                {
                    checkBox.ForeColor = Color.Red; // Markera kryssrutan i rött
                }
                else
                {
                    checkBox.ForeColor = Color.Black; // Återställ färgen till svart
                    checkBox.Enabled = availableOptions.Contains(checkBox.Text); // Ställ in om kryssrutan är aktiverad eller inte
                }
            }
        }


        private static bool MatchesFilter(ExerciseData exercise, params object[] filterPairs) // Metod för att matcha gentemot filter
        {
            for (int i = 0; i < filterPairs.Length; i += 2) // Loopa igenom filterpar
            {
                var filterList = filterPairs[i] as List<string>; // Hämta filterlistan
                var filterValue = filterPairs[i + 1]?.ToString(); // Hämta filtervärdet

                if (filterList != null && filterList.Count > 0 && filterValue != null && !filterList.Contains(filterValue)) // Om filterlistan inte är tom och inte innehåller filtervärdet
                {
                    return false; // Returnera falskt
                }
            }
            return true; // Returnera sant
        }

        private List<string> GetAvailableOptions(Func<ExerciseData, string> selector, Func<ExerciseData, bool> filter) // Metod för att hämta tillgängliga alternativ
        {
            return _allExercises
                .Where(filter)
                .Select(selector)
                .Distinct()
                .ToList();
        }
        private static void UpdateCheckboxAvailability(GroupBox groupBox, List<string> selectedItems, List<string> availableOptions) // Metod för att uppdatera kryssrutornas tillgänglighet
        {
            bool tabStopAssigned = false; // Variabel för att spåra om tabbinställning har tilldelats

            foreach (CheckBox checkBox in groupBox.Controls.OfType<CheckBox>()) // Loopa igenom alla kryssrutor i gruppen
            {
                // Uppdatera kryssrutans tillgänglighet
                checkBox.Enabled = selectedItems.Contains(checkBox.Text) || availableOptions.Contains(checkBox.Text);

                // Ge bara tabläge till den första aktiverade kryssrutan
                if (!tabStopAssigned && checkBox.Enabled)
                {
                    checkBox.TabStop = true; // Tilldela tabläge
                    tabStopAssigned = true; // Markera att tabbinställningen har tilldelats
                }
                else
                {
                    checkBox.TabStop = false; // Inaktivera tabläge för alla andra kryssrutor
                }
            }
        }



        private void UpdateFilteredCount() // Metod för att uppdatera räkneverket
        {
            var filteredExercises = ExerciseLoader.GetFilteredExercises(
                _allExercises, GetSelectedFilters());

            double totalScore = filteredExercises.Sum(e => e.LatestPoint + e.SecondLatestPoint + e.ThirdLatestPoint + e.FourthLatestPoint + e.FifthLatestPoint);
            double maxScore = filteredExercises.Count * 5;
            double percentageScore = maxScore > 0 ? (totalScore / maxScore) * 100 : 0;
            counter.Text = $"{filteredExercises.Count} ({_allExercises.Count})\n{percentageScore:F2}%";

        }

        private void UpdateFilterCriteria(string optionType, string value, bool isChecked) // Metod för att uppdatera filterkriterier
        {
            List<string>? targetList = optionType switch // Välj vilken lista som ska uppdateras baserat på filtertyp
            {
                "Language" => _selectedFilters.Languages, // Om filtertypen är språk, uppdatera språklistan
                "WordClass" => _selectedFilters.WordClasses, // Om filtertypen är ordklass, uppdatera ordklasslistan
                "Chapter" => _selectedFilters.Chapters, // Om filtertypen är kapitel, uppdatera kapitellistan
                "FileName" => _selectedFilters.FileNames, // Om filtertypen är filnamn, uppdatera filnamnslistan
                "AveragePoint" => _selectedFilters.AveragePoints, // Om filtertypen är genomsnittspoäng, uppdatera genomsnittspoängslistan
                "DirtyStatus" => _selectedFilters.DirtyStatuses, // Om filtertypen är "dirty"-status, uppdatera "dirty"-statuslistan
                _ => null // Om filtertypen är ogiltig, returnera null
            };

            if (targetList == null)
            {
                // Visa ett popup-meddelande om filtertypen är ogiltig
                MessageBox.Show("Ogiltig filtertyp angiven.", "Fel", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Avbryt metoden så att inget mer körs
            }

            if (isChecked)
            {
                if (!targetList.Contains(value)) // Om värdet inte redan finns i listan
                    targetList.Add(value); // Lägg till värdet i listan
            }
            else
            {
                targetList.Remove(value);
            }

            UpdateFilteredCount(); // Uppdatera räkneverket efter varje filterändring
            ApplyDynamicFilterUpdates(); // Uppdatera kryssrutornas tillgänglighet baserat på filtren
        }




        private void PopulateFilterOptions() // Metod för att fylla filteralternativ
        {
            PopulateOptions(languageBox, _allExercises.Select(e => e.Language).Distinct().ToList(), "Language"); // Fyll språkfilteralternativ
            PopulateOptions(wordClassBox, _allExercises.Select(e => e.WordClass).Distinct().ToList(), "WordClass"); // Fyll ordklassfilteralternativ
            PopulateOptions(chapterBox, _allExercises.Select(e => e.Chapter).Distinct().ToList(), "Chapter"); // Fyll kapitelfilteralternativ
            PopulateOptions(fileNameBox, _allExercises.Select(e => e.FileName).Distinct().ToList(), "FileName"); // Fyll filnamnsfilteralternativ
            PopulateOptions(averagePointsBox, _allExercises.Select(e => e.AveragePoint).Distinct().ToList(), "AveragePoint"); // Fyll genomsnittspoängsfilteralternativ
            PopulateOptions(dirtyStatusBox, _allExercises.Select(e => e.DirtyStatus).Distinct().ToList(), "DirtyStatus"); // Fyll "dirty"-statusfilteralternativ
        }

        private void PopulateOptions(GroupBox groupBox, List<string> options, string optionType) // Metod för att fylla filteralternativens kryssrutor
        {
            groupBox.Controls.Clear(); // Rensa alla kryssrutor i gruppen
            groupBox.TabStop = true; // Gör gruppen tabb-bar

            options = ExerciseLoader.SortNumbersAndStrings(options); // Sortera siffror och strängar i filteralternativen

            int yOffset = 5; // Y-avstånd för kryssrutorna
            bool firstCheckbox = true; // Flagga för att hålla koll på om det är den första kryssrutan i gruppen

            foreach (var option in options) // Loopa igenom alla filteralternativ
            {
                var checkBox = new CheckBox // Skapa en ny kryssruta för varje filteralternativ
                {
                    Text = option, // Sätt texten för kryssrutan till filteralternativet
                    AutoSize = true, // Sätt storleken för kryssrutan automatiskt
                    Location = new Point(5, yOffset), // Sätt positionen för kryssrutan
                    TabStop = firstCheckbox // Endast den första checkboxen i varje grupp är tabb-bar
                };

                // Lägg till händelse för när checkboxen ändras
                checkBox.CheckedChanged += (s, e) => // När kryssrutan ändras
                {
                    UpdateFilterCriteria(optionType, checkBox.Text, checkBox.Checked); // Uppdatera filterkriterierna
                    UpdateFilteredCount(); // Uppdatera räkneverket
                };

                groupBox.Controls.Add(checkBox); // Lägg till kryssrutan i gruppen
                yOffset += 25; // Öka y-avståndet för nästa kryssruta
                firstCheckbox = false; // Gör alla andra checkboxar otabb-bara
            }
        }



        private void ResetFilters() // Metod för att återställa filter
        {

            SuspendLayout(); // Pausa layouten för att förhindra att UI uppdateras för varje ändring

            _selectedFilters = new FilterCriteria(); // Nollställ filterkriterierna


            ResetCheckBoxes(languageBox); // Ta bort alla kryss från kryssrutor i språkgruppen
            ResetCheckBoxes(wordClassBox); // Ta bort alla kryss från kryssrutor i språkgruppen
            ResetCheckBoxes(chapterBox); // Ta bort alla kryss från kryssrutor i språkgruppen
            ResetCheckBoxes(fileNameBox); // Ta bort alla kryss från kryssrutor i språkgruppen
            ResetCheckBoxes(averagePointsBox); // Ta bort alla kryss från kryssrutor i språkgruppen
            ResetCheckBoxes(dirtyStatusBox); // Ta bort alla kryss från kryssrutor i språkgruppen

            ResumeLayout(true); // Återuppta layouten och uppdatera UI
            Refresh();  // Uppdatera UI

            ApplyDynamicFilterUpdates(); // Uppdatera kryssrutornas tillgänglighet baserat på filtren
            UpdateFilteredCount(); // Uppdatera räkneverket
        }

        private static void ResetCheckBoxes(GroupBox groupBox)
        {
            foreach (CheckBox checkBox in groupBox.Controls.OfType<CheckBox>())
            {
                checkBox.Checked = false;
            }
        }


        private void RestoreCheckboxState(Dictionary<string, List<string>> selectedCheckboxes) // Metod för att återställa kryssrutornas tillstånd
        {
            RestoreCheckboxGroup(languageBox, selectedCheckboxes["Languages"]); // Återställ språkkryssrutornas tillstånd
            RestoreCheckboxGroup(wordClassBox, selectedCheckboxes["WordClasses"]); // Återställ ordklasskryssrutornas tillstånd
            RestoreCheckboxGroup(chapterBox, selectedCheckboxes["Chapters"]); // Återställ kapitelkryssrutornas tillstånd
            RestoreCheckboxGroup(fileNameBox, selectedCheckboxes["FileNames"]); // Återställ filnamnskryssrutornas tillstånd
            RestoreCheckboxGroup(averagePointsBox, selectedCheckboxes["AveragePoints"]); // Återställ genomsnittspoängskryssrutornas tillstånd
            RestoreCheckboxGroup(dirtyStatusBox, selectedCheckboxes["DirtyStatuses"]); // Återställ "dirty"-statuskryssrutornas tillstånd
        }

        private static void RestoreCheckboxGroup(GroupBox groupBox, List<string> selectedItems) // Metod för att återställa kryssrutornas tillstånd i en grupp
        {
            bool tabSet = false; // Flagga för att hålla koll på om tabbning har ställts in
            foreach (var checkBox in groupBox.Controls.OfType<CheckBox>()) // Loopa igenom alla kryssrutor i gruppen
            {
                checkBox.Checked = selectedItems.Contains(checkBox.Text); // Återställ kryssrutans tillstånd
                checkBox.TabStop = !tabSet && checkBox.Enabled; // Om tabbning inte har ställts in och kryssrutan är aktiverad, sätt tabbning
                tabSet = tabSet || checkBox.TabStop; // Uppdatera flaggan för att hålla koll på om tabbning har ställts in
            }
        }



        private void PracticeIntoLanguage_Click(object sender, EventArgs e) => Practice(true); // Metod för att öva till språket, knappen klickas och metoden anropas

        private void PracticeFromLanguage_Click(object sender, EventArgs e) => Practice(false); // Metod för att öva från språket, knappen klickas och metoden anropas

        private void ResetFilters_Click(object sender, EventArgs e) => ResetFilters(); // Metod för att återställa filter, knappen klickas och metoden anropas

        private void ClearScores_Click(object sender, EventArgs e) => ClearScoresForSelectedExercises(); // Metod för att nollställa poäng för valda övningar, knappen klickas och metoden anropas

        private void ReadmeButton_Click(object sender, EventArgs e)
        {
            ReadmeForm readmeForm = new ReadmeForm();
            readmeForm.ShowDialog();
        }

    }












}
