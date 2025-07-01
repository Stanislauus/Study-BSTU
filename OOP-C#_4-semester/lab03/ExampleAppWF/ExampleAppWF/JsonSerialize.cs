using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleAppWF
{
    public static class JsonSerialize
    {
        public static void Serialize<T>(T obj, string filename)
        {
            var settings = new JsonSerializerSettings
            {
               // DefaultValueHandling = DefaultValueHandling.Ignore,
                Formatting = Formatting.Indented
            };
            string json = JsonConvert.SerializeObject(obj, settings);
            File.WriteAllText(filename, json);
        }

        public static T Deserialize<T>(string filename)
        {
            string json = File.ReadAllText(filename);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
