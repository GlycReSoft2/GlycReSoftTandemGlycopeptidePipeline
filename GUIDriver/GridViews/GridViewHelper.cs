using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GlycReSoft.TandemMSGlycopeptideGUI.GridViews
{
    public static class GridViewHelper
    {
        class CircularColorProvider
        {
            int index = 0;
            static List<Color> RowColors = new List<Color>(){
                Color.LightSalmon, Color.LightSteelBlue, Color.LightCoral, Color.LightSeaGreen,
                Color.PaleTurquoise, Color.PaleGreen, Color.PaleVioletRed, Color.PaleGoldenrod,
            };

            public Color GetColor()
            {
                return RowColors[index++ % RowColors.Count()];
            }
        }

        public static object GetCellValueFromColumnHeader(this DataGridViewCellCollection CellCollection, string HeaderText)
        {
            return CellCollection.Cast<DataGridViewCell>().First(c => c.OwningColumn.HeaderText == HeaderText).Value;
        }
        
        public static void ColorRowsByMassScoreGroup(DataGridView gridView)
        {
            CircularColorProvider ColorProvider = new CircularColorProvider();
            double ms1Score = (double)gridView.Rows[0].Cells.GetCellValueFromColumnHeader("MS1 Score");
            double obsMass = (double)gridView.Rows[0].Cells.GetCellValueFromColumnHeader("MS1 Score");
            Color rowColor = ColorProvider.GetColor();
            int timeElapsed = Environment.TickCount;
            foreach (DataGridViewRow row in gridView.Rows)
            {
                int timeTick = Environment.TickCount;                
                double nextMS1Score = (double)row.Cells.GetCellValueFromColumnHeader("MS1 Score");
                double nextObsMass = (double)row.Cells.GetCellValueFromColumnHeader("MS1 Score");
                if (nextMS1Score != ms1Score && nextObsMass != obsMass)
                {
                    rowColor = ColorProvider.GetColor();
                    ms1Score = nextMS1Score;
                    obsMass = nextObsMass;
                }
                row.DefaultCellStyle.BackColor = rowColor;
            }
        }

        public static void SetRowsVisibilityByScore(DataGridView gridView, double threshold, bool visState)
        {
            Console.WriteLine("Hiding");
            gridView.SuspendLayout();
            CurrencyManager currencyManager = gridView.BindingContext[gridView.DataSource] as CurrencyManager;
            currencyManager.SuspendBinding();
            int timeElapsed = Environment.TickCount;
            foreach(DataGridViewRow row in gridView.Rows)
            {
                int timeTick = Environment.TickCount;
                Console.WriteLine("Toggling row {0} - {1}", row, timeTick - timeElapsed);
                timeElapsed = timeTick; 
                try
                {
                    double ms2Score = (double)row.Cells.GetCellValueFromColumnHeader("MS2 Score");
                    if (ms2Score < threshold)
                    {
                        row.Visible = visState;
                    }
                }
                catch
                {
                    Console.WriteLine(row);
                    //row.Visible = false;
                }
            }          
            currencyManager.ResumeBinding();
            gridView.ResumeLayout();
            gridView.Refresh();
        }
    }
}
