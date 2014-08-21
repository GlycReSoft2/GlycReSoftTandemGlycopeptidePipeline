namespace GlycReSoft.MS2GlycopeptideResultsBrowser
{
    partial class WebResultsViewer
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
            this.BrowserCtrl = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // BrowserCtrl
            // 
            this.BrowserCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BrowserCtrl.Location = new System.Drawing.Point(0, 0);
            this.BrowserCtrl.MinimumSize = new System.Drawing.Size(20, 20);
            this.BrowserCtrl.Name = "BrowserCtrl";
            this.BrowserCtrl.Size = new System.Drawing.Size(1247, 691);
            this.BrowserCtrl.TabIndex = 0;
            // 
            // WebResultsViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1247, 691);
            this.Controls.Add(this.BrowserCtrl);
            this.Name = "WebResultsViewer";
            this.Text = "WebResultsViewer";
            this.Load += new System.EventHandler(this.WebResultsViewer_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser BrowserCtrl;
    }
}

