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

        static String[] Modifications = {"Deamidated (N)", "Deamidated (Q)", "Dehydrated (C-term)", "Dehydrated (D)", "Carbamidomethyl (C)"};

        static void Main(string[] args)
        {
            Console.WriteLine("Starting TestMain");
            
            //TestModelBuildRun();
            TestFullRun();

            Console.WriteLine("Any key to continue");
            Console.Read();
        }

        static void TestScriptingInstallDependencies()
        {
            Console.WriteLine("Test Scripting Install Dependencies");
            ScriptManager scripter = new ScriptManager(rscriptExecutablePath: Properties.Resources.DevelRscriptPath);

            Console.WriteLine(scripter);

            scripter.VerifyFileSystemTargets();

            scripter.InstallRDependencies();
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
            Console.WriteLine("TestFullRun");
            AnalysisPipeline pipeline = new AnalysisPipeline(Properties.Resources.TestMS1Matches,
                Properties.Resources.TestGlycosylationSites, Properties.Resources.TestDeconvolutedMS2,
                Properties.Resources.TestGoldStandard, 
                ms1MatchingTolerance:1e-5,
                ms2MatchingTolerance:2e-5,
                constantModifications: Modifications,
                variableModifications: Modifications,
                method:"default",
                outputFilePath:"test.csv",                 
                rscriptPath: Properties.Resources.DevelRscriptPath);
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
            Console.WriteLine("TestModelBuildRun");
            AnalysisPipeline pipeline = new AnalysisPipeline(Properties.Resources.TestMS1Matches,
                Properties.Resources.TestGlycosylationSites, 
                Properties.Resources.TestDeconvolutedMS2,
                Properties.Resources.TestGoldStandard,
                method: "default",
                constantModifications: Modifications,
                variableModifications: Modifications,
                ms1MatchingTolerance: 1e-5,
                ms2MatchingTolerance: 2e-5,
                outputFilePath:"test.csv", rscriptPath: Properties.Resources.DevelRscriptPath);
            ResultsRepresentation model = null;
            try
            {
                model = pipeline.RunModelBuilder();
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
