using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SMARTBusinessTest.Utils
{
    public static class Utils
    {
        public static EntityTypeBuilder<TEntity> Entity<TEntity>(this ModelBuilder modelBuilder, Action<EntityTypeBuilder<TEntity>> action) where TEntity : class
        {
            var r = modelBuilder.Entity<TEntity>();
            action(r);
            return r;
        }
    }
}
