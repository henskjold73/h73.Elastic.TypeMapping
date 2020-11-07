using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace h73.Elastic.TypeMapping
{
    public class TypeMapping<T> : Dictionary<string, ClassMapping> where T : new()
    {
        public void AddMapping(params KeyValuePair<Expression<Func<T, object>>, FieldTypes>[] pairs)
        {
            var p = pairs.Select(pair =>
                new KeyValuePair<string, FieldTypes>(ExpressionHelper.GetPropertyName(pair.Key), pair.Value)).ToArray();

            AddMapping(p);
        }

        public void AddMapping(params KeyValuePair<string, FieldTypes>[] pairs)
        {
            var cm = new ClassMapping();

            foreach (var exp in pairs)
            {
                var pm = new PropertyMapping(exp.Value);

                cm.Properties[exp.Key] = pm;
            }

            if (ContainsKey(typeof(T).FullName))
            {
                foreach (var cmProperty in cm.Properties)
                {
                    this[typeof(T).FullName].Properties[cmProperty.Key] = cmProperty.Value;
                }

                return;
            }

            // ReSharper disable AssignNullToNotNullAttribute
            this[typeof(T).FullName] = cm;
            // ReSharper restore AssignNullToNotNullAttribute
        }

        public void AddObjectMapping<T2>(Expression<Func<T, object>> @object,
            params KeyValuePair<Expression<Func<T2, object>>, FieldTypes>[] pairs)
        {
            AddNestedMapping(@object, pairs);
        }

        public void AddObjectMapping<T2>(string @object,
            params KeyValuePair<string, FieldTypes>[] pairs)
        {
            AddNestedMapping<T2>(@object, pairs);
        }


        public void AddNestedMapping<T2>(Expression<Func<T, object>> nested,
            params KeyValuePair<Expression<Func<T2, object>>, FieldTypes>[] pairs)
        {
            AddNestedMapping<T2>(ExpressionHelper.GetPropertyName(nested),
                pairs.Select(
                        p => new KeyValuePair<string, FieldTypes>(ExpressionHelper.GetPropertyName(p.Key), p.Value))
                    .ToArray());
        }

        public void AddNestedMapping<T2>(string nested,
            params KeyValuePair<string, FieldTypes>[] pairs)
        {
            var propertMapping = this[typeof(T).FullName].Properties[nested];

            if (propertMapping.Properties == null) propertMapping.Properties = new Dictionary<string, IMapping>();

            foreach (var exp in pairs)
            {
                var pm = new PropertyMapping(exp.Value);
                propertMapping.Properties[exp.Key] = pm;
            }
        }
    }
}