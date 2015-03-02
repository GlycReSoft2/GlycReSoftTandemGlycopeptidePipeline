using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycReSoft.TandemMSGlycopeptideGUI.ConfigMenus
{
    public enum DependencyInstalledState
    {
        Unknown, Yes, No
    }
    /// <summary>
    /// Represent the relevant information for interacting with the scripting applications on the host machine
    /// </summary>
    public class ScriptingSettings : JsonConfig
    {
        /// <summary>
        /// Currently unused, but intended for discriminating between different versions of the configuration.
        /// </summary>
        public static double SchemaVersion = 0.4;
        public double Version { get; set; }

        public String PythonInterpreterPath { get; set; }
        public DependencyInstalledState DependenciesFound { get; set; }
        public Boolean PythonDependenciesInstalled {
            get
            {
                return (DependenciesFound == DependencyInstalledState.Yes);
            }
            set
            {
                if (value)
                {
                    DependenciesFound = DependencyInstalledState.Yes;
                }
                else
                {
                    DependenciesFound = DependencyInstalledState.No;
                }
            }
        }

        public ScriptingSettings()
        {
            Version = SchemaVersion;
            PythonInterpreterPath = "";
            DependenciesFound = DependencyInstalledState.Unknown;
        }

        //public override string ToString()
        //{
        //    return JsonConvert.SerializeObject(this, Formatting.Indented);
        //}

    }
}
