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
        }
    }
}
