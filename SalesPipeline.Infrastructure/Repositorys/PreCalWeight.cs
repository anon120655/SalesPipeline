using AutoMapper;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils.Resources.PreApprove;
using SalesPipeline.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SalesPipeline.Infrastructure.Data.Entity;
using SalesPipeline.Utils.ConstTypeModel;

namespace SalesPipeline.Infrastructure.Repositorys
{
    public class PreCalWeight : IPreCalWeight
	{
		private IRepositoryWrapper _repo;
		private readonly IMapper _mapper;
		private readonly IRepositoryBase _db;
		private readonly AppSettings _appSet;

		public PreCalWeight(IRepositoryWrapper repo, IRepositoryBase db, IOptions<AppSettings> appSet, IMapper mapper)
		{
			_db = db;
			_repo = repo;
			_mapper = mapper;
			_appSet = appSet.Value;
		}

		public async Task Validate(List<Pre_Cal_WeightFactorCustom> model)
		{
			await Task.Delay(1);

			var TotalPercentAll = model.Sum(x => x._TotalPercent);
			if (TotalPercentAll != 100)
			{
				throw new ExceptionCustom("น้ำหนักรวมต้องเท่ากับ 100%");
			}

			//if (model.Pre_Cal_Fetu_Stan_ItemOptions == null || model.Pre_Cal_Fetu_Stan_ItemOptions.Count == 0)
			//{
			//	throw new ExceptionCustom("ระบุ Drop Down ไม่ถูกต้อง");
			//}

			//foreach (var item in model.Pre_Cal_Fetu_Stan_ItemOptions)
			//{
			//	if (string.IsNullOrEmpty(item.Name))
			//	{
			//		throw new ExceptionCustom("ระบุ Drop Down ไม่ครบถ้วน");
			//	}
			//}

			//if (!model.HighScore.HasValue || model.HighScore == 0)
			//{
			//	throw new ExceptionCustom("ระบุ คะแนนสูงสุด");
			//}

			//if (model.Pre_Cal_Fetu_Stan_Scores == null || model.Pre_Cal_Fetu_Stan_Scores.Count == 0)
			//{
			//	throw new ExceptionCustom("ระบุ จำนวนและคะแนน");
			//}

			//foreach (var item in model.Pre_Cal_Fetu_Stan_Scores)
			//{
			//	if (string.IsNullOrEmpty(item.Name) || !item.Score.HasValue)
			//	{
			//		throw new ExceptionCustom("ระบุ จำนวนและคะแนนไม่ครบถ้วน");
			//	}
			//	if (item.Score > model.HighScore)
			//	{
			//		throw new ExceptionCustom("คะแนนต้องไม่มากว่าคะแนนสูงสุด");
			//	}
			//}
		}

		public async Task RemoveAllPreCall(Guid id)
		{
			var pre_Cal_WeightFactorR = _repo.Context.Pre_Cal_WeightFactors.Include(x => x.Pre_Cal_WeightFactor_Items).Where(x => x.Pre_CalId == id).ToList();
			if (pre_Cal_WeightFactorR.Count > 0)
			{
				foreach (var item in pre_Cal_WeightFactorR)
				{
					if (item.Pre_Cal_WeightFactor_Items != null)
					{
						_db.DeleteRange(item.Pre_Cal_WeightFactor_Items);
						await _db.SaveAsync();
					}
				}
				_db.DeleteRange(pre_Cal_WeightFactorR);
				await _db.SaveAsync();
			}
		}

