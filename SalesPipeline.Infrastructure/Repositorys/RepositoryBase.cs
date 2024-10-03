using Microsoft.EntityFrameworkCore;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using System.Linq.Expressions;

namespace SalesPipeline.Infrastructure.Repositorys
{
	public class RepositoryBase : IRepositoryBase
	{
		private readonly IRepositoryWrapper unitofWork;
		public RepositoryBase(IRepositoryWrapper unitOfWork)
		{
			unitofWork = unitOfWork;
		}

		public IQueryable<T> Get<T>() where T : class
		{
			return unitofWork.Context.Set<T>().AsQueryable<T>();
		}

		public async Task<IEnumerable<T>> FindAllAsync<T>() where T : class
		{
			return await unitofWork.Context.Set<T>().ToListAsync<T>();
		}

		public async Task<IEnumerable<T>> FindByConditionAync<T>(Expression<Func<T, bool>> predicate) where T : class
		{
			return await unitofWork.Context.Set<T>().Where(predicate).ToListAsync<T>();
		}

		public T Inster<T>(T entity) where T : class
		{
			unitofWork.Context.ChangeTracker.Clear();

			unitofWork.Context.Set<T>().Add(entity);
			return entity;
		}

		public async Task<T> InsterAsync<T>(T entity) where T : class
		{
			unitofWork.Context.ChangeTracker.Clear();

			await unitofWork.Context.Set<T>().AddAsync(entity);
			return entity;
		}

		public void InsterRange<T>(IEnumerable<T> entities) where T : class
		{
			unitofWork.Context.ChangeTracker.Clear();

			unitofWork.Context.Set<T>().AddRange(entities);
		}

		public T Update<T>(T entity) where T : class
		{
			unitofWork.Context.ChangeTracker.Clear();

			unitofWork.Context.Set<T>().Update(entity);

			return entity;
		}

		public void UpdateRange<T>(IEnumerable<T> entities) where T : class
		{
			unitofWork.Context.ChangeTracker.Clear();
			unitofWork.Context.Set<T>().UpdateRange(entities);
		}

		public void Delete<T>(T entity) where T : class
		{
			unitofWork.Context.ChangeTracker.Clear();

			unitofWork.Context.Set<T>().Remove(entity);
		}

		public void DeleteRange<T>(IEnumerable<T> entitys) where T : class
		{
			unitofWork.Context.ChangeTracker.Clear();

			unitofWork.Context.Set<T>().RemoveRange(entitys);
		}

		public async Task SaveAsync()
		{
			await unitofWork.Context.SaveChangesAsync();
		}

	}
}
