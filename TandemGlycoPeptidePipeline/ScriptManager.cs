using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GlycReSoft.TandemGlycopeptidePipeline
{
    
    /// <summary>
    /// Control the script direction and execution layer of the pipeline programmatically.
    /// </summary>
    public class ScriptManager
    {

        public static String PYTHON_PIPELINE_OUTFILE = "python-pipeline-outputfile";

        #region Data Members
        public String PythonExecutablePath;
        public String RscriptExecutablePath;
        public String ScriptRoot;
        public Dictionary<string, string> Data;

        //Storage for retrieving information in another context
        public String PythonPipelineOutFile = "";
        //Useful for both debugging and inspecting the output of the last ProcessManager
        //to exit successfully.
        public ProcessManager LastCall = null;
        #endregion

        #region Default Parameters
        /* Configure the default paths to the Python Interpreter,
         * Rscript Interpreter, Python and R Script Files. The interpreter
         * paths are modifiable so that a user can choose a program that is not
         * on their system path. 
         */
        static String DefaultScriptPath = Path.Combine(
            Path.GetDirectoryName(
                System.Reflection.Assembly.GetExecutingAssembly().Location
                ),
            "python", "glycresoft_ms2_classification");
        static String DefaultPythonInterpreterPath = "python";
        static String DefaultRscriptPath = "Rscript";
        #endregion
        
        public ScriptManager(String pythonExecutablePath = null, 
            String rscriptExecutablePath = null, 
            String scriptRoot = null)
        {
            this.ScriptRoot = (scriptRoot == null ? DefaultScriptPath : scriptRoot);
            this.PythonExecutablePath = 
                (((pythonExecutablePath == null) || (pythonExecutablePath == "")) ? DefaultPythonInterpreterPath : pythonExecutablePath).Escape();
            this.RscriptExecutablePath = 
                (((rscriptExecutablePath == null) || (rscriptExecutablePath == ""))  ? 
                DefaultRscriptPath : rscriptExecutablePath).Escape();

            this.Data = new Dictionary<string, string>();
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
        #region Verify Dependencies
        public bool VerifyPythonExecutable()
        {
            try
            {
                Process.Start(this.PythonExecutablePath, " -h");
                return true;
            }
            catch (FileNotFoundException)
            {
                return false;
            }
            catch (Win32Exception)
            {
                return false;
            }
        }

        public bool VerifyRscriptExecutable()
        {
            Console.WriteLine(this.RscriptExecutablePath);
            try
            {
                Process.Start(this.RscriptExecutablePath, " -h");
                return true;
            }
            catch (FileNotFoundException)
            {
                return false;
            }
            catch (Win32Exception)
            {
                return false;
            }
        }

        public bool VerifyScriptRoot()
        {
            return Directory.Exists(this.ScriptRoot);
        }

        public bool VerifyFileSystemTargets()
        {
            if (!this.VerifyPythonExecutable())
            {
                throw new PythonInterpreterNotFoundException("Could not locate python");
            }
            if (!this.VerifyRscriptExecutable())
            {
                throw new RscriptNotFoundException("Could not locate Rscript");
            }
            if (!this.VerifyScriptRoot())
            {
                throw new ScriptsNotFoundException("Could not locate scripts");
            }
            return true;
        }
        #endregion
       
        public void InstallRDependencies()
        {
            ProcessManager proc = new ProcessManager(this.RscriptExecutablePath, 
                "--vanilla " + Path.Combine(this.ScriptRoot, "R", "install_dependencies.R").QuoteWrap());

            proc.Start();
            proc.WaitForExit();
            if (!proc.CheckExitSuccessfully())
            {
                throw new RscriptErrorException(proc.GenerateDumpMessage());
            }

            this.LastCall = proc;            
        }

        /// <summary>
        /// Executes the Python script pipeline stored at @ScriptRoot, with the 
        /// __main__.py entry-point. The pipeline emits intermediary file names,
        /// and the last line printed on a successful run is the final output 
        /// file name.
        /// 
        /// Will check the exit code of the pipeline, and throw a PythonScriptErrorException
        /// if it is not 0.
        /// </summary>
        /// <param name="ms1MatchFilePath"></param>
        /// <param name="glycosylationSitesFilePath"></param>
        /// <param name="ms2DeconvolutionFilePath"></param>
        /// <param name="goldStandardModelDataFilePath"></param>
        /// <param name="outputFilePath"></param>
        /// <returns>String path to the output file generated by the pipeline</returns>
        public String RunClassificationPythonPipeline(String ms1MatchFilePath, 
            String glycosylationSitesFilePath, String ms2DeconvolutionFilePath,
            String goldStandardModelDataFilePath = null, String outputFilePath = null)
        {
            String argumentStringTemplate = " --rscript-path {0} --ms1-results-file {1} --glycosylation-sites-file {2} --deconvoluted-spectra-file {3} {4} {5}";
            String outFileOpt = outputFilePath == null ? "" : "--out " + outputFilePath.QuoteWrap();
            String goldStandardOpt = goldStandardModelDataFilePath == null ? "" :
                "--gold-standard-file " + goldStandardModelDataFilePath.QuoteWrap();

            String arguments = String.Format(argumentStringTemplate, new String[]{
               this.RscriptExecutablePath, ms1MatchFilePath.QuoteWrap(), glycosylationSitesFilePath.QuoteWrap(), 
               ms2DeconvolutionFilePath.QuoteWrap(), goldStandardOpt, outFileOpt
            });

            dynamic paramExample = new JObject();
            paramExample.rscript_path = this.RscriptExecutablePath;
            paramExample.ms1_results_file = ms1MatchFilePath;
            paramExample.glycosylation_sites_file = glycosylationSitesFilePath;
            paramExample.deconvoluted_spectra_file = ms2DeconvolutionFilePath;
            paramExample.gold_standard_file = goldStandardModelDataFilePath;
            //paramExample.ms1_match_tolerance
            //paramExample.ms2_match_tolerance
            //paramExample.modifications_set
            //paramExample.method
            string tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, paramExample.ToString());
            Console.WriteLine(paramExample);
            Console.WriteLine(tempFile);



            ProcessManager proc = new ProcessManager(this.PythonExecutablePath, 
                Path.Combine(this.ScriptRoot, "__main__.py").QuoteWrap() + " " + arguments);
            
            proc.Start();
            proc.WaitForExit();
            if (!proc.CheckExitSuccessfully())
            {
                throw new PythonScriptErrorException(
                    proc.GenerateDumpMessage()
                    );
            }

            //Sniff the StdOut stream of the pipeline ProcessManager to pull out
            //the output file path. It should be the last line emitted.
            String outputFile = proc.Out.Split(new String[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Last();

            this.PythonPipelineOutFile = outputFile;
            //Store this ProcessManager instance for inspection later
            this.LastCall = proc;

            return outputFile;
        }

        public String RunModelBuildingPythonPipeline(String ms1MatchFilePath,
            String glycosylationSitesFilePath, String ms2DeconvolutionFilePath)
        {
            String argumentStringTemplate = " --rscript-path {0} --ms1-results-file {1} --glycosylation-sites-file {2} --deconvoluted-spectra-file {3}";

            String arguments = String.Format(argumentStringTemplate, new String[]{
               this.RscriptExecutablePath, ms1MatchFilePath.QuoteWrap(), 
               glycosylationSitesFilePath.QuoteWrap(), 
               ms2DeconvolutionFilePath.QuoteWrap()
            });

            ProcessManager proc = new ProcessManager(this.PythonExecutablePath, Path.Combine(this.ScriptRoot, "__main__.py").QuoteWrap() + " " + arguments);

            proc.Start();
            proc.WaitForExit();
            if (!proc.CheckExitSuccessfully())
            {
                throw new PythonScriptErrorException(
                    proc.GenerateDumpMessage()
                    );
            }

            //Sniff the StdOut stream of the pipeline ProcessManager to pull out
            //the output file path. It should be the last line emitted.
            String outputFile = proc.Out.Split(new String[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Last();

            //Store this ProcessManager instance for inspection later
            this.LastCall = proc;
            return outputFile;
        }
    }

    [Serializable]
    public class ScriptingException : TandemGlycoPeptidePipelineException
    {
        public ScriptingException() { }
        public ScriptingException(string message) : base(message) { }
        public ScriptingException(string message, Exception inner) : base(message, inner) { }
        protected ScriptingException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    [Serializable]
    public class ScriptsNotFoundException : ScriptingException
    {
        public ScriptsNotFoundException() { }
        public ScriptsNotFoundException(string message) : base(message) { }
        public ScriptsNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected ScriptsNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    [Serializable]
    public class RscriptNotFoundException : ScriptingException
    {
        public RscriptNotFoundException() { }
        public RscriptNotFoundException(string message) : base(message) { }
        public RscriptNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected RscriptNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    [Serializable]
    public class PythonInterpreterNotFoundException : Exception
    {
        public PythonInterpreterNotFoundException() { }
        public PythonInterpreterNotFoundException(string message) : base(message) { }
        public PythonInterpreterNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected PythonInterpreterNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    [Serializable]
    public class RscriptErrorException : ScriptingException
    {
        public RscriptErrorException() { }
        public RscriptErrorException(string message) : base(message) { }
        public RscriptErrorException(string message, Exception inner) : base(message, inner) { }
        protected RscriptErrorException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    [Serializable]
    public class PythonScriptErrorException : ScriptingException
    {
        public PythonScriptErrorException() { }
        public PythonScriptErrorException(string message) : base(message) { }
        public PythonScriptErrorException(string message, Exception inner) : base(message, inner) { }
        protected PythonScriptErrorException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
