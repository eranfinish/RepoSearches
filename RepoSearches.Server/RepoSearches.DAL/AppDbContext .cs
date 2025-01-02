using Microsoft.EntityFrameworkCore;
using RepoSearches.Models.Entities;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.Configuration;
namespace RepoSearches.DAL
{
    public class AppDbContext : DbContext
    {
        public readonly IConfiguration _configuration;

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<RepositoryEntity> Repositories { get; set; }
        public DbSet<OwnerEntity> Owners { get; set; }
        public DbSet<BookmarkEntity> Bookmarks { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Composite key for BookmarkEntity
            modelBuilder.Entity<BookmarkEntity>()
                .HasKey(b => new { b.UserId, b.RepositoryId });

            // Configure relationships for BookmarkEntity
            modelBuilder.Entity<BookmarkEntity>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookmarks)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BookmarkEntity>()
                .HasOne(b => b.Repository)
                .WithMany(r => r.Bookmarks)
                .HasForeignKey(b => b.RepositoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure relationship between RepositoryEntity and OwnerEntity
            modelBuilder.Entity<RepositoryEntity>()
                .HasOne(r => r.Owner)
                .WithMany(o => o.Repositories)
                .HasForeignKey(r => r.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly("RepoSearches.DAL"));
            }
        }
    }
}

    

