using GlycReSoft.MS2GUIDriver.GridViews;
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
using GlycReSoft.TandemGlycopeptidePipeline;


namespace GlycReSoft.MS2GUIDriver.GridViews
{
    public partial class ModelLabelView : Form
    {
        private BindingSource Binder;
        public ResultsRepresentation Model = null;

        /// <summary>
        /// Ease of Inheritance
        /// </summary>
        protected ModelLabelView() {
            InitializeComponent();
            Binder = new BindingSource();
            MS2MatchDataGridView.AutoGenerateColumns = false;
            MS2MatchDataGridView.AutoSize = false;
            MS2MatchDataGridView.DataSource = Binder;
            InitGridView();  
        }

        public ModelLabelView(ResultsRepresentation model)
        {
            InitializeComponent();
            Model = model;
            Binder = new BindingSource();                
            MS2MatchDataGridView.AutoGenerateColumns = false;
            MS2MatchDataGridView.AutoSize = false;
            MS2MatchDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            MS2MatchDataGridView.DataSource = Binder;
            InitGridView();            
        }

        protected virtual void InitGridView()
        {
            Console.WriteLine("Intializing Grid View (ModelLabelView)");
            DataGridViewColumn obsMassColumn = new DataGridViewTextBoxColumn();
            obsMassColumn.DataPropertyName = "ObsMass";
            obsMassColumn.Name = "Observed Mass";
            obsMassColumn.SortMode = DataGridViewColumnSortMode.Automatic;
            MS2MatchDataGridView.Columns.Add(obsMassColumn);

            DataGridViewColumn ms1ScoreColumn = new DataGridViewTextBoxColumn();
            ms1ScoreColumn.DataPropertyName = "MS1Score";
            ms1ScoreColumn.Name = "MS1 Score";
            MS2MatchDataGridView.Columns.Add(ms1ScoreColumn);

            DataGridViewColumn ppmErrorColumn = new DataGridViewTextBoxColumn();
            ppmErrorColumn.DataPropertyName = "PPMError";
            ppmErrorColumn.Name = "PPM Error";
            MS2MatchDataGridView.Columns.Add(ppmErrorColumn);

            DataGridViewColumn glycopeptideIdentifierColumn = new DataGridViewTextBoxColumn();
            glycopeptideIdentifierColumn.DataPropertyName = "GlycopeptideIdentifier";
            glycopeptideIdentifierColumn.Name = "Glycopeptide Identifier";
            MS2MatchDataGridView.Columns.Add(glycopeptideIdentifierColumn);

            DataGridViewColumn glycanIdentifierColumn = new DataGridViewTextBoxColumn();
            glycanIdentifierColumn.DataPropertyName = "Glycan";
            glycanIdentifierColumn.Name = "Glycan";
            MS2MatchDataGridView.Columns.Add(glycanIdentifierColumn);

            DataGridViewColumn peptideLengthColumn = new DataGridViewTextBoxColumn();
            peptideLengthColumn.DataPropertyName = "PeptideLens";
            peptideLengthColumn.Name = "Peptide Length";

            DataGridViewColumn numOxoniumIonColumn = new DataGridViewTextBoxColumn();
            numOxoniumIonColumn.DataPropertyName = "NumOxIons";
            numOxoniumIonColumn.Name = "Oxonium Ion Count";
            MS2MatchDataGridView.Columns.Add(numOxoniumIonColumn);

            DataGridViewColumn numStubIonsColumn = new DataGridViewTextBoxColumn();
            numStubIonsColumn.DataPropertyName = "NumStubsFound";
            numStubIonsColumn.Name = "Number of Stub Ions";
            MS2MatchDataGridView.Columns.Add(numStubIonsColumn);

            DataGridViewColumn meanCoverageColumn = new DataGridViewTextBoxColumn();
            meanCoverageColumn.DataPropertyName = "MeanPerSiteCoverage";
            meanCoverageColumn.Name = "Mean Coverage";
            MS2MatchDataGridView.Columns.Add(meanCoverageColumn);

            DataGridViewColumn percentUncoveredColumn = new DataGridViewTextBoxColumn();
            percentUncoveredColumn.DataPropertyName = "PercentUncovered";
            percentUncoveredColumn.Name = "Percent Uncovered";
            MS2MatchDataGridView.Columns.Add(percentUncoveredColumn);

            DataGridViewColumn ionCountsColumn = new DataGridViewTextBoxColumn();
            ionCountsColumn.DataPropertyName = "IonCounts";
            ionCountsColumn.Name = "Ion Counts (HexNAc)";
            MS2MatchDataGridView.Columns.Add(ionCountsColumn);

            DataGridViewColumn callPredictionColumn = new DataGridViewCheckBoxColumn();
            callPredictionColumn.DataPropertyName = "Call";
            callPredictionColumn.Name = "Call";
            MS2MatchDataGridView.Columns.Add(callPredictionColumn);

            //DataGridViewColumn ambiguityColumn = new DataGridViewTextBoxColumn();
            //ambiguityColumn.DataPropertyName = "Ambiguity";
            //ambiguityColumn.Name = "Ambiguity";
            //MS2MatchDataGridView.Columns.Add(ambiguityColumn);
        }

        protected virtual void LoadPredictionsToGrid()
        {
            Console.WriteLine("LoadPredictionsToGrid (ModelLabelView)");
            foreach (GlycopeptidePrediction pred in Model.MatchedPredictions)
            {
                Binder.Add(pred);
            }
            GridViewHelper.ColorRowsByMassScoreGroup(MS2MatchDataGridView);           
        }

        protected virtual void FormLoadHandle(object sender, EventArgs e)
        {
            Console.WriteLine("FormLoadHandle (ModelLabelView)");
            if(Model != null) LoadPredictionsToGrid();
        }

        protected void SaveModelButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveModelFileDialog = new SaveFileDialog();
            saveModelFileDialog.Filter = "Comma Separated Text|*.csv|JSON Format|*.json";
            saveModelFileDialog.Title = "Save a Model File";
            saveModelFileDialog.ShowDialog();
            try
            {
                if (saveModelFileDialog.FileName != "")
                {
                    Console.WriteLine("Saving Model");                    
                    var saveFileStreamWriter = new StreamWriter((FileStream)saveModelFileDialog.OpenFile());
                    if (saveModelFileDialog.FilterIndex == 1) Model.WriteToCsv(saveFileStreamWriter);
                    else if (saveModelFileDialog.FilterIndex == 2) Model.WriteToJson(saveFileStreamWriter);
                    //Default
                    else Model.WriteToCsv(saveFileStreamWriter);
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show("An error occured while trying to save the model file: " + ex.Message, "An error occurred");
            }
        }
    }
}
