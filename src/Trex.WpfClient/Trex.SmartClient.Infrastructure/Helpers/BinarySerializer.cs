using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Trex.SmartClient.Infrastructure.Helpers
{
    public class BinarySerializer<T>
    {
        public void SerializeObject(string filename, T objectToSerialize)
        {

            using ( Stream stream = File.Open(filename, FileMode.Create))
            {
                var bFormatter = new BinaryFormatter();
                bFormatter.Serialize(stream, objectToSerialize);
                stream.Close();
            }
           
          
        }

        public void TryDeSerializeObject(string filename,out T output)
        {
            output = default(T);
            
            if (!File.Exists(filename))
            {
                return;
            }

            try
            {
                using (Stream stream = File.Open(filename, FileMode.Open))
                {

                    var bFormatter = new BinaryFormatter();
                    var objectToSerialize = (T)bFormatter.Deserialize(stream);
                    stream.Close();
                    output = objectToSerialize;
                }
            }
            catch (InvalidOperationException)
            {
                return;
                
            }
            catch (SerializationException)
            {
                return;

            }
         
           


        }
    }
}
