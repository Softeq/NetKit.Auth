// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Softeq.NetKit.Auth.Infrastructure.Interfaces.Infrastructure
{
	public interface IReadRepository<TEntity, in TKey> : IReadRepositoryWithoutKey<TEntity> where TEntity : class, IBaseEntity<TKey>
	{
		TEntity GetById(TKey id);
	}

	public interface IReadRepositoryWithoutKey<TEntity>
		where TEntity : class
	{
		Task<IEnumerable<TEntity>> GetAllAsync();
	}

	public interface IWriteRepository<TEntity>
		where TEntity : class
	{
		TEntity Add(TEntity entity);
		void Delete(TEntity entity);
		void Update(TEntity entity);
	}
}