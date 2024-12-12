using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using movies_BLL.models;

namespace movies_DAL
{
    public class DataContext : IdentityDbContext<User, IdentityRole, string>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Movie> Movies { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;
        public DbSet<Genre> Genres { get; set; } = null!;
        public DbSet<MovieGenre> MovieGenres { get; set; } = null!;
        public DbSet<Streamingsite> Streamingsites { get; set; } = null!;
        public DbSet<MovieStreamingsite> MovieStreamingsites { get; set; } = null!;
        public DbSet<UserWatchlistMovie> UserWatchlistMovies { get; set; } = null!;
        public DbSet<UserSeenMovie> UserSeenMovies { get; set; } = null!;
        public DbSet<UserFavouriteMovie> UserFavouriteMovies { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserFavouriteMovie>()
                .HasKey(ufm => new { ufm.UserId, ufm.MovieId });

            modelBuilder.Entity<UserFavouriteMovie>()
                .HasOne(ufm => ufm.User)
                .WithMany(u => u.FavouriteMovies)
                .HasForeignKey(ufm => ufm.UserId);

            modelBuilder.Entity<UserFavouriteMovie>()
                .HasOne(ufm => ufm.Movie)
                .WithMany(m => m.FavouritedByUsers)
                .HasForeignKey(ufm => ufm.MovieId);

            modelBuilder.Entity<UserSeenMovie>()
                .HasKey(usm => new { usm.UserId, usm.MovieId });

            modelBuilder.Entity<UserSeenMovie>()
                .HasOne(usm => usm.User)
                .WithMany(u => u.SeenMovies)
                .HasForeignKey(usm => usm.UserId);

            modelBuilder.Entity<UserSeenMovie>()
                .HasOne(usm => usm.Movie)
                .WithMany(m => m.SeenByUsers)
                .HasForeignKey(usm => usm.MovieId);

            modelBuilder.Entity<UserWatchlistMovie>()
                .HasKey(uwm => new { uwm.UserId, uwm.MovieId });

            modelBuilder.Entity<UserWatchlistMovie>()
                .HasOne(uwm => uwm.User)
                .WithMany(u => u.WatchlistedMovies)
                .HasForeignKey(uwm => uwm.UserId);

            modelBuilder.Entity<UserWatchlistMovie>()
                .HasOne(uwm => uwm.Movie)
                .WithMany(m => m.WatchlistedByUsers)
                .HasForeignKey(uwm => uwm.MovieId);

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.HasKey(m => m.Id);
                entity.Property(m => m.Id).ValueGeneratedOnAdd(); 
                entity.HasMany(m => m.Comments).WithOne(c => c.Movie);
                entity.HasMany(m => m.MovieGenres).WithOne(mg => mg.Movie).HasForeignKey(mg => mg.MovieId);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasMany(u => u.Comments).WithOne(c => c.User);
            });

            modelBuilder.Entity<IdentityUserLogin<string>>()
            .HasKey(l => new { l.LoginProvider, l.ProviderKey });
            modelBuilder.Entity<IdentityUserRole<string>>()
            .HasKey(r => new { r.UserId, r.RoleId });
            modelBuilder.Entity<IdentityUserToken<string>>()
            .HasKey(t => new { t.UserId, t.LoginProvider, t.Name });
        }
    }

}
