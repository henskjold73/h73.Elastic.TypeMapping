using Newtonsoft.Json;

namespace h73.Elastic.TypeMapping
{
    public class Fields
    {
        [JsonProperty("raw")] public object Raw => new {type = PropertyMapping.GetDescription(FieldTypes.Keyword)};
    }
}