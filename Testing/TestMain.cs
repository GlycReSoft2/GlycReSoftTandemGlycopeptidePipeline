using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlycReSoft.TandemGlycopeptidePipeline;

namespace Testing
{
    class TestMain
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting TestMain");
            Console.WriteLine(Properties.Resources.TestMS1Matches);
            Console.WriteLine(Properties.Resources.TestMS1Matches.Replace("\"",""));

            TestModelBuildRun();
            TestFullRun();
            Console.WriteLine("Any key to continue");
            Console.Read();
        }

        static void TestScripting()
        {
            ScriptManager scripter = new ScriptManager(rscriptExecutablePath: Properties.Resources.DevelRscriptPath);

            Console.WriteLine(scripter);

            scripter.VerifyFileSystemTargets();

            scripter.InstallRDependencies();
            Console.WriteLine(scripter.LastCall.GenerateDumpMessage());

            scripter.RunClassificationPythonPipeline(Properties.Resources.TestMS1Matches,
                Properties.Resources.TestGlycosylationSites, Properties.Resources.TestDeconvolutedMS2,
                Properties.Resources.TestGoldStandard, "test.csv");
            Console.WriteLine(scripter.LastCall.GenerateDumpMessage());
        }

        static void TestCsvParsing(String path)
        {

            ResultsRepresentation results = ResultsRepresentation.ParseCsv(path);
            int i = 0;
            foreach (GlycopeptidePrediction pred in results.MatchedPredictions)
            {
                Console.WriteLine(pred);
                i++;
                if (i > 5) break;
            }
        }

        static ResultsRepresentation TestFullRun()
        {
            AnalysisPipeline pipeline = new AnalysisPipeline(Properties.Resources.TestMS1Matches,
                Properties.Resources.TestGlycosylationSites, Properties.Resources.TestDeconvolutedMS2,
                Properties.Resources.TestGoldStandard, "test.csv", rscriptPath: Properties.Resources.DevelRscriptPath);
            ResultsRepresentation results = null;
            try
            {
                results = pipeline.RunClassification();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            

            int i = 0;
            foreach (GlycopeptidePrediction pred in results.MatchedPredictions)
            {
                Console.WriteLine(pred);
                i++;
                if (i > 5) break;
            }
            Console.WriteLine(pipeline.Scripter.LastCall.Out);
            return results;            
        }

        static ResultsRepresentation TestModelBuildRun()
        {
            AnalysisPipeline pipeline = new AnalysisPipeline(Properties.Resources.TestMS1Matches,
                Properties.Resources.TestGlycosylationSites, Properties.Resources.TestDeconvolutedMS2,
                Properties.Resources.TestGoldStandard, "test.csv", rscriptPath: Properties.Resources.DevelRscriptPath);
            ResultsRepresentation model = null;
            try
            {
                var resultsFile = pipeline.Scripter.RunModelBuildingPythonPipeline(pipeline.MS1MatchFilePath, pipeline.GlycosylationSiteFilePath, pipeline.MS2DeconFilePath);
                Console.WriteLine(resultsFile);
                model = new ResultsRepresentation(resultsFile);
                int i = 0;
                foreach (GlycopeptidePrediction pred in model.MatchedPredictions)
                {
                    Console.WriteLine(pred);
                    i++;
                    if (i > 5) break;
                }
                Console.WriteLine(pipeline.Scripter.LastCall.Out);
                return model;  
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message, e);
                throw;
            }
        }

        static void TestIonFragmentParsing()
        {
            String raw = @"[]
";
            Console.WriteLine(raw);
            var frags = IonFragment.ParseArray(raw);
            foreach (IonFragment ifrag in frags)
                Console.WriteLine(ifrag);
            Console.WriteLine(frags);
        }
    } 
}
