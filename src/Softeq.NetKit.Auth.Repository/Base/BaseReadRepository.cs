// Developed by Softeq Development Corporation
// http://www.softeq.com

using Microsoft.EntityFrameworkCore;
using Softeq.NetKit.Auth.Infrastructure.Interfaces.Infrastructure;

namespace Softeq.NetKit.Auth.Repository.Base
{
    public abstract class BaseReadRepository<TEntity, TKey> : BaseReadRepositoryWithoutKey<TEntity>, IReadRepository<TEntity, TKey>
        where TEntity : class, IBaseEntity<TKey>, new()
    {
        protected BaseReadRepository(DbContext context)
            : base(context)
        {
        }

        public virtual TEntity GetById(TKey id)
        {
            return dbset.Find(id);
        }
    }
}