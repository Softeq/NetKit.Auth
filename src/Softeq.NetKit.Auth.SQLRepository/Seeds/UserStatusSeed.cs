// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.NetKit.Auth.Domain.Models.User;
using Softeq.NetKit.Auth.SQLRepository.Abstract;
using Softeq.NetKit.Auth.SQLRepository.Seeds.Abstract;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Softeq.NetKit.Auth.SQLRepository.Seeds
{
	internal class UserStatusSeed : DomainModelBuilder<UserStatus>, IEntitySeedConfiguration
	{
		public override void Build(EntityTypeBuilder<UserStatus> builder)
		{
			builder.HasData(
				new UserStatus { Id = (int)UserStatusEnum.Pending, Name = nameof(UserStatusEnum.Pending) },
				new UserStatus { Id = (int)UserStatusEnum.Active, Name = nameof(UserStatusEnum.Active) },
				new UserStatus { Id = (int)UserStatusEnum.Inactive, Name = nameof(UserStatusEnum.Inactive) },
				new UserStatus { Id = (int)UserStatusEnum.Deleted, Name = nameof(UserStatusEnum.Deleted) });
		}
	}
}
