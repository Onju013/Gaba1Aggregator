using System;

namespace Gaba1Aggregator.Attrs
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SnapFieldAttribute : Attribute
    {
        public string FieldName { get; }

        public SnapFieldAttribute()
        {
        }

        public SnapFieldAttribute(string fieldName)
        {
            FieldName = fieldName;
        }
    }
}