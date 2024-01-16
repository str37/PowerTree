using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Microsoft.Maui.Controls.Shapes;
using PowerTree.Maui.Model;
using System.ComponentModel.DataAnnotations;

namespace PowerTree.Maui
{
    public class PTContext : DbContext
    {
        public DbSet<PTHierarchy> Hierarchies { get; set; }
        public DbSet<PTNode> Nodes { get; set; }
        public DbSet<PTNodeItem> NodeItems { get; set; }



        public PTContext() : base() { }
        public PTContext(DbContextOptions<PTContext> options) : base(options)
        {
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // This Connection string is used by Migration Tools to obtain connection string =============================================================================
            //var connectionString = @"Server=192.168.1.95,1433\mssqlserver;Database=focus;Trusted_Connection=True";
            //var connectionString = @"Server=192.168.1.95,1433\mssqlserver;Database=pcs;Trusted_Connection=True;Encrypt=False";
            //optionsBuilder.UseSqlServer(@"Data Source=192.168.1.95,1433\mssqlserver;Database=pcs;User Id=pcsuser;Password=V34.H22typo;Encrypt=False");
#if DEBUG
            var connectionString = @"Server=192.168.1.95,1433\mssqlserver;Database=powertree;Trusted_Connection=True;Encrypt=False";
            //optionsBuilder.UseSqlServer(connectionString);
            optionsBuilder.UseSqlServer(connectionString, x => x.MigrationsHistoryTable("__PTMigrationsHistory", "dbo")); // && x.MigrationsAssembly("PowerTree.Maui"));

#endif


        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<PTHierarchy>()
                .Property(t => t.HierarchyId)
                .UseIdentityColumn(seed: 10);

            // Subsystems, when creating hierarchies must ensure the names are unique
            modelBuilder.Entity<PTHierarchy>()
                .HasIndex(p => new { p.Subsystem, p.HierarchyName }).IsUnique();

            // Older code before HierarchyRegistration
            //modelBuilder.Entity<PTHierarchy>()
            //    .HasData(new PTHierarchy { HierarchyId = 10, HierarchyName = "Favorites", Subsystem = "Favorites" });

            modelBuilder.Entity<PTNode>()
                .Property(t => t.NodeId)
                .UseIdentityColumn(seed: 10000);

            // Older code before HierarchyRegistration
            //modelBuilder.Entity<PTNode>()
            //    .HasData(new PTNode { NodeId = 10000, NodeName = "RootNode", HierarchyId = 10, NodeOrder = 1, ParentNodeId = null });

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
