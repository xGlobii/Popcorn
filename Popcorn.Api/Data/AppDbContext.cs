using Microsoft.EntityFrameworkCore;
using Popcorn.Api.Models.Entities;

namespace Popcorn.Api.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

		public DbSet<User> Users { get; set; }
		public DbSet<UserSession> UserSessions { get; set; }
		public DbSet<ActivityItem> Activities { get; set; }
		public DbSet<TmdbMedia> MediaItem { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<User>()
				.HasIndex(u => u.Email)
				.IsUnique();
			modelBuilder.Entity<User>()
				.Property(u => u.Email)
				.IsRequired();
			modelBuilder.Entity<User>()
				.Property(u => u.HashedPassword)
				.HasMaxLength(100)
				.IsRequired();
			modelBuilder.Entity<User>()
				.HasIndex(u => u.Username)
				.IsUnique();
			modelBuilder.Entity<User>()
				.Property(u => u.Username)
				.HasMaxLength(50)
				.IsRequired();
			modelBuilder.Entity<UserSession>()
				.HasOne(u => u.User)
				.WithMany(u => u.Sessions)
				.HasForeignKey(u => u.UserId);

			modelBuilder.Entity<ActivityItem>()
				.HasOne(u => u.User)
				.WithMany(u => u.Activities)
				.HasForeignKey(u => u.UserId);
			modelBuilder.Entity<ActivityItem>()
				.HasOne(u => u.MediaItem)
				.WithMany(u => u.UserMediaItems)
				.HasForeignKey(u => new { u.MediaType, u.TmdbId });
			modelBuilder.Entity<TmdbMedia>()
				.HasKey(u => new { u.MediaType, u.TmdbId });
			modelBuilder.Entity<ActivityItem>()
				.HasKey(u => new { u.MediaType, u.TmdbId, u.UserId });
		}
	}
}
