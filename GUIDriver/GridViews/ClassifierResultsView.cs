using GlycReSoft.MS2GUIDriver.GridViews;
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

namespace GlycReSoft.MS2GUIDriver.GridViews
{
    public partial class ClassifierResultsView : GlycReSoft.MS2GUIDriver.GridViews.ModelLabelView
    {
        public ClassifierResultsView() : base()
        {
            InitializeComponent();            
        }
        public ClassifierResultsView(ResultsRepresentation model) : base(model)
        {
            InitializeComponent();           
            Console.WriteLine();
        }

        protected override void FormLoadHandle(object sender, System.EventArgs e)
        {
            Console.WriteLine("FormLoadHandle (ClassifierResultsView)");
            if (Model != null) LoadPredictionsToGrid();            
        }

        protected override void LoadPredictionsToGrid()
        {
            Console.WriteLine("LoadPredictionsToGrid (ClassifierResultsView)");
            base.LoadPredictionsToGrid();
            GridViewHelper.SetRowsVisibilityByScore(MS2MatchDataGridView, 0.1, false);
        }

        protected override void InitGridView()
        {
            Console.WriteLine("Intializing Grid View (ClassifierResultsView)");
            base.InitGridView();
            DataGridViewColumn ms2ScoreColumn = new DataGridViewTextBoxColumn();
            ms2ScoreColumn.DataPropertyName = "MS2Score";
            ms2ScoreColumn.Name = "MS2 Score";
            MS2MatchDataGridView.Columns.Add(ms2ScoreColumn);
        }

        private void HideLowScoringHitsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.Visible = false;
            this.SuspendLayout();
            GridViewHelper.SetRowsVisibilityByScore(this.MS2MatchDataGridView, 0.3, false);
            this.ResumeLayout();
            this.Visible = true;
        }
    }
}
