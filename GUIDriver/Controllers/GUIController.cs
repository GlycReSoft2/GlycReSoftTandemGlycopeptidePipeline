using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GlycReSoft.TandemGlycopeptidePipeline;
using GlycReSoft.CompositionHypothesis;
using System.IO;

namespace GlycReSoft.TandemMSGlycopeptideGUI.Controllers
{
    [System.ComponentModel.Bindable(true)]
    public class TandemMSGlycopeptideGUIController : INotifyPropertyChanged
    {
        #region Properties
        private String _MS1MatchFilePath;
        public String MS1MatchFilePath
        {
            get { return _MS1MatchFilePath; }
            set
            {
                _MS1MatchFilePath = value;
                OnPropertyChanged("MS1MatchFilePath");

            }
        }

        private String _MS2DeconvolutionFilePath;
        public String MS2DeconvolutionFilePath
        {
            get { return _MS2DeconvolutionFilePath; }
            set
            {
                _MS2DeconvolutionFilePath = value;
                OnPropertyChanged("MS2DeconvolutionFilePath");
            }
        }


        private String _GlycosylationSiteFilePath;
        public String GlycosylationSiteFilePath
        {
            get { return _GlycosylationSiteFilePath; }
            set
            {
                _GlycosylationSiteFilePath = value;
                OnPropertyChanged("GlycosylationSiteFilePath");
            }
        }

        private String _ModelFilePath;
        public String ModelFilePath {
            get { return _ModelFilePath; }
            set 
            {                     
                _ModelFilePath = value;
                OnPropertyChanged("ModelFilePath");
            }
        }

        private String _ProteinProspectorMSDigestFilePath;
        public String ProteinProspectorMSDigestFilePath
        {
            get { return _ProteinProspectorMSDigestFilePath; }
            set
            {
                _ProteinProspectorMSDigestFilePath = value;
                OnPropertyChanged("ProteinProspectorMSDigestFilePath");
            }
        }

        public String ResultsFilePath { get; set; }

        private String _DecoyFilePath;
        public String DecoyFilePath
        {
            get { return _DecoyFilePath; }
            set
            {
                _DecoyFilePath = value;
                OnPropertyChanged("DecoyFilePath");
            }
        }

        public AnalysisPipeline Pipeline = null;

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion        

        public TandemMSGlycopeptideGUIController() {
            MS1MatchFilePath = null;
            MS2DeconvolutionFilePath = null;
            GlycosylationSiteFilePath = null;
            ModelFilePath = null;
            ResultsFilePath = null;
            ProteinProspectorMSDigestFilePath = null;
        }


        protected virtual void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        public ResultsRepresentation ClassifyGlycopeptideTandemMS()
        {
            try
            {

                this.Pipeline = new AnalysisPipeline(MS1MatchFilePath, 
                    GlycosylationSiteFilePath, MS2DeconvolutionFilePath, 
                    modelFilePath: ModelFilePath, outputFilePath: null,
                    proteinProspectorXMLFilePath: ProteinProspectorMSDigestFilePath,
                    ms1MatchingTolerance: ConfigurationManager.Algorithm.MS1MassErrorTolerance,
                    ms2MatchingTolerance: ConfigurationManager.Algorithm.MS2MassErrorTolerance,
                    numProcesses: ConfigurationManager.Algorithm.NumProcesses,
                    numDecoys: ConfigurationManager.Algorithm.NumDecoys,
                    onlyRandomDecoys: ConfigurationManager.Algorithm.OnlyRandomDecoys,
                    pythonInterpreterPath: ConfigurationManager.Scripting.PythonInterpreterPath,
                    rscriptPath: null);

                ResultsRepresentation results = Pipeline.RunClassification();
                File.Delete(results.SourceFile);
                return results;

            }
            catch (TandemGlycoPeptidePipelineException e)
            {
                Console.WriteLine(e.Message);
                MessageBox.Show("An error occurred during classification: " + e.Message, "Error");
                throw;
            }
        }

        /// <summary>
        /// Runs the Model Building Pipeline, yielding a partially annotated model in the form of a ResultsRepresentation object
        /// which is made from the parameters loaded into the Controller.
        /// </summary>
        /// <returns></returns>
        public ResultsRepresentation PrepareModelFile()
        {
            this.Pipeline = new AnalysisPipeline(MS1MatchFilePath,
                GlycosylationSiteFilePath, MS2DeconvolutionFilePath,
                modelFilePath: ModelFilePath, outputFilePath: null,
                proteinProspectorXMLFilePath: ProteinProspectorMSDigestFilePath,
                ms1MatchingTolerance: ConfigurationManager.Algorithm.MS1MassErrorTolerance,
                ms2MatchingTolerance: ConfigurationManager.Algorithm.MS2MassErrorTolerance,
                numProcesses: ConfigurationManager.Algorithm.NumProcesses,
                numDecoys: ConfigurationManager.Algorithm.NumDecoys,
                onlyRandomDecoys: ConfigurationManager.Algorithm.OnlyRandomDecoys,
                pythonInterpreterPath: ConfigurationManager.Scripting.PythonInterpreterPath,
                rscriptPath: null);

            ResultsRepresentation modelRepresentation = Pipeline.RunModelBuilder();
            File.Delete(modelRepresentation.SourceFile);
            return modelRepresentation;
        }

        public ResultsRepresentation ReclassifyWithModel(String reclassificationTargetFilePath)
        {
            this.Pipeline = new AnalysisPipeline(MS1MatchFilePath,
                GlycosylationSiteFilePath, MS2DeconvolutionFilePath,
                modelFilePath: ModelFilePath, outputFilePath: null,
                ms1MatchingTolerance: ConfigurationManager.Algorithm.MS1MassErrorTolerance,
                ms2MatchingTolerance: ConfigurationManager.Algorithm.MS2MassErrorTolerance,
                pythonInterpreterPath: ConfigurationManager.Scripting.PythonInterpreterPath,
                numProcesses: ConfigurationManager.Algorithm.NumProcesses,
                numDecoys: ConfigurationManager.Algorithm.NumDecoys,
                onlyRandomDecoys: ConfigurationManager.Algorithm.OnlyRandomDecoys,
                rscriptPath: null);

            ResultsRepresentation modelRepresentation = this.Pipeline.RunReclassification(reclassificationTargetFilePath, ModelFilePath);
            File.Delete(modelRepresentation.SourceFile);
            return modelRepresentation;
            
        }



        [Serializable]
        public class PipelineInitializationException : Exception
        {
            public PipelineInitializationException() { }
            public PipelineInitializationException(string message) : base(message) { }
            public PipelineInitializationException(string message, Exception inner) : base(message, inner) { }
            protected PipelineInitializationException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context)
                : base(info, context) { }
        }
    }
}
