// Developed by Softeq Development Corporation
// http://www.softeq.com

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Softeq.NetKit.Auth.SQLRepository.Abstract
{
	internal interface IDomainModelBuilder
	{
		void Build(ModelBuilder builder);
	}

	internal interface IDomainModelBuilder<T> : IDomainModelBuilder where T : class
	{
		void Build(EntityTypeBuilder<T> builder);
	}
}
