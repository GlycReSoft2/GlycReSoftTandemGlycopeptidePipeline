using GlycReSoft.TandemMSGlycopeptideGUI.ConfigMenus;
using GlycReSoft.TandemMSGlycopeptideGUI.Controllers;
using GlycReSoft.TandemMSGlycopeptideGUI.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using GlycReSoft.TandemGlycopeptidePipeline;
using GlycReSoft.TandemMSGlycopeptideGUI.GridViews;
using GlycReSoft.MS2GlycopeptideResultsBrowser;
using GlycReSoft.Generics.GenericForms;

namespace GlycReSoft.TandemMSGlycopeptideGUI
{
    public partial class TandemGlycopeptideAnalysis : Form
    {
        static string SELECT_MODEL_DEFAULT = "Select a Model";
        static string OPEN_MODEL_FILE_STRING = "Open a File...";

        TandemMSGlycopeptideGUIController Controller;
        Models PreparedModels;
        PleaseWait WaitScreen;

        public TandemGlycopeptideAnalysis()
        {
            InitializeComponent();
            //Initialize/Load Disk-stored configurations
            ConfigurationManager.Scripting = ConfigurationManager.LoadScriptingSettings(Application.StartupPath);
            ConfigurationManager.Algorithm = ConfigurationManager.LoadAlgorithmSettings(Application.StartupPath);

            WaitScreen = new PleaseWait();
            Controller = new TandemMSGlycopeptideGUIController();
            //Load pre-configured model index distributed with software from XML
            PreparedModels = BuiltInModels.Load();
            
            //Bind Controller to Interface Labels
            this.MS1MatchFilePathLabel.DataBindings.Add(new FilePathTrimmingBinding("Text", Controller, "MS1MatchFilePath"));
            this.MS2DeconvolutionFilePathLabel.DataBindings.Add(new FilePathTrimmingBinding("Text", Controller, "MS2DeconvolutionFilePath"));
            this.GlycosylationSiteFilePathLabel.DataBindings.Add(new FilePathTrimmingBinding("Text", Controller, "GlycosylationSiteFilePath"));
            this.ModelFilePathLabel.DataBindings.Add(new FilePathTrimmingBinding("Text", Controller, "ModelFilePath"));
            this.ProteinProspectorMSDigestXMLLabel.DataBindings.Add(new FilePathTrimmingBinding("Text", Controller, "ProteinProspectorMSDigestFilePath"));

            //Initialize ComboBox with label defining function of ComboBox
            SelectModelComboBox.Items.Add(SELECT_MODEL_DEFAULT);
            //Add each preconfigured model to the list of options
            foreach(Model model in PreparedModels.Model){
                SelectModelComboBox.Items.Add(model);
            }
            //Add the string option that is bound to open a file dialog at the end of the list
            SelectModelComboBox.Items.Add(OPEN_MODEL_FILE_STRING);
            //Set the active item to the functionality label which has no function
            SelectModelComboBox.SelectedItem = SELECT_MODEL_DEFAULT;

            var checkProg = new ProcessManager("python", " -c \"import pandas, glycresoft_ms2_classification\"");
            checkProg.Start();
            checkProg.WaitForExit();
            Console.WriteLine(checkProg.GenerateDumpMessage());
            Console.WriteLine(checkProg.CheckExitSuccessfully());
            if (!ConfigurationManager.Scripting.PythonDependenciesInstalled && !(checkProg.CheckExitSuccessfully()))
            {
                MessageBox.Show("If you have not yet verified you have installed all the Python dependencies, please go to the Scripting Settings Menu and use the verification command there.\n\r\n\rYou will need Pandas and Scikit-Learn, which depend upon NumPy and SciPy. If you don't yet have these, I suggest you try installing a scientific Python distribution like Anaconda(http://continuum.io/downloads) or Canopy(https://store.enthought.com/)");
            }
        }
       
