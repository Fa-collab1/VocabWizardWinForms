using OfficeOpenXml;

namespace VocabWizardWinForms
{
    public partial class FlashCardForm : Form
    {
        private readonly List<ExerciseData> _exerciseSet; // Lista med övningar
        private ExerciseData? _currentExercise; // Aktuell övning
        private ExerciseData? _lastExercise; // Förra övningen
        private int _currentIndex = 0; // Index för aktuell övning
        private int _remainingAttempts = 4; // Antal försök kvar att svara rätt
        private double _totalScore = 0; // Totala poängen för sessionen
        private readonly bool oneCardPractice; // Enkortsläge (eller inte)

        public FlashCardForm(List<ExerciseData> exerciseSet, bool oneCardRadioButton)
        {
            oneCardPractice = oneCardRadioButton; // on oneCardRadioButton är true så körs oneCardPractice annars så körs fiveCardPractice
            InitializeComponent(); // Initialisera komponenter
            _exerciseSet = exerciseSet; // Sätt inkomna övningar till aktuella övningar, _ i början för att visa att det är en privat variabel

            lastQuestionLabel.Visible = false;// Gör lastQuestionLabel osynlig när setet startar eftersom det då inte finns någon tidigare besvarad fråga
            lastAnswerLabel.Visible = false; // Gör lastAnswerLabel osynlig när setet startar eftersom det då inte finns någon tidigare besvarad fråga

            if (oneCardPractice) // Om enkortsläge är valt
            {

                showAnswerButton.Visible = true; // Visa knappen för att visa svar
                answerButton2.Visible = false; // Gör de andra svarsalternativen osynliga
                answerButton3.Visible = false;
                answerButton4.Visible = false;
                answerButton5.Visible = false;

                // Ändra storlek på answerButton1 och gör den oklickbar
                answerButton1.Height *= 2;
                answerButton1.Enabled = false;
                EnableSelfScoreButtons(false); // Gör self-scoring knapparna oklickbara först innan ett svar visats
            }

            LoadCard(); // Ladda första kortet
        }

        private void EnableSelfScoreButtons(bool enable) // Metod för att göra self-scoring knapparna klickbara eller inte
        {
            failButton.Enabled = enable; // Gör failButton klickbar eller inte beroende på enable
            barelyOKButton.Enabled = enable;
            OKButton.Enabled = enable;
            goodButton.Enabled = enable;
            excellentButton.Enabled = enable;

            failButton.Visible = enable; // Gör failButton synlig eller inte beroende på enable
            barelyOKButton.Visible = enable;
            OKButton.Visible = enable;
            goodButton.Visible = enable;
            excellentButton.Visible = enable;
        }

        private void ShowAnswerButton_Click(object sender, EventArgs e)// Metod för att visa svar
        {
            if (oneCardPractice) // Om enkortsläge är valt
            {
                EnableSelfScoreButtons(true); // Gör self-scoring knapparna klickbara
                showAnswerButton.Visible = false; // Gör showAnswerButton osynlig

                answerButton1.FlatStyle = FlatStyle.Flat; // Ändra stil på answerButton1
                answerButton1.ForeColor = Color.Black; // Ändra färg på texten i answerButton1
                answerButton1.BackColor = Color.White; // Ändra bakgrundsfärgen på answerButton1
                answerButton1.Enabled = false; // Gör answerButton1 oklickbar
                answerButton1.Visible = true; // Gör answerButton1 synlig
            }
        }

        private void FailButton_Click(object sender, EventArgs e) => SelfScoreAndAdvance(0); // Kör SelfScoreAndAdvance med 0 som argument
        private void BarelyOKButton_Click(object sender, EventArgs e) => SelfScoreAndAdvance(0.125); // Kör SelfScoreAndAdvance med 0.125 som argument
        private void OKButton_Click(object sender, EventArgs e) => SelfScoreAndAdvance(0.25); // Kör SelfScoreAndAdvance med 0.25 som argument
        private void GoodButton_Click(object sender, EventArgs e) => SelfScoreAndAdvance(0.5); // Kör SelfScoreAndAdvance med 0.5 som argument
        private void ExcellentButton_Click(object sender, EventArgs e) => SelfScoreAndAdvance(1); // Kör SelfScoreAndAdvance med 1 som argument

