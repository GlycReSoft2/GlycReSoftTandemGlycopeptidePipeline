using System;
using System.Security.Permissions;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using GlycReSoft.TandemGlycopeptidePipeline;

namespace GlycReSoft.MS2GlycopeptideResultsBrowser

{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public partial class WebResultsViewer : Form
    {
        public String DataProvider;
        public ResultsRepresentation Model;

        public WebResultsViewer(ResultsRepresentation modelData)
        {
            InitializeComponent();
            this.Model = modelData;

            this.DataProvider = BootstrapModelString(Model);


            BrowserCtrl.IsWebBrowserContextMenuEnabled = true;
            var indexURI = new Uri(Path.Combine(Application.StartupPath, "Web", "index.html")).AbsoluteUri;
            BrowserCtrl.ObjectForScripting = this;
            BrowserCtrl.Navigate(indexURI);
            
            //Set up page, feeding in data and registering native-web interfaces
            BrowserCtrl.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(delegate(Object o, WebBrowserDocumentCompletedEventArgs e)
            {
                JavaScriptAction(@"window.objects = window.external.DataProvider;                    registerDataChange(d3.csv.parse(window.external.DataProvider));");
                //Remove previously registered click handler and adds native save
                //function call
                JavaScriptAction("$('#save-csv-btn').off('click').click(function(evt){window.external.SaveResultsToFile()})");
                //JavaScriptAction("console.log = window.external.ExternLog");
            });          
        }

        public WebResultsViewer()
        {
            InitializeComponent();
            BrowserCtrl.IsWebBrowserContextMenuEnabled = true;
            var indexURI = new Uri(Path.Combine(Application.StartupPath, "Web","index.html")).AbsoluteUri;
            BrowserCtrl.ObjectForScripting = this;
            BrowserCtrl.Navigate(indexURI);
            DataProvider = File.ReadAllText(Path.Combine(Application.StartupPath, "Data", "ExampleResults.csv"));
            BrowserCtrl.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(delegate(Object o, WebBrowserDocumentCompletedEventArgs e)
            {
                JavaScriptAction("registerDataChange(d3.csv.parse(window.external.DataProvider));");
                JavaScriptAction("$('#save-csv').click(function(evt){window.external.ExternLog('logging')})");
            });
            BrowserCtrl.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(delegate(Object o, WebBrowserDocumentCompletedEventArgs e)
            {              
                BrowserCtrl.Document.Window.Error += (w, we) =>
                {
                    we.Handled = true;
                    Console.WriteLine(string.Format("Error: {1}\nline: {0}\nurl: {2}", we.LineNumber, we.Description, we.Url));
                };
            });
        }

        /// <summary>
        /// Execute arbitrary JavaScript by injecting script tags
        /// </summary>
        /// <param name="scriptContents">The JavaScript to execute as a string</param>
        void JavaScriptAction(string scriptContents)
        {
            HtmlDocument doc = BrowserCtrl.Document;
            HtmlElement head = doc.GetElementsByTagName("head")[0];
            HtmlElement s = doc.CreateElement("script");
            s.SetAttribute("text", scriptContents);
            head.AppendChild(s);
        }

        /// <summary>
        /// Saves the Model data as a CSV or JSON
        /// </summary>
        public void SaveResultsToFile()
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

        /// <summary>
        /// Get the raw string contents of the ResultsRepresentation model as a CSV by writing
        /// it to a temporary file and reading it back out and returns it.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected string BootstrapModelString(ResultsRepresentation model)
        {
            string tempFile = Path.GetTempFileName();
            StreamWriter tempWriter = new StreamWriter(File.OpenWrite(tempFile));
            model.WriteToCsv(tempWriter);
            string modelString = File.ReadAllText(tempFile);
            File.Delete(tempFile);
            return modelString;
        }

        public void ExternLog(params object[] messages)
        {
            foreach (object msg in messages)
            {
                Console.WriteLine(msg);
            }
        }


        private void WebResultsViewer_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }
    }
}
