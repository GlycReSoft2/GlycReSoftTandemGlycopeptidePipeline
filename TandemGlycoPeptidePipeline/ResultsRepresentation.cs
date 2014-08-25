using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System.IO;
using System.ComponentModel;
using System.Globalization;

namespace GlycReSoft.TandemGlycopeptidePipeline
{
    public class ResultsRepresentation
    {
        public List<GlycopeptidePrediction> MatchedPredictions;
        public String SourceFile = null;

        public ResultsRepresentation()
        {
            MatchedPredictions = new List<GlycopeptidePrediction>();
            SourceFile = "";
        }

        /// <summary>
        /// Parses the given CSV file into a List of GlycopeptidePrediction objects
        /// and stores the information.
        /// </summary>
        /// <param name="resultsFilePath"></param>
        public ResultsRepresentation(String resultsFilePath)
        {
            StreamReader textReader = new StreamReader(resultsFilePath);
            CsvReader csv = new CsvReader(textReader);
            csv.Configuration.SkipEmptyRecords = true;
            csv.Configuration.RegisterClassMap<GlycopeptidePredictionMap>();
            TypeConverterFactory.AddConverter<IonFragment[]>(new IonFragmentConverter());
            this.MatchedPredictions = csv.GetRecords<GlycopeptidePrediction>().ToList();
            this.SourceFile = resultsFilePath;
            textReader.Close();
        }

        public ResultsRepresentation(StreamReader reader, String fileName)
        {
            CsvReader csv = new CsvReader(reader);
            csv.Configuration.SkipEmptyRecords = true;
            csv.Configuration.RegisterClassMap<GlycopeptidePredictionMap>();
            TypeConverterFactory.AddConverter<IonFragment[]>(new IonFragmentConverter());
            this.MatchedPredictions = csv.GetRecords<GlycopeptidePrediction>().ToList();
            this.SourceFile = fileName;
            reader.Close();
        }

        /// <summary>
        /// A static level wrapper for parsing a CSV file into a results representation
        /// </summary>
        /// <param name="resultsFilePath"></param>
        /// <returns></returns>
        public static ResultsRepresentation ParseCsv(String resultsFilePath)
        {
            return new ResultsRepresentation(resultsFilePath);
        }

        public void WriteToCsv(String filePath)
        {
            StreamWriter writer = new StreamWriter(File.OpenWrite(filePath));
            this.WriteToCsv(writer);
        }

        public void WriteToCsv(StreamWriter writer, bool closeAtEnd = true)
        {
            CsvWriter csv = new CsvWriter(writer);
            csv.Configuration.RegisterClassMap<GlycopeptidePredictionMap>();
            csv.Configuration.SkipEmptyRecords = true;
            TypeConverterFactory.AddConverter<IonFragment[]>(new IonFragmentConverter());
            TypeConverterFactory.AddConverter<CallRFactorBoolConverter>(new CallRFactorBoolConverter());
            csv.WriteRecords(MatchedPredictions);
            if(closeAtEnd) writer.Close();
        }

        public void WriteToJson(StreamWriter writer, bool closeAtEnd = true)
        {
            String serialized = JsonConvert.SerializeObject(this, Formatting.Indented);
            writer.Write(serialized);
            if (closeAtEnd) writer.Close();
        }

    }

    /// <summary>
    /// Represents a single Glycopeptide 
    /// </summary>
    public class GlycopeptidePrediction
    {
        public double MS1Score {get; set;}

        public double ObsMass { get; set; }
        public double CalcMass{get; set;}

        public double PPMError{get; set;}
        public double AbsPPMError{get; set;}

        public String Peptide{get; set;}
        public String PeptideMod{get; set;}
        public String Glycan{get; set;}
        public int PeptideLens{get; set;}

        public double Volume{get; set;}

        public int GlycoSites{get; set;}
        public int StatAA{get; set;}
        public int EndAA{get; set;}

        public String SeqWithMod{get; set;}
        public String GlycopeptideIdentifier{get; set;}

        public IonFragment[] OxoniumIons{get; set;}
        public int NumOxIons{get; set;}

        public IonFragment[] BareBIons{get; set;}
        public int TotalBIonsPossible{get; set;}

        public IonFragment[] BareYIons{get; set;}
        public int TotalYIonsPossible{get; set;}

        public IonFragment[] BIonsWithHexNAc{get; set;}
        public double PercentBIonWithHexNAcCoverage { get; set; }

        public IonFragment[] YIonsWithHexNAc{get; set;}
        public double PercentYIonWithHexNAcCoverage { get; set; }

        public IonFragment[] BIonCoverage{get; set;}
        public double PercentBIonCoverage { get; set; }

        public IonFragment[] YIonCoverage{get; set;}
        public double PercentYIonCoverage { get; set; }

        public IonFragment[] StubIons{get; set;}

        public int NumStubsFound{get; set;}
        public double BestCoverage{get; set;}

        public double MeanPerSiteCoverage{get; set;}
        public double PercentUncovered{get; set;}

        public double MS2Score { get; set; }
        
        public bool Call{get; set;}
        public bool Ambiguity { get; set; }

