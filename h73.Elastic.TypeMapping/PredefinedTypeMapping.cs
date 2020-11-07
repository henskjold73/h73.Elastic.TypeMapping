using System;
using System.Collections.Generic;

namespace h73.Elastic.TypeMapping
{
    public class PredefinedTypeMapping
    {
        public static TypeMapping<T> GetPredefinedTypeMapping<T>(string type = null) where T : new()
        {
            var typeMapping = new TypeMapping<T>();
            typeMapping.AddMapping(
                new KeyValuePair<string, FieldTypes>("Address.City" , FieldTypes.Keyword),
                new KeyValuePair<string, FieldTypes>("Address.Country", FieldTypes.Keyword),
                new KeyValuePair<string, FieldTypes>("Address.PostalCode", FieldTypes.Keyword),
                new KeyValuePair<string, FieldTypes>("ValidFrom", FieldTypes.Date),
                new KeyValuePair<string, FieldTypes>("ValidTo", FieldTypes.Date),
                new KeyValuePair<string, FieldTypes>("InstallationDate", FieldTypes.Date),
                new KeyValuePair<string, FieldTypes>("CorporateCode", FieldTypes.Keyword),
                new KeyValuePair<string, FieldTypes>("Location.Shape", FieldTypes.GeoShape),
                new KeyValuePair<string, FieldTypes>("Location.Point", FieldTypes.GeoPoint),
                new KeyValuePair<string, FieldTypes>("AssetTag", FieldTypes.TextRaw)
            );
            return typeMapping;
        }

        public static TypeMapping<T> GetPredefinedTypeMapping<T>(Type type) where T : new()
        {
            return GetPredefinedTypeMapping<T>(type?.FullName);
        }
    }
}