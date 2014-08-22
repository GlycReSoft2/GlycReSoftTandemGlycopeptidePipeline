﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GlycReSoft.TandemGlycopeptidePipeline;

namespace GlycReSoft.MS2GUIDriver.Controllers
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

        public AnalysisPipeline Pipeline = null;               

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion        

        public TandemMSGlycopeptideGUIController() {
            MS1MatchFilePath = null;
            MS2DeconvolutionFilePath = null;
            GlycosylationSiteFilePath = null;
            ModelFilePath = null;
            ResultsFilePath = null;           
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
                    goldStandardPath: ModelFilePath, outputFilePath: null,
                    pythonInterpreterPath: ConfigurationManager.Scripting.PythonInterpreterPath,
                    rscriptPath: ConfigurationManager.Scripting.RscriptInterpreterPath);

                ResultsRepresentation results = Pipeline.RunClassification();                
                return results;

            }
            catch (TandemGlycoPeptidePipelineException e)
            {
                Console.WriteLine(e.Message);
                MessageBox.Show("An error occurred during classification: " + e.Message, "Error");
                return null;
            }
        }

        public ResultsRepresentation PrepareModelFile()
        {
            try
            {
                this.Pipeline = new AnalysisPipeline(MS1MatchFilePath, GlycosylationSiteFilePath, MS2DeconvolutionFilePath, goldStandardPath: null, outputFilePath: null,
                    pythonInterpreterPath: ConfigurationManager.Scripting.PythonInterpreterPath,
                    rscriptPath: ConfigurationManager.Scripting.RscriptInterpreterPath);
                ResultsRepresentation modelRepresentation = Pipeline.RunModelBuilder();
                return modelRepresentation;
            }
            catch (TandemGlycoPeptidePipelineException e)
            {
                MessageBox.Show("An error occurred during model building: " + e.Message, "Error");
                return null;
            }

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
