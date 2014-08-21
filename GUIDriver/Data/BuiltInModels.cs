using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace GlycReSoft.MS2GUIDriver.Data
{
    public static class BuiltInModels
    {
        public static Models Load()
        {
            string path = (Path.Combine(Application.StartupPath, "Data", "Models.xml"));           
            XmlSerializer serializer = new XmlSerializer(typeof(Models));
            return (Models)serializer.Deserialize(File.Open(path, FileMode.Open, FileAccess.Read));
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class Models
    {

        private Model[] modelField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Model")]
        public Model[] Model
        {
            get
            {
                return this.modelField;
            }
            set
            {
                this.modelField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class Model
    {

        private string nameField;

        private string pathField;

        private string notesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Path
        {
            get
            {
                return this.pathField;
            }
            set
            {
                this.pathField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Notes
        {
            get
            {
                return this.notesField;
            }
            set
            {
                this.notesField = value;
            }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }


}
