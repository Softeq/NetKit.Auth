// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.NetKit.Auth.Domain.Models.User;
using Softeq.NetKit.Auth.SQLRepository.Abstract;
using Softeq.NetKit.Auth.SQLRepository.Mappings.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Softeq.NetKit.Auth.SQLRepository.Mappings
{
	public class PasswordHistoryMapping : DomainModelBuilder<PasswordHistory>, IEntityMappingConfiguration
	{
		public override void Build(EntityTypeBuilder<PasswordHistory> builder)
		{
			builder.HasKey(p => p.Id);
			builder.HasIndex(p => new { p.UserId, p.PasswordHash }).IsUnique();
			builder.Property(p => p.UserId).IsRequired();
			builder.Property(p => p.Created).IsRequired();
			builder.HasIndex(p => p.Created);
			builder.Property(p => p.PasswordHash).HasMaxLength(256).IsRequired();
			builder.ToTable("PasswordHistory");

			builder.HasOne(p => p.User).WithMany(u => u.Passwords).HasForeignKey(c => c.UserId).IsRequired();
		}
	}
}