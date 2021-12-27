using DreamsMade.Models;
using Microsoft.EntityFrameworkCore;

namespace DreamsMade
{
    public class Context : DbContext
    {

        public DbSet<User> Users { get; set; }

        public DbSet<Image> Images { get; set; }

        public Context()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=localhost; initial Catalog=CodeFirstEntity; User ID=sa; password=1234; language=Portuguese; Trusted_Connection=True");
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Image>()
                .HasOne(e => e.user)
                .WithMany(e => e.images)
                .OnDelete(DeleteBehavior.ClientCascade);
        }

        internal void SavedChanges()
        {
            throw new NotImplementedException();
        }
    }
}
