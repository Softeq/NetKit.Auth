// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.NetKit.Auth.Domain.Models.User;
using Softeq.NetKit.Auth.SQLRepository.Abstract;
using Softeq.NetKit.Auth.SQLRepository.Mappings.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Softeq.NetKit.Auth.SQLRepository.Mappings
{
    internal class UserStatusMapping : DomainModelBuilder<UserStatus>, IEntityMappingConfiguration
    {
        public override void Build(EntityTypeBuilder<UserStatus> builder)
        {
            builder.HasKey(us => us.Id);
            builder.Property<int>(us => us.Id).ValueGeneratedNever();
            builder.ToTable("UserStatus");

            builder.HasMany(us => us.Users).WithOne(us => us.Status).HasForeignKey(us => us.StatusId).IsRequired();
        }
    }
}
