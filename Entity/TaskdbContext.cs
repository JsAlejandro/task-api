using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
namespace taskmanager_api.Models {
    public partial class TaskdbContext : DbContext {
        public TaskdbContext () { }

        public TaskdbContext (DbContextOptions<TaskdbContext> options) : base (options) { }

        public virtual DbSet<Comments> Comments { get; set; }
        public virtual DbSet<Assignment> Assignment { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        public override int SaveChanges () {
            var entries = ChangeTracker
                .Entries ()
                .Where (e => e.Entity is BaseEntity && (
                    e.State == EntityState.Added ||
                    e.State == EntityState.Modified));

            foreach (var entityEntry in entries) {
                if (entityEntry.State == EntityState.Modified) {
                    entityEntry.Property("CreatedAt").IsModified = false;
                    ((BaseEntity) entityEntry.Entity).UpdatedAt = DateTime.UtcNow;
                } else if (entityEntry.State == EntityState.Added) {
                    ((BaseEntity) entityEntry.Entity).UpdatedAt = DateTime.UtcNow;
                    ((BaseEntity) entityEntry.Entity).CreatedAt = DateTime.UtcNow;
                }
            }

            return base.SaveChanges ();
        }
    }
}