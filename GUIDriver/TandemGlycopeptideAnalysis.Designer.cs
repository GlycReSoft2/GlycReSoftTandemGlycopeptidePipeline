namespace GlycReSoft.TandemMSGlycopeptideGUI
{
    partial class TandemGlycopeptideAnalysis
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TandemGlycopeptideAnalysis));
            this.MS1MatchFilePathLoadButton = new System.Windows.Forms.Button();
            this.MS2DeconvolutionFilePathLoadButton = new System.Windows.Forms.Button();
            this.GlycosylationSiteFilePathLoadButton = new System.Windows.Forms.Button();
            this.MS1MatchFilePathLabel = new System.Windows.Forms.Label();
            this.MS2DeconvolutionFilePathLabel = new System.Windows.Forms.Label();
            this.GlycosylationSiteFilePathLabel = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.FileSelectionPanel = new System.Windows.Forms.Panel();
            this.ProteinProspectorMSDigestXMLLabel = new System.Windows.Forms.Label();
            this.AddProteinProspectorMSDigestXmlFileButton = new System.Windows.Forms.Button();
            this.SelectModelComboBox = new System.Windows.Forms.ComboBox();
            this.SetParameterFilesLabel = new System.Windows.Forms.Label();
            this.ModelFilePathLabel = new System.Windows.Forms.Label();
            this.CreateModelActionButton = new System.Windows.Forms.Button();
            this.ClassifyMSMSSpectraActionButton = new System.Windows.Forms.Button();
            this.ScriptingSettingsButton = new System.Windows.Forms.Button();
            this.ViewModelButton = new System.Windows.Forms.Button();
            this.AlgorithmSettingsButton = new System.Windows.Forms.Button();
            this.ViewResultsButton = new System.Windows.Forms.Button();
            this.FileSelectionPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // MS1MatchFilePathLoadButton
            // 
            this.MS1MatchFilePathLoadButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.MS1MatchFilePathLoadButton.Location = new System.Drawing.Point(12, 38);
            this.MS1MatchFilePathLoadButton.Name = "MS1MatchFilePathLoadButton";
            this.MS1MatchFilePathLoadButton.Size = new System.Drawing.Size(194, 23);
            this.MS1MatchFilePathLoadButton.TabIndex = 0;
            this.MS1MatchFilePathLoadButton.Text = "Add MS1 GlycReSoft Results File";
            this.MS1MatchFilePathLoadButton.UseVisualStyleBackColor = true;
            this.MS1MatchFilePathLoadButton.Click += new System.EventHandler(this.MS1MatchFilePathLoadButton_Click);
            // 
            // MS2DeconvolutionFilePathLoadButton
            // 
            this.MS2DeconvolutionFilePathLoadButton.Location = new System.Drawing.Point(12, 67);
            this.MS2DeconvolutionFilePathLoadButton.Name = "MS2DeconvolutionFilePathLoadButton";
            this.MS2DeconvolutionFilePathLoadButton.Size = new System.Drawing.Size(194, 23);
            this.MS2DeconvolutionFilePathLoadButton.TabIndex = 1;
            this.MS2DeconvolutionFilePathLoadButton.Text = "Add MS2 Deconvolution File";
            this.MS2DeconvolutionFilePathLoadButton.UseVisualStyleBackColor = true;
            this.MS2DeconvolutionFilePathLoadButton.Click += new System.EventHandler(this.MS2DeconvolutionFilePathLoadButton_Click);
            // 
            // GlycosylationSiteFilePathLoadButton
            // 
            this.GlycosylationSiteFilePathLoadButton.Location = new System.Drawing.Point(12, 96);
            this.GlycosylationSiteFilePathLoadButton.Name = "GlycosylationSiteFilePathLoadButton";
            this.GlycosylationSiteFilePathLoadButton.Size = new System.Drawing.Size(194, 23);
            this.GlycosylationSiteFilePathLoadButton.TabIndex = 2;
            this.GlycosylationSiteFilePathLoadButton.Text = "Add Glycosylation Site File";
            this.GlycosylationSiteFilePathLoadButton.UseVisualStyleBackColor = true;
            this.GlycosylationSiteFilePathLoadButton.Click += new System.EventHandler(this.GlycosylationSiteFilePathLoadButton_Click);
            // 
            // MS1MatchFilePathLabel
            // 
            this.MS1MatchFilePathLabel.AutoSize = true;
            this.MS1MatchFilePathLabel.Location = new System.Drawing.Point(225, 43);
            this.MS1MatchFilePathLabel.Name = "MS1MatchFilePathLabel";
            this.MS1MatchFilePathLabel.Size = new System.Drawing.Size(33, 13);
            this.MS1MatchFilePathLabel.TabIndex = 3;
            this.MS1MatchFilePathLabel.Text = "None";
            // 
            // MS2DeconvolutionFilePathLabel
            // 
            this.MS2DeconvolutionFilePathLabel.AutoSize = true;
            this.MS2DeconvolutionFilePathLabel.Location = new System.Drawing.Point(225, 72);
            this.MS2DeconvolutionFilePathLabel.Name = "MS2DeconvolutionFilePathLabel";
            this.MS2DeconvolutionFilePathLabel.Size = new System.Drawing.Size(33, 13);
            this.MS2DeconvolutionFilePathLabel.TabIndex = 4;
            this.MS2DeconvolutionFilePathLabel.Text = "None";
            // 
            // GlycosylationSiteFilePathLabel
            // 
            this.GlycosylationSiteFilePathLabel.AutoSize = true;
            this.GlycosylationSiteFilePathLabel.Location = new System.Drawing.Point(225, 101);
            this.GlycosylationSiteFilePathLabel.Name = "GlycosylationSiteFilePathLabel";
            this.GlycosylationSiteFilePathLabel.Size = new System.Drawing.Size(33, 13);
            this.GlycosylationSiteFilePathLabel.TabIndex = 5;
            this.GlycosylationSiteFilePathLabel.Text = "None";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Title = "Choose a file";
            // 
            // FileSelectionPanel
            // 
            this.FileSelectionPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FileSelectionPanel.Controls.Add(this.ProteinProspectorMSDigestXMLLabel);
            this.FileSelectionPanel.Controls.Add(this.AddProteinProspectorMSDigestXmlFileButton);
            this.FileSelectionPanel.Controls.Add(this.SelectModelComboBox);
            this.FileSelectionPanel.Controls.Add(this.SetParameterFilesLabel);
            this.FileSelectionPanel.Controls.Add(this.ModelFilePathLabel);
            this.FileSelectionPanel.Controls.Add(this.GlycosylationSiteFilePathLabel);
            this.FileSelectionPanel.Controls.Add(this.MS2DeconvolutionFilePathLabel);
            this.FileSelectionPanel.Controls.Add(this.MS1MatchFilePathLabel);
            this.FileSelectionPanel.Controls.Add(this.GlycosylationSiteFilePathLoadButton);
            this.FileSelectionPanel.Controls.Add(this.MS2DeconvolutionFilePathLoadButton);
            this.FileSelectionPanel.Controls.Add(this.MS1MatchFilePathLoadButton);
            this.FileSelectionPanel.Location = new System.Drawing.Point(1, 4);
            this.FileSelectionPanel.Name = "FileSelectionPanel";
            this.FileSelectionPanel.Size = new System.Drawing.Size(628, 181);
            this.FileSelectionPanel.TabIndex = 6;
            // 
            // ProteinProspectorMSDigestXMLLabel
            // 
            this.ProteinProspectorMSDigestXMLLabel.AutoSize = true;
            this.ProteinProspectorMSDigestXMLLabel.Location = new System.Drawing.Point(225, 130);
            this.ProteinProspectorMSDigestXMLLabel.Name = "ProteinProspectorMSDigestXMLLabel";
            this.ProteinProspectorMSDigestXMLLabel.Size = new System.Drawing.Size(33, 13);
            this.ProteinProspectorMSDigestXMLLabel.TabIndex = 11;
            this.ProteinProspectorMSDigestXMLLabel.Text = "None";
            // 
            // AddProteinProspectorMSDigestXmlFileButton
            // 
            this.AddProteinProspectorMSDigestXmlFileButton.Location = new System.Drawing.Point(12, 125);
            this.AddProteinProspectorMSDigestXmlFileButton.Name = "AddProteinProspectorMSDigestXmlFileButton";
            this.AddProteinProspectorMSDigestXmlFileButton.Size = new System.Drawing.Size(194, 23);
            this.AddProteinProspectorMSDigestXmlFileButton.TabIndex = 10;
            this.AddProteinProspectorMSDigestXmlFileButton.Text = "Add Protein Prospector MS-Digest XML";
            this.AddProteinProspectorMSDigestXmlFileButton.UseVisualStyleBackColor = true;
            this.AddProteinProspectorMSDigestXmlFileButton.Click += new System.EventHandler(this.AddProteinProspectorMSDigestXmlFileButton_Click);
            // 
            // SelectModelComboBox
            // 
            this.SelectModelComboBox.FormattingEnabled = true;
            this.SelectModelComboBox.Location = new System.Drawing.Point(12, 153);
            this.SelectModelComboBox.Name = "SelectModelComboBox";
            this.SelectModelComboBox.Size = new System.Drawing.Size(194, 21);
            this.SelectModelComboBox.TabIndex = 9;
            this.SelectModelComboBox.SelectedIndexChanged += new System.EventHandler(this.ExistingModelsComboBox_SelectedIndexChanged);
            // 
            // SetParameterFilesLabel
            // 
            this.SetParameterFilesLabel.AutoSize = true;
            this.SetParameterFilesLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SetParameterFilesLabel.Location = new System.Drawing.Point(11, 11);
            this.SetParameterFilesLabel.Name = "SetParameterFilesLabel";
            this.SetParameterFilesLabel.Size = new System.Drawing.Size(173, 24);
            this.SetParameterFilesLabel.TabIndex = 8;
            this.SetParameterFilesLabel.Text = "Set Parameter Files";
            // 
            // ModelFilePathLabel
            // 
            this.ModelFilePathLabel.AutoSize = true;
            this.ModelFilePathLabel.Location = new System.Drawing.Point(225, 156);
            this.ModelFilePathLabel.Name = "ModelFilePathLabel";
            this.ModelFilePathLabel.Size = new System.Drawing.Size(33, 13);
            this.ModelFilePathLabel.TabIndex = 7;
            this.ModelFilePathLabel.Text = "None";
            // 
            // CreateModelActionButton
            // 
            this.CreateModelActionButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CreateModelActionButton.Location = new System.Drawing.Point(21, 219);
            this.CreateModelActionButton.Name = "CreateModelActionButton";
            this.CreateModelActionButton.Size = new System.Drawing.Size(95, 35);
            this.CreateModelActionButton.TabIndex = 7;
            this.CreateModelActionButton.Text = "Create Model";
            this.CreateModelActionButton.UseVisualStyleBackColor = true;
            this.CreateModelActionButton.Click += new System.EventHandler(this.CreateModelButton_Click);
            // 
            // ClassifyMSMSSpectraActionButton
            // 
            this.ClassifyMSMSSpectraActionButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ClassifyMSMSSpectraActionButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClassifyMSMSSpectraActionButton.Location = new System.Drawing.Point(21, 260);
            this.ClassifyMSMSSpectraActionButton.Name = "ClassifyMSMSSpectraActionButton";
            this.ClassifyMSMSSpectraActionButton.Size = new System.Drawing.Size(95, 34);
            this.ClassifyMSMSSpectraActionButton.TabIndex = 8;
            this.ClassifyMSMSSpectraActionButton.Text = "Classify with Model";
            this.ClassifyMSMSSpectraActionButton.UseVisualStyleBackColor = true;
            this.ClassifyMSMSSpectraActionButton.Click += new System.EventHandler(this.ClassifyMSMSSpectraActionButton_Click);
            // 
            // ScriptingSettingsButton
            // 
            this.ScriptingSettingsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ScriptingSettingsButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ScriptingSettingsButton.Location = new System.Drawing.Point(520, 219);
            this.ScriptingSettingsButton.Name = "ScriptingSettingsButton";
            this.ScriptingSettingsButton.Size = new System.Drawing.Size(99, 35);
            this.ScriptingSettingsButton.TabIndex = 9;
            this.ScriptingSettingsButton.Text = "Scripting Settings";
            this.ScriptingSettingsButton.UseVisualStyleBackColor = true;
            this.ScriptingSettingsButton.Click += new System.EventHandler(this.ScriptingSettingsButton_Click);
            // 
            // ViewModelButton
            // 
            this.ViewModelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ViewModelButton.Location = new System.Drawing.Point(122, 219);
            this.ViewModelButton.Name = "ViewModelButton";
            this.ViewModelButton.Size = new System.Drawing.Size(94, 35);
            this.ViewModelButton.TabIndex = 10;
            this.ViewModelButton.Text = "View Model";
            this.ViewModelButton.UseVisualStyleBackColor = true;
            this.ViewModelButton.Click += new System.EventHandler(this.ViewModelButton_Click);
            // 
            // AlgorithmSettingsButton
            // 
            this.AlgorithmSettingsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.AlgorithmSettingsButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.AlgorithmSettingsButton.Location = new System.Drawing.Point(520, 260);
            this.AlgorithmSettingsButton.Name = "AlgorithmSettingsButton";
            this.AlgorithmSettingsButton.Size = new System.Drawing.Size(99, 34);
            this.AlgorithmSettingsButton.TabIndex = 11;
            this.AlgorithmSettingsButton.Text = "Algorithm Settings";
            this.AlgorithmSettingsButton.UseVisualStyleBackColor = true;
            this.AlgorithmSettingsButton.Click += new System.EventHandler(this.AlgorithmSettingsButton_Click);
            // 
            // ViewResultsButton
            // 
            this.ViewResultsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ViewResultsButton.Location = new System.Drawing.Point(122, 259);
            this.ViewResultsButton.Name = "ViewResultsButton";
            this.ViewResultsButton.Size = new System.Drawing.Size(94, 35);
            this.ViewResultsButton.TabIndex = 12;
            this.ViewResultsButton.Text = "View Results";
            this.ViewResultsButton.UseVisualStyleBackColor = true;
            this.ViewResultsButton.Click += new System.EventHandler(this.ViewResultsButton_Click);
            // 
            // TandemGlycopeptideAnalysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(631, 469);
            this.Controls.Add(this.ViewResultsButton);
            this.Controls.Add(this.AlgorithmSettingsButton);
            this.Controls.Add(this.ViewModelButton);
            this.Controls.Add(this.ScriptingSettingsButton);
            this.Controls.Add(this.ClassifyMSMSSpectraActionButton);
            this.Controls.Add(this.CreateModelActionButton);
            this.Controls.Add(this.FileSelectionPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TandemGlycopeptideAnalysis";
            this.Text = "Tandem MS Glycopeptide Prediction";
            this.FileSelectionPanel.ResumeLayout(false);
            this.FileSelectionPanel.PerformLayout();
            this.ResumeLayout(false);

        }
        
        #endregion

        private System.Windows.Forms.Button MS1MatchFilePathLoadButton;
        private System.Windows.Forms.Button MS2DeconvolutionFilePathLoadButton;
        private System.Windows.Forms.Button GlycosylationSiteFilePathLoadButton;
        private System.Windows.Forms.Label MS1MatchFilePathLabel;
        private System.Windows.Forms.Label MS2DeconvolutionFilePathLabel;
        private System.Windows.Forms.Label GlycosylationSiteFilePathLabel;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Panel FileSelectionPanel;
        private System.Windows.Forms.Label ModelFilePathLabel;
        private System.Windows.Forms.Button CreateModelActionButton;
        private System.Windows.Forms.Label SetParameterFilesLabel;
        private System.Windows.Forms.Button ClassifyMSMSSpectraActionButton;
        private System.Windows.Forms.Button ScriptingSettingsButton;
        private System.Windows.Forms.Button ViewModelButton;
        private System.Windows.Forms.Button AlgorithmSettingsButton;
        private System.Windows.Forms.ComboBox SelectModelComboBox;
        private System.Windows.Forms.Button ViewResultsButton;
        private System.Windows.Forms.Label ProteinProspectorMSDigestXMLLabel;
        private System.Windows.Forms.Button AddProteinProspectorMSDigestXmlFileButton;
    }
}

