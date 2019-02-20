// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.NetKit.Auth.Domain.Models.User;
using Softeq.NetKit.Auth.SQLRepository.Abstract;
using Softeq.NetKit.Auth.SQLRepository.Mappings.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Softeq.NetKit.Auth.SQLRepository.Mappings
{
	public class DeletedUserInfoMapping : DomainModelBuilder<DeletedUserInfo>, IEntityMappingConfiguration
	{
		public override void Build(EntityTypeBuilder<DeletedUserInfo> builder)
        {
            builder.HasKey(dui => dui.UserId);
            builder.Property(dui => dui.UserId).IsRequired();
            builder.Property(dui => dui.Email).HasMaxLength(256).IsRequired();
            builder.Property(dui => dui.NormalizedEmail).HasMaxLength(256).IsRequired();
            builder.Property(dui => dui.UserName).HasMaxLength(256).IsRequired();
            builder.Property(dui => dui.NormalizedUserName).HasMaxLength(256).IsRequired();
            builder.ToTable("DeletedUserInfo");
        }
	}
}