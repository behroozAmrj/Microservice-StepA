using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Common;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence;

public class OrderContext : DbContext
{
    public OrderContext(DbContextOptions<OrderContext> options)
                        : base(options)
    {

    }

    public DbSet<Order> Orders { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {

        ChangeTracker.Entries<EntityBase>()
                     .ToList()
                     .ForEach(ent =>
                     {
                         switch (ent.State)
                         { 
                            case EntityState.Added:
                                 ent.Entity.CreatedDate = DateTime.Now;
                                 ent.Entity.CreatedBy = "Beh";
                                 break;

                             case EntityState.Modified:
                                 ent.Entity.LastModifiedDate = DateTime.Now;
                                 ent.Entity.LastModufiedBy = "Beh";
                                 break;
                         }
                     });

        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }

}
