using Microsoft.EntityFrameworkCore;
using SpyderWeb.Data.Tags;

namespace SpyderWeb.Data
{
    public class SpyderContext : DbContext
    {
        public SpyderContext(DbContextOptions options) : base(options) { }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Audit> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tag>()
                .HasAlternateKey(t => t.Name);
        }
    }
}
