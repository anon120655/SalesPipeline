using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IRepositoryBase
	{
		IQueryable<T> Get<T>() where T : class;
		Task<IEnumerable<T>> FindAllAsync<T>() where T : class;
		Task<IEnumerable<T>> FindByConditionAync<T>(Expression<Func<T, bool>> predicate) where T : class;
		T Inster<T>(T entity) where T : class;
		Task<T> InsterAsync<T>(T entity) where T : class;
		void InsterRange<T>(IEnumerable<T> entities) where T : class;
		T Update<T>(T entity) where T : class;
		void UpdateRange<T>(IEnumerable<T> entities) where T : class;
		void Delete<T>(T entity) where T : class;
		void DeleteRange<T>(IEnumerable<T> entity) where T : class;
		Task SaveAsync();
	}
}
