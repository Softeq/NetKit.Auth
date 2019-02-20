// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Linq;
using System.Linq.Expressions;
using EnsureThat;
using Microsoft.EntityFrameworkCore;
using Softeq.NetKit.Auth.Infrastructure.Interfaces.Infrastructure;

namespace Softeq.NetKit.Auth.Repository.Base
{
	public abstract class BaseWriteRepository<TEntity> : IWriteRepository<TEntity>
		where TEntity : class
	{
		protected readonly DbSet<TEntity> dbset;

		protected BaseWriteRepository(DbContext dataContext)
		{
			Ensure.That(dataContext, "dataContext").IsNotNull();

			DataContext = dataContext;
			dbset = DataContext.Set<TEntity>();
		}

		protected DbContext DataContext { get; }

		public virtual TEntity Add(TEntity entity)
		{
			Ensure.That(entity, "entity").IsNotNull();
			dbset.Add(entity);
			return entity;
		}

		public virtual void Delete(TEntity entity)
		{
			Ensure.That(entity, "entity").IsNotNull();
			dbset.Remove(entity);
		}

		public virtual void Update(TEntity entity)
		{
			Ensure.That(entity, "entity").IsNotNull();
			dbset.Attach(entity);
			DataContext.Entry(entity).State = EntityState.Modified;
		}

		public virtual void Delete(Expression<Func<TEntity, bool>> where)
		{
			Ensure.That(where, "where").IsNotNull();
			var objects = dbset.Where(where).AsEnumerable();
			foreach (var obj in objects)
				dbset.Remove(obj);
		}
	}
}