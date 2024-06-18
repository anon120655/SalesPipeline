using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Data.Entity;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.ConstTypeModel;
using SalesPipeline.Utils.Resources.Customers;
using SalesPipeline.Utils.Resources.PreApprove;
using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Repositorys
{
    public class PreCalInfo : IPreCalInfo
	{
		private IRepositoryWrapper _repo;
		private readonly IMapper _mapper;
		private readonly IRepositoryBase _db;
		private readonly AppSettings _appSet;

		public PreCalInfo(IRepositoryWrapper repo, IRepositoryBase db, IOptions<AppSettings> appSet, IMapper mapper)
		{
			_db = db;
			_repo = repo;
			_mapper = mapper;
			_appSet = appSet.Value;
		}

		public async Task<Pre_Cal_InfoCustom> Validate(Pre_Cal_InfoCustom model)
		{
			await Task.Delay(1);

			if (!model.HighScore.HasValue || model.HighScore == 0)
			{
				throw new ExceptionCustom("ระบุ คะแนนสูงสุด");
			}

			if (model.Pre_Cal_Info_Scores == null || model.Pre_Cal_Info_Scores.Count == 0)
			{
				throw new ExceptionCustom("ระบุ จำนวนและคะแนน");
			}

			foreach (var item in model.Pre_Cal_Info_Scores)
			{
				if (!item.Name.HasValue || !item.Score.HasValue)
				{
					throw new ExceptionCustom("ระบุ จำนวนและคะแนนไม่ครบถ้วน");
				}
				if (item.Score > model.HighScore)
				{
					throw new ExceptionCustom("คะแนนต้องไม่มากว่าคะแนนสูงสุด");
				}
			}

			return model;
		}

		public async Task<Pre_Cal_InfoCustom> Create(Pre_Cal_InfoCustom model)
		{
			await Validate(model);

			using (var _transaction = _repo.BeginTransaction())
			{
				DateTime _dateNow = DateTime.Now;

				var pre_Cal_Info = new Data.Entity.Pre_Cal_Info();
				pre_Cal_Info.Status = StatusModel.Active;
				pre_Cal_Info.CreateDate = _dateNow;
				pre_Cal_Info.Pre_CalId = model.Pre_CalId;
				pre_Cal_Info.HighScore = model.HighScore;
				await _db.InsterAsync(pre_Cal_Info);
				await _db.SaveAsync();

				if (model.Pre_Cal_Info_Scores?.Count > 0)
				{
					int index = 1;
					foreach (var score in model.Pre_Cal_Info_Scores)
					{
						var pre_Cal_Info_Score = new Data.Entity.Pre_Cal_Info_Score();
						pre_Cal_Info_Score.Status = StatusModel.Active;
						pre_Cal_Info_Score.CreateDate = _dateNow;
						pre_Cal_Info_Score.Pre_Cal_InfoId = pre_Cal_Info.Id;
						pre_Cal_Info_Score.SequenceNo = index;
						pre_Cal_Info_Score.Name = score.Name;
						pre_Cal_Info_Score.Score = score.Score;
						await _db.InsterAsync(pre_Cal_Info_Score);
						await _db.SaveAsync();
						index++;
					}
				}

				_transaction.Commit();

				return _mapper.Map<Pre_Cal_InfoCustom>(pre_Cal_Info);
			}
		}

		public async Task<Pre_Cal_InfoCustom> Update(Pre_Cal_InfoCustom model)
		{
			await Validate(model);

			using (var _transaction = _repo.BeginTransaction())
			{
				DateTime _dateNow = DateTime.Now;
				var pre_Cal_Info = await _repo.Context.Pre_Cal_Infos.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
				if (pre_Cal_Info != null)
				{
					pre_Cal_Info.HighScore = model.HighScore;
					_db.Update(pre_Cal_Info);
					await _db.SaveAsync();

					//Update Status To Delete All
					var pre_Cal_Info_ScoreR = _repo.Context.Pre_Cal_Info_Scores.Where(x => x.Pre_Cal_InfoId == pre_Cal_Info.Id).ToList();
					if (pre_Cal_Info_ScoreR.Count > 0)
					{
						_db.DeleteRange(pre_Cal_Info_ScoreR);
						await _db.SaveAsync();
					}

					if (model.Pre_Cal_Info_Scores?.Count > 0)
					{
						int index = 1;
						foreach (var score in model.Pre_Cal_Info_Scores)
						{
							var pre_Cal_Info_Score = new Data.Entity.Pre_Cal_Info_Score();
							pre_Cal_Info_Score.Status = StatusModel.Active;
							pre_Cal_Info_Score.CreateDate = _dateNow;
							pre_Cal_Info_Score.Pre_Cal_InfoId = pre_Cal_Info.Id;
							pre_Cal_Info_Score.SequenceNo = index;
							pre_Cal_Info_Score.Name = score.Name;
							pre_Cal_Info_Score.Score = score.Score;
							await _db.InsterAsync(pre_Cal_Info_Score);
							await _db.SaveAsync();
							index++;
						}
					}

				}

				_transaction.Commit();

				return _mapper.Map<Pre_Cal_InfoCustom>(pre_Cal_Info);
			}
		}

		public async Task<Pre_Cal_InfoCustom> GetById(Guid id)
		{
			var query = await _repo.Context.Pre_Cal_Infos
										 .Include(x => x.Pre_Cal_Info_Scores.OrderBy(s => s.SequenceNo))
										 .OrderByDescending(o => o.CreateDate)
										 .FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Pre_CalId == id);

			return _mapper.Map<Pre_Cal_InfoCustom>(query);
		}

		public async Task<Pre_Cal_InfoCustom> GetByPreCalId(Guid id)
		{
			var query = await _repo.Context.Pre_Cal_Infos
										 .Include(x => x.Pre_Cal_Info_Scores.OrderBy(s => s.SequenceNo))
										 .OrderByDescending(o => o.CreateDate)
										 .FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Pre_CalId == id);

			return _mapper.Map<Pre_Cal_InfoCustom>(query);
		}

	}
}
