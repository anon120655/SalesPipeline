using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Crypto;
using SalesPipeline.Infrastructure.Data.Entity;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.ConstTypeModel;
using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils.Resources.Thailands;
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

		public async Task<IList<Master_ListCustom>> MasterLists(allFilter model)
		{
			var query = _repo.Context.Master_Lists
				.Where(x => x.Status != StatusModel.Delete)
				.OrderBy(x => x.Id)
				.AsQueryable();

			if (model.id != Guid.Empty)
			{
				query = query.Where(x => x.Id == model.id);
			}

			return _mapper.Map<IList<Master_ListCustom>>(await query.ToListAsync());
		}

		public async Task<IList<Master_ProductProgramBankCustom>> ProductProgramBanks(allFilter model)
		{
			var query = _repo.Context.Master_ProductProgramBanks
				.Where(x => x.Status != StatusModel.Delete)
				.OrderBy(x => x.Id)
				.AsQueryable();

			return _mapper.Map<IList<Master_ProductProgramBankCustom>>(await query.ToListAsync());
		}

		public async Task<IList<Master_TypeLoanRequestCustom>> TypeLoanRequests(allFilter model)
		{
			var query = _repo.Context.Master_TypeLoanRequests
				.Where(x => x.Status != StatusModel.Delete)
				.OrderBy(x => x.Id)
				.AsQueryable();

			return _mapper.Map<IList<Master_TypeLoanRequestCustom>>(await query.ToListAsync());
		}

		public async Task<IList<Master_ProceedCustom>> Proceeds(allFilter model)
		{
			var query = _repo.Context.Master_Proceeds
				.Where(x => x.Status != StatusModel.Delete)
				.OrderBy(x => x.Id)
				.AsQueryable();

			return _mapper.Map<IList<Master_ProceedCustom>>(await query.ToListAsync());
		}

		public async Task<IList<Master_PositionCustom>> Positions(allFilter model)
		{
			var query = _repo.Context.Master_Positions
				.Where(x => x.Status != StatusModel.Delete)
				.OrderBy(x => x.Id)
				.AsQueryable();

			//*** ไม่มีส่วนนี้ ยุบเมนูจัดการ user มารวมกับ จัดการระบบผู้ใช้งาน
			//if (int.TryParse(model.type, out int _type))
			//{
			//	query = query.Where(x => x.Type == _type);
			//}

			return _mapper.Map<IList<Master_PositionCustom>>(await query.ToListAsync());
		}

		public async Task<IList<Master_YearCustom>> Year(allFilter model)
		{
			var query = _repo.Context.Master_Years
				.Where(x => x.Status != StatusModel.Delete)
				.OrderBy(x => x.Id)
				.AsQueryable();

			return _mapper.Map<IList<Master_YearCustom>>(await query.ToListAsync());
		}

		public async Task<IList<MenuItemCustom>> MenuItem(allFilter model)
		{
			var query = _repo.Context.MenuItems
				.Where(x => x.Status != StatusModel.Delete)
				.OrderBy(x => x.MenuNumber).ThenBy(x => x.ParentNumber)
				.AsQueryable();

			if (model.status.HasValue)
			{
				query = query.Where(x=>x.Status == model.status.Value);
			}

			return _mapper.Map<IList<MenuItemCustom>>(await query.ToListAsync());
		}

	}
}
