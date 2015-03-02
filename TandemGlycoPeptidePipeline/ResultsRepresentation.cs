using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.ComponentModel;
using System.Globalization;

namespace GlycReSoft.TandemGlycopeptidePipeline
{
    public class ResultsRepresentation
    {
        [JsonProperty("predictions")]
        public List<GlycopeptidePrediction> MatchedPredictions;
        [JsonProperty("metadata")]
        //public Dictionary<String, dynamic> Metadata;
        public JObject Metadata;
        [JsonIgnore()]
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
            String textRepresentation = textReader.ReadToEnd();
            ResultsRepresentation interm = JsonConvert.DeserializeObject<ResultsRepresentation>(textRepresentation);
            this.MatchedPredictions = interm.MatchedPredictions;
            this.Metadata = interm.Metadata;            
            this.SourceFile = resultsFilePath;
            textReader.Close();
        }

        public ResultsRepresentation(StreamReader reader, String fileName)
        {
            String textRepresentation = reader.ReadToEnd();
            ResultsRepresentation interm = JsonConvert.DeserializeObject<ResultsRepresentation>(textRepresentation);
            this.MatchedPredictions = interm.MatchedPredictions;
            this.Metadata = interm.Metadata;          
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
        [JsonProperty("MS1_Score")]
        public double MS1Score {get; set;}
        [JsonProperty("Obs_Mass")]
        public double ObsMass { get; set; }
        [JsonProperty("Calc_mass")]
        public double CalcMass{get; set;}
        [JsonProperty("ppm_error")]
        public double PPMError{get; set;}
        [JsonProperty("Peptide")]
        public String Peptide{get; set;}
        [JsonProperty("Peptide_mod")]
        public String PeptideMod{get; set;}
        [JsonProperty("Glycan")]
        public String Glycan{get; set;}
        [JsonProperty("peptideLens")]
        public int PeptideLens{get; set;}
        [JsonProperty("vol")]
        public double Volume{get; set;}

        [JsonProperty("glyco_sites")]
        public int NumGlycoSites{get; set;}
        [JsonProperty("startAA")]
        public int StatAA{get; set;}
        [JsonProperty("endAA")]
        public int EndAA{get; set;}

        [JsonProperty("Seq_with_mod")]
        public String SeqWithMod{get; set;}
        [JsonProperty("Glycopeptide_identifier")]
        public String GlycopeptideIdentifier{get; set;}
        [JsonProperty("Oxonium_ions")]
        public IonFragment[] OxoniumIons{get; set;}
        [JsonProperty("numOxIons")]
        public int NumOxIons{get; set;}
        [JsonProperty("bare_b_ions")]
        public IonFragment[] BareBIons{get; set;}
        [JsonProperty("total_b_ions_possible")]
        public double TotalBIonsPossible { get; set; }

        [JsonProperty("bare_y_ions")]
        public IonFragment[] BareYIons{get; set;}
        [JsonProperty("total_y_ions_possible")]
        public double TotalYIonsPossible { get; set; }

        [JsonProperty("b_ions_with_HexNAc")]
        public IonFragment[] BIonsWithHexNAc{get; set;}
        [JsonProperty("possible_b_ions_HexNAc")]
        public double PercentBIonWithHexNAcCoverage { get; set; }

        [JsonProperty("y_ions_with_HexNAc")]
        public IonFragment[] YIonsWithHexNAc{get; set;}
        [JsonProperty("possible_y_ions_HexNAc")]
        public double PercentYIonWithHexNAcCoverage { get; set; }

        [JsonProperty("b_ion_coverage")]
        public IonFragment[] BIonCoverage{get; set;}
        [JsonProperty("percent_b_ion_coverage")]
        public double PercentBIonCoverage { get; set; }

        [JsonProperty("y_ion_coverage")]
        public IonFragment[] YIonCoverage{get; set;}
        [JsonProperty("percent_y_ion_coverage")]
        public double PercentYIonCoverage { get; set; }

        [JsonProperty("Stub_ions")]
        public IonFragment[] StubIons{get; set;}
        [JsonProperty("numStubs")]
        public int NumStubsFound{get; set;}

        [JsonProperty("meanCoverage")]
        public double MeanPerSiteCoverage{get; set;}
        [JsonProperty("percentUncovered")]
        public double PercentUncovered{get; set;}

        [JsonProperty("meanHexNAcCoverage")]
        public double MeanHexNAcIonCoverage { get; set;}

        [JsonProperty("peptideCoverageMap")]
        public double[] PeptideCoverageMap { get; set;}
        [JsonProperty("bIonCoverageMap")]
        public double[] BIonPeptideCoverageMap { get; set; }
        [JsonProperty("yIonCoverageMap")]
        public double[] YIonPeptideCoverageMap { get; set; }
        [JsonProperty("hexNAcCoverageMap")]
        public double[] HexNAcCoverageMap { get; set; }
        [JsonProperty("bIonCoverageWithHexNAcMap")]
        public double[] BIonHexNAcCoverageMap { get; set; }
        [JsonProperty("yIonCoverageWithHexNAcMap")]
        public double[] YIonHexNAcCoverageMap { get; set; }

        [JsonProperty("MS2_Score")]
        public double MS2Score { get; set; }

        [JsonProperty("call")]
        public bool Call{get; set;}
        [JsonProperty("ambiguity")]
        public bool Ambiguity { get; set; }

        [JsonProperty("scan_id")]
        public string ScanId { get; set; }

        public Dictionary<String, int> GlycanComposition { get; set; }
        
        
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

    public class FalseDiscoveryRateTestResult {
        [JsonProperty("num_real_matches")]
        public int RealIonsMatched { get; set; }
        [JsonProperty("num_decoy_matches")]
        public int DecoyIonsMatched { get; set; }
        [JsonProperty("false_discovery_rate")]
        public float FalseDiscoveryRate { get; set; }
        [JsonProperty("modification_signature")]
        public string ModificationSignature { get; set; }
        [JsonProperty("threshold")]
        public Dictionary<string, dynamic> Threshold { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }


}
