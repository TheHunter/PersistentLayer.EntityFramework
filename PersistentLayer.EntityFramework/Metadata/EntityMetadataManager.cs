using System.Linq;
using PersistentLayer.EntityFramework.Extensions;

namespace PersistentLayer.EntityFramework.Metadata
{
    public class EntityMetadataManager
    {
        private IContextProvider contextProvider;

        public EntityMetadataManager(IContextProvider contextProvider)
        {
            this.contextProvider = contextProvider;
        }

        public EntityMetadata MakeMetadata<TEntity>()
            where TEntity : class
        {
            var query = this.contextProvider.MakeObjectSet<TEntity>();

            var idProperties = query.EntitySet.ElementType.KeyProperties.Select(property =>
                MetadataProperty.MakeKeyProperty(property.Name,
                    property.IsStoreGeneratedComputed
                        ? ValueGeneration.Computed
                        : property.IsStoreGeneratedIdentity ? ValueGeneration.Identity : ValueGeneration.Assigned)
                );

            var properties =
                query.EntitySet.ElementType.Properties.Select(property =>
                    MetadataProperty.MakeProperty(property.Name,
                        property.IsStoreGeneratedComputed
                            ? ValueGeneration.Computed
                            : property.IsStoreGeneratedIdentity ? ValueGeneration.Identity : ValueGeneration.Assigned)

                    );

            var entityProperties = idProperties.Union(properties);

            return new EntityMetadata(typeof(TEntity), entityProperties);
        }
    }
}
