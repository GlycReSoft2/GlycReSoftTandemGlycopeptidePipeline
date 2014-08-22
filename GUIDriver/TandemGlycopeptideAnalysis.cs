using GlycReSoft.MS2GUIDriver.ConfigMenus;
using GlycReSoft.MS2GUIDriver.Controllers;
using GlycReSoft.MS2GUIDriver.Data;
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
using GlycReSoft.MS2GUIDriver.GridViews;
using GlycReSoft.MS2GlycopeptideResultsBrowser;

namespace GlycReSoft.MS2GUIDriver
{
    public partial class TandemGlycopeptideAnalysis : Form
    {
        static string SELECT_MODEL_DEFAULT = "Select a Model";
        static string OPEN_MODEL_FILE_STRING = "Open a File...";

        TandemMSGlycopeptideGUIController Controller;
        Models PreparedModels;
        public Form WaitScreen;

        public TandemGlycopeptideAnalysis()
        {
            InitializeComponent();
            //Initialize/Load Disk-stored configurations
            ConfigurationManager.Scripting = ConfigurationManager.LoadScriptingSettings(Application.StartupPath);
            ConfigurationManager.Algorithm = ConfigurationManager.LoadAlgorithmSettings(Application.StartupPath);

            Controller = new TandemMSGlycopeptideGUIController();
            //Load pre-configured model index distributed with software from XML
            PreparedModels = BuiltInModels.Load();
            
            //Bind Controller to Interface Labels
            this.MS1MatchFilePathLabel.DataBindings.Add(new Binding("Text", Controller, "MS1MatchFilePath"));
            this.MS2DeconvolutionFilePathLabel.DataBindings.Add(new Binding("Text", Controller, "MS2DeconvolutionFilePath"));
            this.GlycosylationSiteFilePathLabel.DataBindings.Add(new Binding("Text", Controller, "GlycosylationSiteFilePath"));
            this.ModelFilePathLabel.DataBindings.Add(new Binding("Text", Controller, "ModelFilePath"));

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

        private void SetModelFilePath_Action(object sender, EventArgs e)
        {
            DialogResult modelFileDialogResult = openFileDialog1.ShowDialog();
            if (modelFileDialogResult == DialogResult.OK)
            {
                try
                {
                    Controller.ModelFilePath = openFileDialog1.FileName;
                }
                catch (IOException ex)
                {
                    MessageBox.Show("Could not open " + openFileDialog1.FileName +
                        ". Error: " + ex.Message);
                }
            }
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
                createModelWorker.RunWorkerAsync();
                //var preparedModelData = Controller.PrepareModelFile();
                //if (preparedModelData == null)
                //{
                //    return;
                //}
                //this.ShowModelLabelView(preparedModelData);
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
                this.ShowModelLabelView(preparedModelData);
            }            
        }

        void createModelWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var preparedModelData = Controller.PrepareModelFile();
            e.Result = preparedModelData;
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
            Console.WriteLine("Showing Classifier Results View");
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
                classifyWorker.RunWorkerAsync();
                //var classifiedData = Controller.ClassifyGlycopeptideTandemMS();
                //if (classifiedData == null)
                //{
                //    return;
                //}
                //this.ShowClassifierResultsView(classifiedData);
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
            var classifiedData = Controller.ClassifyGlycopeptideTandemMS();
            e.Result = classifiedData;
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
                SetModelFilePath_Action(sender, e);
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



    }
}
