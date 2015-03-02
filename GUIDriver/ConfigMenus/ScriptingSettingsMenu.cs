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
            UpdateDependencyIndicator(ConfigurationManager.Scripting.DependenciesFound);
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
                ConfigurationManager.Scripting.DependenciesFound = DependencyInstalledState.Yes;
                ConfigurationManager.WriteScriptingSettingsToFile(Application.StartupPath, ConfigurationManager.Scripting);
            }
            catch (Exception ex)
            {
                ConfigurationManager.Scripting.DependenciesFound = DependencyInstalledState.No;
                MessageBox.Show(ex.Message);
            }
            UpdateDependencyIndicator(ConfigurationManager.Scripting.DependenciesFound);
        }

        private void UpdateDependencyIndicator(DependencyInstalledState isInstalled)
        {
            switch (isInstalled)
            {
                case (DependencyInstalledState.Yes):
                    {
                        DependencyCheckIndicator.Text = DependencyInstalledState.Yes.ToString();
                        DependencyCheckIndicator.BackColor = Color.Green;
                        break;
                    }
                case DependencyInstalledState.No:
                    {
                        DependencyCheckIndicator.Text = DependencyInstalledState.No.ToString();
                        DependencyCheckIndicator.BackColor = Color.Red;
                        break;
                    }
                case DependencyInstalledState.Unknown:
                    {
                        DependencyCheckIndicator.Text = DependencyInstalledState.Unknown.ToString();
                        DependencyCheckIndicator.BackColor = Color.Yellow;
                        break;
                    }
                default:
                    {
                        throw new Exception("Could not interpret dependency installed enum");
                    }
            }
        }
    }
}
