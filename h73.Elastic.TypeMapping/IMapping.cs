using System.Collections.Generic;

namespace h73.Elastic.TypeMapping
{
    public interface IMapping
    {
        Dictionary<string, IMapping> Properties { get; set; }
    }
}