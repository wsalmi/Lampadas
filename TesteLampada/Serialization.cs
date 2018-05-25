using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;

namespace STF.SI.Common
{
    public static class Serialization
    {
        public static string ObjectToText(object objeto)
        {
            using (var memoria = new System.IO.MemoryStream())
            {
                new BinaryFormatter().Serialize(memoria, objeto);

                return Convert.ToBase64String(memoria.ToArray());
            }
        }

        public static string ObjectToJson(object objeto, JsonConfig conf = default(JsonConfig))
        {
            return (objeto != null) ? Newtonsoft.Json.JsonConvert.SerializeObject(objeto, conf) : string.Empty;
        }

        public static string ObjectToXml(object objeto)
        {
            using (StringWriter writer = new Utf8StringWriter())
            {
                XmlSerializer xml = new XmlSerializer(objeto.GetType());
                xml.Serialize(writer, objeto);
                return writer.ToString();
            }
        }
    }

    public class Utf8StringWriter : StringWriter
    {
        // Use UTF8 encoding but write no BOM to the wire
        public override Encoding Encoding
        {
            get { return new UTF8Encoding(false); }
        }
    }

    public struct JsonConfig
    {
        public int? MaxDepth { get; set; }

        public NullHandling NullValueHandling { get; set; }
        public NullHandling MissingMemberHandling { get; set; }

        public static implicit operator Newtonsoft.Json.JsonSerializerSettings(JsonConfig self)
        {
            return new Newtonsoft.Json.JsonSerializerSettings
            {
                MaxDepth = self.MaxDepth,
                NullValueHandling = (Newtonsoft.Json.NullValueHandling)self.NullValueHandling,
                MissingMemberHandling = (Newtonsoft.Json.MissingMemberHandling)self.MissingMemberHandling
            };
        }

        public enum NullHandling
        {
            IgnoreIfNull = Newtonsoft.Json.NullValueHandling.Ignore,
            AlwaysRender = Newtonsoft.Json.NullValueHandling.Include,
            MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore
        }
    }
}