        private void MS1MatchFilePathLoadButton_Click(object sender, EventArgs e)
        {
            DialogResult ms1MatchFileDialogResult = openFileDialog1.ShowDialog();
            if (ms1MatchFileDialogResult == DialogResult.OK)
            {
                try
                {
                    Controller.MS1MatchFilePath = openFileDialog1.FileName;
                }
                catch (IOException ex)
                {
                    MessageBox.Show("Could not open " + openFileDialog1.FileName + 
                        ". Error: " + ex.Message);
                }
            }
        }

        private void MS2DeconvolutionFilePathLoadButton_Click(object sender, EventArgs e)
        {
            DialogResult ms2DeconvolutionFileDialogResult = openFileDialog1.ShowDialog();
            if (ms2DeconvolutionFileDialogResult == DialogResult.OK)
            {
                try
                {
                    Controller.MS2DeconvolutionFilePath = openFileDialog1.FileName;
                }
                catch (IOException ex)
                {
                    MessageBox.Show("Could not open " + openFileDialog1.FileName +
                        ". Error: " + ex.Message);
                }
            }
        }

        private void GlycosylationSiteFilePathLoadButton_Click(object sender, EventArgs e)
        {
            DialogResult glycosylationFileDialogResult = openFileDialog1.ShowDialog();
            if (glycosylationFileDialogResult == DialogResult.OK)
            {
                try
                {
                    Controller.GlycosylationSiteFilePath = openFileDialog1.FileName;
                }
                catch (IOException ex)
                {
                    MessageBox.Show("Could not open " + openFileDialog1.FileName +
                        ". Error: " + ex.Message);
                }
            }
        }

        private void AddProteinProspectorMSDigestXmlFileButton_Click(object sender, EventArgs e)
        {
            DialogResult proteinProspectorMSDigestXmlResult = openFileDialog1.ShowDialog();
            if (proteinProspectorMSDigestXmlResult == DialogResult.OK)
            {
                try
                {
                    Controller.ProteinProspectorMSDigestFilePath = openFileDialog1.FileName;
                }
                catch (IOException ex)
                {
                    MessageBox.Show("Could not open " + openFileDialog1.FileName +
                        ". Error: " + ex.Message);
                }
            }
        }

        private bool SetModelFilePath_Action(object sender, EventArgs e)
        {
            DialogResult modelFileDialogResult = openFileDialog1.ShowDialog();
            if (modelFileDialogResult == DialogResult.OK)
            {
                try
                {
                    Controller.ModelFilePath = openFileDialog1.FileName;
                    return true;
                }
                catch (IOException ex)
                {
                    MessageBox.Show("Could not open " + openFileDialog1.FileName +
                        ". Error: " + ex.Message);
                    return false;
                }
                
            }
            return false;
        }



        /// <summary>
        /// Given MS, MS/MS, and glycosylation site data, run the pipeline up to but
        /// not including the classification step, returning a formatted CSV file that
        /// the user can label in a GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateModelButton_Click(object sender, EventArgs e)
        {
            if(Controller.MS1MatchFilePath != null && 
                Controller.MS2DeconvolutionFilePath != null &&
                Controller.GlycosylationSiteFilePath != null)
            {
                Console.WriteLine("Preparing Model");
                BackgroundWorker createModelWorker = new BackgroundWorker();
                createModelWorker.DoWork += createModelWorker_DoWork;
                createModelWorker.RunWorkerCompleted += createModelWorker_RunWorkerCompleted;
                try {
                    createModelWorker.RunWorkerAsync();
                    WaitScreen.ShowDialog();
                }
                catch(Exception ex)
                {
                    MessageBox.Show("An error occurred while processing this task: " + ex.Message);
                    WaitScreen.Close();
                }

            }
            else
            {
                MessageBox.Show("You must provide a results file from MS1 analysis with GlycReSoft, deconvoluted MS2 spectra in the appropriate format, and file listing the glycosylation sites for the protein in order to create and label a model to perform predictions wtih. Alternatively, you can use one of the built-in models.", "Missing Parameters");
            }

        }

