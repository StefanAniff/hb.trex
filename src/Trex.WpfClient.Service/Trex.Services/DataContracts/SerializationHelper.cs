using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace TrexSL.Web.DataContracts
{
    public class SerializationHelper
    {
        /// <summary>
        /// Responses with more bytes will be GZipped
        /// </summary>
        private const int CompressResponseThresholdBytes = 8196;

        public readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            DateFormatHandling = DateFormatHandling.IsoDateFormat
        };

        /// <summary>
        /// Serializes the given object to a UTF8-encoded string of JSON
        /// </summary>
        public byte[] Serialize(object data)
        {
            var serializeObject = JsonConvert.SerializeObject(data, JsonSerializerSettings);
            return Encoding.UTF8.GetBytes(serializeObject);
        }

        /// <summary>
        /// Deserializes the given UTF8-encoded JSON string into the specified object
        /// </summary>
        public T Deserialize<T>(byte[] jsonText)
        {
            return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(jsonText), JsonSerializerSettings);
        }

        /// <summary>
        /// Compresses the given byte array
        /// </summary>
        public byte[] Compress(byte[] input)
        {
            using (var output = new MemoryStream())
            {
                using (var zip = new GZipStream(output, CompressionMode.Compress))
                {
                    zip.Write(input, 0, input.Length);
                }
                return output.ToArray();
            }
        }

        /// <summary>
        /// Decompresses the given byte array
        /// </summary>
        public byte[] Decompress(byte[] input)
        {
            using (var output = new MemoryStream(input))
            {
                using (var zip = new GZipStream(output, CompressionMode.Decompress))
                {
                    var bytes = new List<byte>();
                    var b = zip.ReadByte();
                    while (b != -1)
                    {
                        bytes.Add((byte)b);
                        b = zip.ReadByte();
                    }
                    return bytes.ToArray();
                }
            }
        }

        /// <summary>
        /// Will compresss if it seems worth it (size treshold check)
        /// </summary>
        /// <returns>CompressedObject. Check response header if it is compressed</returns>
        public CompressedObject TryCompress(byte[] serializedResponse, Type responseType)
        {
            var transportResponse = new CompressedObject();
            transportResponse.ResponseHeaders.Add(ResponseHeaders.ResponseType, responseType.ToString());
            if (serializedResponse.Length > CompressResponseThresholdBytes)
            {
                var compressedResponse = Compress(serializedResponse);
                transportResponse.ResponseHeaders.Add(ResponseHeaders.Compression,
                                                      ResponseHeaders.CompressionModes.GZip);
                transportResponse.SerializedResponse = compressedResponse;
            }
            else
            {
                transportResponse.SerializedResponse = serializedResponse;
            }
            return transportResponse;
        }
    }
}