        private void SelfScoreAndAdvance(double score) // Metod för att självbedöma och gå vidare
        {
            _totalScore += score;
            if (_currentExercise != null)
            {
                UpdateScore(_currentExercise, score);// Uppdatera poängen för övningen i Excel-filen
            }
            AdvanceIndex(); // Sätt indexräknaren till nästa övning
            LoadCard(); // Ladda nästa kort
        }

        private void LoadCard() // Metod för att ladda kort med aktuellt index
        {
            if (_lastExercise != null) // Om det finns en förra övning
            {
                lastQuestionLabel.Visible = true; // Gör lastQuestionLabel synlig
                lastAnswerLabel.Visible = true; // Gör lastAnswerLabel synlig
                lastQuestionLabel.Text = $"Last card: {_lastExercise.Original}"; // Sätt texten i lastQuestionLabel till förra övningens originaltext
                lastAnswerLabel.Text = $"Translation: {_lastExercise.Translation}"; // Sätt texten i lastAnswerLabel till förra övningens översättning
            }

            if (_currentIndex >= _exerciseSet.Count) // Om indexräknaren är större eller lika med antalet övningar i setet
            {
                Quit(); // Kör Quit-metoden
                return;
            }

            _currentExercise = _exerciseSet[_currentIndex]; // Sätt aktuell övning till övningen på indexräknarens position
            questionLabel.Text = _currentExercise.Original; // Sätt texten i questionLabel till aktuell övningens originaltext
            attributes.Text = $"{_currentExercise.Language} - {_currentExercise.WordClass} - chapter/part: {_currentExercise.Chapter} - {_currentExercise.FileName}"; // Sätt texten i attributes till språk, ordklass, kapitel/del och filnamn för aktuell övning som extra info
            SetCardBackground(_currentExercise.Dirty); // Sätt bakgrundsfärgen på kortet beroende på om övningen är markerad som dirty eller inte. En röd bakgrund betyder att övningen är markerad som dirty.

            if (oneCardPractice) // Om enkortsläge är valt
            {
                answerButton1.Visible = false; // Gör answerButton1 osynlig tills den klickas på
                showAnswerButton.Visible = true; // Gör showAnswerButton synlig
                SetButtonText(answerButton1, _currentExercise.Translation); // Sätt texten i answerButton1 till aktuell övningens översättning
                EnableSelfScoreButtons(false); // Gör self-scoring knapparna oklickbara och osynliga innan ett svar visats
            }
            else
            {
                _remainingAttempts = 4; // Sätt antal försök kvar att svara rätt till 4 som startvärde
                PopulateAnswerOptions(); // Fyll svarsalternativen med övningar i valt set och som är angivna i samma språk som aktuell övningskorts språk
                EnableAnswerButtons(true); // Gör svarsalternativen klickbara
            }

            UpdateProgress(); // Uppdatera sessionstotalpoäng och progressionbar
        }

