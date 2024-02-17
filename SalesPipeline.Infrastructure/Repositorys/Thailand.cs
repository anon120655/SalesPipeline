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
				query = query.Where(x=>x.Master_Department_BranchId == department_BranchId);
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

	}
}
