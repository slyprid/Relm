using Newtonsoft.Json;
using System.IO;
using System.Xml.Serialization;

namespace Relm.Extensions
{
    public static class SerializationExtensions
    {
        public static T DeserializeXml<T>(this string input)
        {
            var xmlSerialize = new XmlSerializer(typeof(T));
            using (var reader = new StringReader(input))
            {
                return (T)xmlSerialize.Deserialize(reader);
            }
        }

        public static string SerializeXml<T>(this T input)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            using (var writer = new StringWriter())
            {
                xmlSerializer.Serialize(writer, input);
                return writer.ToString();
            }
        }

        public static T DeserializeJson<T>(this string input)
        {
            return JsonConvert.DeserializeObject<T>(input);
        }

        public static string SerializeJson<T>(this T input)
        {
            return JsonConvert.SerializeObject(input);
        }
    }
}