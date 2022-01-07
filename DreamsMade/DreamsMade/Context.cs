using DreamsMade.Models;
using Microsoft.EntityFrameworkCore;

namespace DreamsMade
{
    public class Context : DbContext
    {

        public DbSet<User> Users { get; set; }

        public DbSet<Post> Posts { get; set; }

        public Context(DbContextOptions<Context>opt) : base(opt)
        {
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"Data Source=localhost; initial Catalog=DreamsMade; User ID=sa; password=1234; language=Portuguese; Trusted_Connection=True");
        //    optionsBuilder.UseLazyLoadingProxies();
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Post>()
                .HasOne(e => e.user)
                .WithMany(e => e.posts)
                .OnDelete(DeleteBehavior.ClientCascade);
        }

        internal void SavedChanges()
        {
            throw new NotImplementedException();
        }
    }
}