        private void PopulateAnswerOptions() // Metod för att fylla svarsalternativen i 5-kortsläge
        {
            var filteredOptions = _exerciseSet // Filtrera övningar i setet
                .Where(e => e != _currentExercise && e.Language == _currentExercise?.Language) // där övningen inte är samma som aktuell övning och där språket är samma som aktuell övning
                .Select(e => e.Translation)
                .Distinct() // Ta bort dubletter
                .ToList();

            var options = new List<string> { _currentExercise?.Translation ?? string.Empty }; // Lägg till det aktuella kortets korrekta översättning i listan
            foreach (var option in filteredOptions) // Lägg till övningar i listan tills det finns 5 alternativ
            {
                if (options.Count >= 5) break;
                options.Add(option);
            }

            while (options.Count < 5) options.Add(""); // Lägg till tomma strängar tills det finns 5 alternativ om det inte finns tillräckligt med övningar
            options = options.OrderBy(option => string.IsNullOrEmpty(option)).ToList(); // Sortera listan så att tomma strängar hamnar sist
            var nonBlankOptions = options.Where(option => !string.IsNullOrEmpty(option)).OrderBy(_ => new Random().Next()).ToList(); // de icke-tomma strängarna sorteras slumpmässigt
            var blankOptions = options.Where(option => string.IsNullOrEmpty(option)).ToList(); //filtrera ut de tomma strängarna
            options = nonBlankOptions.Concat(blankOptions).ToList(); // de tomma strängarna läggs till sist till den slumpmässigt sorterade listan

            Button[] answerButtons = [answerButton1, answerButton2, answerButton3, answerButton4, answerButton5];  // Skapar en array med svarsknappar
            for (int i = 0; i < answerButtons.Length; i++) // Loopa igenom svarsknapparna
            {
                SetButtonText(answerButtons[i], options[i]); // Sätt texten i svarsknapparna 
                answerButtons[i].Visible = !string.IsNullOrEmpty(options[i]); // Gör svarsknapparna synliga om texten inte är tom
            }
        }

        private static void SetButtonText(Button button, string text) // Metod för att sätta texten i en knapp och anpassa storleken
        {
            button.Text = text; // Sätt texten i knappen
            float fontSize = 25; // Startstorlek på texten
            Size textSize; // Storlek på texten tar floatvärdet precis skapat

            using (Graphics g = button.CreateGraphics()) // Skapa en grafik för knappen
            {
                do // Loopa tills texten är mindre än knappen
                {
                    textSize = TextRenderer.MeasureText(g, text, new Font(button.Font.FontFamily, fontSize)); // Mät texten
                    if (textSize.Width > button.Width || textSize.Height > button.Height) // Om texten är större än knappen
                    {
                        fontSize -= 0.5f; // Minska storleken på texten
                    }
                    else
                    {
                        break; // Annars bryt loopen
                    }
                } while (fontSize > 6); // Loopa på så länge det behövs fontstorleken får inte gå under 6 för att inte bli alltför liten
            }

            button.Font = new Font(button.Font.FontFamily, fontSize);
        }

        private void Quit()
        {
            UpdateProgressbar();

            // Kontrollera om det finns några besvarade/passerade kort och visa avslutningsskärmen
            if (_currentIndex > 0 || _exerciseSet.Count == 0)
            {
                ShowEndScreen();
            }
            else // om man inte ens klickat på eller förbi ett enda kort så avslutas sessionen utan att någon statistik visas och sessionen stängs bara
            {
                this.Close(); // Stäng om ingen fråga är besvarad
            }
        }
        private void UpdateProgressbar() // Metod för att uppdatera progressbaren
        {
            progressBar.Maximum = _exerciseSet.Count; // Maxvärde för progressbaren sätts till antalet övningar i setet
            progressBar.Value = Math.Min(_currentIndex, _exerciseSet.Count); // Värdet på progressbaren sätts till minsta värdet av antingen indexräknaren eller antalet övningar i setet
        }

        private void QuitButton_Click(object sender, EventArgs e) => Quit(); // Kör Quit-metoden när knappen klickas på

