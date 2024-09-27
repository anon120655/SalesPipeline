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
    public class PreCalStan : IPreCalStan
	{
		private IRepositoryWrapper _repo;
		private readonly IMapper _mapper;
		private readonly IRepositoryBase _db;
		private readonly AppSettings _appSet;

		public PreCalStan(IRepositoryWrapper repo, IRepositoryBase db, IOptions<AppSettings> appSet, IMapper mapper)
		{
			_db = db;
			_repo = repo;
			_mapper = mapper;
			_appSet = appSet.Value;
		}

		public async Task<Pre_Cal_Fetu_StanCustom> Validate(Pre_Cal_Fetu_StanCustom model)
		{
			await Task.Delay(1);

			//ปิดชั่วคราว
			//if (model.Pre_Cal_Fetu_Stan_ItemOptions == null || model.Pre_Cal_Fetu_Stan_ItemOptions.Count == 0)
			//{
			//	throw new ExceptionCustom("ระบุ Drop Down ไม่ถูกต้อง");
			//}

			//ปิดชั่วคราว
			//foreach (var item in model.Pre_Cal_Fetu_Stan_ItemOptions)
			//{
			//	if (string.IsNullOrEmpty(item.Name))
			//	{
			//		throw new ExceptionCustom("ระบุ Drop Down ไม่ครบถ้วน");
			//	}
			//}

			if (!model.HighScore.HasValue || model.HighScore == 0)
			{
				throw new ExceptionCustom("ระบุ คะแนนสูงสุด");
			}

			if (model.Pre_Cal_Fetu_Stan_Scores == null || model.Pre_Cal_Fetu_Stan_Scores.Count == 0)
			{
				throw new ExceptionCustom("ระบุ จำนวนและคะแนน");
			}

			//ปิดชั่วคราว
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

			return model;
		}

		public async Task<Pre_Cal_Fetu_StanCustom> Create(Pre_Cal_Fetu_StanCustom model)
		{
			await Validate(model);

			using (var _transaction = _repo.BeginTransaction())
			{
				DateTime _dateNow = DateTime.Now;

				var pre_Cal_Fetu_Stan = new Data.Entity.Pre_Cal_Fetu_Stan();
				pre_Cal_Fetu_Stan.Status = StatusModel.Active;
				pre_Cal_Fetu_Stan.CreateDate = _dateNow;
				pre_Cal_Fetu_Stan.Pre_CalId = model.Pre_CalId;
				pre_Cal_Fetu_Stan.HighScore = model.HighScore;
				await _db.InsterAsync(pre_Cal_Fetu_Stan);
				await _db.SaveAsync();

				if (model.Pre_Cal_Fetu_Stan_ItemOptions?.Count > 0)
				{
					int index = 1;
					foreach (var itemoption in model.Pre_Cal_Fetu_Stan_ItemOptions)
					{
						var pre_Cal_Fetu_Stan_ItemOption = new Data.Entity.Pre_Cal_Fetu_Stan_ItemOption();
						pre_Cal_Fetu_Stan_ItemOption.Id = itemoption.Id;
						pre_Cal_Fetu_Stan_ItemOption.Status = StatusModel.Active;
						pre_Cal_Fetu_Stan_ItemOption.CreateDate = _dateNow;
						pre_Cal_Fetu_Stan_ItemOption.Pre_Cal_Fetu_StanId = pre_Cal_Fetu_Stan.Id;
						pre_Cal_Fetu_Stan_ItemOption.Type = itemoption.Type;
						pre_Cal_Fetu_Stan_ItemOption.SequenceNo = index;
						pre_Cal_Fetu_Stan_ItemOption.Name = itemoption.Name;
						await _db.InsterAsync(pre_Cal_Fetu_Stan_ItemOption);
						await _db.SaveAsync();
						index++;
					}
				}

				if (model.Pre_Cal_Fetu_Stan_Scores?.Count > 0)
				{
					int index = 1;
					foreach (var score in model.Pre_Cal_Fetu_Stan_Scores)
					{
						var pre_Cal_Fetu_Stan_Score = new Data.Entity.Pre_Cal_Fetu_Stan_Score();
						pre_Cal_Fetu_Stan_Score.Id = score.Id;
						pre_Cal_Fetu_Stan_Score.Status = StatusModel.Active;
						pre_Cal_Fetu_Stan_Score.CreateDate = _dateNow;
						pre_Cal_Fetu_Stan_Score.Pre_Cal_Fetu_StanId = pre_Cal_Fetu_Stan.Id;
						pre_Cal_Fetu_Stan_Score.Type = score.Type;
						pre_Cal_Fetu_Stan_Score.Pre_Cal_Fetu_StanItemOptionId = score.Pre_Cal_Fetu_StanItemOptionId;
						pre_Cal_Fetu_Stan_Score.SequenceNo = index;
						pre_Cal_Fetu_Stan_Score.Name = score.Name;
						pre_Cal_Fetu_Stan_Score.Score = score.Score;
						await _db.InsterAsync(pre_Cal_Fetu_Stan_Score);
						await _db.SaveAsync();
						index++;
					}
				}

				_transaction.Commit();

				return _mapper.Map<Pre_Cal_Fetu_StanCustom>(pre_Cal_Fetu_Stan);
			}
		}

		public async Task<Pre_Cal_Fetu_StanCustom> Update(Pre_Cal_Fetu_StanCustom model)
		{
			await Validate(model);

			using (var _transaction = _repo.BeginTransaction())
			{
				DateTime _dateNow = DateTime.Now;
				var pre_Cal_Fetu_Stan = await _repo.Context.Pre_Cal_Fetu_Stans.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
				if (pre_Cal_Fetu_Stan != null)
				{
					pre_Cal_Fetu_Stan.HighScore = model.HighScore;
					_db.Update(pre_Cal_Fetu_Stan);
					await _db.SaveAsync();

					//Update Status To Delete All
					var pre_Cal_Fetu_Stan_ItemOptionR = _repo.Context.Pre_Cal_Fetu_Stan_ItemOptions.Where(x => x.Pre_Cal_Fetu_StanId == pre_Cal_Fetu_Stan.Id).ToList();
					if (pre_Cal_Fetu_Stan_ItemOptionR.Count > 0)
					{
						_db.DeleteRange(pre_Cal_Fetu_Stan_ItemOptionR);
						await _db.SaveAsync();
					}

					var pre_Cal_Fetu_Stan_ScoreR = _repo.Context.Pre_Cal_Fetu_Stan_Scores.Where(x => x.Pre_Cal_Fetu_StanId == pre_Cal_Fetu_Stan.Id).ToList();
					if (pre_Cal_Fetu_Stan_ScoreR.Count > 0)
					{
						_db.DeleteRange(pre_Cal_Fetu_Stan_ScoreR);
						await _db.SaveAsync();
					}


					if (model.Pre_Cal_Fetu_Stan_ItemOptions?.Count > 0)
					{
						int index = 1;
						foreach (var itemoption in model.Pre_Cal_Fetu_Stan_ItemOptions)
						{
							var pre_Cal_Fetu_Stan_ItemOption = new Data.Entity.Pre_Cal_Fetu_Stan_ItemOption();
							pre_Cal_Fetu_Stan_ItemOption.Id = itemoption.Id;
							pre_Cal_Fetu_Stan_ItemOption.Status = StatusModel.Active;
							pre_Cal_Fetu_Stan_ItemOption.CreateDate = _dateNow;
							pre_Cal_Fetu_Stan_ItemOption.Pre_Cal_Fetu_StanId = pre_Cal_Fetu_Stan.Id;
							pre_Cal_Fetu_Stan_ItemOption.Type = itemoption.Type;
							pre_Cal_Fetu_Stan_ItemOption.SequenceNo = index;
							pre_Cal_Fetu_Stan_ItemOption.Name = itemoption.Name;
							await _db.InsterAsync(pre_Cal_Fetu_Stan_ItemOption);
							await _db.SaveAsync();
							index++;
						}
					}

					if (model.Pre_Cal_Fetu_Stan_Scores?.Count > 0)
					{
						int index = 1;
						foreach (var score in model.Pre_Cal_Fetu_Stan_Scores)
						{
							var pre_Cal_Fetu_Stan_Score = new Data.Entity.Pre_Cal_Fetu_Stan_Score();
							pre_Cal_Fetu_Stan_Score.Id = score.Id;
							pre_Cal_Fetu_Stan_Score.Status = StatusModel.Active;
							pre_Cal_Fetu_Stan_Score.CreateDate = _dateNow;
							pre_Cal_Fetu_Stan_Score.Pre_Cal_Fetu_StanId = pre_Cal_Fetu_Stan.Id;
							pre_Cal_Fetu_Stan_Score.Type = score.Type;
							pre_Cal_Fetu_Stan_Score.Pre_Cal_Fetu_StanItemOptionId = score.Pre_Cal_Fetu_StanItemOptionId;
							pre_Cal_Fetu_Stan_Score.SequenceNo = index;
							pre_Cal_Fetu_Stan_Score.Name = score.Name;
							pre_Cal_Fetu_Stan_Score.Score = score.Score;
							await _db.InsterAsync(pre_Cal_Fetu_Stan_Score);
							await _db.SaveAsync();
							index++;
						}
					}

				}

				_transaction.Commit();

				return _mapper.Map<Pre_Cal_Fetu_StanCustom>(pre_Cal_Fetu_Stan);
			}
		}

		public async Task<Pre_Cal_Fetu_StanCustom> GetById(Guid id)
		{
			var query = await _repo.Context.Pre_Cal_Fetu_Stans
										 .Include(x => x.Pre_Cal_Fetu_Stan_ItemOptions.OrderBy(s => s.Type).ThenBy(t=>t.SequenceNo))
										 .Include(x => x.Pre_Cal_Fetu_Stan_Scores.OrderBy(s => s.Type).ThenBy(t => t.SequenceNo))
										 .OrderByDescending(o => o.CreateDate)
										 .FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Pre_CalId == id);

			return _mapper.Map<Pre_Cal_Fetu_StanCustom>(query);
		}

	}
}
