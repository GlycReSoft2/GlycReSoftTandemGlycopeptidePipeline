using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycReSoft.TandemGlycopeptidePipeline
{
    public class ProcessManager
    {

        public static bool Verbose = false;
        public static bool NoWindow = true;
        public static bool DebugOnly = false;

        public String Out = "";
        public String Err = "";
        protected ProcessStartInfo Info;
        protected Process Proc = null;      

        public int ExitCode { get {
            try
            {
                return Proc.ExitCode;
            }
            catch (InvalidOperationException e)
            {
                return 255;
            }
            
        } }
        
        public ProcessManager(String fileName, String args = ""){
            this.Info = new ProcessStartInfo();
            Info.FileName = fileName;
            Info.Arguments = args;

            Info.UseShellExecute = false;
            Info.CreateNoWindow = NoWindow;
            
            Info.RedirectStandardOutput = true;
            Info.RedirectStandardError = true;
        }

        public String CmdStr { get { return Info.FileName + " " + Info.Arguments; } }

        private void SpoolStdOut(object sendingProcess, DataReceivedEventArgs outLine)
        {
            if (Verbose)
            {
                Console.WriteLine("Out: {0}", outLine.Data);
                Console.Out.Flush();
            }
            this.Out += outLine.Data + Environment.NewLine;
        }

        private void SpoolStdErr(object sendingProcess, DataReceivedEventArgs outLine)
        {
            if (Verbose)
            {
                Console.WriteLine("Err: {0}", outLine.Data);
                Console.Out.Flush();
            }
            this.Err += outLine.Data + Environment.NewLine;        
        }

        public void InitProc()
        {
            this.Proc = new Process();
            Proc.StartInfo = this.Info;
            Proc.OutputDataReceived += new DataReceivedEventHandler(this.SpoolStdOut);
            Proc.ErrorDataReceived += new DataReceivedEventHandler(this.SpoolStdErr);            
        }

        public void Start()
        {
            
            try
            {                
                if (Proc == null)
                {
                    InitProc();
                }
                if (DebugOnly)
                {
                    throw new Exception("Short-Circuit");
                }
                Proc.Start();
                Proc.BeginOutputReadLine();
                Proc.BeginErrorReadLine();
            }
            catch (Exception e)
            {
                throw new Exception(this.GenerateDumpMessage() + " Failed: \n" + e.Message, e);
            }
        }

        public void WaitForExit()
        {
            Proc.WaitForExit();
        }

        public bool CheckExitSuccessfully(int exitCode = 0)
        {
            return exitCode == this.ExitCode;
        }

        public String GenerateDumpMessage()
        {
            String msgTemplate =
@"{CmdStr}
---------------------
Exit Code: {ExitCode}
---------------------
Standard Output Stream Dump = {Out}
---------------------
Standard Error Stream Dump = {Err}
---------------------
";
            //String msg = this.CmdStr + Environment.NewLine + "-----------" + 
            //        Environment.NewLine + this.ExitCode.ToString() + 
            //        Environment.NewLine + this.Out + Environment.NewLine + 
            //        "-----------" + Environment.NewLine + this.Err;

            return msgTemplate.FormatWith(this);

        }

    }

    public class PythonProcessManager : ProcessManager
    {
        public String PythonPath;

        public PythonProcessManager(String fileName, String args = "", String pythonPathUpdate = null)
            : base(fileName, args)
        {
            PythonPath = this.Info.EnvironmentVariables["PYTHONPATH"];
            if(pythonPathUpdate != null){
                PythonPath = String.Format("{0};{1}", pythonPathUpdate, PythonPath);
                this.Info.EnvironmentVariables["PYTHONPATH"] = PythonPath;
            }
        }

        public new String GenerateDumpMessage()
        {
            String msgTemplate =
@"{CmdStr}
---------------------
Exit Code: {ExitCode}
---------------------
PythonPath: {PythonPath}
---------------------
Standard Output Stream Dump = {Out}
---------------------
Standard Error Stream Dump = {Err}
---------------------
";
            //String msg = this.CmdStr + Environment.NewLine + "-----------" + 
            //        Environment.NewLine + this.ExitCode.ToString() + 
            //        Environment.NewLine + this.Out + Environment.NewLine + 
            //        "-----------" + Environment.NewLine + this.Err;

            return msgTemplate.FormatWith(this);

        }
    }
}
