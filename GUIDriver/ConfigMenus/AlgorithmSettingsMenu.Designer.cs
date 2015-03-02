namespace GlycReSoft.TandemMSGlycopeptideGUI.ConfigMenus
{
    partial class AlgorithmSettingsMenu
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
            this.MS2MassErrorToleranceLabel = new System.Windows.Forms.Label();
            this.MS1MassErrorToleranceLabel = new System.Windows.Forms.Label();
            this.CancelMenuButton = new System.Windows.Forms.Button();
            this.OkayButton = new System.Windows.Forms.Button();
            this.MS1MassToleranceNumericInput = new System.Windows.Forms.NumericUpDown();
            this.MS2MassToleranceNumericInput = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.NumProcessesNumericInput = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.FDROptionsBox = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.NumDecoysNumericInput = new System.Windows.Forms.NumericUpDown();
            this.OnlyRandomDecoysCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.MS1MassToleranceNumericInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MS2MassToleranceNumericInput)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumProcessesNumericInput)).BeginInit();
            this.FDROptionsBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumDecoysNumericInput)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // MS2MassErrorToleranceLabel
            // 
            this.MS2MassErrorToleranceLabel.AutoSize = true;
            this.MS2MassErrorToleranceLabel.Location = new System.Drawing.Point(37, 65);
            this.MS2MassErrorToleranceLabel.Name = "MS2MassErrorToleranceLabel";
            this.MS2MassErrorToleranceLabel.Size = new System.Drawing.Size(133, 13);
            this.MS2MassErrorToleranceLabel.TabIndex = 1;
            this.MS2MassErrorToleranceLabel.Text = "MS2 Mass Error Tolerance";
            // 
            // MS1MassErrorToleranceLabel
            // 
            this.MS1MassErrorToleranceLabel.AutoSize = true;
            this.MS1MassErrorToleranceLabel.Location = new System.Drawing.Point(37, 26);
            this.MS1MassErrorToleranceLabel.Name = "MS1MassErrorToleranceLabel";
            this.MS1MassErrorToleranceLabel.Size = new System.Drawing.Size(133, 13);
            this.MS1MassErrorToleranceLabel.TabIndex = 3;
            this.MS1MassErrorToleranceLabel.Text = "MS1 Mass Error Tolerance";
            // 
            // CancelMenuButton
            // 
            this.CancelMenuButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelMenuButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CancelMenuButton.Location = new System.Drawing.Point(184, 294);
            this.CancelMenuButton.Name = "CancelMenuButton";
            this.CancelMenuButton.Size = new System.Drawing.Size(75, 23);
            this.CancelMenuButton.TabIndex = 7;
            this.CancelMenuButton.Text = "Cancel";
            this.CancelMenuButton.UseVisualStyleBackColor = true;
            this.CancelMenuButton.Click += new System.EventHandler(this.CancelMenuButton_Click);
            // 
            // OkayButton
            // 
            this.OkayButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OkayButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OkayButton.Location = new System.Drawing.Point(103, 294);
            this.OkayButton.Name = "OkayButton";
            this.OkayButton.Size = new System.Drawing.Size(75, 23);
            this.OkayButton.TabIndex = 6;
            this.OkayButton.Text = "Okay";
            this.OkayButton.UseVisualStyleBackColor = true;
            this.OkayButton.Click += new System.EventHandler(this.OkayButton_Click);
            // 
            // MS1MassToleranceNumericInput
            // 
            this.MS1MassToleranceNumericInput.DecimalPlaces = 3;
            this.MS1MassToleranceNumericInput.Location = new System.Drawing.Point(40, 42);
            this.MS1MassToleranceNumericInput.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.MS1MassToleranceNumericInput.Name = "MS1MassToleranceNumericInput";
            this.MS1MassToleranceNumericInput.Size = new System.Drawing.Size(120, 20);
            this.MS1MassToleranceNumericInput.TabIndex = 8;
            // 
            // MS2MassToleranceNumericInput
            // 
            this.MS2MassToleranceNumericInput.DecimalPlaces = 3;
            this.MS2MassToleranceNumericInput.Location = new System.Drawing.Point(40, 81);
            this.MS2MassToleranceNumericInput.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.MS2MassToleranceNumericInput.Name = "MS2MassToleranceNumericInput";
            this.MS2MassToleranceNumericInput.Size = new System.Drawing.Size(120, 20);
            this.MS2MassToleranceNumericInput.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(163, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "PPM Error";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(163, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "PPM Error";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.MS2MassToleranceNumericInput);
            this.groupBox1.Controls.Add(this.MS1MassToleranceNumericInput);
            this.groupBox1.Controls.Add(this.MS1MassErrorToleranceLabel);
            this.groupBox1.Controls.Add(this.MS2MassErrorToleranceLabel);
            this.groupBox1.Location = new System.Drawing.Point(12, 61);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(236, 116);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Mass Matching Options";
            // 
            // NumProcessesNumericInput
            // 
            this.NumProcessesNumericInput.Location = new System.Drawing.Point(29, 32);
            this.NumProcessesNumericInput.Name = "NumProcessesNumericInput";
            this.NumProcessesNumericInput.Size = new System.Drawing.Size(120, 20);
            this.NumProcessesNumericInput.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(140, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Number of Processes to use";
            // 
            // FDROptionsBox
            // 
            this.FDROptionsBox.Controls.Add(this.OnlyRandomDecoysCheckBox);
            this.FDROptionsBox.Controls.Add(this.label4);
            this.FDROptionsBox.Controls.Add(this.NumDecoysNumericInput);
            this.FDROptionsBox.Location = new System.Drawing.Point(12, 179);
            this.FDROptionsBox.Name = "FDROptionsBox";
            this.FDROptionsBox.Size = new System.Drawing.Size(236, 100);
            this.FDROptionsBox.TabIndex = 14;
            this.FDROptionsBox.TabStop = false;
            this.FDROptionsBox.Text = "False Discovery Rate Estimation Options";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(37, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(163, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Decoy Sequences per Prediction";
            // 
            // NumDecoysNumericInput
            // 
            this.NumDecoysNumericInput.Location = new System.Drawing.Point(40, 40);
            this.NumDecoysNumericInput.Name = "NumDecoysNumericInput";
            this.NumDecoysNumericInput.Size = new System.Drawing.Size(120, 20);
            this.NumDecoysNumericInput.TabIndex = 0;
            // 
            // OnlyRandomDecoysCheckBox
            // 
            this.OnlyRandomDecoysCheckBox.AutoSize = true;
            this.OnlyRandomDecoysCheckBox.Location = new System.Drawing.Point(37, 64);
            this.OnlyRandomDecoysCheckBox.Name = "OnlyRandomDecoysCheckBox";
            this.OnlyRandomDecoysCheckBox.Size = new System.Drawing.Size(195, 30);
            this.OnlyRandomDecoysCheckBox.TabIndex = 16;
            this.OnlyRandomDecoysCheckBox.Text = "Use only random sequence decoys,\r\ndo not shuffle predictions.";
            this.OnlyRandomDecoysCheckBox.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.NumProcessesNumericInput);
            this.groupBox2.Location = new System.Drawing.Point(12, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(231, 59);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "General Settings";
            // 
            // AlgorithmSettingsMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(267, 329);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.FDROptionsBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.CancelMenuButton);
            this.Controls.Add(this.OkayButton);
            this.Name = "AlgorithmSettingsMenu";
            this.Text = "Algorithm Scripting";
            ((System.ComponentModel.ISupportInitialize)(this.MS1MassToleranceNumericInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MS2MassToleranceNumericInput)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumProcessesNumericInput)).EndInit();
            this.FDROptionsBox.ResumeLayout(false);
            this.FDROptionsBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumDecoysNumericInput)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label MS2MassErrorToleranceLabel;
        private System.Windows.Forms.Label MS1MassErrorToleranceLabel;
        private System.Windows.Forms.Button CancelMenuButton;
        private System.Windows.Forms.Button OkayButton;
        private System.Windows.Forms.NumericUpDown MS1MassToleranceNumericInput;
        private System.Windows.Forms.NumericUpDown MS2MassToleranceNumericInput;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown NumProcessesNumericInput;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox FDROptionsBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown NumDecoysNumericInput;
        private System.Windows.Forms.CheckBox OnlyRandomDecoysCheckBox;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}