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
using System.Threading;

namespace GlycReSoft.TandemGlycopeptidePipeline
{
    
    /// <summary>
    /// Control the script direction and execution layer of the pipeline programmatically.
    /// </summary>
    public class ScriptManager
    {
        /// <summary>
        /// The name of the entry point script, since multiprocessing, __main__ and Windows do not play
        /// well together under Python 2.7, at least with our set of dependencies.
        /// </summary>
        //public static String EntryPointFileName = "python";

        public static String EntryPointCommand = " -c \"import sys;from pkg_resources import load_entry_point;sys.exit(load_entry_point('GlycReSoft', 'console_scripts', 'glycresoft-ms2')());\" ";

        #region Data Members
        public String PythonExecutablePath;
        public String RscriptExecutablePath;
        public Dictionary<string, string> Data;

        //Storage for retrieving information in another context
        public String PythonPipelineOutFile = "";
        //Useful for both debugging and inspecting the output of the last ProcessManager
        //to exit successfully.
        public ProcessManager LastCall = null;

        public String PythonEntryPoint
        {
            get
            {

                return PythonExecutablePath + EntryPointCommand;
            }
        }

        #endregion

        #region Default Parameters
        /* Configure the default paths to the Python Interpreter,
         * Rscript Interpreter, Python and R Script Files. The interpreter
         * paths are modifiable so that a user can choose a program that is not
         * on their system path. 
         */
        static String DefaultPythonInterpreterPath = "python";
        static String DefaultRscriptPath = "Rscript";
        #endregion
        
