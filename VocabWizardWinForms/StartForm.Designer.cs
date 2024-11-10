namespace VocabWizardWinForms
{
    partial class StartForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            languageBox = new GroupBox();
            label1 = new Label();
            label2 = new Label();
            wordClassBox = new GroupBox();
            label3 = new Label();
            chapterBox = new GroupBox();
            label4 = new Label();
            fileNameBox = new GroupBox();
            practiceFromEnglish = new Button();
            practiceToEnglish = new Button();
            counter = new Label();
            quitButton = new Button();
            resetFiltersButton = new Button();
            clearScoresButton = new Button();
            AveragePointsLabel = new Label();
            averagePointsBox = new GroupBox();
            Dirtylabel = new Label();
            dirtyStatusBox = new GroupBox();
            oneCardRadionButton = new RadioButton();
            fiveCardRadionButton = new RadioButton();
            ReadMe = new Button();
            toggleRadioButton = new Button();
            SuspendLayout();
            // 
            // languageBox
            // 
            languageBox.Location = new Point(12, 43);
            languageBox.Name = "languageBox";
            languageBox.Size = new Size(250, 613);
            languageBox.TabIndex = 1;
            languageBox.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(18, 22);
            label1.Name = "label1";
            label1.Size = new Size(80, 20);
            label1.TabIndex = 0;
            label1.Text = "Languages";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(287, 22);
            label2.Name = "label2";
            label2.Size = new Size(94, 20);
            label2.TabIndex = 0;
            label2.Text = "Word classes";
            // 
            // wordClassBox
            // 
            wordClassBox.Location = new Point(281, 43);
            wordClassBox.Name = "wordClassBox";
            wordClassBox.Size = new Size(250, 822);
            wordClassBox.TabIndex = 2;
            wordClassBox.TabStop = false;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(552, 22);
            label3.Name = "label3";
            label3.Size = new Size(106, 20);
            label3.TabIndex = 0;
            label3.Text = "Chapters/parts";
            // 
            // chapterBox
            // 
            chapterBox.Location = new Point(546, 43);
            chapterBox.Name = "chapterBox";
            chapterBox.Size = new Size(250, 822);
            chapterBox.TabIndex = 3;
            chapterBox.TabStop = false;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(815, 22);
            label4.Name = "label4";
            label4.Size = new Size(79, 20);
            label4.TabIndex = 0;
            label4.Text = "File names";
            // 
            // fileNameBox
            // 
            fileNameBox.Location = new Point(809, 43);
            fileNameBox.Name = "fileNameBox";
            fileNameBox.Size = new Size(250, 822);
            fileNameBox.TabIndex = 4;
            fileNameBox.TabStop = false;
            // 
            // practiceFromEnglish
            // 
            practiceFromEnglish.Location = new Point(316, 885);
            practiceFromEnglish.Name = "practiceFromEnglish";
            practiceFromEnglish.Size = new Size(192, 29);
            practiceFromEnglish.TabIndex = 9;
            practiceFromEnglish.Text = "Practice (into Language)";
            practiceFromEnglish.UseVisualStyleBackColor = true;
            practiceFromEnglish.Click += PracticeIntoLanguage_Click;
            // 
            // practiceToEnglish
            // 
            practiceToEnglish.Location = new Point(542, 885);
            practiceToEnglish.Name = "practiceToEnglish";
            practiceToEnglish.Size = new Size(192, 29);
            practiceToEnglish.TabIndex = 10;
            practiceToEnglish.Text = "Practice (from Language)";
            practiceToEnglish.UseVisualStyleBackColor = true;
            practiceToEnglish.Click += PracticeFromLanguage_Click;
            // 
            // counter
            // 
            counter.AutoSize = true;
            counter.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            counter.Location = new Point(326, 949);
            counter.MinimumSize = new Size(400, 35);
            counter.Name = "counter";
            counter.Size = new Size(400, 41);
            counter.TabIndex = 10;
            counter.Text = "0 (0)";
            counter.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // quitButton
            // 
            quitButton.Location = new Point(965, 5);
            quitButton.Name = "quitButton";
            quitButton.Size = new Size(94, 37);
            quitButton.TabIndex = 14;
            quitButton.Text = "Quit";
            quitButton.UseVisualStyleBackColor = true;
            quitButton.Click += QuitButton_Click;
            // 
            // resetFiltersButton
            // 
            resetFiltersButton.Location = new Point(14, 986);
            resetFiltersButton.Name = "resetFiltersButton";
            resetFiltersButton.Size = new Size(94, 49);
            resetFiltersButton.TabIndex = 11;
            resetFiltersButton.Text = "Reset filters";
            resetFiltersButton.UseVisualStyleBackColor = true;
            resetFiltersButton.Click += ResetFilters_Click;
            // 
            // clearScoresButton
            // 
            clearScoresButton.Location = new Point(955, 985);
            clearScoresButton.Name = "clearScoresButton";
            clearScoresButton.Size = new Size(105, 49);
            clearScoresButton.TabIndex = 13;
            clearScoresButton.Text = "Clear scores";
            clearScoresButton.UseVisualStyleBackColor = true;
            clearScoresButton.Click += ClearScores_Click;
            // 
            // AveragePointsLabel
            // 
            AveragePointsLabel.AutoSize = true;
            AveragePointsLabel.Location = new Point(16, 685);
            AveragePointsLabel.Name = "AveragePointsLabel";
            AveragePointsLabel.Size = new Size(107, 20);
            AveragePointsLabel.TabIndex = 0;
            AveragePointsLabel.Text = "Average Points";
            // 
            // averagePointsBox
            // 
            averagePointsBox.Location = new Point(12, 707);
            averagePointsBox.Name = "averagePointsBox";
            averagePointsBox.Size = new Size(113, 158);
            averagePointsBox.TabIndex = 5;
            averagePointsBox.TabStop = false;
            // 
            // Dirtylabel
            // 
            Dirtylabel.AutoSize = true;
            Dirtylabel.Location = new Point(148, 685);
            Dirtylabel.Name = "Dirtylabel";
            Dirtylabel.Size = new Size(83, 20);
            Dirtylabel.TabIndex = 0;
            Dirtylabel.Text = "Dirty status";
            // 
            // dirtyStatusBox
            // 
            dirtyStatusBox.Location = new Point(141, 708);
            dirtyStatusBox.Name = "dirtyStatusBox";
            dirtyStatusBox.Size = new Size(121, 158);
            dirtyStatusBox.TabIndex = 6;
            dirtyStatusBox.TabStop = false;
            // 
            // oneCardRadionButton
            // 
            oneCardRadionButton.AutoSize = true;
            oneCardRadionButton.Location = new Point(89, 887);
            oneCardRadionButton.Name = "oneCardRadionButton";
            oneCardRadionButton.Size = new Size(161, 24);
            oneCardRadionButton.TabIndex = 7;
            oneCardRadionButton.TabStop = true;
            oneCardRadionButton.Text = "Single card practice";
            oneCardRadionButton.UseVisualStyleBackColor = true;
            // 
            // fiveCardRadionButton
            // 
            fiveCardRadionButton.AutoSize = true;
            fiveCardRadionButton.Location = new Point(89, 917);
            fiveCardRadionButton.Name = "fiveCardRadionButton";
            fiveCardRadionButton.Size = new Size(146, 24);
            fiveCardRadionButton.TabIndex = 8;
            fiveCardRadionButton.TabStop = true;
            fiveCardRadionButton.Text = "Five card practice";
            fiveCardRadionButton.UseVisualStyleBackColor = true;
            // 
            // ReadMe
            // 
            ReadMe.Location = new Point(128, 986);
            ReadMe.Name = "ReadMe";
            ReadMe.Size = new Size(94, 49);
            ReadMe.TabIndex = 12;
            ReadMe.Text = "ReadMe";
            ReadMe.UseVisualStyleBackColor = true;
            ReadMe.Click += ReadmeButton_Click;
            // 
            // toggleRadioButton
            // 
            toggleRadioButton.FlatAppearance.BorderSize = 0;
            toggleRadioButton.FlatStyle = FlatStyle.Flat;
            toggleRadioButton.ForeColor = SystemColors.ButtonFace;
            toggleRadioButton.Location = new Point(89, 949);
            toggleRadioButton.Name = "toggleRadioButton";
            toggleRadioButton.Size = new Size(13, 20);
            toggleRadioButton.TabIndex = 0;
            toggleRadioButton.TabStop = false;
            toggleRadioButton.UseVisualStyleBackColor = true;
            toggleRadioButton.Click += toggle_RadioButton_Click;
            // 
            // StartForm
            // 
            AcceptButton = toggleRadioButton;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1072, 1046);
            Controls.Add(toggleRadioButton);
            Controls.Add(ReadMe);
            Controls.Add(fiveCardRadionButton);
            Controls.Add(oneCardRadionButton);
            Controls.Add(Dirtylabel);
            Controls.Add(dirtyStatusBox);
            Controls.Add(AveragePointsLabel);
            Controls.Add(averagePointsBox);
            Controls.Add(clearScoresButton);
            Controls.Add(resetFiltersButton);
            Controls.Add(quitButton);
            Controls.Add(counter);
            Controls.Add(practiceToEnglish);
            Controls.Add(practiceFromEnglish);
            Controls.Add(label4);
            Controls.Add(fileNameBox);
            Controls.Add(label3);
            Controls.Add(chapterBox);
            Controls.Add(label2);
            Controls.Add(wordClassBox);
            Controls.Add(label1);
            Controls.Add(languageBox);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "StartForm";
            Text = "VocabWizardWinForms";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox languageBox;
        private Label label1;
        private Label label2;
        private GroupBox wordClassBox;
        private Label label3;
        private GroupBox chapterBox;
        private Label label4;
        private GroupBox fileNameBox;
        private Button practiceFromEnglish;
        private Button practiceToEnglish;
        private Label counter;
        private Button quitButton;
        private Button resetFiltersButton;
        private Button clearScoresButton;
        private Label AveragePointsLabel;
        private GroupBox averagePointsBox;
        private Label Dirtylabel;
        private GroupBox dirtyStatusBox;
        private RadioButton oneCardRadionButton;
        private RadioButton fiveCardRadionButton;
        private Button ReadMe;
        private Button toggleRadioButton;
    }
}
