// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.NetKit.Auth.Domain.Models.User;
using Softeq.NetKit.Auth.SQLRepository.Abstract;
using Softeq.NetKit.Auth.SQLRepository.Mappings.Abstract;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Softeq.NetKit.Auth.SQLRepository.Mappings
{
    public class UserMapping : DomainModelBuilder<User>, IEntityMappingConfiguration
    {
        public override void Build(EntityTypeBuilder<User> builder)
        {
	        builder.HasKey(u => u.Id);
			builder.Property(u => u.StatusId).IsRequired();
	        builder.Property(u => u.Created).IsRequired();
	        builder.Property(u => u.IsAcceptedTermsAndPolicy).IsRequired();
	        builder.Property(u => u.TokenRevokedDate).IsRequired(false);
	        builder.Property(u => u.LastPasswordChangedDate).IsRequired(false);
	        builder.Property(u => u.LastPasswordExpiredEmailSentDate).IsRequired(false);
	        builder.Property(u => u.LastPasswordExpiresEmailSentDate).IsRequired(false);
	        builder.Property(u => u.LastAccountFailedAttemptsEmailSentDate).IsRequired(false);

			builder.HasOne(u => u.DeletedUserInfo).WithOne(u => u.User).HasForeignKey<DeletedUserInfo>(c => c.UserId).IsRequired();
	        builder.HasMany(u => u.UserRoles).WithOne(u => u.User).HasForeignKey(u => u.UserId).IsRequired();
	        builder.HasMany(u => u.Passwords).WithOne(u => u.User).HasForeignKey(u => u.UserId).IsRequired();
		}
    }
}