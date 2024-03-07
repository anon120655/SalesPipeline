using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Data.Entity;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Thailands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SalesPipeline.Infrastructure.Repositorys
{
	public class Thailand : IThailand
	{
		private IRepositoryWrapper _repo;
		private readonly IMapper _mapper;
		private readonly IRepositoryBase _db;
		private readonly AppSettings _appSet;

		public Thailand(IRepositoryWrapper repo, IRepositoryBase db, IOptions<AppSettings> appSet, IMapper mapper)
		{
			_db = db;
			_repo = repo;
			_mapper = mapper;
			_appSet = appSet.Value;
		}

		public async Task<IList<InfoProvinceCustom>> GetProvince(Guid? department_BranchId = null)
		{
			var query = _repo.Context.InfoProvinces.AsQueryable();

			if (department_BranchId.HasValue)
			{
				query = query.Where(x => x.Master_Department_BranchId == department_BranchId);
			}

			return _mapper.Map<IList<InfoProvinceCustom>>(await query.ToListAsync());
		}

		public async Task<IList<InfoAmphurCustom>> GetAmphur(int provinceID)
		{
			var query = _repo.Context.InfoAmphurs.Where(x => x.ProvinceID == provinceID).AsQueryable();
			return _mapper.Map<IList<InfoAmphurCustom>>(await query.ToListAsync());
		}

		public async Task<IList<InfoTambolCustom>> GetTambol(int provinceID, int amphurID)
		{
			var query = _repo.Context.InfoTambols.Where(x => x.ProvinceID == provinceID && x.AmphurID == amphurID).AsQueryable();
			return _mapper.Map<IList<InfoTambolCustom>>(await query.ToListAsync());
		}

		public async Task<InfoProvinceCustom> GetProvinceByid(int id)
		{
			var query = await _repo.Context.InfoProvinces.Where(x => x.ProvinceID == id).FirstOrDefaultAsync();
			return _mapper.Map<InfoProvinceCustom>(query);
		}

		public async Task<InfoAmphurCustom> GetAmphurByid(int id)
		{
			var query = await _repo.Context.InfoAmphurs.Where(x => x.AmphurID == id).FirstOrDefaultAsync();
			return _mapper.Map<InfoAmphurCustom>(query);
		}

		public async Task<InfoTambolCustom> GetTambolByid(int id)
		{
			var query = await _repo.Context.InfoTambols.Where(x => x.TambolID == id).FirstOrDefaultAsync();
			return _mapper.Map<InfoTambolCustom>(query);
		}

		public async Task<string?> GetProvinceNameByid(int id)
		{
			var name = await _repo.Context.InfoProvinces.Where(x => x.ProvinceID == id).Select(x => x.ProvinceName).FirstOrDefaultAsync();
			return name;
		}

		public async Task<string?> GetAmphurNameByid(int id)
		{
			var name = await _repo.Context.InfoAmphurs.Where(x => x.AmphurID == id).Select(x => x.AmphurName).FirstOrDefaultAsync();
			return name;
		}

		public async Task<string?> GetTambolNameByid(int id)
		{
			var name = await _repo.Context.InfoTambols.Where(x => x.TambolID == id).Select(x => x.TambolName).FirstOrDefaultAsync();
			return name;
		}

		public async Task MapZipCode(List<InfoTambolCustom> tambolList)
		{
			foreach (var item in tambolList)
			{
				var query = await _repo.Context.InfoTambols.Where(x => x.TambolCode == item.TambolCode).FirstOrDefaultAsync();
				if (query != null)
				{
					query.ZipCode = item.ZipCode;
					_db.Update(query);
					await _db.SaveAsync();
				}
			}

		}

		public async Task<InfoBranchCustom> CreateBranch(InfoBranchCustom model)
		{
			int id = 1;
			if (_repo.Context.InfoBranches.FirstOrDefault() != null)
			{
				id = _repo.Context.InfoBranches.Max(u => u == null ? 0 : u.BranchID) + 1;
			}

			var infoBranch = new Data.Entity.InfoBranch()
			{
				BranchID = id,
				ProvinceID = model.ProvinceID,
				BranchCode = model.BranchCode,
				BranchName = model.BranchName,
				BranchNameMain = model.BranchNameMain,
			};
			await _db.InsterAsync(infoBranch);
			await _db.SaveAsync();

			return _mapper.Map<InfoBranchCustom>(infoBranch);
		}

		public async Task<InfoBranchCustom> UpdateBranch(InfoBranchCustom model)
		{
			var infoBranches = await _repo.Context.InfoBranches.ToListAsync();
			foreach (var item in infoBranches)
			{
				item.BranchCode = item.BranchID.ToString("0000");
				_db.Update(item);
			}
			await _db.SaveAsync();

			return new();

			//var infoBranches = await _repo.Context.InfoBranches
			//									   .FirstOrDefaultAsync(x => x.BranchID == model.BranchID);
			//if (infoBranches != null)
			//{
			//	infoBranches.ProvinceID = model.ProvinceID;
			//	infoBranches.BranchCode = model.BranchCode;
			//	infoBranches.BranchName = model.BranchName;
			//	infoBranches.BranchNameMain = model.BranchNameMain;
			//	_db.Update(infoBranches);
			//  await _db.SaveAsync();
			//}

			//return _mapper.Map<InfoBranchCustom>(infoBranches);
		}

		public async Task<IList<InfoBranchCustom>> GetBranch(int provinceID)
		{
			var query = _repo.Context.InfoBranches.Where(x => x.ProvinceID == provinceID).AsQueryable();
			return _mapper.Map<IList<InfoBranchCustom>>(await query.ToListAsync());
		}

		public async Task<string?> GetBranchNameByid(int id)
		{
			var name = await _repo.Context.InfoBranches.Where(x => x.BranchID == id).Select(x => x.BranchName).FirstOrDefaultAsync();
			return name;
		}

		public async Task<InfoBranchCustom?> GetBranchByid(int id)
		{
			var InfoBranches = await _repo.Context.InfoBranches.Where(x => x.BranchID == id).FirstOrDefaultAsync();
			return _mapper.Map<InfoBranchCustom>(InfoBranches);
		}
	}
}
