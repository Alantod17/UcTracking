using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AwesomeDi.Api.Models
{
	public class _DbContext
	{
        public class AwesomeDiContext : DbContext
        {
            public AwesomeDiContext(DbContextOptions<AwesomeDiContext> options) : base(options)
            { }

            public DbSet<User> User { get; set; }
            public DbSet<UserToken> UserToken { get; set; }
            public DbSet<FileEntry> FileEntry { get; set; }
            public DbSet<FileEntryEncryptionLog> FileEntryEncryptionLog { get; set; }
            public DbSet<Configuration> Configuration { get; set; }
            public DbSet<SharesiesConfiguration> SharesiesConfiguration { get; set; }
            public DbSet<SharesiesInstrument> SharesiesInstrument { get; set; }
            public DbSet<SharesiesInstrumentPriceHistory> SharesiesInstrumentPriceHistory { get; set; }
            public DbSet<SharesiesInstrumentComparisonPrice> SharesiesInstrumentComparisonPrice { get; set; }
            public DbSet<SharesiesInstrumentCategory> SharesiesInstrumentCategory { get; set; }
            public DbSet<SharesiesInstrumentXCategory> SharesiesInstrumentXCategory { get; set; }
            public DbSet<ResearchArticle> ResearchArticle { get; set; }
            public DbSet<TrackingRequestData> TrackingRequestData { get; set; }
            public DbSet<TrackingUiData> TrackingUiData { get; set; }
            public DbSet<TrackingEventData> TrackingEventData { get; set; }
            
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                foreach (var property in modelBuilder.Model.GetEntityTypes()
                    .SelectMany(t => t.GetProperties())
                    .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
                {
                    property.SetPrecision(18);
                    property.SetScale(6);
                }
                modelBuilder.Entity<User>()
                    .HasIndex(x => x.Email).IsUnique();

                modelBuilder.Entity<FileEntry>()
                    .HasIndex(x => x.LastWriteUtcDateTime);

            }

            public override int SaveChanges()
            {
                AddTimestamps();
                return base.SaveChanges();
            }

            public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
            {
                AddTimestamps();
                return await base.SaveChangesAsync(cancellationToken);
            }

            private void AddTimestamps()
            {
                var entities = ChangeTracker.Entries().Where(x => x.Entity is DatedEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));
                foreach (var entity in entities)
                {
                    if (entity.State == EntityState.Added)
                    {
                        ((DatedEntity)entity.Entity).CreatedUtcDateTime = DateTime.UtcNow;
                    }
                    ((DatedEntity)entity.Entity).ModifiedUtcDateTime = DateTime.UtcNow;
                }
            }
        }
    }
}
