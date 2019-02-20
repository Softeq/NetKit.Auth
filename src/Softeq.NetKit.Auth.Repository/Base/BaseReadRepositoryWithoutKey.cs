// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Threading.Tasks;
using EnsureThat;
using Microsoft.EntityFrameworkCore;
using Softeq.NetKit.Auth.Infrastructure.Interfaces.Infrastructure;

namespace Softeq.NetKit.Auth.Repository.Base
{
    public abstract class BaseReadRepositoryWithoutKey<TEntity> : IReadRepositoryWithoutKey<TEntity>
        where TEntity : class, new()
    {
        protected readonly DbSet<TEntity> dbset;
        protected DbContext DataContext { get; }

        protected BaseReadRepositoryWithoutKey(DbContext dataContext)
        {
            Ensure.That(dataContext, "dataContext").IsNotNull();

            DataContext = dataContext;
            dbset = DataContext.Set<TEntity>();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await dbset.ToListAsync();
        }
    }
}