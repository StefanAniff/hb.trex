using System;
using System.IO;
using System.Xml.Serialization;

namespace Trex.SmartClient.Core.Utils
{
    public class SerializationUtils
    {
   
        public static string Serialize(object obj)
        {
            if (obj == null)
                return string.Empty;

            var serializer = new XmlSerializer(obj.GetType());
            var xmlString = string.Empty;
            
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, obj);
                xmlString = writer.ToString();
            }
            return xmlString;
        }

        public static object DeSerialize(string xmlString, Type type)
        {
            if (string.IsNullOrEmpty(xmlString))
                return null;

            var serializer = new XmlSerializer(type);
            using(var reader = new StringReader(xmlString))
            {
                var obj = serializer.Deserialize(reader);
                return obj;
            }
            
        }

    }
}