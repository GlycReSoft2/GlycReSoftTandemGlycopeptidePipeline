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
        public String GoldStandardFilePath;
        

        public double MS1MatchingTolerance;
        public double MS2MatchingTolerance;

        public String[] ConstantModifications;
        public String[] VariableModifications;
        public String Method;

        public String ResultFilePath;
        public ResultsRepresentation ResultsTable;        
        public TandemGlycoPeptideTaskState State;

        /// <summary>
        /// Wrapper class for driving all facets of the analysis pipeline
        /// </summary>
        /// <param name="MS1MatchFilePath"></param>
        /// <param name="glycosylationSiteFilePath"></param>
        /// <param name="MS2DeconFilePath"></param>
        /// <param name="goldStandardPath"></param>
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
            String MS2DeconFilePath, String goldStandardPath = null,
            String outputFilePath = null, double ms1MatchingTolerance = 1e-5,
            double ms2MatchingTolerance = 2e-5, String[] constantModifications = null,
            String[] variableModifications = null, String method = "default",
            String pythonInterpreterPath = null, String rscriptPath = null, 
            String scriptsRoot = null)
        {
            this.MS1MatchFilePath = MS1MatchFilePath;
            this.GlycosylationSiteFilePath = glycosylationSiteFilePath;
            this.MS2DeconFilePath = MS2DeconFilePath;
            this.GoldStandardFilePath = goldStandardPath;

            this.ResultFilePath = outputFilePath;
            this.ResultsTable = null;
            this.State = TandemGlycoPeptideTaskState.New;

            this.MS1MatchingTolerance = ms1MatchingTolerance;
            this.MS2MatchingTolerance = ms2MatchingTolerance;
            this.Method = method;
            this.ConstantModifications = constantModifications;
            this.VariableModifications = variableModifications;

            //Make sure that all needed resources can be found
            this.Scripter = new ScriptManager(pythonInterpreterPath, rscriptPath, scriptsRoot);
            Scripter.VerifyFileSystemTargets();
        }

        /// <summary>
        /// Install R libraries used by the pipeline. 
        /// </summary>
        public void InstallRLibraryDependencies()
        {
            Scripter.InstallRDependencies();
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
                goldStandardModelDataFilePath:this.GoldStandardFilePath,
                ms1MatchingTolerance:this.MS1MatchingTolerance, 
                ms2MatchingTolerance:this.MS2MatchingTolerance,
                constantModifications:this.ConstantModifications,
                variableModifications:this.VariableModifications,
                method:Method,
                outputFilePath:this.ResultFilePath);

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
                constantModifications: this.ConstantModifications,
                variableModifications: this.VariableModifications,
                method:Method);

            ResultsRepresentation model = new ResultsRepresentation(outfile);
            return model;
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
