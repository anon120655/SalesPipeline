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
    public class PreCalApp : IPreCalApp
	{
		private IRepositoryWrapper _repo;
		private readonly IMapper _mapper;
		private readonly IRepositoryBase _db;
		private readonly AppSettings _appSet;

		public PreCalApp(IRepositoryWrapper repo, IRepositoryBase db, IOptions<AppSettings> appSet, IMapper mapper)
		{
			_db = db;
			_repo = repo;
			_mapper = mapper;
			_appSet = appSet.Value;
		}

		public async Task<Pre_Cal_Fetu_AppCustom> Validate(Pre_Cal_Fetu_AppCustom model)
		{
			await Task.Delay(1);

			if (model.Pre_Cal_Fetu_App_Items == null || model.Pre_Cal_Fetu_App_Items.Count == 0)
			{
				throw new ExceptionCustom("ระบุรายการข้อมูลไม่ถูกต้อง");
			}

			foreach (var item in model.Pre_Cal_Fetu_App_Items)
			{
				if (string.IsNullOrEmpty(item.Name))
				{
					throw new ExceptionCustom("ระบุรายการข้อมูลไม่ครบถ้วน");
				}

				if (item.Pre_Cal_Fetu_App_Item_Scores == null || item.Pre_Cal_Fetu_App_Item_Scores.Count == 0)
				{
					throw new ExceptionCustom("ระบุรคะแนนไม่ถูกต้อง");
				}

				foreach (var itemScore in item.Pre_Cal_Fetu_App_Item_Scores)
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

		public async Task<Pre_Cal_Fetu_AppCustom> Create(Pre_Cal_Fetu_AppCustom model)
		{
			await Validate(model);

			using (var _transaction = _repo.BeginTransaction())
			{
				DateTime _dateNow = DateTime.Now;

				var pre_Cal_Fetu_App = new Data.Entity.Pre_Cal_Fetu_App();
				pre_Cal_Fetu_App.Status = StatusModel.Active;
				pre_Cal_Fetu_App.CreateDate = _dateNow;
				pre_Cal_Fetu_App.Pre_CalId = model.Pre_CalId;
				pre_Cal_Fetu_App.HighScore = model.HighScore;
				await _db.InsterAsync(pre_Cal_Fetu_App);
				await _db.SaveAsync();

				if (model.Pre_Cal_Fetu_App_Items?.Count > 0)
				{
					int index = 1;
					foreach (var item in model.Pre_Cal_Fetu_App_Items)
					{
						var pre_Cal_Fetu_App_Item = new Data.Entity.Pre_Cal_Fetu_App_Item();
						pre_Cal_Fetu_App_Item.Id = item.Id;
						pre_Cal_Fetu_App_Item.Status = StatusModel.Active;
						pre_Cal_Fetu_App_Item.CreateDate = _dateNow;
						pre_Cal_Fetu_App_Item.Pre_Cal_Fetu_AppId = pre_Cal_Fetu_App.Id;
						pre_Cal_Fetu_App_Item.SequenceNo = index;
						pre_Cal_Fetu_App_Item.Name = item.Name;
						await _db.InsterAsync(pre_Cal_Fetu_App_Item);
						await _db.SaveAsync();
						index++;

						if (item.Pre_Cal_Fetu_App_Item_Scores?.Count > 0)
						{
							int indexScore = 1;
							foreach (var itemScore in item.Pre_Cal_Fetu_App_Item_Scores)
							{
								var pre_Cal_Fetu_App_Item_Score = new Data.Entity.Pre_Cal_Fetu_App_Item_Score();
								pre_Cal_Fetu_App_Item_Score.Id = itemScore.Id;
								pre_Cal_Fetu_App_Item_Score.Status = StatusModel.Active;
								pre_Cal_Fetu_App_Item_Score.CreateDate = _dateNow;
								pre_Cal_Fetu_App_Item_Score.Pre_Cal_Fetu_App_ItemId = pre_Cal_Fetu_App_Item.Id;
								pre_Cal_Fetu_App_Item_Score.SequenceNo = indexScore;
								pre_Cal_Fetu_App_Item_Score.Name = itemScore.Name;
								pre_Cal_Fetu_App_Item_Score.Score = itemScore.Score;
								await _db.InsterAsync(pre_Cal_Fetu_App_Item_Score);
								await _db.SaveAsync();
								indexScore++;
							}
						}

					}
				}

				_transaction.Commit();

				return _mapper.Map<Pre_Cal_Fetu_AppCustom>(pre_Cal_Fetu_App);
			}
		}

		public async Task<Pre_Cal_Fetu_AppCustom> Update(Pre_Cal_Fetu_AppCustom model)
		{
			await Validate(model);

			using (var _transaction = _repo.BeginTransaction())
			{
				DateTime _dateNow = DateTime.Now;
				var pre_Cal_Fetu_App = await _repo.Context.Pre_Cal_Fetu_Apps.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
				if (pre_Cal_Fetu_App != null)
				{
					pre_Cal_Fetu_App.HighScore = model.HighScore;
					_db.Update(pre_Cal_Fetu_App);
					await _db.SaveAsync();

					//Update Status To Delete All
					var pre_Cal_Fetu_App_ItemsR = _repo.Context.Pre_Cal_Fetu_App_Items.Where(x => x.Pre_Cal_Fetu_AppId == pre_Cal_Fetu_App.Id).ToList();
					if (pre_Cal_Fetu_App_ItemsR.Count > 0)
					{
						foreach (var item in pre_Cal_Fetu_App_ItemsR)
						{
							var pre_Cal_Fetu_App_Item_ScoreR = _repo.Context.Pre_Cal_Fetu_App_Item_Scores.Where(x => x.Pre_Cal_Fetu_App_ItemId == item.Id).ToList();
							if (pre_Cal_Fetu_App_Item_ScoreR.Count > 0)
							{
								_db.DeleteRange(pre_Cal_Fetu_App_Item_ScoreR);
								await _db.SaveAsync();
							}
						}
						_db.DeleteRange(pre_Cal_Fetu_App_ItemsR);
						await _db.SaveAsync();
					}

					if (model.Pre_Cal_Fetu_App_Items?.Count > 0)
					{
						int index = 1;
						foreach (var item in model.Pre_Cal_Fetu_App_Items)
						{
							var pre_Cal_Fetu_App_Item = new Data.Entity.Pre_Cal_Fetu_App_Item();
							pre_Cal_Fetu_App_Item.Id = item.Id;
							pre_Cal_Fetu_App_Item.Status = StatusModel.Active;
							pre_Cal_Fetu_App_Item.CreateDate = _dateNow;
							pre_Cal_Fetu_App_Item.Pre_Cal_Fetu_AppId = pre_Cal_Fetu_App.Id;
							pre_Cal_Fetu_App_Item.SequenceNo = index;
							pre_Cal_Fetu_App_Item.Name = item.Name;
							await _db.InsterAsync(pre_Cal_Fetu_App_Item);
							await _db.SaveAsync();
							index++;

							if (item.Pre_Cal_Fetu_App_Item_Scores?.Count > 0)
							{
								int indexScore = 1;
								foreach (var itemScore in item.Pre_Cal_Fetu_App_Item_Scores)
								{
									var pre_Cal_Fetu_App_Item_Score = new Data.Entity.Pre_Cal_Fetu_App_Item_Score();
									pre_Cal_Fetu_App_Item_Score.Id = itemScore.Id;
									pre_Cal_Fetu_App_Item_Score.Status = StatusModel.Active;
									pre_Cal_Fetu_App_Item_Score.CreateDate = _dateNow;
									pre_Cal_Fetu_App_Item_Score.Pre_Cal_Fetu_App_ItemId = pre_Cal_Fetu_App_Item.Id;
									pre_Cal_Fetu_App_Item_Score.SequenceNo = indexScore;
									pre_Cal_Fetu_App_Item_Score.Name = itemScore.Name;
									pre_Cal_Fetu_App_Item_Score.Score = itemScore.Score;
									await _db.InsterAsync(pre_Cal_Fetu_App_Item_Score);
									await _db.SaveAsync();
									indexScore++;
								}
							}

						}
					}


				}

				_transaction.Commit();

				return _mapper.Map<Pre_Cal_Fetu_AppCustom>(pre_Cal_Fetu_App);
			}
		}

		public async Task<Pre_Cal_Fetu_AppCustom> GetById(Guid id)
		{
			var query = await _repo.Context.Pre_Cal_Fetu_Apps
										 .Include(x => x.Pre_Cal_Fetu_App_Items.OrderBy(s => s.SequenceNo)).ThenInclude(t=>t.Pre_Cal_Fetu_App_Item_Scores.OrderBy(s => s.SequenceNo))
										 .OrderByDescending(o => o.CreateDate)
										 .FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Pre_CalId == id);

			return _mapper.Map<Pre_Cal_Fetu_AppCustom>(query);
		}

	}
}
