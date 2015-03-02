using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GlycReSoft.TandemMSGlycopeptideGUI.ConfigMenus
{
    public partial class AlgorithmSettingsMenu : Form
    {

        public AlgorithmSettingsMenu()
        {
            InitializeComponent();
            this.MS1MassToleranceNumericInput.Value = (decimal)(ConfigurationManager.Algorithm.MS1MassErrorTolerance * Math.Pow(10, 6));
            this.MS2MassToleranceNumericInput.Value = (decimal)(ConfigurationManager.Algorithm.MS2MassErrorTolerance * Math.Pow(10, 6));
            this.NumDecoysNumericInput.Value = ConfigurationManager.Algorithm.NumDecoys;
            this.NumProcessesNumericInput.Value = ConfigurationManager.Algorithm.NumProcesses;
            this.OnlyRandomDecoysCheckBox.Checked = ConfigurationManager.Algorithm.OnlyRandomDecoys;
        }


        private void OkayButton_Click(object sender, EventArgs e)
        {
            ConfigurationManager.Algorithm.MS1MassErrorTolerance = (double)this.MS1MassToleranceNumericInput.Value / Math.Pow(10, 6);
            ConfigurationManager.Algorithm.MS2MassErrorTolerance = (double)this.MS2MassToleranceNumericInput.Value / Math.Pow(10, 6);
            ConfigurationManager.Algorithm.NumDecoys = (int)this.NumDecoysNumericInput.Value;
            ConfigurationManager.Algorithm.NumProcesses = (int)this.NumProcessesNumericInput.Value;
            ConfigurationManager.Algorithm.OnlyRandomDecoys = this.OnlyRandomDecoysCheckBox.Checked;
            try
            {
                ConfigurationManager.WriteAlgorithmSettingsToFile(Application.StartupPath, ConfigurationManager.Algorithm);
            }
            catch (SettingFileException)
            {
                MessageBox.Show("Unable to save settings to file. You may be required to re-enter these options again next time the application launches");
            }
            Console.WriteLine(ConfigurationManager.Algorithm);
            this.Close();
        }

        private void CancelMenuButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
