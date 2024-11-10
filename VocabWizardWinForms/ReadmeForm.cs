using Markdig;

namespace VocabWizardWinForms
{
    public partial class ReadmeForm : Form
    {
        public ReadmeForm()
        {
            InitializeComponent();
            LoadReadmeContent();
        }

        private async void LoadReadmeContent()
        {
            try
            {
                // Säkerställ att CoreWebView2 är laddad genom att anropa EnsureCoreWebView2Async
                await webViewBox.EnsureCoreWebView2Async();

                string readmePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "README.md"); // sökväg till README.md
                if (File.Exists(readmePath)) // Kontrollera om README.md finns
                {
                    string markdown = File.ReadAllText(readmePath);
                    string html = Markdown.ToHtml(markdown); // Säkerställ att du har Markdig installerat
                    webViewBox.NavigateToString(html);
                }
                else
                {
                    MessageBox.Show("Readme-filen hittades inte.", "Fel", MessageBoxButtons.OK, MessageBoxIcon.Warning); // Visa varning om README.md inte finns
                }

            }
            catch (Exception ex) // Fånga eventuella fel
            {
                MessageBox.Show($"An error occurred while loading the readme: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); // Visa felmeddelande om något går fel
            }
        }

    }
}




