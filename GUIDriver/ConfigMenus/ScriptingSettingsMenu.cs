using GlycReSoft.TandemMSGlycopeptideGUI.ConfigMenus;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GlycReSoft.TandemGlycopeptidePipeline;


namespace GlycReSoft.TandemMSGlycopeptideGUI.ConfigMenus
{
    public partial class ScriptingSettingsMenu : Form
    {
        public ScriptingSettingsMenu()
        {
            InitializeComponent();
            Console.WriteLine(ConfigurationManager.Scripting);
            this.PythonExecutablePathTextBox.Text = ConfigurationManager.Scripting.PythonInterpreterPath;
            this.RscriptExecutablePathTextBox.Text = ConfigurationManager.Scripting.RscriptInterpreterPath;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OkayButton_Click(object sender, EventArgs e)
        {
            ConfigurationManager.Scripting.PythonInterpreterPath = this.PythonExecutablePathTextBox.Text;
            ConfigurationManager.Scripting.RscriptInterpreterPath = this.RscriptExecutablePathTextBox.Text;
            try
            {
                ConfigurationManager.WriteScriptingSettingsToFile(Application.StartupPath, ConfigurationManager.Scripting);
            }
            catch (SettingFileException)
            {
                MessageBox.Show("Unable to save settings to file. You may be required to re-enter these options again next time the application launches");
            }
            this.Close();
        }

        private void InstallPythonDependenciesButton_Click(object sender, EventArgs e)
        {
            ScriptManager scriptingManager = new ScriptManager(ConfigurationManager.Scripting.PythonInterpreterPath,
                ConfigurationManager.Scripting.RscriptInterpreterPath);
            try
            {
                scriptingManager.VerifyFileSystemTargets();
                scriptingManager.InstallPythonDependencies();
                ConfigurationManager.Scripting.PythonDependenciesInstalled = true;
                ConfigurationManager.WriteScriptingSettingsToFile(Application.StartupPath, ConfigurationManager.Scripting);
                MessageBox.Show(ConfigurationManager.Scripting.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void InstallRDependenciesButton_Click(object sender, EventArgs e)
        {
            ScriptManager scriptingManager = new ScriptManager(ConfigurationManager.Scripting.PythonInterpreterPath,
                ConfigurationManager.Scripting.RscriptInterpreterPath);
            try
            {
                scriptingManager.VerifyFileSystemTargets();
                scriptingManager.InstallRDependencies();
                ConfigurationManager.Scripting.RDependenciesInstalled = true;
                ConfigurationManager.WriteScriptingSettingsToFile(Application.StartupPath, ConfigurationManager.Scripting);
                MessageBox.Show(ConfigurationManager.Scripting.ToString());
            }
            catch (ScriptingException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