		public async Task<Pre_Cal_WeightFactorCustom> Create(Pre_Cal_WeightFactorCustom model)
		{
			DateTime _dateNow = DateTime.Now;

			var pre_Cal_WeightFactor = new Data.Entity.Pre_Cal_WeightFactor();
			pre_Cal_WeightFactor.Status = StatusModel.Active;
			pre_Cal_WeightFactor.CreateDate = _dateNow;
			pre_Cal_WeightFactor.Pre_CalId = model.Pre_CalId;
			pre_Cal_WeightFactor.Type = model.Type;
			pre_Cal_WeightFactor.TotalPercent = model._TotalPercent;
			await _db.InsterAsync(pre_Cal_WeightFactor);
			await _db.SaveAsync();

			if (model.Pre_Cal_WeightFactor_Items?.Count > 0)
			{
				int index = 1;
				foreach (var item in model.Pre_Cal_WeightFactor_Items)
				{
					var pre_Cal_WeightFactor_Item = new Data.Entity.Pre_Cal_WeightFactor_Item();
					pre_Cal_WeightFactor_Item.Status = StatusModel.Active;
					pre_Cal_WeightFactor_Item.CreateDate = _dateNow;
					pre_Cal_WeightFactor_Item.Pre_Cal_WeightFactorId = pre_Cal_WeightFactor.Id;
					pre_Cal_WeightFactor_Item.SequenceNo = index;
					pre_Cal_WeightFactor_Item.Name = item.Name;
					pre_Cal_WeightFactor_Item.Percent = item.Percent;
					pre_Cal_WeightFactor_Item.RefItemId = item.RefItemId;
					pre_Cal_WeightFactor_Item.StanScoreType = item.StanScoreType;
					await _db.InsterAsync(pre_Cal_WeightFactor_Item);
					await _db.SaveAsync();
					index++;
				}
			}

			return _mapper.Map<Pre_Cal_WeightFactorCustom>(pre_Cal_WeightFactor);
		}

		public async Task<Pre_Cal_WeightFactorCustom> Update(Pre_Cal_WeightFactorCustom model)
		{
			DateTime _dateNow = DateTime.Now;
			var pre_Cal_WeightFactor = await _repo.Context.Pre_Cal_WeightFactors.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
			if (pre_Cal_WeightFactor != null)
			{
				pre_Cal_WeightFactor.TotalPercent = model.TotalPercent;
				_db.Update(pre_Cal_WeightFactor);
				await _db.SaveAsync();

				//Update Status To Delete All
				var pre_Cal_WeightFactor_ItemR = _repo.Context.Pre_Cal_WeightFactor_Items.Where(x => x.Pre_Cal_WeightFactorId == pre_Cal_WeightFactor.Id).ToList();
				if (pre_Cal_WeightFactor_ItemR.Count > 0)
				{
					_db.DeleteRange(pre_Cal_WeightFactor_ItemR);
					await _db.SaveAsync();
				}

				if (model.Pre_Cal_WeightFactor_Items?.Count > 0)
				{
					int index = 1;
					foreach (var item in model.Pre_Cal_WeightFactor_Items)
					{
						var pre_Cal_WeightFactor_Item = new Data.Entity.Pre_Cal_WeightFactor_Item();
						pre_Cal_WeightFactor_Item.Id = item.Id;
						pre_Cal_WeightFactor_Item.Status = StatusModel.Active;
						pre_Cal_WeightFactor_Item.CreateDate = _dateNow;
						pre_Cal_WeightFactor_Item.Pre_Cal_WeightFactorId = pre_Cal_WeightFactor.Id;
						pre_Cal_WeightFactor_Item.SequenceNo = index;
						pre_Cal_WeightFactor_Item.Percent = item.Percent;
						pre_Cal_WeightFactor_Item.RefItemId = item.RefItemId;
						pre_Cal_WeightFactor_Item.StanScoreType = item.StanScoreType;
						await _db.InsterAsync(pre_Cal_WeightFactor_Item);
						await _db.SaveAsync();
						index++;
					}
				}

			}

			return _mapper.Map<Pre_Cal_WeightFactorCustom>(pre_Cal_WeightFactor);
		}

		public async Task<Pre_Cal_WeightFactorCustom> GetById(Guid id)
		{
			var query = await _repo.Context.Pre_Cal_WeightFactors
										 .Include(x => x.Pre_Cal_WeightFactor_Items.OrderBy(s => s.SequenceNo))
										 .OrderByDescending(o => o.CreateDate)
										 .FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Pre_CalId == id);

			return _mapper.Map<Pre_Cal_WeightFactorCustom>(query);
		}

		public async Task<List<Pre_Cal_WeightFactorCustom>> GetAllPreCalById(Guid id)
		{
			var query = await _repo.Context.Pre_Cal_WeightFactors
										 .Include(x => x.Pre_Cal_WeightFactor_Items.OrderBy(s => s.SequenceNo))
										 .OrderByDescending(o => o.CreateDate)
										 .Where(x => x.Status != StatusModel.Delete && x.Pre_CalId == id).ToListAsync();

			return _mapper.Map<List<Pre_Cal_WeightFactorCustom>>(query);
		}

	}
}
