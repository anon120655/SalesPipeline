using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Data.Entity;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.PreApprove;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Repositorys
{
	public class PreFactor : IPreFactor
	{
		private IRepositoryWrapper _repo;
		private readonly IMapper _mapper;
		private readonly IRepositoryBase _db;
		private readonly AppSettings _appSet;

		public PreFactor(IRepositoryWrapper repo, IRepositoryBase db, IOptions<AppSettings> appSet, IMapper mapper)
		{
			_db = db;
			_repo = repo;
			_mapper = mapper;
			_appSet = appSet.Value;
		}

		public async Task<Pre_FactorCustom> Process(Pre_FactorCustom model)
		{
			using (var _transaction = _repo.BeginTransaction())
			{
				var response = new Pre_Process();

				var sales = await _repo.Context.Sales.Where(x => x.Id == model.SaleId).FirstOrDefaultAsync();

				DateTime _dateNow = DateTime.Now;

				var pre_Factor = new Data.Entity.Pre_Factor();
				pre_Factor.Status = StatusModel.Active;
				pre_Factor.CreateDate = _dateNow;
				pre_Factor.CreateBy = model.CurrentUserId;
				pre_Factor.SaleId = model.SaleId;
				pre_Factor.CompanyName = sales?.CompanyName;
				await _db.InsterAsync(pre_Factor);
				await _db.SaveAsync();

				if (model.Pre_Factor_Infos?.Count > 0)
				{
					foreach (var item in model.Pre_Factor_Infos)
					{
						string? loanIName = null;
						string? master_Pre_Applicant_LoanName = null;
						string? master_Pre_BusinessTypeName = null;

						if (item.LoanId.HasValue)
						{
							loanIName = await _repo.Loan.GetNameById(item.LoanId.Value);
						}

						master_Pre_Applicant_LoanName = await _repo.Master_Pre_App_Loan.GetNameById(item.Master_Pre_Applicant_LoanId);
						master_Pre_BusinessTypeName = await _repo.Master_Pre_BusType.GetNameById(item.Master_Pre_BusinessTypeId);

						var pre_Factor_Info = new Data.Entity.Pre_Factor_Info();
						pre_Factor_Info.Status = StatusModel.Active;
						pre_Factor_Info.CreateDate = _dateNow;
						pre_Factor_Info.Pre_FactorId = pre_Factor.Id;
						pre_Factor_Info.LoanId = item.LoanId;
						pre_Factor_Info.LoanIName = loanIName;
						pre_Factor_Info.InstallmentPayYear = item.InstallmentPayYear;
						pre_Factor_Info.LoanValue = item.LoanValue;
						pre_Factor_Info.LoanPeriod = item.LoanPeriod;
						pre_Factor_Info.Master_Pre_Applicant_LoanId = item.Master_Pre_Applicant_LoanId;
						pre_Factor_Info.Master_Pre_Applicant_LoanName = master_Pre_Applicant_LoanName;
						pre_Factor_Info.Master_Pre_BusinessTypeId = item.Master_Pre_BusinessTypeId;
						pre_Factor_Info.Master_Pre_BusinessTypeName = master_Pre_BusinessTypeName;
						await _db.InsterAsync(pre_Factor_Info);
						await _db.SaveAsync();
					}
				}

				if (model.Pre_Factor_Stans?.Count > 0)
				{
					foreach (var item in model.Pre_Factor_Stans)
					{
						string? Stan_ItemOptionName_Type1 = null;
						string? Stan_ItemOptionName_Type2 = null;

						if (item.Stan_ItemOptionId_Type1.HasValue)
						{
							var stan_ItemOption = await _repo.Context.Pre_Cal_Fetu_Stan_ItemOptions
								.FirstOrDefaultAsync(x=>x.Id == item.Stan_ItemOptionId_Type1.Value);
							if (stan_ItemOption == null) throw new ExceptionCustom($"stan_ItemOption type1 not found.");
							Stan_ItemOptionName_Type1 = stan_ItemOption.Name;
						}
						if (item.Stan_ItemOptionId_Type2.HasValue)
						{
							var stan_ItemOption = await _repo.Context.Pre_Cal_Fetu_Stan_ItemOptions
								.FirstOrDefaultAsync(x => x.Id == item.Stan_ItemOptionId_Type2.Value);
							if (stan_ItemOption == null) throw new ExceptionCustom($"stan_ItemOption type2 not found.");
							Stan_ItemOptionName_Type2 = stan_ItemOption.Name;
						}

						var pre_Factor_Stan = new Data.Entity.Pre_Factor_Stan();
						pre_Factor_Stan.Status = StatusModel.Active;
						pre_Factor_Stan.CreateDate = _dateNow;
						pre_Factor_Stan.Pre_FactorId = pre_Factor.Id;
						pre_Factor_Stan.Income = item.Income;
						pre_Factor_Stan.Expenses = item.Expenses;
						pre_Factor_Stan.OtherDebts = item.OtherDebts;
						pre_Factor_Stan.IncomeDebtPeriod = item.IncomeDebtPeriod;
						pre_Factor_Stan.DepositBAAC = item.DepositBAAC;
						pre_Factor_Stan.Stan_ItemOptionId_Type1 = item.Stan_ItemOptionId_Type1;
						pre_Factor_Stan.Stan_ItemOptionName_Type1 = Stan_ItemOptionName_Type1;
						pre_Factor_Stan.Stan_ItemOptionId_Type2 = item.Stan_ItemOptionId_Type2;
						pre_Factor_Stan.Stan_ItemOptionName_Type2 = Stan_ItemOptionName_Type2;
						await _db.InsterAsync(pre_Factor_Stan);
						await _db.SaveAsync();
					}
				}

				if (model.Pre_Factor_Apps?.Count > 0)
				{
					foreach (var item in model.Pre_Factor_Apps)
					{
						string? pre_Cal_Fetu_App_ItemName = null;
						string? pre_Cal_Fetu_App_Item_ScoreName = null;

						if (item.Pre_Cal_Fetu_App_ItemId.HasValue)
						{
							var app_Items = await _repo.Context.Pre_Cal_Fetu_App_Items
								.FirstOrDefaultAsync(x => x.Id == item.Pre_Cal_Fetu_App_ItemId.Value);
							if (app_Items == null) throw new ExceptionCustom($"app_ItemId not found.");
							pre_Cal_Fetu_App_ItemName = app_Items.Name;
						}
						if (item.Pre_Cal_Fetu_App_Item_ScoreId.HasValue)
						{
							var stan_ItemOption_Score = await _repo.Context.Pre_Cal_Fetu_App_Item_Scores
								.FirstOrDefaultAsync(x => x.Id == item.Pre_Cal_Fetu_App_Item_ScoreId.Value);
							if (stan_ItemOption_Score == null) throw new ExceptionCustom($"app_Item_ScoreId not found.");
							pre_Cal_Fetu_App_Item_ScoreName = stan_ItemOption_Score.Name;
						}

						var pre_Factor_App = new Data.Entity.Pre_Factor_App();
						pre_Factor_App.Status = StatusModel.Active;
						pre_Factor_App.CreateDate = _dateNow;
						pre_Factor_App.Pre_FactorId = pre_Factor.Id;
						pre_Factor_App.Pre_Cal_Fetu_App_ItemId = item.Pre_Cal_Fetu_App_ItemId;
						pre_Factor_App.Pre_Cal_Fetu_App_ItemName = pre_Cal_Fetu_App_ItemName;
						pre_Factor_App.Pre_Cal_Fetu_App_Item_ScoreId = item.Pre_Cal_Fetu_App_Item_ScoreId;
						pre_Factor_App.Pre_Cal_Fetu_App_Item_ScoreName = pre_Cal_Fetu_App_Item_ScoreName;
						await _db.InsterAsync(pre_Factor_App);
						await _db.SaveAsync();
					}
				}

				if (model.Pre_Factor_Bus?.Count > 0)
				{
					foreach (var item in model.Pre_Factor_Bus)
					{
						string? pre_Cal_Fetu_Bus_ItemName = null;
						string? pre_Cal_Fetu_Bus_Item_ScoreName = null;

						if (item.Pre_Cal_Fetu_Bus_ItemId.HasValue)
						{
							var bus_Items = await _repo.Context.Pre_Cal_Fetu_Bus_Items
								.FirstOrDefaultAsync(x => x.Id == item.Pre_Cal_Fetu_Bus_ItemId.Value);
							if (bus_Items == null) throw new ExceptionCustom($"bus_ItemId not found.");
							pre_Cal_Fetu_Bus_ItemName = bus_Items.Name;
						}
						if (item.Pre_Cal_Fetu_Bus_Item_ScoreId.HasValue)
						{
							var stan_ItemOption_Score = await _repo.Context.Pre_Cal_Fetu_Bus_Item_Scores
								.FirstOrDefaultAsync(x => x.Id == item.Pre_Cal_Fetu_Bus_Item_ScoreId.Value);
							if (stan_ItemOption_Score == null) throw new ExceptionCustom($"bus_Item_ScoreId not found.");
							pre_Cal_Fetu_Bus_Item_ScoreName = stan_ItemOption_Score.Name;
						}

						var pre_Factor_Bus = new Data.Entity.Pre_Factor_Bu();
						pre_Factor_Bus.Status = StatusModel.Active;
						pre_Factor_Bus.CreateDate = _dateNow;
						pre_Factor_Bus.Pre_FactorId = pre_Factor.Id;
						pre_Factor_Bus.Pre_Cal_Fetu_Bus_ItemId = item.Pre_Cal_Fetu_Bus_ItemId;
						pre_Factor_Bus.Pre_Cal_Fetu_Bus_ItemName = pre_Cal_Fetu_Bus_ItemName;
						pre_Factor_Bus.Pre_Cal_Fetu_Bus_Item_ScoreId = item.Pre_Cal_Fetu_Bus_Item_ScoreId;
						pre_Factor_Bus.Pre_Cal_Fetu_Bus_Item_ScoreName = pre_Cal_Fetu_Bus_Item_ScoreName;
						await _db.InsterAsync(pre_Factor_Bus);
						await _db.SaveAsync();
					}
				}

				_transaction.Commit();

				return new();
			}
		}

		public async Task<Pre_FactorCustom> GetById(Guid id)
		{
			var query = await _repo.Context.Pre_Factors
				.Include(x => x.Pre_Factor_Infos)
				.Include(x => x.Pre_Factor_Stans)
				.Include(x => x.Pre_Factor_Apps)
				.Include(x => x.Pre_Factor_Bus)
				.Include(x => x.Pre_Results)
				.OrderByDescending(o => o.CreateDate)
				.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Id == id);

			return _mapper.Map<Pre_FactorCustom>(query);
		}

	}
}
