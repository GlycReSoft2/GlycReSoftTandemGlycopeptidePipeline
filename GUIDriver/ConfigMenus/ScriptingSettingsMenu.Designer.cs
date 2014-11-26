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
            this.PythonExecutablePathTextBox = new System.Windows.Forms.TextBox();
            this.OkayButton = new System.Windows.Forms.Button();
            this.CancelMenuButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.CheckPythonDependenciesButton = new System.Windows.Forms.Button();
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
            // PythonExecutablePathTextBox
            // 
            this.PythonExecutablePathTextBox.Location = new System.Drawing.Point(12, 29);
            this.PythonExecutablePathTextBox.Name = "PythonExecutablePathTextBox";
            this.PythonExecutablePathTextBox.Size = new System.Drawing.Size(266, 20);
            this.PythonExecutablePathTextBox.TabIndex = 2;
            // 
            // OkayButton
            // 
            this.OkayButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
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
            this.CancelMenuButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
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
            this.label3.Location = new System.Drawing.Point(9, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(234, 65);
            this.label3.TabIndex = 6;
            this.label3.Text = resources.GetString("label3.Text");
            // 
            // CheckPythonDependenciesButton
            // 
            this.CheckPythonDependenciesButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CheckPythonDependenciesButton.Location = new System.Drawing.Point(12, 55);
            this.CheckPythonDependenciesButton.Name = "CheckPythonDependenciesButton";
            this.CheckPythonDependenciesButton.Size = new System.Drawing.Size(156, 23);
            this.CheckPythonDependenciesButton.TabIndex = 7;
            this.CheckPythonDependenciesButton.Text = "Check Python Dependencies";
            this.CheckPythonDependenciesButton.UseVisualStyleBackColor = true;
            this.CheckPythonDependenciesButton.Click += new System.EventHandler(this.CheckPythonDependenciesButton_Click);
            // 
            // ScriptingSettingsMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.CheckPythonDependenciesButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.CancelMenuButton);
            this.Controls.Add(this.OkayButton);
            this.Controls.Add(this.PythonExecutablePathTextBox);
            this.Controls.Add(this.label1);
            this.Name = "ScriptingSettingsMenu";
            this.Text = "ScriptingSettingsMenu";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox PythonExecutablePathTextBox;
        private System.Windows.Forms.Button OkayButton;
        private System.Windows.Forms.Button CancelMenuButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button CheckPythonDependenciesButton;
    }
}