using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Data.Entity;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.ConstTypeModel;
using SalesPipeline.Utils.Resources.PreApprove;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Repositorys
{
    public class PreCalBus : IPreCalBus
	{
		private IRepositoryWrapper _repo;
		private readonly IMapper _mapper;
		private readonly IRepositoryBase _db;
		private readonly AppSettings _appSet;

		public PreCalBus(IRepositoryWrapper repo, IRepositoryBase db, IOptions<AppSettings> appSet, IMapper mapper)
		{
			_db = db;
			_repo = repo;
			_mapper = mapper;
			_appSet = appSet.Value;
		}

		public async Task<Pre_Cal_Fetu_BuCustom> Validate(Pre_Cal_Fetu_BuCustom model)
		{
			await Task.Delay(1);

			if (model.Pre_Cal_Fetu_Bus_Items == null || model.Pre_Cal_Fetu_Bus_Items.Count == 0)
			{
				throw new ExceptionCustom("ระบุรายการข้อมูลไม่ถูกต้อง");
			}

			foreach (var item in model.Pre_Cal_Fetu_Bus_Items)
			{
				if (string.IsNullOrEmpty(item.Name))
				{
					throw new ExceptionCustom("ระบุรายการข้อมูลไม่ครบถ้วน");
				}

				if (item.Pre_Cal_Fetu_Bus_Item_Scores == null || item.Pre_Cal_Fetu_Bus_Item_Scores.Count == 0)
				{
					throw new ExceptionCustom("ระบุรคะแนนไม่ถูกต้อง");
				}

				foreach (var itemScore in item.Pre_Cal_Fetu_Bus_Item_Scores)
				{
					if (string.IsNullOrEmpty(itemScore.Name) || !itemScore.Score.HasValue)
					{
						throw new ExceptionCustom("ระบุ จำนวนและคะแนนไม่ครบถ้วน");
					}
					if (itemScore.Score > model.HighScore)
					{
						throw new ExceptionCustom("คะแนนต้องไม่มากว่าคะแนนสูงสุด");
					}
				}
			}

			if (!model.HighScore.HasValue || model.HighScore == 0)
			{
				throw new ExceptionCustom("ระบุ คะแนนสูงสุด");
			}

			return model;
		}

		public async Task<Pre_Cal_Fetu_BuCustom> Create(Pre_Cal_Fetu_BuCustom model)
		{
			await Validate(model);

			using (var _transaction = _repo.BeginTransaction())
			{
				DateTime _dateNow = DateTime.Now;

				var pre_Cal_Fetu_Bus = new Data.Entity.Pre_Cal_Fetu_Bu();
				pre_Cal_Fetu_Bus.Status = StatusModel.Active;
				pre_Cal_Fetu_Bus.CreateDate = _dateNow;
				pre_Cal_Fetu_Bus.Pre_CalId = model.Pre_CalId;
				pre_Cal_Fetu_Bus.HighScore = model.HighScore;
				await _db.InsterAsync(pre_Cal_Fetu_Bus);
				await _db.SaveAsync();

				if (model.Pre_Cal_Fetu_Bus_Items?.Count > 0)
				{
					int index = 1;
					foreach (var item in model.Pre_Cal_Fetu_Bus_Items)
					{
						var pre_Cal_Fetu_Bus_Item = new Data.Entity.Pre_Cal_Fetu_Bus_Item();
						pre_Cal_Fetu_Bus_Item.Id = item.Id;
						pre_Cal_Fetu_Bus_Item.Status = StatusModel.Active;
						pre_Cal_Fetu_Bus_Item.CreateDate = _dateNow;
						pre_Cal_Fetu_Bus_Item.Pre_Cal_Fetu_BusId = pre_Cal_Fetu_Bus.Id;
						pre_Cal_Fetu_Bus_Item.SequenceNo = index;
						pre_Cal_Fetu_Bus_Item.Name = item.Name;
						await _db.InsterAsync(pre_Cal_Fetu_Bus_Item);
						await _db.SaveAsync();
						index++;

						if (item.Pre_Cal_Fetu_Bus_Item_Scores?.Count > 0)
						{
							int indexScore = 1;
							foreach (var itemScore in item.Pre_Cal_Fetu_Bus_Item_Scores)
							{
								var pre_Cal_Fetu_Bus_Item_Score = new Data.Entity.Pre_Cal_Fetu_Bus_Item_Score();
								pre_Cal_Fetu_Bus_Item_Score.Id = itemScore.Id;
								pre_Cal_Fetu_Bus_Item_Score.Status = StatusModel.Active;
								pre_Cal_Fetu_Bus_Item_Score.CreateDate = _dateNow;
								pre_Cal_Fetu_Bus_Item_Score.Pre_Cal_Fetu_Bus_ItemId = pre_Cal_Fetu_Bus_Item.Id;
								pre_Cal_Fetu_Bus_Item_Score.SequenceNo = indexScore;
								pre_Cal_Fetu_Bus_Item_Score.Name = itemScore.Name;
								pre_Cal_Fetu_Bus_Item_Score.Score = itemScore.Score;
								await _db.InsterAsync(pre_Cal_Fetu_Bus_Item_Score);
								await _db.SaveAsync();
								indexScore++;
							}
						}

					}
				}

				_transaction.Commit();

				return _mapper.Map<Pre_Cal_Fetu_BuCustom>(pre_Cal_Fetu_Bus);
			}
		}

		public async Task<Pre_Cal_Fetu_BuCustom> Update(Pre_Cal_Fetu_BuCustom model)
		{
			await Validate(model);

			using (var _transaction = _repo.BeginTransaction())
			{
				DateTime _dateNow = DateTime.Now;
				var pre_Cal_Fetu_Bus = await _repo.Context.Pre_Cal_Fetu_Bus.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
				if (pre_Cal_Fetu_Bus != null)
				{
					pre_Cal_Fetu_Bus.HighScore = model.HighScore;
					_db.Update(pre_Cal_Fetu_Bus);
					await _db.SaveAsync();

					//Update Status To Delete All
					var pre_Cal_Fetu_Bus_ItemsR = _repo.Context.Pre_Cal_Fetu_Bus_Items.Where(x => x.Pre_Cal_Fetu_BusId == pre_Cal_Fetu_Bus.Id).ToList();
					if (pre_Cal_Fetu_Bus_ItemsR.Count > 0)
					{
						foreach (var item in pre_Cal_Fetu_Bus_ItemsR)
						{
							var pre_Cal_Fetu_Bus_Item_ScoreR = _repo.Context.Pre_Cal_Fetu_Bus_Item_Scores.Where(x => x.Pre_Cal_Fetu_Bus_ItemId == item.Id).ToList();
							if (pre_Cal_Fetu_Bus_Item_ScoreR.Count > 0)
							{
								_db.DeleteRange(pre_Cal_Fetu_Bus_Item_ScoreR);
								await _db.SaveAsync();
							}
						}
						_db.DeleteRange(pre_Cal_Fetu_Bus_ItemsR);
						await _db.SaveAsync();
					}

					if (model.Pre_Cal_Fetu_Bus_Items?.Count > 0)
					{
						int index = 1;
						foreach (var item in model.Pre_Cal_Fetu_Bus_Items)
						{
							var pre_Cal_Fetu_Bus_Item = new Data.Entity.Pre_Cal_Fetu_Bus_Item();
							pre_Cal_Fetu_Bus_Item.Id = item.Id;
							pre_Cal_Fetu_Bus_Item.Status = StatusModel.Active;
							pre_Cal_Fetu_Bus_Item.CreateDate = _dateNow;
							pre_Cal_Fetu_Bus_Item.Pre_Cal_Fetu_BusId = pre_Cal_Fetu_Bus.Id;
							pre_Cal_Fetu_Bus_Item.SequenceNo = index;
							pre_Cal_Fetu_Bus_Item.Name = item.Name;
							await _db.InsterAsync(pre_Cal_Fetu_Bus_Item);
							await _db.SaveAsync();
							index++;

							if (item.Pre_Cal_Fetu_Bus_Item_Scores?.Count > 0)
							{
								int indexScore = 1;
								foreach (var itemScore in item.Pre_Cal_Fetu_Bus_Item_Scores)
								{
									var pre_Cal_Fetu_Bus_Item_Score = new Data.Entity.Pre_Cal_Fetu_Bus_Item_Score();
									pre_Cal_Fetu_Bus_Item_Score.Id = itemScore.Id;
									pre_Cal_Fetu_Bus_Item_Score.Status = StatusModel.Active;
									pre_Cal_Fetu_Bus_Item_Score.CreateDate = _dateNow;
									pre_Cal_Fetu_Bus_Item_Score.Pre_Cal_Fetu_Bus_ItemId = pre_Cal_Fetu_Bus_Item.Id;
									pre_Cal_Fetu_Bus_Item_Score.SequenceNo = indexScore;
									pre_Cal_Fetu_Bus_Item_Score.Name = itemScore.Name;
									pre_Cal_Fetu_Bus_Item_Score.Score = itemScore.Score;
									await _db.InsterAsync(pre_Cal_Fetu_Bus_Item_Score);
									await _db.SaveAsync();
									indexScore++;
								}
							}

						}
					}

				}

				_transaction.Commit();

				return _mapper.Map<Pre_Cal_Fetu_BuCustom>(pre_Cal_Fetu_Bus);
			}
		}

		public async Task<Pre_Cal_Fetu_BuCustom> GetById(Guid id)
		{
			var query = await _repo.Context.Pre_Cal_Fetu_Bus
										 .Include(x => x.Pre_Cal_Fetu_Bus_Items.OrderBy(s => s.SequenceNo)).ThenInclude(t => t.Pre_Cal_Fetu_Bus_Item_Scores.OrderBy(s => s.SequenceNo))
										 .OrderByDescending(o => o.CreateDate)
										 .FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Pre_CalId == id);

			return _mapper.Map<Pre_Cal_Fetu_BuCustom>(query);
		}

	}
}
