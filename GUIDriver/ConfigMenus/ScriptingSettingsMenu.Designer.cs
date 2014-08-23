namespace GlycReSoft.TandemMSGlycopeptideGUI.ConfigMenus
{
    partial class ScriptingSettingsMenu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScriptingSettingsMenu));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.PythonExecutablePathTextBox = new System.Windows.Forms.TextBox();
            this.RscriptExecutablePathTextBox = new System.Windows.Forms.TextBox();
            this.OkayButton = new System.Windows.Forms.Button();
            this.CancelMenuButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.InstallPythonDependenciesButton = new System.Windows.Forms.Button();
            this.InstallRDependenciesButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Python Executable Path";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(121, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Rscript Executable Path";
            // 
            // PythonExecutablePathTextBox
            // 
            this.PythonExecutablePathTextBox.Location = new System.Drawing.Point(12, 29);
            this.PythonExecutablePathTextBox.Name = "PythonExecutablePathTextBox";
            this.PythonExecutablePathTextBox.Size = new System.Drawing.Size(266, 20);
            this.PythonExecutablePathTextBox.TabIndex = 2;
            // 
            // RscriptExecutablePathTextBox
            // 
            this.RscriptExecutablePathTextBox.Location = new System.Drawing.Point(12, 98);
            this.RscriptExecutablePathTextBox.Name = "RscriptExecutablePathTextBox";
            this.RscriptExecutablePathTextBox.Size = new System.Drawing.Size(266, 20);
            this.RscriptExecutablePathTextBox.TabIndex = 3;
            // 
            // OkayButton
            // 
            this.OkayButton.Location = new System.Drawing.Point(116, 227);
            this.OkayButton.Name = "OkayButton";
            this.OkayButton.Size = new System.Drawing.Size(75, 23);
            this.OkayButton.TabIndex = 4;
            this.OkayButton.Text = "Okay";
            this.OkayButton.UseVisualStyleBackColor = true;
            this.OkayButton.Click += new System.EventHandler(this.OkayButton_Click);
            // 
            // CancelMenuButton
            // 
            this.CancelMenuButton.Location = new System.Drawing.Point(197, 227);
            this.CancelMenuButton.Name = "CancelMenuButton";
            this.CancelMenuButton.Size = new System.Drawing.Size(75, 23);
            this.CancelMenuButton.TabIndex = 5;
            this.CancelMenuButton.Text = "Cancel";
            this.CancelMenuButton.UseVisualStyleBackColor = true;
            this.CancelMenuButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 152);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(272, 65);
            this.label3.TabIndex = 6;
            this.label3.Text = resources.GetString("label3.Text");
            // 
            // InstallPythonDependenciesButton
            // 
            this.InstallPythonDependenciesButton.Location = new System.Drawing.Point(12, 55);
            this.InstallPythonDependenciesButton.Name = "InstallPythonDependenciesButton";
            this.InstallPythonDependenciesButton.Size = new System.Drawing.Size(156, 23);
            this.InstallPythonDependenciesButton.TabIndex = 7;
            this.InstallPythonDependenciesButton.Text = "Install Python Dependencies";
            this.InstallPythonDependenciesButton.UseVisualStyleBackColor = true;
            this.InstallPythonDependenciesButton.Click += new System.EventHandler(this.InstallPythonDependenciesButton_Click);
            // 
            // InstallRDependenciesButton
            // 
            this.InstallRDependenciesButton.Location = new System.Drawing.Point(12, 124);
            this.InstallRDependenciesButton.Name = "InstallRDependenciesButton";
            this.InstallRDependenciesButton.Size = new System.Drawing.Size(156, 23);
            this.InstallRDependenciesButton.TabIndex = 8;
            this.InstallRDependenciesButton.Text = "Install R Dependencies";
            this.InstallRDependenciesButton.UseVisualStyleBackColor = true;
            this.InstallRDependenciesButton.Click += new System.EventHandler(this.InstallRDependenciesButton_Click);
            // 
            // ScriptingSettingsMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.InstallRDependenciesButton);
            this.Controls.Add(this.InstallPythonDependenciesButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.CancelMenuButton);
            this.Controls.Add(this.OkayButton);
            this.Controls.Add(this.RscriptExecutablePathTextBox);
            this.Controls.Add(this.PythonExecutablePathTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "ScriptingSettingsMenu";
            this.Text = "ScriptingSettingsMenu";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox PythonExecutablePathTextBox;
        private System.Windows.Forms.TextBox RscriptExecutablePathTextBox;
        private System.Windows.Forms.Button OkayButton;
        private System.Windows.Forms.Button CancelMenuButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button InstallPythonDependenciesButton;
        private System.Windows.Forms.Button InstallRDependenciesButton;
    }
}