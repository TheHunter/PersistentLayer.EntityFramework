using System;
using System.Collections.Generic;
using System.Linq;

namespace PersistentLayer.EntityFramework.Metadata
{
    public class EntityMetadata
    {
        private readonly IEnumerable<MetadataProperty> properties;

        public EntityMetadata(Type entityType, IEnumerable<MetadataProperty> properties)
        {
            this.EntityType = entityType;
            this.properties = new List<MetadataProperty>(properties);
        }

        public Type EntityType { get; private set; }

        public IEnumerable<MetadataProperty> Properties { get { return this.properties; } }

        public IEnumerable<MetadataProperty> GetIdentifier()
        {
            return this.properties.Where(property => property.IsIdentifier).ToList();
        }

        public IEnumerable<object> GetIdentifierValue(object instance)
        {
            throw new NotImplementedException();
        }

        public object GetPropertyValue(object instance, string propertyName)
        {
            throw new NotImplementedException();
        }
    }
}