        private void AnswerButton_Click(object? sender, EventArgs e)
        {
            // Kontrollera om avsändaren av händelsen är en knapp och om det inte är en enstaka kort-övning för säkerhets skull
            if (sender is not Button button || oneCardPractice) return;


            // Kontrollera om det bara finns ett alternativ kvar
            int remainingEnabledButtons = new[] { answerButton1, answerButton2, answerButton3, answerButton4, answerButton5 }
                .Count(b => b.Visible && b.Enabled);

            if (remainingEnabledButtons == 1)
            {
                // Om det bara finns ett alternativ kvar, sätt poängen till noll
                _remainingAttempts = 0; // Indikerar att inga försök återstår
            }


            // Kontrollera om knappens text matchar den aktuella övningens översättning
            if (button.Text == _currentExercise?.Translation)
            {
                // Beräkna poäng baserat på antal återstående försök
                double newScore = _remainingAttempts switch
                {
                    4 => 1,       // Full poäng om det är första försöket
                    3 => 0.5,     // Halv poäng om det är andra försöket
                    2 => 0.25,    // Kvartspoäng om det är tredje försöket
                    1 => 0.125,   // Lägsta poäng om det är fjärde försöket och sista försöket som kan ge poäng 
                    _ => 0        // ingen poäng om det bara finns ett alternativ att klicka på
                };

                // Lägg till den nya poängen till totalpoängen
                _totalScore += newScore;

                // Uppdatera poängen för den aktuella övningen
                UpdateScore(_currentExercise, newScore);

                // Gå vidare till nästa kort
                AdvanceIndex();
                LoadCard(); // Ladda nästa kort
            }
            else // Om svaret är felaktigt
            {
                // Minska antalet återstående försök
                _remainingAttempts--;

                // Om det finns fler försök kvar, inaktivera knappen och spara den aktuella övningen
                if (_remainingAttempts >= 0)
                {
                    button.Enabled = false; // Gör knappen otryckbar
                    _lastExercise = _currentExercise; // Spara den aktuella övningen som senaste övning
                }
            }
        }


        private void EnableAnswerButtons(bool enable) // Metod för att göra svarsalternativen klickbara eller inte
        {
            answerButton1.Enabled = enable;
            answerButton2.Enabled = enable;
            answerButton3.Enabled = enable;
            answerButton4.Enabled = enable;
            answerButton5.Enabled = enable;
        }

        private void SkipButton_Click(object sender, EventArgs e) // Metod för att hoppa över en övning
        {
            AdvanceIndex(); // Sätt indexräknaren till nästa övning och sätt förra övningen till aktuell övning
            LoadCard(); // Ladda nästa kort
        }

        private void AdvanceIndex() // Metod för att sätta indexen för nästa övning
        {
            _lastExercise = _currentExercise; // Sätt nuvarande övningen till förra övningen
            _currentIndex++; // Öka indexräknaren med 1 på nuvarande övnings index
        }

        private void UpdateProgress() // Metod för att uppdatera sessionstotalpoäng och progressionbar
        {
            progress.Text = $"Card {_currentIndex + 1} ({_exerciseSet.Count})"; // Sätt texten i progress till aktuellt kort av antal kort i setet
            scoreLabel.Text = $"Score: {_totalScore} ({SessionResult()}%)"; // Sätt texten i scoreLabel till totalpoängen för sessionen och procenten visar andel av möjliga totalpoäng

            // Uppdatera progressbaren
            UpdateProgressbar();
        }

        private double SessionResult() => _currentIndex != 0 ? Math.Round(_totalScore / _currentIndex * 100, 2) : 0; // Metod för att få sessionens procentuella resultat

        private static void UpdateScore(ExerciseData exercise, double newScore) // Metod för att uppdatera poängen för en övning i Excel-filen
        {
            if (exercise is not null) // Om övningen inte är null...
            {
                exercise.FifthLatestPoint = exercise.FourthLatestPoint; // Sätt femte senaste poängen till tidigare fjärde senaste poängen
                exercise.FourthLatestPoint = exercise.ThirdLatestPoint; // Sätt fjärde senaste poängen till tidigare tredje senaste poängen
                exercise.ThirdLatestPoint = exercise.SecondLatestPoint; // Sätt tredje senaste poängen till tidigare näst senaste poängen
                exercise.SecondLatestPoint = exercise.LatestPoint; // Sätt näst senaste poängen till tidigare senaste poängen
                exercise.LatestPoint = newScore; // Sätt senaste poängen till den nya senaste poängen
                exercise.UpdateDate = DateTime.Now; // Sätt datumet för senaste uppdateringen till nuvarande datum

                SaveExerciseData(exercise); // Spara statiken i excel-filen
            }
        }

