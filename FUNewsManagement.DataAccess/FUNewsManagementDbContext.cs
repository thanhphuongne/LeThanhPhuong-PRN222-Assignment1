using Microsoft.EntityFrameworkCore;
using FUNewsManagement.BusinessObjects;

namespace FUNewsManagement.DataAccess
{
    public class FUNewsManagementDbContext : DbContext
    {
        public FUNewsManagementDbContext(DbContextOptions<FUNewsManagementDbContext> options) : base(options)
        {
        }

        public DbSet<SystemAccount> SystemAccounts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<NewsArticle> NewsArticles { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<NewsTag> NewsTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure SystemAccount entity
            modelBuilder.Entity<SystemAccount>(entity =>
            {
                entity.HasKey(e => e.AccountID);
                entity.Property(e => e.AccountID).ValueGeneratedOnAdd();
                entity.HasIndex(e => e.AccountEmail).IsUnique();
            });

            // Configure Category entity
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CategoryID);
                entity.Property(e => e.CategoryID).ValueGeneratedOnAdd();

                // Self-referencing relationship
                entity.HasOne(e => e.ParentCategory)
                      .WithMany(e => e.SubCategories)
                      .HasForeignKey(e => e.ParentCategoryID)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure NewsArticle entity
            modelBuilder.Entity<NewsArticle>(entity =>
            {
                entity.HasKey(e => e.NewsArticleID);

                // Relationship with Category
                entity.HasOne(e => e.Category)
                      .WithMany(e => e.NewsArticles)
                      .HasForeignKey(e => e.CategoryID)
                      .OnDelete(DeleteBehavior.Restrict);

                // Relationship with SystemAccount (CreatedBy)
                entity.HasOne(e => e.CreatedBy)
                      .WithMany(e => e.NewsArticles)
                      .HasForeignKey(e => e.CreatedByID)
                      .OnDelete(DeleteBehavior.Restrict);

                // Relationship with SystemAccount (UpdatedBy)
                entity.HasOne(e => e.UpdatedBy)
                      .WithMany()
                      .HasForeignKey(e => e.UpdatedByID)
                      .OnDelete(DeleteBehavior.Restrict);

                // Many-to-many relationship with Tags through NewsTag
                entity.HasMany(e => e.Tags)
                      .WithMany(e => e.NewsArticles)
                      .UsingEntity<NewsTag>(
                          j => j.HasOne(nt => nt.Tag)
                                .WithMany()
                                .HasForeignKey(nt => nt.TagID),
                          j => j.HasOne(nt => nt.NewsArticle)
                                .WithMany()
                                .HasForeignKey(nt => nt.NewsArticleID),
                          j => j.HasKey(nt => new { nt.NewsArticleID, nt.TagID }));
            });

            // Configure Tag entity
            modelBuilder.Entity<Tag>(entity =>
            {
                entity.HasKey(e => e.TagID);
                entity.Property(e => e.TagID).ValueGeneratedOnAdd();
            });

            // Configure NewsTag entity
            modelBuilder.Entity<NewsTag>(entity =>
            {
                entity.HasKey(e => new { e.NewsArticleID, e.TagID });
            });

            // Seed data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed default accounts
            modelBuilder.Entity<SystemAccount>().HasData(
                new SystemAccount
                {
                    AccountID = 1,
                    AccountName = "System Administrator",
                    AccountEmail = "admin@FUNewsManagementSystem.org",
                    AccountPassword = "@@abc123@@",
                    AccountRole = 3 // Admin
                },
                new SystemAccount
                {
                    AccountID = 2,
                    AccountName = "Staff User",
                    AccountEmail = "staff@FUNewsManagementSystem.org",
                    AccountPassword = "staff123",
                    AccountRole = 1 // Staff
                },
                new SystemAccount
                {
                    AccountID = 3,
                    AccountName = "Lecturer User",
                    AccountEmail = "lecturer@FUNewsManagementSystem.org",
                    AccountPassword = "lecturer123",
                    AccountRole = 2 // Lecturer
                }
            );

            // Seed default categories
            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    CategoryID = 1,
                    CategoryName = "General News",
                    CategoryDescription = "General news and announcements",
                    IsActive = true
                },
                new Category
                {
                    CategoryID = 2,
                    CategoryName = "Academic News",
                    CategoryDescription = "Academic related news and updates",
                    IsActive = true
                },
                new Category
                {
                    CategoryID = 3,
                    CategoryName = "Student Activities",
                    CategoryDescription = "Student activities and events",
                    IsActive = true
                }
            );
        }
    }
}
