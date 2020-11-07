using System.Collections.Generic;
using Newtonsoft.Json;

namespace h73.Elastic.TypeMapping
{
    public class ClassMapping
    {
        [JsonProperty("properties")] public Dictionary<string, IMapping> Properties { get; set; }

        public ClassMapping()
        {
            Properties = new Dictionary<string, IMapping>();
        }
    }
}