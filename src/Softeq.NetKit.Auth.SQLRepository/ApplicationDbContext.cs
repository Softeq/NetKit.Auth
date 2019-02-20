// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Softeq.NetKit.Auth.Domain.Models.Role;
using Softeq.NetKit.Auth.Domain.Models.User;
using Softeq.NetKit.Auth.Domain.Models.UserRoles;
using Softeq.NetKit.Auth.Infrastructure.Interfaces.Infrastructure;
using Softeq.NetKit.Auth.SQLRepository.Extensions;
using Softeq.NetKit.Auth.SQLRepository.Mappings.Abstract;
using Softeq.NetKit.Auth.SQLRepository.Seeds.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Softeq.NetKit.Auth.SQLRepository
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, string, IdentityUserClaim<string>, UserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

		public DbSet<Role> Role { get; set; }
		public DbSet<UserRole> UserRole { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserStatus> UserStatus { get; set; }
        public DbSet<DeletedUserInfo> DeletedUserInfo { get; set; }
		public DbSet<PasswordHistory> PasswordHistory { get; set; }

		public override int SaveChanges()
        {
            AddTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = new CancellationToken())
        {
            AddTimestamps();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            AddTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
			builder.HasDefaultSchema("auth");
			//put db configuration here
			base.OnModelCreating(builder);

            builder.AddEntityConfigurationsFromAssembly<IEntityMappingConfiguration>(GetType().Assembly);
            builder.AddEntityConfigurationsFromAssembly<IEntitySeedConfiguration>(GetType().Assembly);
        }

        private void AddTimestamps()
        {
            var entitiesAdded =
                this.ChangeTracker.Entries().Where(x => x.Entity is ICreated && x.State == EntityState.Added);
            foreach (var entity in entitiesAdded)
                ((ICreated)entity.Entity).Created = DateTime.UtcNow;

            var entitiesModified =
                this.ChangeTracker.Entries().Where(x => x.Entity is IUpdated && x.State == EntityState.Modified);
            foreach (var entity in entitiesModified)
                ((IUpdated)entity.Entity).Updated = DateTime.UtcNow;
        }
    }
}