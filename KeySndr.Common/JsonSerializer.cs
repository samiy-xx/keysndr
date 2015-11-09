using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace KeySndr.Common
{
    public static class JsonSerializer
    {
        public static string Serialize<T>(T obj)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            var stream = new MemoryStream();
            serializer.WriteObject(stream, obj);
            stream.Position = 0;
            var sr = new StreamReader(stream);
            return sr.ReadToEnd();
        }

        public static T Deserialize<T>(string data, IEnumerable<Type> types = null)
        {
            var serializer = types == null ? new DataContractJsonSerializer(typeof(T)) : new DataContractJsonSerializer(typeof(T), types);

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(data)))
            {
                return (T)serializer.ReadObject(stream);
            }
        }
    }
}
