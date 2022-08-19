using System;
using System.IO;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Relm.Extensions;

namespace Relm.Output
{
    [Serializable]
    public class SaveFile<T>
        where T : class
    {
        [XmlIgnore]
        [JsonIgnore]
        public virtual string OutputDirectory { get; set; }

        [XmlIgnore]
        [JsonIgnore]
        public virtual string Filename { get; set; }

        public virtual void Save()
        {
            var xml = this.SerializeXml();
            var path = Path.Combine(OutputDirectory, Filename);

            if (!Directory.Exists(OutputDirectory))
            {
                Directory.CreateDirectory(OutputDirectory);
            }

            File.WriteAllText(path, xml);
        }

        public virtual T Load()
        {
            var path = Path.Combine(OutputDirectory, Filename);
            var xml = File.ReadAllText(path);
            return xml.DeserializeXml<T>();
        }
    }
}