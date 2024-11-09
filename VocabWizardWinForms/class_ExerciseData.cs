namespace VocabWizardWinForms
{
    public class ExerciseData
    {
        // Radnummer för att hitta dataraden i filen vis läsning och skrivning
        public int RowNumber { get; set; }

        // Språk för övningen
        public required string Language { get; set; }

        // Ordklass för övningen
        public required string WordClass { get; set; }

        // Kapitel/del för övningen
        public required string Chapter { get; set; }

        // Filnamn som övningen kommer från
        public required string FileName { get; set; }

        // Översättning av texten
        public required string Translation { get; set; }

        // Ursprunglig text
        public required string Original { get; set; }

        // Datum för senaste uppdateringen
        public DateTime UpdateDate { get; set; }

        // Senaste poängen för övningen
        public double LatestPoint { get; set; }

        // Näst senaste poängen
        public double SecondLatestPoint { get; set; }

        // Tredje senaste poängen
        public double ThirdLatestPoint { get; set; }

        // Fjärde senaste poängen
        public double FourthLatestPoint { get; set; }

        // Femte senaste poängen
        public double FifthLatestPoint { get; set; }

        // Beräknar genomsnittspoängen och returnerar det som en procentandel i textform
        public string AveragePoint
        {
            get
            {
                // Beräknar genomsnittet av de fem poängen 
                double sum = LatestPoint + SecondLatestPoint + ThirdLatestPoint + FourthLatestPoint + FifthLatestPoint;
                double average = sum / 5;

                // Returnerar genomsnittet som en formaterad procent i textform
                if (average >= 1)
                    return "100,00 %";
                else if (average >= 0.875)
                    return " 87,50 %";
                else if (average >= 0.75)
                    return " 75,00 %";
                else if (average >= 0.625)
                    return " 62,50 %";
                else if (average >= 0.5)
                    return " 50,00 %";
                else if (average >= 0.375)
                    return " 37,50 %";
                else if (average >= 0.25)
                    return " 25,00 %";
                else if (average >= 0.125)
                    return " 12,50 %";
                else
                    return "  0,00 %";
            }
        }

        // Markerar raden som smutsig eller ren (smutsig ska man använda om man ser att någonting är fel så att man lätt kan leta upp dessa rader i filen och rätta dem)
        public bool Dirty { get; set; } = false;

        // Returnerar status för "Dirty" som text
        public string DirtyStatus
        {
            get
            {
                if (Dirty == false) return "Clean";
                return "Dirty";
            }
        }
    }
}