        public ScriptManager(String pythonExecutablePath = null, 
            String rscriptExecutablePath = null, 
            String scriptRoot = null)
        {
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

        public bool VerifyFileSystemTargets()
        {
            if (!this.VerifyPythonExecutable())
            {
                throw new PythonInterpreterNotFoundException("Could not locate python");
            }

            return true;
        }
        #endregion

        public void InstallPythonDependencies()
        {
            PythonProcessManager proc = new PythonProcessManager(this.PythonExecutablePath, "-m pip -h");
            proc.Start();
            proc.WaitForExit();
            if (!proc.CheckExitSuccessfully())
            {
                throw new PythonDependencyInstallerException("The Python Package Manager pip wasn't found. Is it installed? If not, go to https://pip.readthedocs.org/en/latest/installing.html for more information on how to get it. " + proc.GenerateDumpMessage());
            }
            proc = new PythonProcessManager(PythonEntryPoint, " -h");
            proc.Start();
            proc.WaitForExit();
            Console.WriteLine(proc.ExitCode);
            if (!proc.CheckExitSuccessfully())
            {
                throw new PythonScriptErrorException(proc.GenerateDumpMessage());
            }
            
        }
        //public void CheckPythonDependencies()
        //{

        //}
        /// <summary>
        /// Executes the Python script pipeline stored at @ScriptRoot, with the 
        /// entry_point.py entry-point. The pipeline emits intermediary file names,
        /// and the last line printed on a successful run is the final output 
        /// file name.
        /// 
        /// Will check the exit code of the pipeline, and throw a PythonScriptErrorException
        /// if it is not 0.
        /// </summary>
        /// <returns>String path to the output file generated by the pipeline</returns>
        public String RunClassificationPythonPipeline(String ms1MatchFilePath, 
            String glycosylationSitesFilePath, String ms2DeconvolutionFilePath,
            String modelDataFilePath, 
            double ms1MatchingTolerance, double ms2MatchingTolerance,
            String proteinProspectorXMLFilePath,
            String method, String outputFilePath = null,
            int numProcesses = 2,
            int decoyToRealRatio = 20,
            bool randomOnly = false
            )
        {
            String argumentStringTemplate = " --n {9}  classify-with-model --ms1-results-file {0} --glycosylation-sites-file {1} --deconvoluted-spectra-file {2} --model-file {3} --ms1-match-tolerance {4} --ms2-match-tolerance {5} --method {6} --protein_prospector_xml {7} {8} --decoy-to-real-ratio {10} {11}";

            String arguments = String.Format(argumentStringTemplate, new object[]{
               ms1MatchFilePath.QuoteWrap(),
               glycosylationSitesFilePath.QuoteWrap(),             
               ms2DeconvolutionFilePath.QuoteWrap(),
               modelDataFilePath.QuoteWrap(), 
               ms1MatchingTolerance, 
               ms2MatchingTolerance,
               method.QuoteWrap(),
               proteinProspectorXMLFilePath.QuoteWrap(),
               outputFilePath == null ? "" : "--out " + outputFilePath.QuoteWrap(), 
               numProcesses,
               decoyToRealRatio,
               randomOnly ? " --random-only " : ""
            });

            PythonProcessManager proc = new PythonProcessManager(PythonEntryPoint, arguments);
            
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
            String outputFile = null;
            try
            {
                outputFile = proc.Out.Split(new String[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Last();
            }
            catch (InvalidOperationException)
            {
                try
                {
                    Thread.Sleep(100);
                    outputFile = proc.Out.Split(new String[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Last();
                }
                catch (InvalidOperationException ex)
                {
                    throw new PythonScriptErrorException("Could not find output file in " + proc.Out);
                }
            }
            

            this.PythonPipelineOutFile = outputFile;
            //Store this ProcessManager instance for inspection later
            this.LastCall = proc;

            return outputFile;
        }

        public String RunModelBuildingPythonPipeline(String ms1MatchFilePath,
            String glycosylationSitesFilePath, String ms2DeconvolutionFilePath,
            double ms1MatchingTolerance, double ms2MatchingTolerance,
            String proteinProspectorXMLFilePath, String method, int numProcesses=2)
        {
            String argumentStringTemplate = " --n {7} build-model --ms1-results-file {0} --glycosylation-sites-file {1} --deconvoluted-spectra-file {2} --ms1-match-tolerance {3} --ms2-match-tolerance {4}" +
                " --protein_prospector_xml {5} --method {6}";

            String arguments = String.Format(argumentStringTemplate, new object[]{
               ms1MatchFilePath.QuoteWrap(), 
               glycosylationSitesFilePath.QuoteWrap(), 
               ms2DeconvolutionFilePath.QuoteWrap(), ms1MatchingTolerance, 
               ms2MatchingTolerance, proteinProspectorXMLFilePath.QuoteWrap(),
               method.QuoteWrap(), numProcesses
            });

            Console.WriteLine(arguments);

            PythonProcessManager proc = new PythonProcessManager(PythonEntryPoint, arguments);

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
            String outputFile = null;
            try
            {
                outputFile = proc.Out.Split(new String[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Last();
            }
            catch (InvalidOperationException)
            {
                try
                {
                    Thread.Sleep(100);
                    outputFile = proc.Out.Split(new String[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Last();
                }
                catch (InvalidOperationException ex)
                {
                    throw new PythonScriptErrorException("Could not find output file in " + proc.Out, new PythonScriptErrorException(
                    proc.GenerateDumpMessage()));
                }
            }

            //Store this ProcessManager instance for inspection later
            this.LastCall = proc;
            return outputFile;
        }

        /// <summary>
        /// Given a model file with annotations, try to generate some diagnostic information about the model, 
        /// and generate plots to describe them.
        /// </summary>
        /// <param name="modelFilePath"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public String RunModelDiagnosticTask(String modelFilePath, String method)
        {
            String argumentStringTemplate = " model-diagnostics --model-file {0} --method {1}";

            String arguments = String.Format(argumentStringTemplate, modelFilePath.QuoteWrap(), method);
            PythonProcessManager proc = new PythonProcessManager(PythonEntryPoint, arguments);

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
            String outputFile = null;
            try
            {
                outputFile = proc.Out.Split(new String[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Last();
            }
            catch (InvalidOperationException)
            {
                try
                {
                    Thread.Sleep(100);
                    outputFile = proc.Out.Split(new String[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Last();
                }
                catch (InvalidOperationException ex)
                {
                    throw new PythonScriptErrorException("Could not find output file in " + proc.Out);
                }
            }

            //Store this ProcessManager instance for inspection later
            this.LastCall = proc;
            return outputFile;
        }

        public String RunReclassifyWithModelTask(String targetFilePath, String modelFilePath, String method)
        {
            String argumentStringTemplate = " reclassify-with-model --target-file {0} --model-file {1} --method {2}";
            String arguments = String.Format(argumentStringTemplate, targetFilePath.QuoteWrap(), modelFilePath.QuoteWrap(), method.QuoteWrap());
            PythonProcessManager proc = new PythonProcessManager(PythonEntryPoint, arguments);

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
            String outputFile = null;
            try
            {
                outputFile = proc.Out.Split(new String[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Last();
            }
            catch (InvalidOperationException)
            {
                try
                {
                    Thread.Sleep(100);
                    outputFile = proc.Out.Split(new String[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Last();
                }
                catch (InvalidOperationException ex)
                {
                    throw new PythonScriptErrorException("Could not find output file in " + proc.Out);
                }
            }

            //Store this ProcessManager instance for inspection later
            this.LastCall = proc;
            return outputFile;
        }

        public String ConvertToCSV(String targetFilePath, String resultFile=null)
        {
            resultFile = resultFile == null ?
                Path.Combine(Path.GetDirectoryName(targetFilePath), Path.GetFileNameWithoutExtension(targetFilePath) + ".csv") : resultFile;

            String arguments = String.Format(
" -c \"from glycresoft_ms2_classification import prediction_tools;data=prediction_tools.prepare_model_file(r'{0}');data.to_csv(r'{1}')\"", 
targetFilePath, resultFile);
            Console.WriteLine(arguments);
            PythonProcessManager proc = new PythonProcessManager(PythonExecutablePath, arguments);

            proc.Start();
            proc.WaitForExit();
            if (!proc.CheckExitSuccessfully())
            {
                throw new PythonScriptErrorException(
                    proc.GenerateDumpMessage()
                    );
            }


            return resultFile;
        }
    }

    public static class ArgumentPassingUtility
    {
        public static string IterableToAppendArgument(String optionName, IEnumerable<String> parameters, bool uriEncode = false)
        {
            string output = "";
            if (parameters == null) return output;
            foreach (string param in parameters)
            {
                output += string.Format(" {0} {1} ", optionName, (uriEncode ? Uri.EscapeDataString(param) : param).QuoteWrap());
            }
            return output;
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

    [Serializable]
    public class PythonDependencyInstallerException : ScriptingException
    {
        public PythonDependencyInstallerException() { }
        public PythonDependencyInstallerException(string message) : base(message) { }
        public PythonDependencyInstallerException(string message, Exception inner) : base(message, inner) { }
        protected PythonDependencyInstallerException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
