namespace VocabWizardWinForms
{
    partial class FlashCardForm
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
            components = new System.ComponentModel.Container();
            attributes = new Label();
            questionLabel = new Label();
            answerButton1 = new Button();
            answerButton2 = new Button();
            answerButton3 = new Button();
            answerButton4 = new Button();
            answerButton5 = new Button();
            skipButton = new Button();
            progress = new Label();
            lastQuestionLabel = new Label();
            lastAnswerLabel = new Label();
            scoreLabel = new Label();
            quitButton = new Button();
            toggleDirtyStatusButton = new Button();
            showAnswerButton = new Button();
            failButton = new Button();
            barelyOKButton = new Button();
            OKButton = new Button();
            goodButton = new Button();
            excellentButton = new Button();
            progressBar = new ProgressBar();
            vScrollBar1 = new VScrollBar();
            toolTipDirtyStatus = new ToolTip(components);
            toolTipProgressBar = new ToolTip(components);
            SuspendLayout();
            // 
            // attributes
            // 
            attributes.AutoSize = true;
            attributes.Location = new Point(313, 17);
            attributes.Name = "attributes";
            attributes.Size = new Size(72, 20);
            attributes.TabIndex = 0;
            attributes.Text = "attributes";
            // 
            // questionLabel
            // 
            questionLabel.Font = new Font("Segoe UI", 25.8000011F, FontStyle.Regular, GraphicsUnit.Point, 0);
            questionLabel.Location = new Point(33, 71);
            questionLabel.Name = "questionLabel";
            questionLabel.Size = new Size(1000, 120);
            questionLabel.TabIndex = 4;
            questionLabel.Text = "questionLabel";
            questionLabel.TextAlign = ContentAlignment.TopCenter;
            // 
            // answerButton1
            // 
            answerButton1.Font = new Font("Segoe UI", 25.8000011F);
            answerButton1.Location = new Point(35, 201);
            answerButton1.MinimumSize = new Size(1000, 120);
            answerButton1.Name = "answerButton1";
            answerButton1.Size = new Size(1000, 120);
            answerButton1.TabIndex = 5;
            answerButton1.UseVisualStyleBackColor = true;
            answerButton1.Click += AnswerButton_Click;
            // 
            // answerButton2
            // 
            answerButton2.Font = new Font("Segoe UI", 25.8000011F);
            answerButton2.Location = new Point(35, 346);
            answerButton2.MinimumSize = new Size(1000, 120);
            answerButton2.Name = "answerButton2";
            answerButton2.Size = new Size(1000, 120);
            answerButton2.TabIndex = 6;
            answerButton2.UseVisualStyleBackColor = true;
            answerButton2.Click += AnswerButton_Click;
            // 
            // answerButton3
            // 
            answerButton3.Font = new Font("Segoe UI", 25.8000011F);
            answerButton3.Location = new Point(35, 491);
            answerButton3.MinimumSize = new Size(1000, 120);
            answerButton3.Name = "answerButton3";
            answerButton3.Size = new Size(1000, 120);
            answerButton3.TabIndex = 7;
            answerButton3.UseVisualStyleBackColor = true;
            answerButton3.Click += AnswerButton_Click;
            // 
            // answerButton4
            // 
            answerButton4.Font = new Font("Segoe UI", 25.8000011F);
            answerButton4.Location = new Point(35, 637);
            answerButton4.MinimumSize = new Size(1000, 120);
            answerButton4.Name = "answerButton4";
            answerButton4.Size = new Size(1000, 120);
            answerButton4.TabIndex = 8;
            answerButton4.UseVisualStyleBackColor = true;
            answerButton4.Click += AnswerButton_Click;
            // 
            // answerButton5
            // 
            answerButton5.Font = new Font("Segoe UI", 25.8000011F);
            answerButton5.Location = new Point(35, 785);
            answerButton5.MinimumSize = new Size(1000, 120);
            answerButton5.Name = "answerButton5";
            answerButton5.Size = new Size(1000, 120);
            answerButton5.TabIndex = 9;
            answerButton5.UseVisualStyleBackColor = true;
            answerButton5.Click += AnswerButton_Click;
            // 
            // skipButton
            // 
            skipButton.Location = new Point(984, 949);
            skipButton.Name = "skipButton";
            skipButton.Size = new Size(94, 29);
            skipButton.TabIndex = 10;
            skipButton.Text = "Pass";
            skipButton.UseVisualStyleBackColor = true;
            skipButton.Click += SkipButton_Click;
            // 
            // progress
            // 
            progress.AutoSize = true;
            progress.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            progress.Location = new Point(43, 9);
            progress.Name = "progress";
            progress.Size = new Size(93, 28);
            progress.TabIndex = 11;
            progress.Text = "progress";
            progress.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lastQuestionLabel
            // 
            lastQuestionLabel.AutoSize = true;
            lastQuestionLabel.Location = new Point(5, 938);
            lastQuestionLabel.Name = "lastQuestionLabel";
            lastQuestionLabel.Size = new Size(15, 20);
            lastQuestionLabel.TabIndex = 12;
            lastQuestionLabel.Text = "-";
            lastQuestionLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lastAnswerLabel
            // 
            lastAnswerLabel.AutoSize = true;
            lastAnswerLabel.Location = new Point(5, 958);
            lastAnswerLabel.Name = "lastAnswerLabel";
            lastAnswerLabel.Size = new Size(15, 20);
            lastAnswerLabel.TabIndex = 13;
            lastAnswerLabel.Text = "-";
            lastAnswerLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // scoreLabel
            // 
            scoreLabel.AutoSize = true;
            scoreLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            scoreLabel.Location = new Point(41, 41);
            scoreLabel.Name = "scoreLabel";
            scoreLabel.Size = new Size(112, 28);
            scoreLabel.TabIndex = 14;
            scoreLabel.Text = "scoreLabel";
            scoreLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // quitButton
            // 
            quitButton.Location = new Point(984, 8);
            quitButton.Name = "quitButton";
            quitButton.Size = new Size(94, 29);
            quitButton.TabIndex = 15;
            quitButton.Text = "End";
            quitButton.UseVisualStyleBackColor = true;
            quitButton.Click += QuitButton_Click;
            // 
            // toggleDirtyStatusButton
            // 
            toggleDirtyStatusButton.Location = new Point(931, 53);
            toggleDirtyStatusButton.Name = "toggleDirtyStatusButton";
            toggleDirtyStatusButton.Size = new Size(147, 29);
            toggleDirtyStatusButton.TabIndex = 17;
            toggleDirtyStatusButton.Text = "Toggle dirty status";
            toolTipDirtyStatus.SetToolTip(toggleDirtyStatusButton, "Mark this card as 'dirty' for later review");
            toggleDirtyStatusButton.UseVisualStyleBackColor = true;
            toggleDirtyStatusButton.Click += ToggleDirtyStatusButton_Click;
            // 
            // showAnswerButton
            // 
            showAnswerButton.Font = new Font("Segoe UI", 18F);
            showAnswerButton.Location = new Point(422, 572);
            showAnswerButton.Name = "showAnswerButton";
            showAnswerButton.Size = new Size(235, 59);
            showAnswerButton.TabIndex = 18;
            showAnswerButton.Text = "Show answer";
            showAnswerButton.UseVisualStyleBackColor = true;
            showAnswerButton.Visible = false;
            showAnswerButton.Click += ShowAnswerButton_Click;
            // 
            // failButton
            // 
            failButton.Font = new Font("Segoe UI", 18F);
            failButton.Location = new Point(54, 720);
            failButton.Name = "failButton";
            failButton.Size = new Size(172, 52);
            failButton.TabIndex = 19;
            failButton.Text = "Fail";
            failButton.UseVisualStyleBackColor = true;
            failButton.Visible = false;
            failButton.Click += FailButton_Click;
            // 
            // barelyOKButton
            // 
            barelyOKButton.Font = new Font("Segoe UI", 18F);
            barelyOKButton.Location = new Point(251, 720);
            barelyOKButton.Name = "barelyOKButton";
            barelyOKButton.Size = new Size(172, 52);
            barelyOKButton.TabIndex = 20;
            barelyOKButton.Text = "Barely OK";
            barelyOKButton.UseVisualStyleBackColor = true;
            barelyOKButton.Visible = false;
            barelyOKButton.Click += BarelyOKButton_Click;
            // 
            // OKButton
            // 
            OKButton.Font = new Font("Segoe UI", 18F);
            OKButton.Location = new Point(448, 720);
            OKButton.Name = "OKButton";
            OKButton.Size = new Size(172, 52);
            OKButton.TabIndex = 21;
            OKButton.Text = "OK";
            OKButton.UseVisualStyleBackColor = true;
            OKButton.Visible = false;
            OKButton.Click += OKButton_Click;
            // 
            // goodButton
            // 
            goodButton.Font = new Font("Segoe UI", 18F);
            goodButton.Location = new Point(645, 720);
            goodButton.Name = "goodButton";
            goodButton.Size = new Size(172, 52);
            goodButton.TabIndex = 22;
            goodButton.Text = "Good";
            goodButton.UseVisualStyleBackColor = true;
            goodButton.Visible = false;
            goodButton.Click += GoodButton_Click;
            // 
            // excellentButton
            // 
            excellentButton.Font = new Font("Segoe UI", 18F);
            excellentButton.Location = new Point(842, 720);
            excellentButton.Name = "excellentButton";
            excellentButton.Size = new Size(172, 52);
            excellentButton.TabIndex = 23;
            excellentButton.Text = "Excellent";
            excellentButton.UseVisualStyleBackColor = true;
            excellentButton.Visible = false;
            excellentButton.Click += ExcellentButton_Click;
            // 
            // progressBar
            // 
            progressBar.Location = new Point(391, 43);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(275, 29);
            progressBar.TabIndex = 24;
            toolTipProgressBar.SetToolTip(progressBar, "Bar showing practice set progress");
            // 
            // vScrollBar1
            // 
            vScrollBar1.Location = new Point(715, 51);
            vScrollBar1.Name = "vScrollBar1";
            vScrollBar1.Size = new Size(8, 8);
            vScrollBar1.TabIndex = 25;
            // 
            // FlashCardForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1089, 987);
            Controls.Add(vScrollBar1);
            Controls.Add(progressBar);
            Controls.Add(excellentButton);
            Controls.Add(goodButton);
            Controls.Add(OKButton);
            Controls.Add(barelyOKButton);
            Controls.Add(failButton);
            Controls.Add(showAnswerButton);
            Controls.Add(toggleDirtyStatusButton);
            Controls.Add(quitButton);
            Controls.Add(scoreLabel);
            Controls.Add(lastAnswerLabel);
            Controls.Add(lastQuestionLabel);
            Controls.Add(progress);
            Controls.Add(skipButton);
            Controls.Add(answerButton5);
            Controls.Add(answerButton4);
            Controls.Add(answerButton3);
            Controls.Add(answerButton2);
            Controls.Add(answerButton1);
            Controls.Add(questionLabel);
            Controls.Add(attributes);
            Name = "FlashCardForm";
            Text = "FlashCardForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion


        private Label questionLabel;
        private Button answerButton1;
        private Button answerButton2;
        private Button answerButton3;
        private Button answerButton4;
        private Button answerButton5;
        private Label progress;
        private Button skipButton;
        private Label lastQuestionLabel;
        private Label lastAnswerLabel;
        private Label scoreLabel;
        private Label attributes;
        private Button quitButton;
        private Button toggleDirtyStatusButton;
        private Button showAnswerButton;
        private Button failButton;
        private Button barelyOKButton;
        private Button OKButton;
        private Button goodButton;
        private Button excellentButton;
        private ProgressBar progressBar;
        private VScrollBar vScrollBar1;
        private ToolTip toolTipDirtyStatus;
        private ToolTip toolTipProgressBar;
    }
}