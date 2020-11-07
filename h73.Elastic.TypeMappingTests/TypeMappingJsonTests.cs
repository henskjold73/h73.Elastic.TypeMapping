using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using h73.Elastic.TypeMapping;
using h73.Elastic.TypeMapping.Attributes;
using Newtonsoft.Json;
using NUnit.Framework;

namespace h73.Elastic.TypeMappingTests
{
    public class TypeMappingJsonTests
    {
        [Test]
        public void TypeMappingJson_1()
        {
            var typeMapping = new TypeMapping<IndexedClass>();
            var mappings =
                new KeyValuePair<Expression<Func<IndexedClass, object>>, FieldTypes>(ic => ic.Objects,
                    FieldTypes.Nested);
            typeMapping.AddMapping(mappings);

            typeMapping.AddNestedMapping(ic => ic.Objects, new KeyValuePair<Expression<Func<IndexedClass2, object>>, FieldTypes>(ic => ic.BString,
                FieldTypes.Keyword));

            var json = JsonConvert.SerializeObject(new {mappings = typeMapping});
            Assert.AreEqual("{\"mappings\":{\"h73.Elastic.TypeMappingTests.IndexedClass\":{\"properties\":{\"Objects\":{\"properties\":{\"BString\":{\"type\":\"keyword\"}},\"type\":\"nested\"}}}}}", json);
        }
    }

    public class IndexedClass
    {
        [ElasticTypeMapping(FieldTypes.Keyword)]
        public string AString { get; set; }
        public List<IndexedClass2> Objects { get; set; }
    }

    public class IndexedClass2
    {
        public string BString { get; set; }
        public int Amount { get; set; }
    }
}