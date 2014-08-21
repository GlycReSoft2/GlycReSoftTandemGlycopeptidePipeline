namespace GlycReSoft.MS2GUIDriver.GridViews
{
    partial class ModelLabelView
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
            this.MS2MatchDataGridView = new System.Windows.Forms.DataGridView();
            this.SaveModelButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.MS2MatchDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // MS2MatchDataGridView
            // 
            this.MS2MatchDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MS2MatchDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.MS2MatchDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.MS2MatchDataGridView.Location = new System.Drawing.Point(7, 7);
            this.MS2MatchDataGridView.Name = "MS2MatchDataGridView";
            this.MS2MatchDataGridView.Size = new System.Drawing.Size(1217, 489);
            this.MS2MatchDataGridView.TabIndex = 0;
            // 
            // SaveModelButton
            // 
            this.SaveModelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.SaveModelButton.Location = new System.Drawing.Point(12, 513);
            this.SaveModelButton.Name = "SaveModelButton";
            this.SaveModelButton.Size = new System.Drawing.Size(75, 23);
            this.SaveModelButton.TabIndex = 1;
            this.SaveModelButton.Text = "Save Model";
            this.SaveModelButton.UseVisualStyleBackColor = true;
            this.SaveModelButton.Click += new System.EventHandler(this.SaveModelButton_Click);
            // 
            // ModelLabelView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1236, 562);
            this.Controls.Add(this.SaveModelButton);
            this.Controls.Add(this.MS2MatchDataGridView);
            this.Name = "ModelLabelView";
            this.Text = "ModelLabelView";
            this.Load += new System.EventHandler(this.FormLoadHandle);
            ((System.ComponentModel.ISupportInitialize)(this.MS2MatchDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.DataGridView MS2MatchDataGridView;
        protected System.Windows.Forms.Button SaveModelButton;
    }
}