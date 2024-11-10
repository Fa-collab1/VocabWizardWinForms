namespace VocabWizardWinForms
{
    partial class ReadmeForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;


        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            richTextBox1 = new RichTextBox();
            webViewBox = new Microsoft.Web.WebView2.WinForms.WebView2();
            ((System.ComponentModel.ISupportInitialize)webViewBox).BeginInit();
            SuspendLayout();
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(-5, 2);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(799, 964);
            richTextBox1.TabIndex = 0;
            richTextBox1.Text = "";
            // 
            // webViewBox
            // 
            webViewBox.AllowExternalDrop = true;
            webViewBox.CreationProperties = null;
            webViewBox.DefaultBackgroundColor = Color.White;
            webViewBox.Dock = DockStyle.Fill;
            webViewBox.Location = new Point(0, 0);
            webViewBox.Name = "webViewBox";
            webViewBox.Size = new Size(800, 966);
            webViewBox.TabIndex = 1;
            webViewBox.ZoomFactor = 1D;
            // 
            // ReadmeForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 966);
            Controls.Add(webViewBox);
            Controls.Add(richTextBox1);
            Name = "ReadmeForm";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)webViewBox).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private RichTextBox richTextBox1;
        private Microsoft.Web.WebView2.WinForms.WebView2 webViewBox;
    }
}



