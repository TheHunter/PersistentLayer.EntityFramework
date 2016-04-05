using System;

namespace PersistentLayer.EntityFramework.Metadata
{
    public class MetadataProperty
    {
        private MetadataProperty(string name, bool isIdentifier, ValueGeneration valueGeneration)
        {
            this.Name = name;
            this.IsIdentifier = isIdentifier;
            this.ValueGeneration = valueGeneration;
        }

        public string Name { get; private set; }

        public bool IsIdentifier { get; private set; }

        public ValueGeneration ValueGeneration { get; private set; }

        public static MetadataProperty MakeKeyProperty(string name, ValueGeneration valueGeneration = ValueGeneration.Assigned)
        {
            return new MetadataProperty(name, true, valueGeneration);
        }

        public static MetadataProperty MakeProperty(string name, ValueGeneration valueGeneration = ValueGeneration.Assigned)
        {
            return new MetadataProperty(name, false, ValueGeneration.Identity);
        }
    }

    public enum ValueGeneration
    {
        Assigned = 0,
        Identity = 2,
        Computed = 5
    }
}
