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

		public async Task<IList<InfoProvince>> GetProvince()
		{
			var query = await _repo.Context.InfoProvinces.ToListAsync();
			return query;
		}

		public async Task<IList<InfoAmphur>> GetAmphur(int provinceID)
		{
			var query = await _repo.Context.InfoAmphurs.Where(x => x.ProvinceID == provinceID).ToListAsync();
			return query;
		}

		public async Task<IList<InfoTambol>> GetTambol(int provinceID, int amphurID)
		{
			var query = await _repo.Context.InfoTambols.Where(x => x.ProvinceID == provinceID && x.AmphurID == amphurID).ToListAsync();
			return query;
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
