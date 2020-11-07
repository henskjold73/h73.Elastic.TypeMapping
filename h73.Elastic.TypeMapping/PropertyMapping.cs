using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace h73.Elastic.TypeMapping
{
    public class PropertyMapping : IMapping
    {
        [JsonProperty("properties", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, IMapping> Properties { get; set; }

        [JsonProperty("type")] public string Type => GetDescription(FieldType);

        [JsonProperty("tree", NullValueHandling = NullValueHandling.Ignore)]
        public string Tree { get; set; }

        [JsonProperty("precision", NullValueHandling = NullValueHandling.Ignore)]
        public string Precision { get; set; }

        [JsonProperty("fielddata", NullValueHandling = NullValueHandling.Ignore)]
        public bool? FieldData { get; set; }

        [JsonProperty("fields", NullValueHandling = NullValueHandling.Ignore)]
        public Fields Fields { get; set; }

        [JsonIgnore] private FieldTypes FieldType { get; }

        [JsonProperty("format", NullValueHandling = NullValueHandling.Ignore)]
        public string Format
        {
            get
            {
                switch (FieldType)
                {
                    case FieldTypes.Date:
                        return "strict_date_optional_time||epoch_millis";
                    default:
                        return null;
                }
            }
        }

        public PropertyMapping(FieldTypes fieldType)
        {
            FieldType = fieldType;
            if (fieldType == FieldTypes.TextRaw)
            {
                FieldData = true;
                Fields = new Fields();
            }

            if (fieldType != FieldTypes.GeoShape) return;
            Tree = "quadtree";
            Precision = "1m";
        }

        public static string GetDescription<T>(T enumerationValue) where T : struct
        {
            var type = enumerationValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException("EnumerationValue must be of Enum type", nameof(enumerationValue));
            }

            var memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo.Length <= 0) return enumerationValue.ToString();
            var attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attrs.Length > 0
                ? ((DescriptionAttribute) attrs[0]).Description.ToLower()
                : enumerationValue.ToString().ToLower();
        }
    }
}