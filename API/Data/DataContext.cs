using System.Reflection;

namespace API.Data
{
    public class DataContext : IdentityDbContext<AppUser, AppRole, int,
         IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>,
         IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions<DataContext> options)
         : base(options)
        {
        }

        public DbSet<Photo> Photos { get; set; }
        public DbSet<UserLike> Likes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
             

            builder.Entity<AppUser>()
                   .HasMany(ur => ur.UserRoles)
                   .WithOne(u => u.User)
                   .HasForeignKey(ur => ur.UserId)
                   .IsRequired();

            builder.Entity<AppRole>()
                   .HasMany(ur => ur.UserRoles)
                   .WithOne(u => u.Role)
                   .HasForeignKey(ur => ur.RoleId)
                   .IsRequired();

            builder.Entity<AppUser>()
                .Property(u => u.Id)
                 .ValueGeneratedOnAdd();



            builder.Entity<UserLike>()
            .HasKey(k => new { k.SourceUserId, k.LikedUserId });

            builder.Entity<UserLike>()
                .HasOne(s => s.SourceUser)
                .WithMany(l => l.LikedUsers)
                .HasForeignKey(s => s.SourceUserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<UserLike>()
                .HasOne(s => s.LikedUser)
                .WithMany(l => l.LikedByUsers)
                .HasForeignKey(s => s.LikedUserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

        }

    }

}


