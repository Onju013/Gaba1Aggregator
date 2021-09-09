using System;

namespace Gaba1Aggregator.Attrs
{
    [AttributeUsage(AttributeTargets.Property)]
    public class OutputPropertyAttribute : Attribute
    {
        public OutputPropertyAttribute()
        {
        }
    }
}