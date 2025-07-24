using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Crypto;
using SalesPipeline.Infrastructure.Data.Entity;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Shares;
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
			var query = _repo.Context.InfoProvinces.AsNoTracking().AsQueryable();

			if (department_BranchId.HasValue)
			{
				query = query.Where(x => x.Master_Department_BranchId == department_BranchId);
			}

			return _mapper.Map<IList<InfoProvinceCustom>>(await query.ToListAsync());
		}

		public async Task<IList<InfoAmphurCustom>> GetAmphur(int provinceID)
		{
			var query = _repo.Context.InfoAmphurs.AsNoTracking().Where(x => x.ProvinceID == provinceID).AsQueryable();
			return _mapper.Map<IList<InfoAmphurCustom>>(await query.ToListAsync());
		}

		public async Task<IList<InfoTambolCustom>> GetTambol(int provinceID, int amphurID)
		{
			var query = _repo.Context.InfoTambols.AsNoTracking().Where(x => x.ProvinceID == provinceID && x.AmphurID == amphurID).AsQueryable();
			return _mapper.Map<IList<InfoTambolCustom>>(await query.ToListAsync());
		}

		public async Task<InfoProvinceCustom> GetProvinceByid(int id)
		{
			var query = await _repo.Context.InfoProvinces.AsNoTracking().Where(x => x.ProvinceID == id).FirstOrDefaultAsync();
			return _mapper.Map<InfoProvinceCustom>(query);
		}

		public async Task<InfoAmphurCustom> GetAmphurByid(int id)
		{
			var query = await _repo.Context.InfoAmphurs.AsNoTracking().Where(x => x.AmphurID == id).FirstOrDefaultAsync();
			return _mapper.Map<InfoAmphurCustom>(query);
		}

		public async Task<InfoTambolCustom> GetTambolByid(int id)
		{
			var query = await _repo.Context.InfoTambols.AsNoTracking().Where(x => x.TambolID == id).FirstOrDefaultAsync();
			return _mapper.Map<InfoTambolCustom>(query);
		}

		public async Task<string?> GetProvinceNameByid(int id)
		{
			var name = await _repo.Context.InfoProvinces.AsNoTracking().Where(x => x.ProvinceID == id).Select(x => x.ProvinceName).FirstOrDefaultAsync();
			return name;
		}

		public async Task<string?> GetAmphurNameByid(int id)
		{
			var name = await _repo.Context.InfoAmphurs.AsNoTracking().Where(x => x.AmphurID == id).Select(x => x.AmphurName).FirstOrDefaultAsync();
			return name;
		}

		public async Task<string?> GetTambolNameByid(int id)
		{
			var name = await _repo.Context.InfoTambols.AsNoTracking().Where(x => x.TambolID == id).Select(x => x.TambolName).FirstOrDefaultAsync();
			return name;
		}

		public async Task<int?> GetProvinceIdByName(string name)
		{
			name = name.Replace("จังหวัด", string.Empty);
			var query = await _repo.Context.InfoProvinces.AsNoTracking().Where(x => x.ProvinceName.Contains(name)).FirstOrDefaultAsync();
			if (query != null)
			{
				return query.ProvinceID;
			}
			return null;
		}

		public async Task<int?> GetAmphurIdByName(string name)
		{
			name = name.Replace("เขต", string.Empty);
			name = name.Replace("อำเภอ", string.Empty);
			var query = await _repo.Context.InfoAmphurs.AsNoTracking().Where(x => x.AmphurName.Contains(name)).FirstOrDefaultAsync();
			if (query != null)
			{
				return query.AmphurID;
			}
			return null;
		}

		public async Task<int?> GetTambolIdByName(string name)
		{
			var query = await _repo.Context.InfoTambols.AsNoTracking().Where(x => x.TambolName.Contains(name)).FirstOrDefaultAsync();
			if (query != null)
			{
				return query.TambolID;
			}
			return null;
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
				await _db.SaveAsync();
			}

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
			var query = _repo.Context.InfoBranches.AsNoTracking().AsQueryable();

			if (provinceID > 0)
			{
				query = query.Where(x => x.ProvinceID == provinceID);
			}

			return _mapper.Map<IList<InfoBranchCustom>>(await query.ToListAsync());
		}

		public async Task<IList<InfoBranchCustom>> GetBranchByDepBranchId(allFilter model)
		{
			//if ((model.DepBranch == null || model.DepBranch.Count == 0) && model.ids != null)
			//{
			//	List<string> ids_list = model.ids.Split(',').ToList<string>();
			//	if (ids_list.Count > 0)
			//	{
			//		model.Selecteds = new();
			//		foreach (var item in ids_list)
			//		{
			//			model.Selecteds.Add(item);
			//		}
			//	}
			//}

			if (model.DepBranchs == null || model.DepBranchs.Count == 0) return new List<InfoBranchCustom>();

			List<Guid?> idList = new();
			if (model.DepBranchs?.Count > 0)
			{
				idList = GeneralUtils.ListStringToGuid(model.DepBranchs);
			}

			var infoProvinces = _repo.Context.InfoProvinces.Where(x => idList.Contains(x.Master_Department_BranchId)).Select(x => x.ProvinceID).ToList();
			if (infoProvinces.Count > 0)
			{
				var query = _repo.Context.InfoBranches.Where(x => infoProvinces.Contains(x.ProvinceID)).AsQueryable();
				return _mapper.Map<IList<InfoBranchCustom>>(await query.ToListAsync());
			}
			return new List<InfoBranchCustom>();
		}

		public async Task<string?> GetBranchNameByid(int id)
		{
			var name = await _repo.Context.InfoBranches.AsNoTracking().Where(x => x.BranchID == id).Select(x => x.BranchName).FirstOrDefaultAsync();
			return name;
		}

		public async Task<InfoBranchCustom?> GetBranchByid(int id)
		{
			var InfoBranches = await _repo.Context.InfoBranches.AsNoTracking().Where(x => x.BranchID == id).FirstOrDefaultAsync();
			return _mapper.Map<InfoBranchCustom>(InfoBranches);
		}

	}
}
