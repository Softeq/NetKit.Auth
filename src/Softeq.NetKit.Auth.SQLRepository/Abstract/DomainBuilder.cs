// Developed by Softeq Development Corporation
// http://www.softeq.com

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Softeq.NetKit.Auth.SQLRepository.Abstract
{
	public abstract class DomainModelBuilder<T> : IDomainModelBuilder<T> where T : class
	{
		public abstract void Build(EntityTypeBuilder<T> builder);

		public void Build(ModelBuilder builder)
		{
			Build(builder.Entity<T>());
		}
	}
}
