using System;

namespace h73.Elastic.TypeMapping.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ElasticTypeMappingAttribute : ElasticAttribute
    {
        public FieldTypes ElasticType { get; }

        public ElasticTypeMappingAttribute(FieldTypes type)
        {
            ElasticType = type;
        }
    }
}