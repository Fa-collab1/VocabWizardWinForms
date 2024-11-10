using OfficeOpenXml;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace VocabWizardWinForms
{
    public partial class StartForm : Form
    {
        private List<ExerciseData> _allExercises = []; // Lista som lagrar alla �vningar
        private FilterCriteria _selectedFilters = new(); // Filterkriterier f�r att filtrera �vningar
        private FileSystemWatcher? _fileWatcher; // Fil�vervakare f�r att uppt�cka �ndringar i Excel-filerna som inneh�ller �vningar
        private bool filesInUse = false; // Flagga f�r att f�rhindra att filer som h�ller p� att uppdateras inte l�ses in innan anv�ndaren �r klar


        public StartForm()
        {
            InitializeComponent(); // Initialisera komponenter
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // S�tt licenskontext f�r att undvika licensvarningar
            InitializeFileWatcher(); // Initialisera fil�vervakaren
            LoadExercisesFromFolder(); // Ladda in �vningar fr�n mappen
            PopulateFilterOptions(); // Fyll filteralternativ
            oneCardRadionButton.Checked = false; // Avmarkera radioknappen f�r en �vning
            fiveCardRadionButton.Checked = true; // Markera radioknappen f�r fem �vningar
        }


        private void toggle_RadioButton_Click(object sender, EventArgs e) // Metod f�r att byta mellan att �va p� en �vning eller fem �vningar har satt denna som acceptknapp f�r att kunna anv�nda enter f�r att byta mellan att �va p� en eller fem �vningar (anv�nder en knapp som knappt syns f�r detta)    
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


        private void QuitButton_Click(object sender, EventArgs e) // Metod f�r att avsluta programmet n�r quit-knappen klickas
        {
            Application.Exit(); // Avsluta programmet
        }


        private void InitializeFileWatcher() // Metod f�r att initialisera fil�vervakaren
        {
            string directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "excel"); // Skapa s�kv�g till mappen med Excel-filer

            if (!Directory.Exists(directoryPath)) // Om mappen inte finns
            {
                Directory.CreateDirectory(directoryPath); // Skapa mappen
            }

            _fileWatcher = new FileSystemWatcher(directoryPath, "*.xlsx") // Skapa en ny fil�vervakare f�r mappen med Excel-filer
            {
                EnableRaisingEvents = true, // Aktivera h�ndelserbevakare
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite // Aktivera h�ndelsebevakare f�r filnamn och senaste skrivning
            };

            _fileWatcher.Created += OnFileChanged; // L�gg till ett event f�r n�r en fil skapas
            _fileWatcher.Changed += OnFileChanged; // L�gg till ett event f�r n�r en fil �ndras
            _fileWatcher.Deleted += OnFileChanged; // L�gg till ett event f�r n�r en fil tas bort
            _fileWatcher.Renamed += OnFileRenamed; // L�gg till ett event f�r n�r en fil byter namn


#if DEBUG // Om programmet k�rs i debug-l�ge
            string devFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Excel"); // Skapa s�kv�g till utvecklingsmappen med Excel-filer

            if (Directory.Exists(devFolderPath)) // Om utvecklingsmappen finns
            {
                foreach (var sourceFilePath in Directory.GetFiles(devFolderPath, "*.xlsx")) // Loopa igenom alla Excel-filer i utvecklingsmappen
                {
                    var destFilePath = Path.Combine(directoryPath, Path.GetFileName(sourceFilePath)); // Skapa s�kv�g till destinationsmappen

                    if (File.Exists(destFilePath)) // Om filen redan finns i destinationsmappen
                    {
                        DateTime sourceLastWriteTime = File.GetLastWriteTime(sourceFilePath); // H�mta senaste skrivningstid f�r k�llfilen
                        DateTime destLastWriteTime = File.GetLastWriteTime(destFilePath); // H�mta senaste skrivningstid f�r destinationsfilen

                        if (sourceLastWriteTime > destLastWriteTime) // Om k�llfilen �r nyare �n destinationsfilen
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

        private void OnFileChanged(object sender, FileSystemEventArgs e) // Metod f�r att hantera fil�ndringar
        {
            if (filesInUse == false) // om fil anv�nds, g�r inget
            {

                if (InvokeRequired) // Om anropet inte sker p� r�tt tr�d
                {
                    Invoke(new Action(() => OnFileChanged(sender, e))); // Anropa metoden p� r�tt tr�d
                    return; // Returnera
                }

                ReLoad(e); // Ladda om filen

            }
        }

        private void ReLoad(FileSystemEventArgs e)
        {
            try
            {
                // Kontroll om filen �r tillg�nglig
                using (var stream = new FileStream(e.FullPath, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    // Kontroll om filen �r tillg�nglig
                    stream.Close(); // St�ng filen s� att den kan anv�ndas av f�ljande processer
                }

                LoadExercisesFromFolder(); // Ladda in �vningar fr�n mappen
                PopulateFilterOptions(); // Fyll filteralternativ
                UpdateFilteredCount(); // Uppdatera r�kneverket

            }
            catch (IOException) // Om det uppst�r ett IO-fel
            {
                MessageBox.Show($"Unable to access the file '{e.Name}'. It might be open in another application.", "File Access Error"); // Visa ett felmeddelande
            }
        }

        private void OnFileRenamed(object sender, RenamedEventArgs e)
        {
            if (filesInUse == false) // om fil anv�nds, g�r inget
            {
                if (InvokeRequired) // Kontrollera om metoden anropas fr�n en annan tr�d �n huvudtr�den
                {
                    // Om anropet sker fr�n en annan tr�d �n huvudtr�den, k�r metoden igen p� huvudtr�den
                    Invoke(new Action(() => OnFileRenamed(sender, e))); // Flytta anropet till huvudtr�den f�r att s�kert uppdatera UI
                    return; // Avbryt metoden s� att den inte forts�tter att k�ras p� fel tr�d
                }

                ReLoad(e); // Ladda om filen
            }
        }


        private void Practice(bool intoLanguage) // Metod f�r att �va p� �vningar
        {
            filesInUse = true; // S�tt filesInUse till true f�r att f�rhindra att filtret uppdateras medan vi �var s� det inte laddar om datat varje g�ng vi sparar po�ng efter varje ord/mening vi �var p�, det segar ner programmet otroligt mycket d�


            var storedFilters = GetSelectedFilters(); // Spara nuvarande filterstatus och markeringar f�r att kunna �terst�lla dem senare
            var selectedCheckboxes = GetSelectedCheckboxes(); // Spara markerade checkboxar f�r varje grupp f�r att kunna �terst�lla dem senare

            var filteredExercises = ExerciseLoader.GetFilteredExercises( // Anv�nd ExerciseLoader f�r att h�mta de filtrerade �vningarna
                _allExercises, storedFilters); // Anv�nd alla �vningar och de valda filtren

            if (filteredExercises.Count == 0) // Om inga �vningar matchade filtret
            {
                MessageBox.Show("No exercises matched your filter criteria. Please adjust your selections.", "No Results"); // Visa ett meddelande om att inga �vningar matchade filtret, ingen id� att �va om det inte finns n�gra �vningar att �va p�... s� vi avbryter h�r
                filesInUse = false; // �terst�ll filesInUse-flaggan
                return; // Avbryt metoden s� att inget mer k�rs
            }

            var practiceSet = filteredExercises.Select(e => new ExerciseData // Skapa en ny lista med �vningar att �va p�
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
                Original = intoLanguage ? e.Translation : e.Original, // baserat p� flaggan, byt plats (eller inte) p� �vers�ttning och original
                Translation = intoLanguage ? e.Original : e.Translation, // baserat p� flaggan, byt plats (eller inte) p� �vers�ttning och original 
                Dirty = e.Dirty
            }).OrderBy(x => new Random().Next()).ToList();

            var flashcardForm = new FlashCardForm(practiceSet, oneCardRadionButton.Checked); // Skapa en ny instans av FlashCardForm med �vningarna ovan att �va p� och bool-v�rdet p� radioknappen f�r en �vning eller fem �vningar skickas med
            flashcardForm.ShowDialog(); // Visa FlashCardForm som en dialogruta
            filesInUse = false; // �terst�ll filesInUse-flaggan efter att �vningen �r klar

            // Ladda om uppdaterad data fr�n mappen och �terst�ll filter och markeringar
            LoadExercisesFromFolder(); // Ladda in �vningar fr�n mappen
            PopulateFilterOptions(); // Fyll filteralternativ
            UpdateFilteredCount(); // Uppdatera r�kneverket

            // �terst�ll filter och markeringar
            _selectedFilters = storedFilters; // �terst�ll filterkriterierna
            RestoreCheckboxState(selectedCheckboxes); // �terst�ll markerade kryssrutor
            ApplyDynamicFilterUpdates();  // Uppdatera kryssrutornas tillg�nglighet baserat p� filtren
        }




        private void ClearScoresForSelectedExercises()
        {
            // S�tt filesInUse till true f�r att f�rhindra att filtret uppdateras medan vi g�r �ndringar
            filesInUse = true;

            // Spara nuvarande filterstatus och markeringar f�r att �terst�lla senare
            FilterCriteria storedFilters = GetSelectedFilters();


            // Spara markerade checkboxar f�r varje grupp
            Dictionary<string, List<string>> selectedCheckboxes = GetSelectedCheckboxes();

            // Filtrera och hitta alla �vningar som matchar nuvarande filter
            var filteredExercises = ExerciseLoader.GetFilteredExercises( // Anv�nd ExerciseLoader f�r att h�mta de filtrerade �vningarna
                _allExercises, GetSelectedFilters()); // Anv�nd alla �vningar och de valda filtren

            if (filteredExercises.Count == 0) // Om inga �vningar matchade filtret
            {
                MessageBox.Show("Inga �vningar matchade ditt urval.", "Inga matchningar"); // Visa ett meddelande
                filesInUse = false;
                return;
            }

            // Nollst�ll po�ng f�r varje �vning i det valda urvalet och spara de p�verkade filerna
            var affectedFiles = new HashSet<string>();
            foreach (var exercise in filteredExercises)
            {
                exercise.LatestPoint = 0;
                exercise.SecondLatestPoint = 0;
                exercise.ThirdLatestPoint = 0;
                exercise.FourthLatestPoint = 0;
                exercise.FifthLatestPoint = 0;
                exercise.UpdateDate = DateTime.Now;// Uppdatera datumet f�r senaste �ndringen till nuvarande tidpunkt
                affectedFiles.Add(exercise.FileName);
            }

            // Skriv tillbaka nollst�llda po�ng till de p�verkade Excel-filerna
            foreach (var fileName in affectedFiles)
            {
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "excel", fileName);
                using var package = new ExcelPackage(new FileInfo(filePath));
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // H�mta f�rsta arket i Excel-filen
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

            // Ladda om uppdaterad data fr�n mappen och �terst�ll filter och markeringar
            LoadExercisesFromFolder();

            _selectedFilters = storedFilters;// �terst�ll filterkriterierna
            RestoreCheckboxState(selectedCheckboxes); // �terst�ll markerade kryssrutor

            // Uppdatera r�kneverk och filterdynamik efter att ha �terst�llt filter och markeringar
            ApplyDynamicFilterUpdates(); // Uppdatera kryssrutornas tillg�nglighet baserat p� filtren
            UpdateFilteredCount(); // Uppdatera r�kneverket 

            // �terst�ll filesInUse-flaggan
            filesInUse = false;

        }


        private void LoadExercisesFromFolder() // Metod f�r att ladda in �vningar fr�n mappen
        {
            string directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "excel"); // Skapa s�kv�g till mappen med Excel-filer
            _allExercises = ExerciseLoader.LoadAllExercises(directoryPath); // Ladda in alla �vningar fr�n mappen
            UpdateFilteredCount(); // Uppdatera r�kneverket
        }



        private FilterCriteria GetSelectedFilters() // Metod f�r att h�mta valda filter
        {
            return new FilterCriteria // Returnera en ny instans av FilterCriteria med valda filter
            {
                Languages = new List<string>(_selectedFilters.Languages), // Kopiera listan med valda spr�k
                WordClasses = new List<string>(_selectedFilters.WordClasses), // Kopiera listan med valda ordklasser
                Chapters = new List<string>(_selectedFilters.Chapters), // Kopiera listan med valda kapitel
                FileNames = new List<string>(_selectedFilters.FileNames), // Kopiera listan med valda filnamn
                AveragePoints = new List<string>(_selectedFilters.AveragePoints), // Kopiera listan med valda genomsnittspo�ng
                DirtyStatuses = new List<string>(_selectedFilters.DirtyStatuses) // Kopiera listan med valda "dirty"-statusar
            };
        }

        private Dictionary<string, List<string>> GetSelectedCheckboxes() // Metod f�r att h�mta valda kryssrutor
        {
            var selectedCheckboxes = new Dictionary<string, List<string>> // Skapa en ny dictionary f�r att lagra valda kryssrutor
            {
                ["Languages"] = languageBox.Controls.OfType<CheckBox>() // H�mta alla kryssrutor i spr�kgruppen
                            .Where(cb => cb.Checked)
                            .Select(cb => cb.Text).ToList(),
                ["WordClasses"] = wordClassBox.Controls.OfType<CheckBox>() // H�mta alla kryssrutor i ordklassgruppen
                            .Where(cb => cb.Checked)
                            .Select(cb => cb.Text).ToList(),
                ["Chapters"] = chapterBox.Controls.OfType<CheckBox>() // H�mta alla kryssrutor i kapitelgruppen
                            .Where(cb => cb.Checked)
                            .Select(cb => cb.Text).ToList(),
                ["FileNames"] = fileNameBox.Controls.OfType<CheckBox>() // H�mta alla kryssrutor i filnamnsgruppen
                            .Where(cb => cb.Checked)
                            .Select(cb => cb.Text).ToList(),
                ["AveragePoints"] = averagePointsBox.Controls.OfType<CheckBox>() // H�mta alla kryssrutor i genomsnittspo�ngsgruppen
                            .Where(cb => cb.Checked)
                            .Select(cb => cb.Text).ToList(),
                ["DirtyStatuses"] = dirtyStatusBox.Controls.OfType<CheckBox>() // H�mta alla kryssrutor i dirtyStatus-gruppen
                            .Where(cb => cb.Checked)
                            .Select(cb => cb.Text).ToList()
            };



            return selectedCheckboxes; // Returnera dictionaryn med valda kryssrutor
        }

        private void ApplyDynamicFilterUpdates() // Metod f�r att uppdatera filterdynamik
        {
            var availableLanguages = GetAvailableOptions(e => e.Language, e => MatchesFilter(e, _selectedFilters.WordClasses, e.WordClass, _selectedFilters.Chapters, e.Chapter, _selectedFilters.FileNames, e.FileName, _selectedFilters.AveragePoints, e.AveragePoint, _selectedFilters.DirtyStatuses, e.DirtyStatus)); // H�mta tillg�ngliga spr�k
            var availableWordClasses = GetAvailableOptions(e => e.WordClass, e => MatchesFilter(e, _selectedFilters.Languages, e.Language, _selectedFilters.Chapters, e.Chapter, _selectedFilters.FileNames, e.FileName, _selectedFilters.AveragePoints, e.AveragePoint, _selectedFilters.DirtyStatuses, e.DirtyStatus)); // H�mta tillg�ngliga ordklasser
            var availableChapters = GetAvailableOptions(e => e.Chapter, e => MatchesFilter(e, _selectedFilters.Languages, e.Language, _selectedFilters.WordClasses, e.WordClass, _selectedFilters.FileNames, e.FileName, _selectedFilters.AveragePoints, e.AveragePoint, _selectedFilters.DirtyStatuses, e.DirtyStatus)); // H�mta tillg�ngliga kapitel
            var availableFileNames = GetAvailableOptions(e => e.FileName, e => MatchesFilter(e, _selectedFilters.Languages, e.Language, _selectedFilters.WordClasses, e.WordClass, _selectedFilters.Chapters, e.Chapter, _selectedFilters.AveragePoints, e.AveragePoint, _selectedFilters.DirtyStatuses, e.DirtyStatus)); // H�mta tillg�ngliga filnamn
            var availableAveragePoints = GetAvailableOptions(e => e.AveragePoint, e => MatchesFilter(e, _selectedFilters.Languages, e.Language, _selectedFilters.WordClasses, e.WordClass, _selectedFilters.Chapters, e.Chapter, _selectedFilters.FileNames, e.FileName, _selectedFilters.DirtyStatuses, e.DirtyStatus)); // H�mta tillg�ngliga genomsnittspo�ng
            var availableDirtyStatus = GetAvailableOptions(e => e.DirtyStatus, e => MatchesFilter(e, _selectedFilters.Languages, e.Language, _selectedFilters.WordClasses, e.WordClass, _selectedFilters.Chapters, e.Chapter, _selectedFilters.FileNames, e.FileName, _selectedFilters.AveragePoints, e.AveragePoint)); // H�mta tillg�ngliga "dirty"-statusar

            // Uppdatera kryssrutornas tillg�nglighet baserat p� filtren
            UpdateCheckboxAvailability(languageBox, _selectedFilters.Languages, availableLanguages);// Uppdatera spr�kkryssrutornas tillg�nglighet
            UpdateCheckboxAvailability(wordClassBox, _selectedFilters.WordClasses, availableWordClasses); // Uppdatera ordklasskryssrutornas tillg�nglighet
            UpdateCheckboxAvailability(chapterBox, _selectedFilters.Chapters, availableChapters); // Uppdatera kapitelkryssrutornas tillg�nglighet
            UpdateCheckboxAvailability(fileNameBox, _selectedFilters.FileNames, availableFileNames); // Uppdatera filnamnskryssrutornas tillg�nglighet
            UpdateCheckboxAvailability(averagePointsBox, _selectedFilters.AveragePoints, availableAveragePoints); // Uppdatera genomsnittspo�ngskryssrutornas tillg�nglighet
            UpdateCheckboxAvailability(dirtyStatusBox, _selectedFilters.DirtyStatuses, availableDirtyStatus); // Uppdatera "dirty"-statuskryssrutornas tillg�nglighet

            ApplyColorToCheckBoxes(languageBox, availableLanguages);
            ApplyColorToCheckBoxes(wordClassBox, availableWordClasses);
            ApplyColorToCheckBoxes(chapterBox, availableChapters);
            ApplyColorToCheckBoxes(fileNameBox, availableFileNames);
            ApplyColorToCheckBoxes(averagePointsBox, availableAveragePoints);
            ApplyColorToCheckBoxes(dirtyStatusBox, availableDirtyStatus);

        }


        // Metod f�r att f�rgl�gga kryssrutorna baserat p� tillg�ngliga alternativ
        private static void ApplyColorToCheckBoxes(GroupBox groupBox, List<string> availableOptions)
        {
            foreach (CheckBox checkBox in groupBox.Controls.OfType<CheckBox>())
            {
                // Om kryssrutan �r markerad och inte finns i listan �ver tillg�ngliga alternativ, markera den visuellt
                if (checkBox.Checked && !availableOptions.Contains(checkBox.Text))
                {
                    checkBox.ForeColor = Color.Red; // Markera kryssrutan i r�tt
                }
                else
                {
                    checkBox.ForeColor = Color.Black; // �terst�ll f�rgen till svart
                    checkBox.Enabled = availableOptions.Contains(checkBox.Text); // St�ll in om kryssrutan �r aktiverad eller inte
                }
            }
        }


        private static bool MatchesFilter(ExerciseData exercise, params object[] filterPairs) // Metod f�r att matcha gentemot filter
        {
            for (int i = 0; i < filterPairs.Length; i += 2) // Loopa igenom filterpar
            {
                var filterList = filterPairs[i] as List<string>; // H�mta filterlistan
                var filterValue = filterPairs[i + 1]?.ToString(); // H�mta filterv�rdet

                if (filterList != null && filterList.Count > 0 && filterValue != null && !filterList.Contains(filterValue)) // Om filterlistan inte �r tom och inte inneh�ller filterv�rdet
                {
                    return false; // Returnera falskt
                }
            }
            return true; // Returnera sant
        }

        private List<string> GetAvailableOptions(Func<ExerciseData, string> selector, Func<ExerciseData, bool> filter) // Metod f�r att h�mta tillg�ngliga alternativ
        {
            return _allExercises
                .Where(filter)
                .Select(selector)
                .Distinct()
                .ToList();
        }
        private static void UpdateCheckboxAvailability(GroupBox groupBox, List<string> selectedItems, List<string> availableOptions) // Metod f�r att uppdatera kryssrutornas tillg�nglighet
        {
            bool tabStopAssigned = false; // Variabel f�r att sp�ra om tabbinst�llning har tilldelats

            foreach (CheckBox checkBox in groupBox.Controls.OfType<CheckBox>()) // Loopa igenom alla kryssrutor i gruppen
            {
                // Uppdatera kryssrutans tillg�nglighet
                checkBox.Enabled = selectedItems.Contains(checkBox.Text) || availableOptions.Contains(checkBox.Text);

                // Ge bara tabl�ge till den f�rsta aktiverade kryssrutan
                if (!tabStopAssigned && checkBox.Enabled)
                {
                    checkBox.TabStop = true; // Tilldela tabl�ge
                    tabStopAssigned = true; // Markera att tabbinst�llningen har tilldelats
                }
                else
                {
                    checkBox.TabStop = false; // Inaktivera tabl�ge f�r alla andra kryssrutor
                }
            }
        }



        private void UpdateFilteredCount() // Metod f�r att uppdatera r�kneverket
        {
            var filteredExercises = ExerciseLoader.GetFilteredExercises(
                _allExercises, GetSelectedFilters());

            double totalScore = filteredExercises.Sum(e => e.LatestPoint + e.SecondLatestPoint + e.ThirdLatestPoint + e.FourthLatestPoint + e.FifthLatestPoint);
            double maxScore = filteredExercises.Count * 5;
            double percentageScore = maxScore > 0 ? (totalScore / maxScore) * 100 : 0;
            counter.Text = $"{filteredExercises.Count} ({_allExercises.Count})\n{percentageScore:F2}%";

        }

        private void UpdateFilterCriteria(string optionType, string value, bool isChecked) // Metod f�r att uppdatera filterkriterier
        {
            List<string>? targetList = optionType switch // V�lj vilken lista som ska uppdateras baserat p� filtertyp
            {
                "Language" => _selectedFilters.Languages, // Om filtertypen �r spr�k, uppdatera spr�klistan
                "WordClass" => _selectedFilters.WordClasses, // Om filtertypen �r ordklass, uppdatera ordklasslistan
                "Chapter" => _selectedFilters.Chapters, // Om filtertypen �r kapitel, uppdatera kapitellistan
                "FileName" => _selectedFilters.FileNames, // Om filtertypen �r filnamn, uppdatera filnamnslistan
                "AveragePoint" => _selectedFilters.AveragePoints, // Om filtertypen �r genomsnittspo�ng, uppdatera genomsnittspo�ngslistan
                "DirtyStatus" => _selectedFilters.DirtyStatuses, // Om filtertypen �r "dirty"-status, uppdatera "dirty"-statuslistan
                _ => null // Om filtertypen �r ogiltig, returnera null
            };

            if (targetList == null)
            {
                // Visa ett popup-meddelande om filtertypen �r ogiltig
                MessageBox.Show("Ogiltig filtertyp angiven.", "Fel", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Avbryt metoden s� att inget mer k�rs
            }

            if (isChecked)
            {
                if (!targetList.Contains(value)) // Om v�rdet inte redan finns i listan
                    targetList.Add(value); // L�gg till v�rdet i listan
            }
            else
            {
                targetList.Remove(value);
            }

            UpdateFilteredCount(); // Uppdatera r�kneverket efter varje filter�ndring
            ApplyDynamicFilterUpdates(); // Uppdatera kryssrutornas tillg�nglighet baserat p� filtren
        }




        private void PopulateFilterOptions() // Metod f�r att fylla filteralternativ
        {
            PopulateOptions(languageBox, _allExercises.Select(e => e.Language).Distinct().ToList(), "Language"); // Fyll spr�kfilteralternativ
            PopulateOptions(wordClassBox, _allExercises.Select(e => e.WordClass).Distinct().ToList(), "WordClass"); // Fyll ordklassfilteralternativ
            PopulateOptions(chapterBox, _allExercises.Select(e => e.Chapter).Distinct().ToList(), "Chapter"); // Fyll kapitelfilteralternativ
            PopulateOptions(fileNameBox, _allExercises.Select(e => e.FileName).Distinct().ToList(), "FileName"); // Fyll filnamnsfilteralternativ
            PopulateOptions(averagePointsBox, _allExercises.Select(e => e.AveragePoint).Distinct().ToList(), "AveragePoint"); // Fyll genomsnittspo�ngsfilteralternativ
            PopulateOptions(dirtyStatusBox, _allExercises.Select(e => e.DirtyStatus).Distinct().ToList(), "DirtyStatus"); // Fyll "dirty"-statusfilteralternativ
        }

        private void PopulateOptions(GroupBox groupBox, List<string> options, string optionType) // Metod f�r att fylla filteralternativens kryssrutor
        {
            groupBox.Controls.Clear(); // Rensa alla kryssrutor i gruppen
            groupBox.TabStop = true; // G�r gruppen tabb-bar

            options = ExerciseLoader.SortNumbersAndStrings(options); // Sortera siffror och str�ngar i filteralternativen

            int yOffset = 5; // Y-avst�nd f�r kryssrutorna
            bool firstCheckbox = true; // Flagga f�r att h�lla koll p� om det �r den f�rsta kryssrutan i gruppen

            foreach (var option in options) // Loopa igenom alla filteralternativ
            {
                var checkBox = new CheckBox // Skapa en ny kryssruta f�r varje filteralternativ
                {
                    Text = option, // S�tt texten f�r kryssrutan till filteralternativet
                    AutoSize = true, // S�tt storleken f�r kryssrutan automatiskt
                    Location = new Point(5, yOffset), // S�tt positionen f�r kryssrutan
                    TabStop = firstCheckbox // Endast den f�rsta checkboxen i varje grupp �r tabb-bar
                };

                // L�gg till h�ndelse f�r n�r checkboxen �ndras
                checkBox.CheckedChanged += (s, e) => // N�r kryssrutan �ndras
                {
                    UpdateFilterCriteria(optionType, checkBox.Text, checkBox.Checked); // Uppdatera filterkriterierna
                    UpdateFilteredCount(); // Uppdatera r�kneverket
                };

                groupBox.Controls.Add(checkBox); // L�gg till kryssrutan i gruppen
                yOffset += 25; // �ka y-avst�ndet f�r n�sta kryssruta
                firstCheckbox = false; // G�r alla andra checkboxar otabb-bara
            }
        }



        private void ResetFilters() // Metod f�r att �terst�lla filter
        {

            SuspendLayout(); // Pausa layouten f�r att f�rhindra att UI uppdateras f�r varje �ndring

            _selectedFilters = new FilterCriteria(); // Nollst�ll filterkriterierna


            ResetCheckBoxes(languageBox); // Ta bort alla kryss fr�n kryssrutor i spr�kgruppen
            ResetCheckBoxes(wordClassBox); // Ta bort alla kryss fr�n kryssrutor i spr�kgruppen
            ResetCheckBoxes(chapterBox); // Ta bort alla kryss fr�n kryssrutor i spr�kgruppen
            ResetCheckBoxes(fileNameBox); // Ta bort alla kryss fr�n kryssrutor i spr�kgruppen
            ResetCheckBoxes(averagePointsBox); // Ta bort alla kryss fr�n kryssrutor i spr�kgruppen
            ResetCheckBoxes(dirtyStatusBox); // Ta bort alla kryss fr�n kryssrutor i spr�kgruppen

            ResumeLayout(true); // �teruppta layouten och uppdatera UI
            Refresh();  // Uppdatera UI

            ApplyDynamicFilterUpdates(); // Uppdatera kryssrutornas tillg�nglighet baserat p� filtren
            UpdateFilteredCount(); // Uppdatera r�kneverket
        }

        private static void ResetCheckBoxes(GroupBox groupBox)
        {
            foreach (CheckBox checkBox in groupBox.Controls.OfType<CheckBox>())
            {
                checkBox.Checked = false;
            }
        }


        private void RestoreCheckboxState(Dictionary<string, List<string>> selectedCheckboxes) // Metod f�r att �terst�lla kryssrutornas tillst�nd
        {
            RestoreCheckboxGroup(languageBox, selectedCheckboxes["Languages"]); // �terst�ll spr�kkryssrutornas tillst�nd
            RestoreCheckboxGroup(wordClassBox, selectedCheckboxes["WordClasses"]); // �terst�ll ordklasskryssrutornas tillst�nd
            RestoreCheckboxGroup(chapterBox, selectedCheckboxes["Chapters"]); // �terst�ll kapitelkryssrutornas tillst�nd
            RestoreCheckboxGroup(fileNameBox, selectedCheckboxes["FileNames"]); // �terst�ll filnamnskryssrutornas tillst�nd
            RestoreCheckboxGroup(averagePointsBox, selectedCheckboxes["AveragePoints"]); // �terst�ll genomsnittspo�ngskryssrutornas tillst�nd
            RestoreCheckboxGroup(dirtyStatusBox, selectedCheckboxes["DirtyStatuses"]); // �terst�ll "dirty"-statuskryssrutornas tillst�nd
        }

        private static void RestoreCheckboxGroup(GroupBox groupBox, List<string> selectedItems) // Metod f�r att �terst�lla kryssrutornas tillst�nd i en grupp
        {
            bool tabSet = false; // Flagga f�r att h�lla koll p� om tabbning har st�llts in
            foreach (var checkBox in groupBox.Controls.OfType<CheckBox>()) // Loopa igenom alla kryssrutor i gruppen
            {
                checkBox.Checked = selectedItems.Contains(checkBox.Text); // �terst�ll kryssrutans tillst�nd
                checkBox.TabStop = !tabSet && checkBox.Enabled; // Om tabbning inte har st�llts in och kryssrutan �r aktiverad, s�tt tabbning
                tabSet = tabSet || checkBox.TabStop; // Uppdatera flaggan f�r att h�lla koll p� om tabbning har st�llts in
            }
        }



        private void PracticeIntoLanguage_Click(object sender, EventArgs e) => Practice(true); // Metod f�r att �va till spr�ket, knappen klickas och metoden anropas

        private void PracticeFromLanguage_Click(object sender, EventArgs e) => Practice(false); // Metod f�r att �va fr�n spr�ket, knappen klickas och metoden anropas

        private void ResetFilters_Click(object sender, EventArgs e) => ResetFilters(); // Metod f�r att �terst�lla filter, knappen klickas och metoden anropas

        private void ClearScores_Click(object sender, EventArgs e) => ClearScoresForSelectedExercises(); // Metod f�r att nollst�lla po�ng f�r valda �vningar, knappen klickas och metoden anropas

        private void ReadmeButton_Click(object sender, EventArgs e)
        {
            ReadmeForm readmeForm = new ReadmeForm();
            readmeForm.ShowDialog();
        }

    }












}
