using GYM.Domain.Entities;
using GYM.Infrastructure.Seeds;
using GYM.Mi.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GYM.Infrastructure
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser,
     ApplicationRole, Guid,
        ApplicationUserClaim, ApplicationUserRole,
        ApplicationUserLogin, ApplicationRoleClaim,
        ApplicationUserToken>    
    {
        private readonly string _connectionString;
        private readonly string _migrationAssembly;


       public DbSet<User> Users {  get; set; } 
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<Blog> Blogs { get; set; }


        public ApplicationDbContext(string connectionString,string migrationAssembly) 
        {
            _connectionString = connectionString;
            _migrationAssembly = migrationAssembly;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString, (x) => x.MigrationsAssembly(_migrationAssembly));
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApplicationRole>().HasData(RoleSeed.GetRoles());
            base.OnModelCreating(builder);

            builder.Entity<User>()
                .HasOne(u=>u.Trainer)
                .WithMany(e=>e.Students)
                .HasForeignKey(U=>U.TrainerEmployeeId)
                .OnDelete(DeleteBehavior.SetNull);

            //Member Ships
            builder.Entity<Membership>(entity =>
            {
                entity.HasKey(m => m.Id);

                entity.Property(m => m.PlanName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(m => m.Amount)
                    .HasColumnType("decimal(18,2)");

                entity.Property(m => m.PaymentStatus)
                    .HasMaxLength(20)
                    .HasDefaultValue("Pending");

                entity.Ignore(m => m.IsActive);
                entity.Ignore(m => m.DaysRemaining);
                entity.Ignore(m => m.ProgressPercent);

                entity.HasOne(m => m.User)
                    .WithMany(u => u.Memberships)
                    .HasForeignKey(m => m.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            //Blog
            builder.Entity<Blog>(entity =>
            {
                entity.HasKey(b => b.Id);

                entity.Property(b => b.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(b => b.Slug)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.HasIndex(b => b.Slug)
                    .IsUnique();

                entity.Property(b => b.ShortDescription)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(b => b.FullContent)
                    .IsRequired();

                entity.Property(b => b.FeaturedImageUrl)
                    .HasMaxLength(500);

                entity.Property(b => b.AuthorName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(b => b.IsPublished)
                    .HasDefaultValue(false);
            });
        }
    }
}
