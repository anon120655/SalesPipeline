using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NetTopologySuite.Index.HPRtree;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;
using SalesPipeline.Infrastructure.Data.Entity;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.ConstTypeModel;
using SalesPipeline.Utils.PropertiesModel;
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
				List<Pre_Result_ItemCustom> pre_Result_Items = new();

				var sales = await _repo.Context.Sales.Where(x => x.Id == model.SaleId).FirstOrDefaultAsync();
				if (sales == null) throw new ExceptionCustom($"sales not found.");
				var pre_Cal = await _repo.Context.Pre_Cals.Where(x => x.Id == model.Pre_CalId).FirstOrDefaultAsync();
				if (pre_Cal == null) throw new ExceptionCustom($"pre_Cal not found.");

				DateTime _dateNow = DateTime.Now;

				var pre_Factor = new Data.Entity.Pre_Factor();
				pre_Factor.Status = StatusModel.Active;
				pre_Factor.CreateDate = _dateNow;
				pre_Factor.CreateBy = model.CurrentUserId;
				pre_Factor.SaleId = model.SaleId;
				pre_Factor.Pre_CalId = model.Pre_CalId;
				pre_Factor.CompanyName = sales?.CompanyName;
				await _db.InsterAsync(pre_Factor);
				await _db.SaveAsync();


				var calInfo = await _repo.PreCalInfo.GetById(model.Pre_CalId);
				var calStan = await _repo.PreCalStan.GetById(model.Pre_CalId);
				var calApp = await _repo.PreCalApp.GetById(model.Pre_CalId);
				var calBus = await _repo.PreCalBus.GetById(model.Pre_CalId);

				Pre_Cal_WeightFactorCustom? calWeightInfo = null;
				Pre_Cal_WeightFactorCustom? calWeightStan = null;
				Pre_Cal_WeightFactorCustom? calWeightAppLoan = null;
				Pre_Cal_WeightFactorCustom? calWeightBusType = null;
				var calWeightList = await _repo.PreCalWeight.GetAllPreCalById(model.Pre_CalId);
				if (calWeightList.Count > 0)
				{
					calWeightInfo = calWeightList.FirstOrDefault(x => x.Type == PreCalType.Info);
					calWeightStan = calWeightList.FirstOrDefault(x => x.Type == PreCalType.Stan);
					calWeightAppLoan = calWeightList.FirstOrDefault(x => x.Type == PreCalType.AppLoan);
					calWeightBusType = calWeightList.FirstOrDefault(x => x.Type == PreCalType.BusType);
				}

				if (model.Pre_Factor_Infos?.Count > 0)
				{
					foreach (var item in model.Pre_Factor_Infos)
					{
						var calByAppBus = await _repo.PreCal.GetCalByAppBusId(item.Master_Pre_Applicant_LoanId, item.Master_Pre_BusinessTypeId);
						if (calByAppBus == null) throw new ExceptionCustom($"factor_Info calByAppBus not found.");
						if (calByAppBus.Id != pre_Cal.Id)
						{
							throw new ExceptionCustom($"ไม่พบตัวแปรคำนวณ ประเภทผู้ขอสินเชื่อ และประเภทธุรกิจที่ท่านเลือก");
						}

						string? loanIName = null;
						string? master_Pre_Applicant_LoanName = null;
						string? master_Pre_BusinessTypeName = null;

						if (item.LoanId.HasValue)
						{
							loanIName = await _repo.Loan.GetNameById(item.LoanId.Value);
						}

						master_Pre_Applicant_LoanName = await _repo.Master_Pre_App_Loan.GetNameById(item.Master_Pre_Applicant_LoanId);
						master_Pre_BusinessTypeName = await _repo.Master_Pre_BusType.GetNameById(item.Master_Pre_BusinessTypeId);

						var loanValue = item.LoanValue ?? 0;

						var pre_Factor_Info = new Data.Entity.Pre_Factor_Info();
						pre_Factor_Info.Status = StatusModel.Active;
						pre_Factor_Info.CreateDate = _dateNow;
						pre_Factor_Info.Pre_FactorId = pre_Factor.Id;
						pre_Factor_Info.LoanId = item.LoanId;
						pre_Factor_Info.LoanIName = loanIName;
						pre_Factor_Info.InstallmentPayYear = item.InstallmentPayYear;
						pre_Factor_Info.LoanValue = loanValue;
						pre_Factor_Info.LoanPeriod = item.LoanPeriod;
						pre_Factor_Info.Master_Pre_Applicant_LoanId = item.Master_Pre_Applicant_LoanId;
						pre_Factor_Info.Master_Pre_Applicant_LoanName = master_Pre_Applicant_LoanName;
						pre_Factor_Info.Master_Pre_BusinessTypeId = item.Master_Pre_BusinessTypeId;
						pre_Factor_Info.Master_Pre_BusinessTypeName = master_Pre_BusinessTypeName;
						await _db.InsterAsync(pre_Factor_Info);
						await _db.SaveAsync();

						decimal? score = null;
						decimal? ratio = null;
						decimal? scoreResult = null;

						if (calInfo != null && calInfo.Pre_Cal_Info_Scores?.Count > 0)
						{
							// ค้นหาค่าที่ใกล้เคียงที่สุด							
							var scoreClosest = calInfo.Pre_Cal_Info_Scores.OrderBy(x => Math.Abs((decimal)(x.Name ?? 0) - loanValue)).First();
							if (scoreClosest != null)
							{
								score = scoreClosest.Score;
							}
						}
						if (calWeightInfo != null && calWeightInfo.Pre_Cal_WeightFactor_Items?.Count > 0)
						{
							ratio = calWeightInfo.Pre_Cal_WeightFactor_Items.FirstOrDefault()?.Percent;
						}

						scoreResult = (score * ratio) / 100;

						pre_Result_Items.Add(new()
						{
							Type = PreCalType.Info,
							AnalysisFactor = "มูลค่าสินเชื่อ",
							Feature = loanValue.ToString(GeneralTxt.FormatDecimal2),
							Score = score,
							Ratio = ratio,
							ScoreResult = scoreResult
						});

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
								.FirstOrDefaultAsync(x => x.Type == PreStanDropDownType.CollateralType && x.Id == item.Stan_ItemOptionId_Type1.Value);
							if (stan_ItemOption == null) throw new ExceptionCustom($"stan_ItemOption type1 not found.");
							Stan_ItemOptionName_Type1 = stan_ItemOption.Name;
						}
						if (item.Stan_ItemOptionId_Type2.HasValue)
						{
							var stan_ItemOption = await _repo.Context.Pre_Cal_Fetu_Stan_ItemOptions
								.FirstOrDefaultAsync(x => x.Type == PreStanDropDownType.PaymentHistory && x.Id == item.Stan_ItemOptionId_Type2.Value);
							if (stan_ItemOption == null) throw new ExceptionCustom($"stan_ItemOption type2 not found.");
							Stan_ItemOptionName_Type2 = stan_ItemOption.Name;
						}

						decimal depositBAAC = item.DepositBAAC ?? 0;

						var pre_Factor_Stan = new Data.Entity.Pre_Factor_Stan();
						pre_Factor_Stan.Status = StatusModel.Active;
						pre_Factor_Stan.CreateDate = _dateNow;
						pre_Factor_Stan.Pre_FactorId = pre_Factor.Id;
						pre_Factor_Stan.Income = item.Income;
						pre_Factor_Stan.Expenses = item.Expenses;
						pre_Factor_Stan.OtherDebts = item.OtherDebts;
						pre_Factor_Stan.IncomeDebtPeriod = item.IncomeDebtPeriod;
						pre_Factor_Stan.DepositBAAC = depositBAAC;
						pre_Factor_Stan.Stan_ItemOptionId_Type1 = item.Stan_ItemOptionId_Type1;
						pre_Factor_Stan.Stan_ItemOptionName_Type1 = Stan_ItemOptionName_Type1;
						pre_Factor_Stan.Stan_ItemOptionValue_Type1 = item.Stan_ItemOptionValue_Type1;
						pre_Factor_Stan.Stan_ItemOptionId_Type2 = item.Stan_ItemOptionId_Type2;
						pre_Factor_Stan.Stan_ItemOptionName_Type2 = Stan_ItemOptionName_Type2;
						await _db.InsterAsync(pre_Factor_Stan);
						await _db.SaveAsync();

						decimal? score = null;
						decimal? ratio = null;
						decimal? scoreResult = null;

						if (calStan != null && calStan.Pre_Cal_Fetu_Stan_Scores?.Count > 0)
						{
							var weightIncomeExpenses = calStan.Pre_Cal_Fetu_Stan_Scores.Where(x=>x.Type == PreStanScoreType.WeightIncomeExpenses).ToList();
							var weighCollateraltDebtValue = calStan.Pre_Cal_Fetu_Stan_Scores.Where(x=>x.Type == PreStanScoreType.WeighCollateraltDebtValue).ToList();
							var weighLiabilitieOtherIncome = calStan.Pre_Cal_Fetu_Stan_Scores.Where(x=>x.Type == PreStanScoreType.WeighLiabilitieOtherIncome).ToList();
							var cashBank = calStan.Pre_Cal_Fetu_Stan_Scores.Where(x=>x.Type == PreStanScoreType.CashBank).ToList();
							var collateralType = calStan.Pre_Cal_Fetu_Stan_Scores.Where(x=>x.Type == PreStanScoreType.CollateralType).ToList();
							var collateralValue = calStan.Pre_Cal_Fetu_Stan_Scores.Where(x=>x.Type == PreStanScoreType.CollateralValue).ToList();
							var paymentHistory = calStan.Pre_Cal_Fetu_Stan_Scores.Where(x=>x.Type == PreStanScoreType.PaymentHistory).ToList();


							//ปริมาณเงินฝาก ธกส.
							if (cashBank.Count > 0)
							{
								var decimals = cashBank.Select(o =>
								{
									try
									{
										if (o == null) return 0m; // ค่า null
										if (o.Name is string s && decimal.TryParse(s, out decimal result)) return result; // ถ้าเป็น string และแปลงเป็น decimal ได้
										return 0m; // ค่าดีฟอลต์สำหรับค่าอื่น ๆ ที่ไม่สามารถแปลงได้
									}
									catch
									{
										return 0m; // ค่าดีฟอลต์ในกรณีที่เกิดข้อผิดพลาด
									}
								}).ToList();

								var scoreClosest = decimals.OrderBy(x => Math.Abs(x - depositBAAC)).First();
								score = cashBank.FirstOrDefault(x => x.Name == scoreClosest.ToString())?.Score;

								if (calWeightStan != null && calWeightStan.Pre_Cal_WeightFactor_Items?.Count > 0)
								{
									ratio = calWeightStan.Pre_Cal_WeightFactor_Items.FirstOrDefault(x=>x.StanScoreType == PreStanScoreType.CashBank)?.Percent;
								}

								scoreResult = (score * ratio) / 100;

								pre_Result_Items.Add(new()
								{
									Type = PreCalType.Stan,
									AnalysisFactor = PropertiesMain.PerCalFetuStanName(PreStanScoreType.CashBank.ToString()).Name,
									Feature = depositBAAC.ToString(GeneralTxt.FormatDecimal2),
									Score = score,
									Ratio = ratio,
									ScoreResult = scoreResult
								});
							}

							//ประเภทหลักประกัน
							if (collateralType.Count > 0)
							{
								score = null;
								ratio = null;
								scoreResult = null;

								if (item.Stan_ItemOptionId_Type1.HasValue)
								{
									var itemOption = collateralType.FirstOrDefault(x=>x.Pre_Cal_Fetu_StanItemOptionId == item.Stan_ItemOptionId_Type1);
									if (itemOption != null)
									{
										score = itemOption.Score;
									}
								}

								if (calWeightStan != null && calWeightStan.Pre_Cal_WeightFactor_Items?.Count > 0)
								{
									ratio = calWeightStan.Pre_Cal_WeightFactor_Items.FirstOrDefault(x => x.StanScoreType == PreStanScoreType.CollateralType)?.Percent;
								}

								scoreResult = (score * ratio) / 100;

								pre_Result_Items.Add(new()
								{
									Type = PreCalType.Stan,
									AnalysisFactor = PropertiesMain.PerCalFetuStanName(PreStanScoreType.CollateralType.ToString()).Name,
									//Feature = loanValue.ToString(GeneralTxt.FormatDecimal2),
									Score = score,
									Ratio = ratio,
									ScoreResult = scoreResult
								});
							}


						}
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
							if (app_Items == null) throw new ExceptionCustom($"pre_Cal_Fetu_App_ItemId not found.");
							pre_Cal_Fetu_App_ItemName = app_Items.Name;
						}
						if (item.Pre_Cal_Fetu_App_Item_ScoreId.HasValue)
						{
							var stan_ItemOption_Score = await _repo.Context.Pre_Cal_Fetu_App_Item_Scores
								.FirstOrDefaultAsync(x => x.Id == item.Pre_Cal_Fetu_App_Item_ScoreId.Value);
							if (stan_ItemOption_Score == null) throw new ExceptionCustom($"pre_Cal_Fetu_App_Item_ScoreId not found.");
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
							if (bus_Items == null) throw new ExceptionCustom($"pre_Cal_Fetu_Bus_ItemId not found.");
							pre_Cal_Fetu_Bus_ItemName = bus_Items.Name;
						}
						if (item.Pre_Cal_Fetu_Bus_Item_ScoreId.HasValue)
						{
							var stan_ItemOption_Score = await _repo.Context.Pre_Cal_Fetu_Bus_Item_Scores
								.FirstOrDefaultAsync(x => x.Id == item.Pre_Cal_Fetu_Bus_Item_ScoreId.Value);
							if (stan_ItemOption_Score == null) throw new ExceptionCustom($"pre_Cal_Fetu_Bus_Item_ScoreId not found.");
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

				//Result
				var pre_Result = new Data.Entity.Pre_Result();
				pre_Result.Status = StatusModel.Active;
				pre_Result.CreateDate = _dateNow;
				pre_Result.CreateBy = model.CurrentUserId;
				pre_Result.Pre_FactorId = pre_Factor.Id;
				pre_Result.TotalScore = 0;
				pre_Result.ResultLoan = null;
				pre_Result.ChancePercent = null;
				await _db.InsterAsync(pre_Result);
				await _db.SaveAsync();

				if (pre_Result_Items.Count > 0)
				{
					foreach (var item in pre_Result_Items)
					{
						var pre_Result_Item = new Data.Entity.Pre_Result_Item();
						pre_Result_Item.Status = StatusModel.Active;
						pre_Result_Item.CreateDate = _dateNow;
						pre_Result_Item.Pre_ResultId = pre_Result.Id;
						pre_Result_Item.Type = item.Type;
						pre_Result_Item.AnalysisFactor = item.AnalysisFactor;
						pre_Result_Item.Feature = item.Feature;
						pre_Result_Item.Score = item.Score;
						pre_Result_Item.Ratio = item.Ratio;
						pre_Result_Item.ScoreResult = item.ScoreResult;
						await _db.InsterAsync(pre_Result_Item);
						await _db.SaveAsync();
					}
				}

				//_transaction.Commit();

				var factor_result = await GetById(pre_Factor.Id);

				return factor_result;
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
