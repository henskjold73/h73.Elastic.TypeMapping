using System.ComponentModel;

namespace h73.Elastic.TypeMapping
{
    public enum FieldTypes
    {
        Text,
        [Description("text")]
        TextRaw,
        Keyword,
        Date,
        Long,
        Double,
        Boolean,
        Ip,
        Object,
        Nested,
        [Description("geo_point")]
        GeoPoint,
        [Description("geo_shape")]
        GeoShape,
        Completion
    }
}