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
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OkayButton_Click(object sender, EventArgs e)
        {
            ConfigurationManager.Scripting.PythonInterpreterPath = this.PythonExecutablePathTextBox.Text;
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

        private void CheckPythonDependenciesButton_Click(object sender, EventArgs e)
        {
            ScriptManager scriptingManager = new ScriptManager(ConfigurationManager.Scripting.PythonInterpreterPath);
            try
            {
                scriptingManager.VerifyFileSystemTargets();
                Console.WriteLine("Ping");
                scriptingManager.InstallPythonDependencies();
                Console.WriteLine("Pong");
                ConfigurationManager.Scripting.PythonDependenciesInstalled = true;
                ConfigurationManager.WriteScriptingSettingsToFile(Application.StartupPath, ConfigurationManager.Scripting);
                MessageBox.Show(ConfigurationManager.Scripting.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
