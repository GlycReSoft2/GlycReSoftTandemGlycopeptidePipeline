using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlycReSoft.MS2GUIDriver.ConfigMenus
{
    /// <summary>
    /// Represent the relevant information for interacting with the scripting applications on the host machine
    /// </summary>
    public class ScriptingSettings : JsonConfig
    {
        /// <summary>
        /// Currently unused, but intended for discriminating between different versions of the configuration.
        /// </summary>
        public static double SchemaVersion = 0.1;
        public double Version { get; set; }

        public String PythonInterpreterPath { get; set; }
        public String RscriptInterpreterPath { get; set; }

        public ScriptingSettings()
        {
            Version = SchemaVersion;
            PythonInterpreterPath = "";
            RscriptInterpreterPath = "";
        }

        //public override string ToString()
        //{
        //    return JsonConvert.SerializeObject(this, Formatting.Indented);
        //}

    }
}
