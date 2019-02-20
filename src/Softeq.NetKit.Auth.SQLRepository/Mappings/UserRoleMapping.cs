// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.NetKit.Auth.Domain.Models.UserRoles;
using Softeq.NetKit.Auth.SQLRepository.Abstract;
using Softeq.NetKit.Auth.SQLRepository.Mappings.Abstract;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Softeq.NetKit.Auth.SQLRepository.Mappings
{
	public class UserRoleMapping : DomainModelBuilder<UserRole>, IEntityMappingConfiguration
	{
		public override void Build(EntityTypeBuilder<UserRole> builder)
		{
			builder.HasKey(ur => new { ur.UserId, ur.RoleId });

			builder.HasOne(ur => ur.User).WithMany(u => u.UserRoles).HasForeignKey(ur => ur.UserId).IsRequired();
			builder.HasOne(ur => ur.Role).WithMany(u => u.UserRoles).HasForeignKey(ur => ur.RoleId).IsRequired();
		}
	}
}