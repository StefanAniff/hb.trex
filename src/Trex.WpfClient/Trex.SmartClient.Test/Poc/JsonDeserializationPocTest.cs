using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using NUnit.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Linq;

namespace Trex.SmartClient.Test.Poc
{
    [TestFixture, Ignore("Just for pock'in and future refernce")]
    public class JsonDeserializationPocTest
    {
        public class TestLogEntry
        {
            public string Details { get; set; }
            public DateTime LogDate { get; set; }
        }

        [Test]
        public void WriteJsonDates()
        {
            var entry = new TestLogEntry
            {
                LogDate = new DateTime(2013, 8, 31, 0, 0, 0),
                Details = "Bla bla bla...."
            };

            var defaultJson = JsonConvert.SerializeObject(entry);
            Debug.Print("defaultjson: {0}", JsonConvert.DeserializeObject(defaultJson));
            // {"Details":"Application started.","LogDate":"\/Date(1234656000000)\/"}

            var javascriptJson = JsonConvert.SerializeObject(entry, new JavaScriptDateTimeConverter());
            Debug.Print("javascript: {0}", JsonConvert.DeserializeObject(javascriptJson));
            // {"Details":"Application started.","LogDate":new Date(1234656000000)}

            var isoJson = JsonConvert.SerializeObject(entry, new IsoDateTimeConverter());
            Debug.Print("Iso: {0}", JsonConvert.DeserializeObject(isoJson));
            // {"Details":"Application started.","LogDate":"2009-02-15T00:00:00Z"}

        }

        public class TestSomeClass
        {
            public string Name { get; set; }
            public IList<int> Numbers { get; set; }
        }

        [Test]
        public void WriteJsonToFile()
        {
            // Arrange
            var list = new List<TestSomeClass>
                {
                    new TestSomeClass
                        {
                            Name = "Sweeet",
                            Numbers = new List<int> {1, 2, 3, 4, 5}
                        },
                    new TestSomeClass
                        {
                            Name = "Souuur",
                            Numbers = new List<int> {6, 7, 8, 9, 10}
                        },
                };

            var json = JsonConvert.SerializeObject(list);


            // Act

            // Write file
            File.WriteAllText(@"c:\jsondump.txt", json);

            // Read file
            List<TestSomeClass> readItems = null;
            using (var reader = new StreamReader(@"c:\jsondump.txt"))
            {
                var readJson = reader.ReadToEnd();
                readItems = JsonConvert.DeserializeObject<List<TestSomeClass>>(readJson);
            }

            // Assert
            Assert.That(readItems.Any(x => x.Name == "Sweeet"), Is.True);
            Assert.That(readItems.Any(x => x.Name == "Souuur"), Is.True);
            Assert.That(readItems.Count, Is.EqualTo(2));
        }
    }
}