        private void ShowEndScreen() // Metod för att visa avslutningsskärmen med sessionens resultat
        {
            // Avaktivera alla kontroller i formuläret
            foreach (Control control in this.Controls)
            {
                control.Enabled = false;
            }

            // Skapa en panel för avslutningsskärmen
            Panel endScreenPanel = new Panel
            {
                Name = "endScreenPanel",
                Size = new Size(this.ClientSize.Width, this.ClientSize.Height), // Storlek på panelen
                BackColor = Color.LightGray, // Bakgrundsfärg på panelen
                BorderStyle = BorderStyle.FixedSingle // Ram runt panelen
            };

            // Titel för avslutningsskärmen
            Label titleLabel = new Label
            {
                Text = "Session Complete!",
                Font = new Font("Arial", 24, FontStyle.Bold),
                Dock = DockStyle.Top, // Fäst titeln i toppen av panelen
                TextAlign = ContentAlignment.MiddleCenter, // Centrera texten
                Padding = new Padding(20), // Lägg till padding runt texten
                Height = 80 // Sätt höjden på titeln
            };

            // Visa slutpoängen
            Label scoreLabel = new Label
            {
                Text = $"Final Score: {_totalScore} ({SessionResult()}%)",
                Font = new Font("Arial", 18, FontStyle.Bold),
                Dock = DockStyle.Top, // Fäst poängen i toppen
                TextAlign = ContentAlignment.MiddleCenter,
                Padding = new Padding(10),
                Height = 60
            };

            // Beräkna stjärnbetyget baserat på procent
            double percentage = SessionResult();
            double starRating = percentage * 5 / 100;
            double roundedStarRating = Math.Floor(starRating * 2) / 2.0; // Runda ner till närmaste halva eller exakt

            string starImagePath = $"Images/{roundedStarRating.ToString("0.0", System.Globalization.CultureInfo.InvariantCulture)}.png"; // Sökväg till stjärnbilden baserat på stjärnbetyget ser till att . och inte , används i strängen...


            // Skapa en PictureBox för att visa stjärnbilden
            PictureBox starsPictureBox = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.Zoom,
                Height = 200,
                Width = 250,
                Dock = DockStyle.Top,
            };

            // Kontrollera om bilden finns och ladda den, annars anropa placeholder
            if (File.Exists(starImagePath)) // Om stjärnbilden finns
            {
                starsPictureBox.Image = Image.FromFile(starImagePath); // Ladda stjärnbilden 
            }
            else // Om stjärnbilden inte finns
            {
                starsPictureBox.Image = GeneratePlaceholderImage(roundedStarRating); // Generera en platsmarkering för stjärnbetyget
            }


            // Skapa en tom panel för extra utrymme mellan stjärnbild och övrig topptext
            Panel spacerPanel = new Panel
            {
                Height = 125, // Sätt höjden för att skapa extra utrymme
                Dock = DockStyle.Top // Fäst den högst upp så att den skjuter ner övriga kontroller
            };


            // Avsluta sessionen-knapp
            Button closeButton = new Button
            {
                Name = "closeButton", // Namn för knappen
                Text = "End Session", // Text för knappen
                Font = new Font("Arial", 16, FontStyle.Bold), // Storlek och stil på texten
                ForeColor = Color.White,
                BackColor = Color.FromArgb(70, 130, 180), // Bakgrundsfärg på knappen
                Size = new Size(200, 50), // Storlek på knappen
                Dock = DockStyle.Bottom, // Fäst knappen i botten av panelen
                Margin = new Padding(20) // Lägg till marginal runt knappen
            };
            closeButton.Click += (s, e) => this.Close(); // Koppla knappen till att stänga sessionen