        public String IonCounts
        {
            get
            {
                try
                {
                    return String.Format("B:{0}({1}), Y:{2}({3})",
                        BareBIons.Count(), BIonsWithHexNAc.Count(),
                        BareYIons.Count(), YIonsWithHexNAc.Count());
                }
                catch
                {
                    return "-";
                }
            }
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

    }
    public sealed class GlycopeptidePredictionMap : CsvClassMap<GlycopeptidePrediction>
    {
        public GlycopeptidePredictionMap()
        {
            Map(m => m.MS1Score).Name("MS1_Score");
            Map(m => m.MS2Score).Name("MS2_Score").Default(0);
            Map(m => m.ObsMass).Name("Obs_Mass");
            Map(m => m.CalcMass).Name("Calc_mass");
            Map(m => m.PPMError).Name("ppm_error");
            Map(m => m.AbsPPMError).Name("abs_ppm_error");
            Map(m => m.Peptide).Name("Peptide");
            Map(m => m.PeptideMod).Name("Peptide_mod");
            Map(m => m.Glycan).Name("Glycan");
            Map(m => m.PeptideLens).Name("peptideLens");
            Map(m => m.Volume).Name("vol");
            Map(m => m.GlycoSites).Name("glyco_sites");
            Map(m => m.StatAA).Name("startAA");
            Map(m => m.EndAA).Name("endAA");
            Map(m => m.SeqWithMod).Name("Seq_with_mod");
            Map(m => m.GlycopeptideIdentifier).Name("Glycopeptide_identifier");
            Map(m => m.OxoniumIons).Name("Oxonium_ions").TypeConverter<IonFragmentConverter>();
            Map(m => m.NumOxIons).Name("numOxIons");
            Map(m => m.BareBIons).Name("bare_b_ions").TypeConverter<IonFragmentConverter>();
            Map(m => m.TotalBIonsPossible).Name("total_b_ions_possible");
            Map(m => m.BareYIons).Name("bare_y_ions").TypeConverter<IonFragmentConverter>();
            Map(m => m.TotalYIonsPossible).Name("total_y_ions_possible");

            Map(m => m.BIonsWithHexNAc).Name("b_ions_with_HexNAc").TypeConverter<IonFragmentConverter>();
            Map(m => m.YIonsWithHexNAc).Name("y_ions_with_HexNAc").TypeConverter<IonFragmentConverter>();
            Map(m => m.BIonCoverage).Name("b_ion_coverage").TypeConverter<IonFragmentConverter>();
            Map(m => m.YIonCoverage).Name("y_ion_coverage").TypeConverter<IonFragmentConverter>();

            Map(m => m.PercentBIonCoverage).Name("percent_b_ion_coverage").Default(0);
            Map(m => m.PercentYIonCoverage).Name("percent_y_ion_coverage").Default(0);
            Map(m => m.PercentBIonWithHexNAcCoverage).Name("percent_b_ion_with_HexNAc_coverage").Default(0);
            Map(m => m.PercentYIonWithHexNAcCoverage).Name("percent_y_ion_with_HexNAc_coverage").Default(0);
            
            Map(m => m.StubIons).Name("Stub_ions").TypeConverter<IonFragmentConverter>();
            Map(m => m.NumStubsFound).Name("numStubs");
            Map(m => m.BestCoverage).Name("bestCoverage");
            Map(m => m.MeanPerSiteCoverage).Name("meanCoverage");
            Map(m => m.PercentUncovered).Name("percentUncovered");
            Map(m => m.Call).Name("call").TypeConverter<CallRFactorBoolConverter>();
            Map(m => m.Ambiguity).Name("ambiguity").TypeConverterOption(true, "TRUE").TypeConverterOption(false, "FALSE");
        }
    }

    public class IonFragment
    {
        [JsonProperty("ppm_error")]
        double PPMError {get;set;}
        [JsonProperty("obs_ion")]
        double ObsIon { get; set; }
        [JsonProperty("key")]
        String Key { get; set; }

        public IonFragment(double ppmErr = 0, double obsIon = 0, String key = null)
        {
            this.PPMError = ppmErr;
            this.ObsIon = obsIon;
            this.Key = key;
        }

        public static IonFragment[] ParseArray(String raw)
        {
            if(raw  == ""){
                return new IonFragment[0];
            }
            IonFragment[] tmp = JsonConvert.DeserializeObject<IonFragment[]>(raw);
            return tmp;
        }

        override public String ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

    }

    public class IonFragmentConverter : EnumerableConverter 
    {
        public override bool CanConvertFrom(System.Type type)
        {
            if (type == typeof(String))
            {
                return true;
            }
            return false;
        }
        public override object ConvertFromString(TypeConverterOptions options, String text)
        {
            IonFragment[] frags = IonFragment.ParseArray(text);
            return frags;
        }

        public override String ConvertToString(TypeConverterOptions options, object value)
        {
            return JsonConvert.SerializeObject((IonFragment[])value);
        }
    }

    public class CallRFactorBoolConverter : CsvHelper.TypeConversion.BooleanConverter
    {

        //public bool CanConvertFrom(Type type)
        //{
        //    return type == typeof(string);
        //}

        //public bool CanConvertTo(Type type)
        //{
        //    return type == typeof(bool);
        //}

        public override string ConvertToString(TypeConverterOptions options, object value)
        {
            //Console.Write("In ConvertTo: {0} ", (bool)value);
            if (value == null)
            {
                return "Dud";
            }
            var boolVal = (bool)value;
            var retVal = boolVal ? "Yes" : "No";
            return retVal;
        }

        public override object ConvertFromString(TypeConverterOptions options, String text)
        {
            //Console.WriteLine("In ConvertFrom {0}", text);
            if (text == "Yes")
            {
                return true;
            }
            else if (text == "No")
            {
                return false;
            }
            else return false;
            
        }
    }

}
