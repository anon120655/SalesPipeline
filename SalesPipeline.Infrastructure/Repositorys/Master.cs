using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Repositorys
{
	public class Master : IMaster
	{
		private IRepositoryWrapper _repo;
		private readonly IMapper _mapper;
		private readonly IRepositoryBase _db;
		private readonly AppSettings _appSet;

		public Master(IRepositoryWrapper repo, IRepositoryBase db, IOptions<AppSettings> appSet, IMapper mapper)
		{
			_db = db;
			_repo = repo;
			_mapper = mapper;
			_appSet = appSet.Value;
		}

		public async Task<IList<Master_PositionCustom>> Positions(allFilter model)
		{
			var query = _repo.Context.Master_Positions
				.Where(x => x.Status != StatusModel.Delete)
				.OrderBy(x => x.Id)
				.AsQueryable();

			return _mapper.Map<IList<Master_PositionCustom>>(await query.ToListAsync());
		}

		public async Task<IList<Master_DepartmentCustom>> Departments(allFilter model)
		{
			var query = _repo.Context.Master_Departments
				.Where(x => x.Status != StatusModel.Delete)
				.OrderBy(x => x.Id)
				.AsQueryable();

			return _mapper.Map<IList<Master_DepartmentCustom>>(await query.ToListAsync());
		}

		public async Task<IList<Master_RegionCustom>> Regions(allFilter model)
		{
			var query = _repo.Context.Master_Regions
				.Where(x => x.Status != StatusModel.Delete)
				.OrderBy(x => x.Id)
				.AsQueryable();

			return _mapper.Map<IList<Master_RegionCustom>>(await query.ToListAsync());
		}

		public async Task<IList<Master_BranchCustom>> Branchs(allFilter model)
		{
			var query = _repo.Context.Master_Branches
				.Where(x => x.Status != StatusModel.Delete)
				.OrderBy(x => x.Id)
				.AsQueryable();

			if (model.ids != null)
			{
				List<string> Ids = model.ids.Split(',').ToList<string>();
				query = query.Where(x => Ids.Contains(x.RegionId.ToString()));
			}

			return _mapper.Map<IList<Master_BranchCustom>>(await query.ToListAsync());
		}

		public async Task<IList<MenuItemCustom>> MenuItem(allFilter model)
		{
			var query = _repo.Context.MenuItems
				.Where(x => x.Status != StatusModel.Delete)
				.OrderBy(x => x.MenuNumber).ThenBy(x => x.ParentNumber)
				.AsQueryable();

			return _mapper.Map<IList<MenuItemCustom>>(await query.ToListAsync());
		}

	}
}