            // Sätt knappen som acceptknapp
            this.AcceptButton = closeButton;

            // Lägg till delarna i panelen
            endScreenPanel.Controls.Add(scoreLabel);
            endScreenPanel.Controls.Add(starsPictureBox);
            endScreenPanel.Controls.Add(spacerPanel);
            endScreenPanel.Controls.Add(titleLabel);
            endScreenPanel.Controls.Add(closeButton);

            // Lägg till panelen och placera den överst
            this.Controls.Add(endScreenPanel);
            endScreenPanel.BringToFront();
        }

        private Image GeneratePlaceholderImage(double starRating) // Metod för att generera en platsmarkering för stjärnbetyg
        {
            Bitmap placeholder = new Bitmap(250, 50); // Skapa en bitmap med storlek 250x50
            using (Graphics g = Graphics.FromImage(placeholder)) // Skapa en grafik från bitmapen
            {
                g.Clear(Color.LightGray); // Fyll grafiken med en ljusgrå färg
                using (Font font = new Font("Arial", 16)) // Skapa en font med storlek 16
                {
                    g.DrawString($"Rating: {starRating}", font, Brushes.Black, new PointF(10, 10)); // Rita texten "Rating: {starRating}" i grafiken
                }
            }
            return placeholder;
        }


        private void ToggleDirtyStatusButton_Click(object sender, EventArgs e) // Metod för att markera en övning som smutsig eller ren
        {
            // Om det finns en aktuell övning
            if (_currentExercise != null)
            {
                _currentExercise.Dirty = !_currentExercise.Dirty; // Vänd på dirty-statusen
                SaveExerciseData(_currentExercise, _currentExercise.Dirty); // Spara övningen med den nya dirty-statusen
                SetCardBackground(_currentExercise.Dirty); // Sätt bakgrundsfärgen på kortet beroende på om övningen är markerad som dirty eller inte
            }
        }

        private static void SaveExerciseData(ExerciseData exercise, bool? markDirty = null) // Metod för att spara övningar i Excel-filen
        {
            string filePath = Path.Combine("excel", exercise.FileName); // Sätt sökvägen till Excel-filen
            using var package = new ExcelPackage(new FileInfo(filePath)); // Skapa en ny Excel-filsinstans
            var worksheet = package.Workbook.Worksheets[0]; // Hämta första arket i Excel-filen

            int row = exercise.RowNumber; // Ange radnumret för övningen (info finns i aktuell övning)

            if (markDirty.HasValue) // Om dirty-statusen flagga skickats in så gör metoden endast denna specifika uppdatering
            {
                worksheet.Cells[row, 12].Value = markDirty.Value ? "1" : "0";
            }
            else // Annars uppdatera alla värden för övningen
            {

                worksheet.Cells[row, 6].Value = exercise.UpdateDate; // Sätt datumet för senaste uppdateringen
                worksheet.Cells[row, 7].Value = exercise.LatestPoint; // Sätt senaste poängen
                worksheet.Cells[row, 8].Value = exercise.SecondLatestPoint; // Sätt näst senaste poängen
                worksheet.Cells[row, 9].Value = exercise.ThirdLatestPoint; // Sätt tredje senaste poängen
                worksheet.Cells[row, 10].Value = exercise.FourthLatestPoint; // Sätt fjärde senaste poängen
                worksheet.Cells[row, 11].Value = exercise.FifthLatestPoint; // Sätt femte senaste poängen
            }

            package.Save();// Spara Excel-filen
        }

        private void SetCardBackground(bool dirty) // Metod för att sätta bakgrundsfärgen på kortet beroende på om övningen är markerad som dirty eller inte
        {
            this.BackColor = dirty ? Color.LightCoral : DefaultBackColor; // Om övningen är markerad som dirty sätt bakgrundsfärgen till ljus korall annars sätt bakgrundsfärgen till standardfärgen
        }
    }
}