        void createModelWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ResultsRepresentation preparedModelData = e.Result as ResultsRepresentation;
            if (preparedModelData == null)
            {
                return;
            }
            else
            {
                WaitScreen.Close();
                this.ShowModelLabelView(preparedModelData);
            }            
        }

        void createModelWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                var preparedModelData = Controller.PrepareModelFile();
                e.Result = preparedModelData;
            }
            catch (ScriptingException ex)
            {
                MessageBox.Show("An error occurred during classification: " + ex.Message, "Error");
                throw;
            }
        }

        /// <summary>
        /// Given a ResultsRepresentation, show them in a ModelLabelView
        /// </summary>
        /// <param name="preparedModelData"></param>
        private void ShowModelLabelView(ResultsRepresentation preparedModelData)
        {
            Console.WriteLine("Showing Model Label View");
            ModelLabelView labelView = new ModelLabelView(preparedModelData);
            labelView.Show();
        }

        /// <summary>
        /// Given a ResultsRepresentation, show them in a WebResultsView
        /// </summary>
        /// <param name="classifierData"></param>
        private void ShowClassifierResultsView(ResultsRepresentation classifierData)
        {            
            WebResultsViewer resultsView = new WebResultsViewer(classifierData);            
            resultsView.Show();
        }

        /// <summary>
        /// Run the classification pipeline on the selected input files
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClassifyMSMSSpectraActionButton_Click(object sender, EventArgs e)
        {
            if (Controller.MS1MatchFilePath != null &&
                Controller.MS2DeconvolutionFilePath != null &&
                Controller.GlycosylationSiteFilePath != null)
            {
                Console.WriteLine("Running Classifier");
                BackgroundWorker classifyWorker = new BackgroundWorker();
                classifyWorker.DoWork += classifyWorker_DoWork;
                classifyWorker.RunWorkerCompleted += classifyWorker_RunWorkerCompleted;
                try
                {
                    classifyWorker.RunWorkerAsync();
                    WaitScreen.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while processing this task: " + ex.Message);
                    WaitScreen.Close();
                }
                
            }
            else
            {
                MessageBox.Show("You must provide a results file from MS1 analysis with GlycReSoft, deconvoluted MS2 spectra in the appropriate format, and file listing the glycosylation sites for the protein, and a labeled model file in order to use the classifier.");
            }

        }

        /// <summary>
        /// Handles the completion of the ClassifyMSMSSpectraActionButton_Click task.
        /// If there are results from the pipeline, present them, otherwise do nothing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void classifyWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            WaitScreen.Close();
            ResultsRepresentation classifiedData = e.Result as ResultsRepresentation;
            if (classifiedData == null)
            {
                return;
            }
            else
            {
                this.ShowClassifierResultsView(classifiedData);
            }

        }

        /// <summary>
        /// Handles running the pipeline for the ClassifyMSMSSpectraActionButton_Click task.
        /// Passes along the ResultsRepresentation as the Result value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void classifyWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                var classifiedData = Controller.ClassifyGlycopeptideTandemMS();
                e.Result = classifiedData;
            }
            catch (TandemGlycoPeptidePipelineException ex)
            {            
                Console.WriteLine(sender);
            }
        }

        private void ScriptingSettingsButton_Click(object sender, EventArgs e)
        {
            ScriptingSettingsMenu menu = new ScriptingSettingsMenu();
            menu.Show();
        }

        /// <summary>
        /// View a previously generated model file. If no model is loaded, show an open
        /// file dialog and set the model path, and load the model into a ModelLabelView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewModelButton_Click(object sender, EventArgs e)
        {
            ResultsRepresentation model = null;
            try
            {
                model = new ResultsRepresentation(Controller.ModelFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                bool didSelect = SetModelFilePath_Action(sender, e);
                //If the user didn't actually select a model, cancelling the dialog, do nothing
                if (!didSelect)
                {
                    return;
                }
                    
                model = new ResultsRepresentation(Controller.ModelFilePath);
            }
            
            var modelView = new ModelLabelView(model);
            modelView.Show();
        }

        /// <summary>
        /// Opens the Algorithm Settings Menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AlgorithmSettingsButton_Click(object sender, EventArgs e)
        {
            AlgorithmSettingsMenu menu = new AlgorithmSettingsMenu();
            menu.Show();          
        }

        /// <summary>
        /// If the Combo Box selects a pre-built model, load it from file.
        /// If the Combo Box selects a string constant OPEN_MODEL_FILE_STRING, 
        /// show a file open dialog to select a model file from the user file system
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExistingModelsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {           
            if(SelectModelComboBox.SelectedItem.GetType() == typeof(String)){
                if (OPEN_MODEL_FILE_STRING == SelectModelComboBox.SelectedItem)
                {
                    SetModelFilePath_Action(sender, e);
                }
            }
            else if (SelectModelComboBox.SelectedItem.GetType() == typeof(Model))
            {
                Model selectedModel = (Model)SelectModelComboBox.SelectedItem;
                this.Controller.ModelFilePath = Path.Combine(Application.StartupPath, "Data", selectedModel.Path);                
            }
        }

        /// <summary>
        /// Load an already generated results CSV and view it using the WebResultsViewer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewResultsButton_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Viewing Results");
            DialogResult modelFileDialogResult = openFileDialog1.ShowDialog();
            if (modelFileDialogResult == DialogResult.OK)
            {
                try
                {
                    ResultsRepresentation resultFile = new ResultsRepresentation(openFileDialog1.FileName);
                    WebResultsViewer resultsView = new WebResultsViewer(resultFile);
                    resultsView.Show();
                }
                catch (IOException ex)
                {
                    MessageBox.Show("Could not open " + openFileDialog1.FileName +
                        ". Error: " + ex.Message);
                }
            }

        }

        private void ReclassifyResultsButton_Click(object sender, EventArgs e)
        {
            if (Controller.ModelFilePath == null)
            {
                MessageBox.Show("Please select a model first");
                SelectModelComboBox.Focus();
                return;
            }
            DialogResult targetFileDialogResult = openFileDialog1.ShowDialog();
            if (targetFileDialogResult == DialogResult.OK)
            {
                try
                {
                    String targetFilePath = openFileDialog1.FileName;
                    BackgroundWorker reclassifyWorker = new BackgroundWorker();
                    reclassifyWorker.DoWork += reclassifyWorker_DoWork;
                    reclassifyWorker.RunWorkerCompleted += classifyWorker_RunWorkerCompleted;
                    reclassifyWorker.RunWorkerAsync(targetFilePath);
                    WaitScreen.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while processing this task: " + ex.Message);
                    WaitScreen.Close();
                }
            }
        }

        void reclassifyWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            String targetFilePath = e.Argument as String;
            try
            {
                var classifiedData = Controller.ReclassifyWithModel(targetFilePath);
                e.Result = classifiedData;
            }
            catch (TandemGlycoPeptidePipelineException ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void CSVExportButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "JSON file|*.json";
            DialogResult targetFileDialogResult = openFileDialog1.ShowDialog();
            if (targetFileDialogResult == DialogResult.OK)
            {
                try
                {
                    String targetFilePath = openFileDialog1.FileName;
                    SaveFileDialog fileSaver = new SaveFileDialog();
                    fileSaver.DefaultExt = "csv";
                    fileSaver.Filter = "Comma separated volume|*.csv";
                    fileSaver.FileName = Path.Combine(Path.GetDirectoryName(targetFilePath), Path.GetFileNameWithoutExtension(targetFilePath) + ".csv");
                    DialogResult resultFileDialog = fileSaver.ShowDialog();
                    if (resultFileDialog == DialogResult.OK)
                    {
                        String resultFileName = fileSaver.FileName;
                        ScriptManager scripter = new ScriptManager(ConfigurationManager.Scripting.PythonInterpreterPath);
                        scripter.ConvertToCSV(targetFilePath, resultFileName);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while processing this task: " + ex.Message);
                    WaitScreen.Close();
                }
            }
        }



    }
}
