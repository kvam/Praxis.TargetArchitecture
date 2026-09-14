using Praxis.TargetArchitecture.AppInfrastructure.Authorization;
using Praxis.TargetArchitecture.Entities;
using Praxis.TargetArchitecture.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Praxis.TargetArchitecture.Data;

public class PraxisDbContext(DbContextOptions<PraxisDbContext> options, CurrentUser? currentUser = null)
    : DbContext(options)
{
    public DbSet<ArchitecturePrinciple> ArchitecturePrinciples => Set<ArchitecturePrinciple>();

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        StampAuditFields(DateTime.UtcNow);

        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        StampAuditFields(DateTime.UtcNow);

        return base.SaveChanges();
    }

    private void StampAuditFields(DateTime utcNow)
    {
        var actingUser = currentUser?.AzureAdUserId.ToString() ?? "system";

        foreach (var entry in ChangeTracker.Entries<IDateTrackedEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedUtc = utcNow;
                entry.Entity.CreatedBy = actingUser;
            }

            if (entry.State is EntityState.Added or EntityState.Modified)
            {
                entry.Entity.UpdatedUtc = utcNow;
                entry.Entity.UpdatedBy = actingUser;
            }
        }
    }
}
