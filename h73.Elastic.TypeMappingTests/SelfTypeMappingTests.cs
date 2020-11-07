using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using h73.Elastic.TypeMapping;
using h73.Elastic.TypeMapping.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace h73.Elastic.TypeMappingTests
{
    [TestClass]
    public class SelfTypeMappingTests
    {
        [TestMethod]
        public void KeyWordClass_Test()
        {
            var typeMapping = new TypeMapping<KeyWordClass>();
            var mappings = new KeyValuePair<Expression<Func<KeyWordClass, object>>, FieldTypes>(
                ic => ic.Word, FieldTypes.Keyword
            );
            typeMapping.AddMapping(mappings);

            var kwTm = typeof(KeyWordClass).GetTypeMapping<KeyWordClass>();


            var json1 = JsonConvert.SerializeObject(new { mappings = typeMapping });
            var json2 = JsonConvert.SerializeObject(new { mappings = kwTm });

            Assert.AreEqual(json1, json2);

        }

        [TestMethod]
        public void MultipleOneLayerClass_Test()
        {
            var typeMapping = new TypeMapping<MultipleOneLayerClass>();
            var mappings1 = new KeyValuePair<Expression<Func<MultipleOneLayerClass, object>>, FieldTypes>(ic => ic.Word, FieldTypes.Keyword);
            var mappings2 = new KeyValuePair<Expression<Func<MultipleOneLayerClass, object>>, FieldTypes>(ic => ic.Value, FieldTypes.Double);
            var mappings3 = new KeyValuePair<Expression<Func<MultipleOneLayerClass, object>>, FieldTypes>(ic => ic.DateTime, FieldTypes.Date);
            typeMapping.AddMapping(mappings1, mappings2, mappings3);

            var mlTm = typeof(MultipleOneLayerClass).GetTypeMapping<MultipleOneLayerClass>();


            var json1 = JsonConvert.SerializeObject(new { mappings = typeMapping });
            var json2 = JsonConvert.SerializeObject(new { mappings = mlTm });

            Assert.AreEqual(json1, json2);

        }

        [TestMethod]
        public void MultipleLayerClass_Test()
        {
            var typeMapping = new TypeMapping<Multiple1LayerClass>();
            var mappings1 = new KeyValuePair<Expression<Func<Multiple1LayerClass, object>>, FieldTypes>(ic => ic.Obj, FieldTypes.Object); 
            var mappings2 = new KeyValuePair<Expression<Func<Multiple1LayerClass, object>>, FieldTypes>(ic => ic.Nested, FieldTypes.Nested); 
            typeMapping.AddMapping(mappings1, mappings2);

            typeMapping.AddNestedMapping(mlc=>mlc.Nested, 
                new KeyValuePair<Expression<Func<Multiple2LayerClass, object>>, FieldTypes>(ml2 => ml2.Word, FieldTypes.Keyword),
                new KeyValuePair<Expression<Func<Multiple2LayerClass, object>>, FieldTypes>(ml2 => ml2.Value, FieldTypes.Double),
                new KeyValuePair<Expression<Func<Multiple2LayerClass, object>>, FieldTypes>(ml2 => ml2.DateTime, FieldTypes.Date));

            typeMapping.AddObjectMapping(mlc => mlc.Obj,
                new KeyValuePair<Expression<Func<Multiple2LayerClass, object>>, FieldTypes>(ml2 => ml2.Word, FieldTypes.Keyword),
                new KeyValuePair<Expression<Func<Multiple2LayerClass, object>>, FieldTypes>(ml2 => ml2.Value, FieldTypes.Double),
                new KeyValuePair<Expression<Func<Multiple2LayerClass, object>>, FieldTypes>(ml2 => ml2.DateTime, FieldTypes.Date));

            var mlTm = typeof(Multiple1LayerClass).GetTypeMapping<Multiple1LayerClass>();


            var json1 = JsonConvert.SerializeObject(new { mappings = typeMapping });
            var json2 = JsonConvert.SerializeObject(new { mappings = mlTm });

            Assert.AreEqual(json1, json2);

        }
    }

    internal class KeyWordClass
    {
        [ElasticTypeMapping(FieldTypes.Keyword)]
        public string Word { get; set; }
    }

    internal class MultipleOneLayerClass
    {
        [ElasticTypeMapping(FieldTypes.Keyword)]
        public string Word { get; set; }

        [ElasticTypeMapping(FieldTypes.Double)]
        public double Value { get; set; }

        [ElasticTypeMapping(FieldTypes.Date)]
        public DateTime DateTime { get; set; }
    }

    internal class Multiple1LayerClass
    {
        [ElasticTypeMapping(FieldTypes.Object)]
        public Multiple2LayerClass Obj { get; set; }

        [ElasticTypeMapping(FieldTypes.Nested)]
        public Multiple2LayerClass Nested { get; set; }
    }
    internal class Multiple2LayerClass
    {
        [ElasticTypeMapping(FieldTypes.Keyword)]
        public string Word { get; set; }

        [ElasticTypeMapping(FieldTypes.Double)]
        public double Value { get; set; }

        [ElasticTypeMapping(FieldTypes.Date)]
        public DateTime DateTime { get; set; }
    }
}