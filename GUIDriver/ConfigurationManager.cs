using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows.Forms;
using GlycReSoft.MS2GUIDriver.ConfigMenus;

namespace GlycReSoft.MS2GUIDriver
{

    public class JsonConfig
    {
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }

    /// <summary>
    /// Handles reading and writing of configuration information to and from disk for persistent but volatile data representing the host machine's settings and services. Currently provides services related locating the Python and Rscript executables through the ScriptingSettings class.
    /// </summary>
    public class ConfigurationManager
    {
        /// <summary>
        /// The defacto file name for storing configurations
        /// </summary>
        static string SCRIPT_CONF_FILE_NAME = "Scripting.conf.json";
        static string ALGORITHM_CONF_FILE_NAME = "Algorithm.conf.json";

        /// <summary>
        /// The globally shared settings.
        /// </summary>
        public static ScriptingSettings Scripting = null;
        public static AlgorithmSettings Algorithm = null;

        /// <summary>
        /// Attempts to load the scripting settings from the defacto file at the given path.
        /// 
        /// If the file does not exist, it will create a new, empty file there. 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static ScriptingSettings LoadScriptingSettings(String path)
        {
            String targetPath = Path.Combine(path, SCRIPT_CONF_FILE_NAME);
            ScriptingSettings settings = new ScriptingSettings();
            try
            {
                string raw = File.ReadAllText(targetPath);
                settings = JsonConvert.DeserializeObject<ScriptingSettings>(raw);
                if (settings.Version < ScriptingSettings.SchemaVersion)
                {
                    MessageBox.Show("Your scripting parameters are outdated and have been set to defaults. Please updated them.", "Settings Outdated");
                    settings = new ScriptingSettings();
                    throw new SettingsFileOutdatedException();
                }
            }
            catch
            {
                try
                {
                    WriteScriptingSettingsToFile(path, settings);
                }
                catch
                {
                    throw new SettingFileException(string.Format("Could not access settings file at {0}", targetPath));
                }
            }
            Console.WriteLine(settings);
            return settings;
        }

        /// <summary>
        /// Write the given scripting settings to the defacto file at the given path.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static String WriteScriptingSettingsToFile(String path, ScriptingSettings settings)
        {
            String outputFile = Path.Combine(path, SCRIPT_CONF_FILE_NAME);
            StreamWriter writer = new StreamWriter(
                new FileStream(
                    outputFile, 
                    FileMode.OpenOrCreate,
                    FileAccess.Write)
                );
            String content = JsonConvert.SerializeObject(settings);
            writer.Write(content);
            writer.Close();
            return outputFile;
        }


        public static AlgorithmSettings LoadAlgorithmSettings(String path)
        {
            String targetPath = Path.Combine(path, ALGORITHM_CONF_FILE_NAME);
            AlgorithmSettings settings = new AlgorithmSettings();
            try
            {
                string raw = File.ReadAllText(targetPath);
                settings = JsonConvert.DeserializeObject<AlgorithmSettings>(raw);
                if (settings.Version < AlgorithmSettings.SchemaVersion)
                {
                    MessageBox.Show("Your algorithm parameters are outdated and have been set to defaults. Please updated them.", "Settings Outdated");
                    settings = new AlgorithmSettings();
                    throw new SettingsFileOutdatedException();
                }
            }
            catch
            {
                try
                {
                    WriteAlgorithmSettingsToFile(path, settings);
                }
                catch
                {
                    throw new SettingFileException(string.Format("Could not access settings file at {0}", targetPath));
                }
            }

            Console.WriteLine(settings);
            return settings;
        }

        public static string WriteAlgorithmSettingsToFile(string path, AlgorithmSettings settings)
        {
            String outputFile = Path.Combine(path, ALGORITHM_CONF_FILE_NAME);
            StreamWriter writer = new StreamWriter(
                new FileStream(
                    outputFile,
                    FileMode.OpenOrCreate,
                    FileAccess.Write)
                );
            String content = JsonConvert.SerializeObject(settings);
            writer.Write(content);
            writer.Close();
            return outputFile;
        }
    }

    [Serializable]
    public class SettingFileException : Exception
    {
        public SettingFileException() { }
        public SettingFileException(string message) : base(message) { }
        public SettingFileException(string message, Exception inner) : base(message, inner) { }
        protected SettingFileException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
    [Serializable]
    public class SettingsFileOutdatedException : SettingFileException
    {
        public SettingsFileOutdatedException() { }
        public SettingsFileOutdatedException(string message) : base(message) { }
        public SettingsFileOutdatedException(string message, Exception inner) : base(message, inner) { }
        protected SettingsFileOutdatedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
