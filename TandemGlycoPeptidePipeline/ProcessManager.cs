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

        public String Out = "";
        public String Err = "";
        ProcessStartInfo Info;
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
            Info.CreateNoWindow = true;
            
            Info.RedirectStandardOutput = true;
            Info.RedirectStandardError = true;

            InitProc();
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
}
