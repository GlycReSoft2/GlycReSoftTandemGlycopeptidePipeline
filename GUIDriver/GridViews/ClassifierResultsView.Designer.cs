namespace GlycReSoft.MS2GUIDriver.GridViews
{
    partial class ClassifierResultsView
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
            this.HideLowScoringHitsCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // SaveModelButton
            // 
            this.SaveModelButton.Size = new System.Drawing.Size(81, 23);
            this.SaveModelButton.Text = "Save Results";
            // 
            // HideLowScoringHitsCheckBox
            // 
            this.HideLowScoringHitsCheckBox.AutoSize = true;
            this.HideLowScoringHitsCheckBox.Location = new System.Drawing.Point(133, 517);
            this.HideLowScoringHitsCheckBox.Name = "HideLowScoringHitsCheckBox";
            this.HideLowScoringHitsCheckBox.Size = new System.Drawing.Size(131, 17);
            this.HideLowScoringHitsCheckBox.TabIndex = 2;
            this.HideLowScoringHitsCheckBox.Text = "Hide Low Scoring Hits";
            this.HideLowScoringHitsCheckBox.UseVisualStyleBackColor = true;
            this.HideLowScoringHitsCheckBox.CheckedChanged += new System.EventHandler(this.HideLowScoringHitsCheckBox_CheckedChanged);
            // 
            // ClassifierResultsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(1236, 562);
            this.Controls.Add(this.HideLowScoringHitsCheckBox);
            this.Name = "ClassifierResultsView";
            this.Text = "ClassifierResultsView";
            this.Load += new System.EventHandler(this.FormLoadHandle);
            this.Controls.SetChildIndex(this.SaveModelButton, 0);
            this.Controls.SetChildIndex(this.HideLowScoringHitsCheckBox, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }



        #endregion

        private System.Windows.Forms.CheckBox HideLowScoringHitsCheckBox;

    }
}