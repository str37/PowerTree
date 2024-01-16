
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using PowerTree.Sample.Models;
using System.ComponentModel.DataAnnotations;



namespace PowerTree.Sample
{
    public class PODContext : DbContext
    {
        // Appointment, Notification, and Task Related
        public DbSet<Link> Links { get; set; }
        public DbSet<LinkIcon> LinkIcons { get; set; }


        public readonly string _connectionString;
        public PODContext() : base() { }
        public PODContext(DbContextOptions<PODContext> options) : base(options)
        {
            _connectionString = ((SqlServerOptionsExtension)options.Extensions.Skip(1).Take(1).First()).ConnectionString;
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // This Connection string is used by Migration Tools to obtain connection string =============================================================================
#if DEBUG
        var connectionString = @"Server=192.168.1.95,1433\mssqlserver;Database=powertree;Trusted_Connection=True;Encrypt=False";
                    optionsBuilder.UseSqlServer(connectionString);
            optionsBuilder.EnableSensitiveDataLogging();

#endif


        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Link>()
                .Property(t => t.LinkId)
                .UseIdentityColumn(seed: 100);


        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var entities = from e in ChangeTracker.Entries()
                               where e.State == EntityState.Added
                                   || e.State == EntityState.Modified
                               select e.Entity;
                foreach (var entity in entities)
                {
                    var validationContext = new ValidationContext(entity);
                    Validator.ValidateObject(
                        entity,
                        validationContext,
                        validateAllProperties: true);
                }

                return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            }
            catch (ValidationException exc2)
            {
                throw new Exception("ValidationError: " + exc2?.Message, exc2);

            }
            catch (DbUpdateException exc)
            {
                throw new Exception("DBUpdateError: " + exc?.InnerException?.Message, exc);
            }
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var entities = from e in ChangeTracker.Entries()
                               where e.State == EntityState.Added
                                   || e.State == EntityState.Modified
                               select e.Entity;
                foreach (var entity in entities)
                {
                    var validationContext = new ValidationContext(entity);
                    Validator.ValidateObject(
                        entity,
                        validationContext,
                        validateAllProperties: true);
                }

                return await base.SaveChangesAsync(cancellationToken);
            }
            catch (ValidationException exc2)
            {
                throw new Exception("ValidationError: " + exc2?.Message, exc2);

            }
            catch (DbUpdateException exc)
            {
                throw new Exception("DBUpdateError: " + exc?.InnerException?.Message, exc);
            }
        }
        public override int SaveChanges()
        {
            try
            {
                var entities = from e in ChangeTracker.Entries()
                               where e.State == EntityState.Added
                                   || e.State == EntityState.Modified
                               select e.Entity;
                foreach (var entity in entities)
                {
                    var validationContext = new ValidationContext(entity);
                    Validator.ValidateObject(
                        entity,
                        validationContext,
                        validateAllProperties: true);
                }

                return base.SaveChanges();
            }
            catch (ValidationException exc2)
            {
                throw new Exception("ValidationError: " + exc2?.Message, exc2);

            }
            catch (DbUpdateException exc)
            {
                throw new Exception("DBUpdateError: " + exc?.InnerException?.Message, exc);
            }
        }
    }
}
