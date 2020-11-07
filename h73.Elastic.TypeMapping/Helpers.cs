using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using h73.Elastic.TypeMapping.Attributes;

namespace h73.Elastic.TypeMapping
{
    public static class Helpers
    {
        public static Type[] GetAllTypes(this Type type, Assembly assembly = null)
        {
            if (assembly == null)
            {
                assembly = type.Assembly;
            }

            var assemblyTypes = assembly.GetTypes();
            return assemblyTypes.Where(type.IsAssignableFrom).ToArray();
        }

        public static TypeMapping<T> GetTypeMapping<T>(this Type type) where T : new()
        {
            var tm = new TypeMapping<T>();
            var properties = type.GetProperties();

            foreach (var propertyInfo in properties)
            {
                if (!(propertyInfo.GetCustomAttribute(typeof(ElasticTypeMappingAttribute), false) is
                    ElasticTypeMappingAttribute attr)) continue;
                tm.AddMapping(new KeyValuePair<string, FieldTypes>(propertyInfo.Name, attr.ElasticType));
                if (attr.ElasticType == FieldTypes.Nested || attr.ElasticType == FieldTypes.Object)
                {
                    tm = tm.GetRecursiveTypeMapping(propertyInfo, attr.ElasticType);
                }
            }

            return tm;
        }

        private static TypeMapping<T> GetRecursiveTypeMapping<T>(this TypeMapping<T> tm, PropertyInfo pi, FieldTypes fType) where T : new()
        {
            var props = pi.PropertyType.GetProperties();

            foreach (var propertyInfo in props)
            {
                if (!(propertyInfo.GetCustomAttribute(typeof(ElasticTypeMappingAttribute), false) is
                    ElasticTypeMappingAttribute attr)) continue;
                if(fType == FieldTypes.Nested)
                    tm.AddNestedMapping<T>(pi.Name, new KeyValuePair<string, FieldTypes>(propertyInfo.Name, attr.ElasticType));
                if (fType == FieldTypes.Object)
                    tm.AddObjectMapping<T>(pi.Name, new KeyValuePair<string, FieldTypes>(propertyInfo.Name, attr.ElasticType));
                if (attr.ElasticType == FieldTypes.Nested || attr.ElasticType == FieldTypes.Object)
                {
                    tm = tm.GetRecursiveTypeMapping(propertyInfo, attr.ElasticType);
                }
            }
            

            return tm;
        }
    }
}