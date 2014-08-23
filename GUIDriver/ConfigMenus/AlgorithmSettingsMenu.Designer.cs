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
            ((System.ComponentModel.ISupportInitialize)(this.MS1MassToleranceNumericInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MS2MassToleranceNumericInput)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MS2MassErrorToleranceLabel
            // 
            this.MS2MassErrorToleranceLabel.AutoSize = true;
            this.MS2MassErrorToleranceLabel.Location = new System.Drawing.Point(85, 65);
            this.MS2MassErrorToleranceLabel.Name = "MS2MassErrorToleranceLabel";
            this.MS2MassErrorToleranceLabel.Size = new System.Drawing.Size(133, 13);
            this.MS2MassErrorToleranceLabel.TabIndex = 1;
            this.MS2MassErrorToleranceLabel.Text = "MS2 Mass Error Tolerance";
            // 
            // MS1MassErrorToleranceLabel
            // 
            this.MS1MassErrorToleranceLabel.AutoSize = true;
            this.MS1MassErrorToleranceLabel.Location = new System.Drawing.Point(85, 26);
            this.MS1MassErrorToleranceLabel.Name = "MS1MassErrorToleranceLabel";
            this.MS1MassErrorToleranceLabel.Size = new System.Drawing.Size(133, 13);
            this.MS1MassErrorToleranceLabel.TabIndex = 3;
            this.MS1MassErrorToleranceLabel.Text = "MS1 Mass Error Tolerance";
            // 
            // CancelMenuButton
            // 
            this.CancelMenuButton.Location = new System.Drawing.Point(181, 195);
            this.CancelMenuButton.Name = "CancelMenuButton";
            this.CancelMenuButton.Size = new System.Drawing.Size(75, 23);
            this.CancelMenuButton.TabIndex = 7;
            this.CancelMenuButton.Text = "Cancel";
            this.CancelMenuButton.UseVisualStyleBackColor = true;
            this.CancelMenuButton.Click += new System.EventHandler(this.CancelMenuButton_Click);
            // 
            // OkayButton
            // 
            this.OkayButton.Location = new System.Drawing.Point(100, 195);
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
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(236, 116);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Mass Matching Options";
            // 
            // AlgorithmSettingsMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(264, 230);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.CancelMenuButton);
            this.Controls.Add(this.OkayButton);
            this.Name = "AlgorithmSettingsMenu";
            this.Text = "Algorithm Scripting";
            ((System.ComponentModel.ISupportInitialize)(this.MS1MassToleranceNumericInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MS2MassToleranceNumericInput)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
    }
}