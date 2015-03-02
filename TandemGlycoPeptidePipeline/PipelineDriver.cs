using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycReSoft.TandemGlycopeptidePipeline
{
    public enum TandemGlycoPeptideTaskState
    {
        New, Error, Complete
    }

    public class AnalysisPipeline
    {

        public ScriptManager Scripter;
        
        public String MS1MatchFilePath;
        public String GlycosylationSiteFilePath;
        public String MS2DeconFilePath;
        public String ModelFilePath;
        

        public double MS1MatchingTolerance;
        public double MS2MatchingTolerance;

        public String ProteinProspectorXMLFilePath;
        public String Method;

        public int NumProcesses { get; set; }
        public int NumDecoys { get; set; }
        public bool OnlyRandomDecoys { get; set; }

        public String ResultFilePath;
        public ResultsRepresentation ResultsTable;        
        public TandemGlycoPeptideTaskState State;

        /// <summary>
        /// Wrapper class for driving all facets of the analysis pipeline
        /// </summary>
        /// <param name="MS1MatchFilePath"></param>
        /// <param name="glycosylationSiteFilePath"></param>
        /// <param name="MS2DeconFilePath"></param>
        /// <param name="modelFilePath"></param>
        /// <param name="pythonInterpreterPath">
        /// Passed along to the ScriptManager, used to locate the Python Interpreter executable
        /// </param>
        /// <param name="rscriptPath">
        /// Passed along to the ScriptManager, used to locate the Rscript executable
        /// </param>
        /// <param name="scriptsRoot">
        /// Passed along to the ScriptManager, used to locate the script pipeline files
        /// </param>
        public AnalysisPipeline(String MS1MatchFilePath, String glycosylationSiteFilePath,
            String MS2DeconFilePath, String modelFilePath = null,
            String outputFilePath = null, double ms1MatchingTolerance = 1e-5,
            double ms2MatchingTolerance = 2e-5, String proteinProspectorXMLFilePath = null, String method = "full_random_forest",
            String pythonInterpreterPath = null, String rscriptPath = null, 
            String scriptsRoot = null, int numProcesses = 2, bool onlyRandomDecoys = false, 
            int numDecoys = 20)
        {
            this.MS1MatchFilePath = MS1MatchFilePath;
            this.GlycosylationSiteFilePath = glycosylationSiteFilePath;
            this.MS2DeconFilePath = MS2DeconFilePath;
            this.ModelFilePath = modelFilePath;

            this.ResultFilePath = outputFilePath;
            this.ResultsTable = null;
            this.State = TandemGlycoPeptideTaskState.New;

            this.MS1MatchingTolerance = ms1MatchingTolerance;
            this.MS2MatchingTolerance = ms2MatchingTolerance;
            this.Method = method;
            this.ProteinProspectorXMLFilePath = proteinProspectorXMLFilePath;

            this.NumDecoys = numDecoys;
            this.NumProcesses = numProcesses;
            this.OnlyRandomDecoys = onlyRandomDecoys;

            //Make sure that all needed resources can be found
            this.Scripter = new ScriptManager(pythonInterpreterPath, rscriptPath, scriptsRoot);
            Scripter.VerifyFileSystemTargets();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ResultsRepresentation RunClassification()
        {
            String outfile = Scripter.RunClassificationPythonPipeline(this.MS1MatchFilePath, 
                this.GlycosylationSiteFilePath, 
                this.MS2DeconFilePath, 
                modelDataFilePath:this.ModelFilePath,
                ms1MatchingTolerance:this.MS1MatchingTolerance, 
                ms2MatchingTolerance:this.MS2MatchingTolerance,
                proteinProspectorXMLFilePath:this.ProteinProspectorXMLFilePath,
                method:Method,
                outputFilePath:this.ResultFilePath,
                numProcesses: NumProcesses);

            this.ResultFilePath = outfile;

            ResultsRepresentation resultsTable = new ResultsRepresentation(outfile);
            this.ResultsTable = resultsTable;
            return resultsTable;            
        }

        public ResultsRepresentation RunModelBuilder()
        {
            String outfile = Scripter.RunModelBuildingPythonPipeline(this.MS1MatchFilePath,
                this.GlycosylationSiteFilePath,
                this.MS2DeconFilePath,
                ms1MatchingTolerance:this.MS1MatchingTolerance, 
                ms2MatchingTolerance:this.MS2MatchingTolerance,
                proteinProspectorXMLFilePath: this.ProteinProspectorXMLFilePath,
                method:Method);

            ResultsRepresentation model = new ResultsRepresentation(outfile);
            return model;
        }

        public ResultsRepresentation RunReclassification(String targetFilePath, String modelFilePath)
        {
            String outfile = Scripter.RunReclassifyWithModelTask(targetFilePath, modelFilePath, Method);
            ResultsRepresentation resultsTable = new ResultsRepresentation(outfile);
            this.ResultFilePath = outfile;
            this.ResultsTable = resultsTable;
            return resultsTable;
        }

    }

    [Serializable]
    public class TandemGlycoPeptidePipelineException : Exception
    {
        public TandemGlycoPeptidePipelineException() { }
        public TandemGlycoPeptidePipelineException(string message) : base(message) { }
        public TandemGlycoPeptidePipelineException(string message, Exception inner) : base(message, inner) { }
        protected TandemGlycoPeptidePipelineException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
