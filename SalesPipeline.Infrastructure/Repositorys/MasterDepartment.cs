using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.ConstTypeModel;
using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Repositorys
{
    public class MasterDepartment : IMasterDepartment
	{
		private IRepositoryWrapper _repo;
		private readonly IMapper _mapper;
		private readonly IRepositoryBase _db;
		private readonly AppSettings _appSet;

		public MasterDepartment(IRepositoryWrapper repo, IRepositoryBase db, IOptions<AppSettings> appSet, IMapper mapper)
		{
			_db = db;
			_repo = repo;
			_mapper = mapper;
			_appSet = appSet.Value;
		}

		public Task<Master_DepartmentCustom> Create(Master_DepartmentCustom model)
		{
			throw new NotImplementedException();
		}

		public Task<Master_DepartmentCustom> Update(Master_DepartmentCustom model)
		{
			throw new NotImplementedException();
		}

		public Task DeleteById(UpdateModel model)
		{
			throw new NotImplementedException();
		}

		public Task<Master_DepartmentCustom> GetById(Guid id)
		{
			throw new NotImplementedException();
		}

		public async Task<string?> GetNameById(Guid id)
		{
			var name = await _repo.Context.Master_Departments.Where(x => x.Id == id).Select(x => x.Name).FirstOrDefaultAsync();
			return name;
		}

		public async Task<PaginationView<List<Master_DepartmentCustom>>> GetList(allFilter model)
		{
			var query = _repo.Context.Master_Departments
												 .Where(x => x.Status != StatusModel.Delete)
												 .OrderByDescending(x => x.UpdateDate).ThenByDescending(x => x.CreateDate)
												 .AsQueryable();
			if (model.status.HasValue)
			{
				query = query.Where(x => x.Status == model.status);
			}

			if (!String.IsNullOrEmpty(model.val1))
			{
				query = query.Where(x => x.Code != null && x.Code.Contains(model.val1));
			}

			if (!String.IsNullOrEmpty(model.val2))
			{
				query = query.Where(x => x.Name != null && x.Name.Contains(model.val2));
			}

			var pager = new Pager(query.Count(), model.page, model.pagesize, null);

			var items = query.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

			return new PaginationView<List<Master_DepartmentCustom>>()
			{
				Items = _mapper.Map<List<Master_DepartmentCustom>>(await items.ToListAsync()),
				Pager = pager
			};
		}

	}
}
