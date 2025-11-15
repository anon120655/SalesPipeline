using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.ConstTypeModel;
using SalesPipeline.Utils.PropertiesModel;
using SalesPipeline.Utils.Resources.PreApprove;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Infrastructure.Repositorys
{
	public class PreFactor : IPreFactor
	{
		private readonly IRepositoryWrapper _repo;
		private readonly IMapper _mapper;
		private readonly IRepositoryBase _db;

		public PreFactor(IRepositoryWrapper repo, IRepositoryBase db, IMapper mapper)
		{
			_db = db;
			_repo = repo;
			_mapper = mapper;
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
				pre_Factor.CompanyName = sales.CompanyName;
				await _db.InsterAsync(pre_Factor);
				await _db.SaveAsync();

				string? loanIName = null;
				int? loanPeriod = null;

				//มูลค่าสินเชื่อที่ขอ
				decimal loanValue = 0;
				//มูลค่าหลักประกัน
				decimal collValue = 0;
				//หนี้สินอื่นๆ
				decimal otherDebts = 0;
				//รายได้ที่ได้ตามระยะงวดหนี้สินด้านบน
				decimal incomeDebtPeriod = 0;


				decimal? score = null;
				decimal? ratio = null;
				decimal? scoreResult = null;

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

					decimal checkWeight100 = (calWeightInfo?._TotalPercent ?? 0)
				   + (calWeightStan?._TotalPercent ?? 0)
				   + (calWeightAppLoan?._TotalPercent ?? 0)
				   + (calWeightBusType?._TotalPercent ?? 0);

					if (checkWeight100 != 100)
					{
						throw new ExceptionCustom($"ตัวแปรคำนวณ น้ำหนักของแต่ละปัจจัยยังไม่สมบูรณ์");
					}
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

						string? master_Pre_Applicant_LoanName = null;
						string? master_Pre_BusinessTypeName = null;

						if (item.LoanId.HasValue)
						{
							loanIName = await _repo.Loan.GetNameById(item.LoanId.Value);
						}

						master_Pre_Applicant_LoanName = await _repo.Master_Pre_App_Loan.GetNameById(item.Master_Pre_Applicant_LoanId);
						master_Pre_BusinessTypeName = await _repo.Master_Pre_BusType.GetNameById(item.Master_Pre_BusinessTypeId);

						loanValue = item.LoanValue ?? 0;
						loanPeriod = item.LoanPeriod;

						var pre_Factor_Info = new Data.Entity.Pre_Factor_Info();
						pre_Factor_Info.Status = StatusModel.Active;
						pre_Factor_Info.CreateDate = _dateNow;
						pre_Factor_Info.Pre_FactorId = pre_Factor.Id;
						pre_Factor_Info.LoanId = item.LoanId;
						pre_Factor_Info.LoanIName = loanIName;
						pre_Factor_Info.InstallmentPayYear = item.InstallmentPayYear;
						pre_Factor_Info.LoanValue = loanValue;
						pre_Factor_Info.LoanPeriod = loanPeriod;
						pre_Factor_Info.Master_Pre_Applicant_LoanId = item.Master_Pre_Applicant_LoanId;
						pre_Factor_Info.Master_Pre_Applicant_LoanName = master_Pre_Applicant_LoanName;
						pre_Factor_Info.Master_Pre_BusinessTypeId = item.Master_Pre_BusinessTypeId;
						pre_Factor_Info.Master_Pre_BusinessTypeName = master_Pre_BusinessTypeName;
						await _db.InsterAsync(pre_Factor_Info);
						await _db.SaveAsync();

						score = null;
						ratio = null;

						if (calInfo != null && calInfo.Pre_Cal_Info_Scores?.Count > 0)
						{
							var lookupArray = calInfo.Pre_Cal_Info_Scores.Select(x => new XLookUpModel()
							{
								ID = x.Id,
								CheckValue = (double)(x.Name ?? 0),
								ReturnValue = x.Score.ToString()
							}).ToList();

							lookupArray = lookupArray.OrderByDescending(x => x.CheckValue).ToList();

							//20240926 ตัวแปรและคะแนนสวนทางกันได้ตามการหารือ (ยิ่งค่าน้อย คะแนนยิ่งสูง) 
							//20240926 ทำการปรับแก้ Logic ใช้ match_mode=1
							var lookupResult = LoanCalculator.XLookupLists((double)loanValue, lookupArray, 1);
							if (lookupResult != null)
							{
								var scoreClosest = calInfo.Pre_Cal_Info_Scores.FirstOrDefault(x => x.Id == lookupResult.ID);
								if (scoreClosest != null)
								{
									score = scoreClosest.Score;
								}
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
						collValue = item.Stan_ItemOptionValue_Type1 ?? 0;
						otherDebts = item.OtherDebts ?? 0;
						incomeDebtPeriod = item.IncomeDebtPeriod ?? 0;

						var pre_Factor_Stan = new Data.Entity.Pre_Factor_Stan();
						pre_Factor_Stan.Status = StatusModel.Active;
						pre_Factor_Stan.CreateDate = _dateNow;
						pre_Factor_Stan.Pre_FactorId = pre_Factor.Id;
						pre_Factor_Stan.Income = item.Income;
						pre_Factor_Stan.Expenses = item.Expenses;
						pre_Factor_Stan.OtherDebts = otherDebts;
						pre_Factor_Stan.IncomeDebtPeriod = incomeDebtPeriod;
						pre_Factor_Stan.DepositBAAC = depositBAAC;
						pre_Factor_Stan.Stan_ItemOptionId_Type1 = item.Stan_ItemOptionId_Type1;
						pre_Factor_Stan.Stan_ItemOptionName_Type1 = Stan_ItemOptionName_Type1;
						pre_Factor_Stan.Stan_ItemOptionValue_Type1 = collValue;
						pre_Factor_Stan.Stan_ItemOptionId_Type2 = item.Stan_ItemOptionId_Type2;
						pre_Factor_Stan.Stan_ItemOptionName_Type2 = Stan_ItemOptionName_Type2;
						await _db.InsterAsync(pre_Factor_Stan);
						await _db.SaveAsync();

						if (calStan != null && calStan.Pre_Cal_Fetu_Stan_Scores?.Count > 0)
						{
							var weightIncomeExpenses = calStan.Pre_Cal_Fetu_Stan_Scores.Where(x => x.Type == PreStanScoreType.WeightIncomeExpenses).ToList();
							var weighCollateraltDebtValue = calStan.Pre_Cal_Fetu_Stan_Scores.Where(x => x.Type == PreStanScoreType.WeighCollateraltDebtValue).ToList();
							var weighLiabilitieOtherIncome = calStan.Pre_Cal_Fetu_Stan_Scores.Where(x => x.Type == PreStanScoreType.WeighLiabilitieOtherIncome).ToList();
							var cashBank = calStan.Pre_Cal_Fetu_Stan_Scores.Where(x => x.Type == PreStanScoreType.CashBank).ToList();
							var collateralType = calStan.Pre_Cal_Fetu_Stan_Scores.Where(x => x.Type == PreStanScoreType.CollateralType).ToList();
							var collateralValue = calStan.Pre_Cal_Fetu_Stan_Scores.Where(x => x.Type == PreStanScoreType.CollateralValue).ToList();
							var paymentHistory = calStan.Pre_Cal_Fetu_Stan_Scores.Where(x => x.Type == PreStanScoreType.PaymentHistory).ToList();

							//อัตราส่วนรายได้ต่อรายจ่าย
							if (weightIncomeExpenses.Count > 0)
							{
								score = 0;
								ratio = null;
								string? _feature = null;

								//รายได้/รายจ่าย
								var incomeExpenses = (item.Income / item.Expenses) ?? 0;

								var lookupArray = weightIncomeExpenses.Select(s =>
								{
									if (double.TryParse(s.Name, out double checkValue))
									{
										return new XLookUpModel
										{
											ID = s.Id,
											CheckValue = checkValue,
											ReturnValue = s.Score.ToString()
										};
									}
									else
									{
										return new();
									}
								}).ToList();

								lookupArray = lookupArray.OrderBy(x => x.CheckValue).ToList();

								// ค้นหาค่าที่ระบุ ถ้าไม่พบจะคืนค่าลำดับที่น้อยกว่า	
								var lookupResult = LoanCalculator.XLookupLists((double)incomeExpenses, lookupArray, -1);
								if (lookupResult != null)
								{
									var scoreClosest = weightIncomeExpenses.FirstOrDefault(x => x.Id == lookupResult.ID);
									if (scoreClosest != null)
									{
										_feature = scoreClosest.Name;
										score = scoreClosest.Score;
									}
								}

								if (calWeightStan != null && calWeightStan.Pre_Cal_WeightFactor_Items?.Count > 0)
								{
									ratio = calWeightStan.Pre_Cal_WeightFactor_Items.FirstOrDefault(x => x.StanScoreType == PreStanScoreType.WeightIncomeExpenses)?.Percent;
								}

								scoreResult = (score * ratio) / 100;

								pre_Result_Items.Add(new()
								{
									Type = PreCalType.Stan,
									AnalysisFactor = PropertiesMain.PerCalFetuStanName(PreStanScoreType.WeightIncomeExpenses.ToString()).Name,
									Feature = _feature,
									Score = score,
									Ratio = ratio,
									ScoreResult = scoreResult
								});
							}

							//อัตราส่วนหลักประกันต่อมูลค่าหนี้
							if (weighCollateraltDebtValue.Count > 0)
							{
								score = 0;
								ratio = null;
								string? _feature = null;

								//มูลค่าหลักประกัน/มูลค่าสินเชื่อที่ขอ

								//20240926 ทำการปรับแก้ Logic แก้ไขเป็น มูลค่าสินเชื่อ/มูลค่าหลักประกัน
								var collValueloanValue = loanValue / collValue;

								var lookupArray = weighCollateraltDebtValue.Select(s =>
								{
									if (double.TryParse(s.Name, out double checkValue))
									{
										return new XLookUpModel
										{
											ID = s.Id,
											CheckValue = checkValue,
											ReturnValue = s.Score.ToString()
										};
									}
									else
									{
										return new();
									}
								}).ToList();

								lookupArray = lookupArray.OrderBy(x => x.CheckValue).ToList();

								//20240926 ตัวแปรและคะแนนสวนทางกันได้ตามการหารือ (ยิ่งค่าน้อย คะแนนยิ่งสูง) 
								//20240926 ทำการปรับแก้ Logic ใช้ match_mode=1
								var lookupResult = LoanCalculator.XLookupLists((double)collValueloanValue, lookupArray, 1);
								if (lookupResult != null)
								{
									var scoreClosest = weighCollateraltDebtValue.FirstOrDefault(x => x.Id == lookupResult.ID);
									if (scoreClosest != null)
									{
										_feature = scoreClosest.Name;
										score = scoreClosest.Score;
									}
								}


								if (calWeightStan != null && calWeightStan.Pre_Cal_WeightFactor_Items?.Count > 0)
								{
									ratio = calWeightStan.Pre_Cal_WeightFactor_Items.FirstOrDefault(x => x.StanScoreType == PreStanScoreType.WeighCollateraltDebtValue)?.Percent;
								}

								scoreResult = (score * ratio) / 100;

								pre_Result_Items.Add(new()
								{
									Type = PreCalType.Stan,
									AnalysisFactor = PropertiesMain.PerCalFetuStanName(PreStanScoreType.WeighCollateraltDebtValue.ToString()).Name,
									Feature = _feature,
									Score = score,
									Ratio = ratio,
									ScoreResult = scoreResult
								});
							}

							//อัตราส่วนภาระชำระหนี้สินอื่น ๆ ต่อรายได้ (ตามรอบธุรกิจ/รอบผลผลิต)
							if (weighLiabilitieOtherIncome.Count > 0)
							{
								score = 0;
								ratio = null;
								string? _feature = null;

								//(รายได้ตามรอบธุรกิจ/รอบผลผลิต) / อัตราส่วนภาระชำระหนี้สินอื่น ๆ ต่อรายได้ (ตามรอบธุรกิจ/รอบผลผลิต)

								//20240926 ทำการปรับแก้ Logic ในการคำนวณเป็น “ภาระชำระหนี้สินอื่น ๆ ตามรอบธุรกิจ/รอบผลผลิต” / “รายได้ตามรอบธุรกิจ/รอบผลผลิต”
								var incomeDebtPeriodOtherDebts = otherDebts / incomeDebtPeriod;

								var lookupArray = weighLiabilitieOtherIncome.Select(s =>
								{
									if (double.TryParse(s.Name, out double checkValue))
									{
										return new XLookUpModel
										{
											ID = s.Id,
											CheckValue = checkValue,
											ReturnValue = s.Score.ToString()
										};
									}
									else
									{
										return new();
									}
								}).ToList();

								lookupArray = lookupArray.OrderBy(x => x.CheckValue).ToList();

								//20240926 ตัวแปรและคะแนนสวนทางกันได้ตามการหารือ (ยิ่งค่าน้อย คะแนนยิ่งสูง) 
								//20240926 ทำการปรับแก้ Logic ใช้ match_mode=1
								var lookupResult = LoanCalculator.XLookupLists((double)incomeDebtPeriodOtherDebts, lookupArray, 1);
								if (lookupResult != null)
								{
									var scoreClosest = weighLiabilitieOtherIncome.FirstOrDefault(x => x.Id == lookupResult.ID);
									if (scoreClosest != null)
									{
										_feature = scoreClosest.Name;
										score = scoreClosest.Score;
									}
								}


								if (calWeightStan != null && calWeightStan.Pre_Cal_WeightFactor_Items?.Count > 0)
								{
									ratio = calWeightStan.Pre_Cal_WeightFactor_Items.FirstOrDefault(x => x.StanScoreType == PreStanScoreType.WeighLiabilitieOtherIncome)?.Percent;
								}

								scoreResult = (score * ratio) / 100;

								pre_Result_Items.Add(new()
								{
									Type = PreCalType.Stan,
									AnalysisFactor = PropertiesMain.PerCalFetuStanName(PreStanScoreType.WeighLiabilitieOtherIncome.ToString()).Name,
									Feature = _feature,
									Score = score,
									Ratio = ratio,
									ScoreResult = scoreResult
								});
							}

							//ปริมาณเงินฝาก ธกส.
							if (cashBank.Count > 0)
							{
								var lookupArray = cashBank.Select(s =>
								{
									if (double.TryParse(s.Name, out double checkValue))
									{
										return new XLookUpModel
										{
											ID = s.Id,
											CheckValue = checkValue,
											ReturnValue = s.Score.ToString()
										};
									}
									else
									{
										return new();
									}
								}).ToList();

								lookupArray = lookupArray.OrderBy(x => x.CheckValue).ToList();

								// ค้นหาค่าที่ระบุ ถ้าไม่พบจะคืนค่าลำดับที่น้อยกว่า	
								var lookupResult = LoanCalculator.XLookupLists((double)depositBAAC, lookupArray, -1);
								if (lookupResult != null)
								{
									var scoreClosest = cashBank.FirstOrDefault(x => x.Id == lookupResult.ID);
									if (scoreClosest != null)
									{
										score = scoreClosest.Score;
									}
								}

								if (calWeightStan != null && calWeightStan.Pre_Cal_WeightFactor_Items?.Count > 0)
								{
									ratio = calWeightStan.Pre_Cal_WeightFactor_Items.FirstOrDefault(x => x.StanScoreType == PreStanScoreType.CashBank)?.Percent;
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
								string? _feature = null;

								if (item.Stan_ItemOptionId_Type1.HasValue)
								{
									var itemOption = collateralType.FirstOrDefault(x => x.Pre_Cal_Fetu_StanItemOptionId == item.Stan_ItemOptionId_Type1);
									if (itemOption != null)
									{
										_feature = itemOption.Name;
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
									Feature = _feature,
									Score = score,
									Ratio = ratio,
									ScoreResult = scoreResult
								});
							}

							//มูลค่าหลักประกัน
							if (collateralValue.Count > 0)
							{
								score = null;
								ratio = null;
								scoreResult = null;

								var lookupArray = collateralValue.Select(s =>
								{
									if (double.TryParse(s.Name, out double checkValue))
									{
										return new XLookUpModel
										{
											ID = s.Id,
											CheckValue = checkValue,
											ReturnValue = s.Score.ToString()
										};
									}
									else
									{
										return new();
									}
								}).ToList();

								lookupArray = lookupArray.OrderBy(x => x.CheckValue).ToList();

								// ค้นหาค่าที่ระบุ ถ้าไม่พบจะคืนค่าลำดับที่น้อยกว่า	
								var lookupResult = LoanCalculator.XLookupLists((double)collValue, lookupArray, -1);
								if (lookupResult != null)
								{
									var scoreClosest = collateralValue.FirstOrDefault(x => x.Id == lookupResult.ID);
									if (scoreClosest != null)
									{
										score = scoreClosest.Score;
									}
								}

								scoreResult = (score * ratio) / 100;

								pre_Result_Items.Add(new()
								{
									Type = PreCalType.Stan,
									AnalysisFactor = PropertiesMain.PerCalFetuStanName(PreStanScoreType.CollateralValue.ToString()).Name,
									Feature = collValue.ToString(GeneralTxt.FormatDecimal2),
									Score = score,
									Ratio = ratio,
									ScoreResult = scoreResult
								});
							}

							//ประเภทหลักประกัน
							if (paymentHistory.Count > 0)
							{
								score = null;
								ratio = null;
								scoreResult = null;
								string? _feature = null;

								if (item.Stan_ItemOptionId_Type2.HasValue)
								{
									var itemOption = paymentHistory.FirstOrDefault(x => x.Pre_Cal_Fetu_StanItemOptionId == item.Stan_ItemOptionId_Type2);
									if (itemOption != null)
									{
										_feature = itemOption.Name;
										score = itemOption.Score;
									}
								}

								if (calWeightStan != null && calWeightStan.Pre_Cal_WeightFactor_Items?.Count > 0)
								{
									ratio = calWeightStan.Pre_Cal_WeightFactor_Items.FirstOrDefault(x => x.StanScoreType == PreStanScoreType.PaymentHistory)?.Percent;
								}

								scoreResult = (score * ratio) / 100;

								pre_Result_Items.Add(new()
								{
									Type = PreCalType.Stan,
									AnalysisFactor = PropertiesMain.PerCalFetuStanName(PreStanScoreType.PaymentHistory.ToString()).Name,
									Feature = _feature,
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
					if (calWeightAppLoan?.Pre_Cal_WeightFactor_Items?.Count != model.Pre_Factor_Apps.Count)
					{
						throw new ExceptionCustom($"คุณสมบัติตามประเภทผู้ขอไม่ครบ");
					}
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

							if (item.Pre_Cal_Fetu_App_Item_ScoreId.HasValue)
							{
								var stan_ItemOption_Score = await _repo.Context.Pre_Cal_Fetu_App_Item_Scores
									.FirstOrDefaultAsync(x => x.Id == item.Pre_Cal_Fetu_App_Item_ScoreId.Value && x.Pre_Cal_Fetu_App_ItemId == app_Items.Id);
								if (stan_ItemOption_Score == null) throw new ExceptionCustom($"pre_Cal_Fetu_App_Item_ScoreId not found.");
								pre_Cal_Fetu_App_Item_ScoreName = stan_ItemOption_Score.Name;
							}
							else
							{
								throw new ExceptionCustom($"pre_Cal_Fetu_App_Item_ScoreId require.");
							}
						}
						else
						{
							throw new ExceptionCustom($"pre_Cal_Fetu_App_ItemId require.");
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


						score = null;
						ratio = null;
						scoreResult = null;

						if (calApp != null && calApp.Pre_Cal_Fetu_App_Items?.Count > 0)
						{
							var app_Item = calApp.Pre_Cal_Fetu_App_Items.FirstOrDefault(x => x.Id == item.Pre_Cal_Fetu_App_ItemId);
							if (app_Item != null && app_Item.Pre_Cal_Fetu_App_Item_Scores?.Count > 0)
							{
								var app_Item_Score = app_Item.Pre_Cal_Fetu_App_Item_Scores.FirstOrDefault(x => x.Id == item.Pre_Cal_Fetu_App_Item_ScoreId);
								if (app_Item_Score != null)
								{
									score = app_Item_Score.Score;
								}
							}
						}

						if (calWeightAppLoan != null && calWeightAppLoan.Pre_Cal_WeightFactor_Items?.Count > 0)
						{
							ratio = calWeightAppLoan.Pre_Cal_WeightFactor_Items.FirstOrDefault(x => x.RefItemId == item.Pre_Cal_Fetu_App_ItemId)?.Percent;
						}

						scoreResult = (score * ratio) / 100;

						pre_Result_Items.Add(new()
						{
							Type = PreCalType.AppLoan,
							AnalysisFactor = pre_Cal_Fetu_App_ItemName,
							Feature = pre_Cal_Fetu_App_Item_ScoreName,
							Score = score,
							Ratio = ratio,
							ScoreResult = scoreResult
						});
					}
				}

				if (model.Pre_Factor_Bus?.Count > 0)
				{
					if (calWeightBusType?.Pre_Cal_WeightFactor_Items?.Count != model.Pre_Factor_Bus.Count)
					{
						throw new ExceptionCustom($"คุณสมบัติตามประเภทธุรกิจไม่ครบ");
					}
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

							if (item.Pre_Cal_Fetu_Bus_Item_ScoreId.HasValue)
							{
								var stan_ItemOption_Score = await _repo.Context.Pre_Cal_Fetu_Bus_Item_Scores
									.FirstOrDefaultAsync(x => x.Id == item.Pre_Cal_Fetu_Bus_Item_ScoreId.Value && x.Pre_Cal_Fetu_Bus_ItemId == bus_Items.Id);
								if (stan_ItemOption_Score == null) throw new ExceptionCustom($"pre_Cal_Fetu_Bus_Item_ScoreId not found.");
								pre_Cal_Fetu_Bus_Item_ScoreName = stan_ItemOption_Score.Name;
							}
							else
							{
								throw new ExceptionCustom($"pre_Cal_Fetu_Bus_Item_ScoreId require.");
							}
						}
						else
						{
							throw new ExceptionCustom($"pre_Cal_Fetu_Bus_ItemId require.");
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


						score = null;
						ratio = null;
						scoreResult = null;

						if (calBus != null && calBus.Pre_Cal_Fetu_Bus_Items?.Count > 0)
						{
							var bus_Item = calBus.Pre_Cal_Fetu_Bus_Items.FirstOrDefault(x => x.Id == item.Pre_Cal_Fetu_Bus_ItemId);
							if (bus_Item != null && bus_Item.Pre_Cal_Fetu_Bus_Item_Scores?.Count > 0)
							{
								var bus_Item_Score = bus_Item.Pre_Cal_Fetu_Bus_Item_Scores.FirstOrDefault(x => x.Id == item.Pre_Cal_Fetu_Bus_Item_ScoreId);
								if (bus_Item_Score != null)
								{
									score = bus_Item_Score.Score;
								}
							}
						}

						if (calWeightBusType != null && calWeightBusType.Pre_Cal_WeightFactor_Items?.Count > 0)
						{
							ratio = calWeightBusType.Pre_Cal_WeightFactor_Items.FirstOrDefault(x => x.RefItemId == item.Pre_Cal_Fetu_Bus_ItemId)?.Percent;
						}

						scoreResult = (score * ratio) / 100;

						pre_Result_Items.Add(new()
						{
							Type = PreCalType.BusType,
							AnalysisFactor = pre_Cal_Fetu_Bus_ItemName,
							Feature = pre_Cal_Fetu_Bus_Item_ScoreName,
							Score = score,
							Ratio = ratio,
							ScoreResult = scoreResult
						});
					}
				}

				//Result
				decimal totalScore = 0;
				string? cr_Level = null;
				int? cr_CreditScore = null;
				string? cr_Grade = null;
				string? cr_LimitMultiplier = null;
				string? cr_RateMultiplier = null;
				string? resultLoan = null;
				double? chanceNumber = null;
				double? chanceNumber2digit = null;
				string? chancePercent = null;

				if (pre_Result_Items.Count > 0)
				{
					totalScore = pre_Result_Items.Sum(x => x.ScoreResult ?? 0);

					var creditScoreList = await _repo.PreCreditScore.GetList(new() { page = 1, pagesize = 300 });
					if (creditScoreList.Items.Count > 0)
					{
						//totalScore = (decimal)72.60;

						var gradeCC = creditScoreList.Items.FirstOrDefault(x => x.Grade == "CC");
						if (gradeCC != null)
						{
							resultLoan = totalScore >= gradeCC.CreditScore ? "ผ่าน" : "ไม่ผ่าน";
						}

						var lookupArray = creditScoreList.Items.Select(x => new XLookUpModel()
						{
							ID = x.Id,
							CheckValue = (double)(x.CreditScore ?? 0),
							ReturnValue = (x.Grade ?? string.Empty).ToString()
						}).ToList();

						lookupArray = lookupArray.OrderBy(x => x.CheckValue).ToList();

						// ค้นหาค่าที่ระบุ ถ้าไม่พบจะคืนค่าลำดับที่น้อยกว่า	
						var lookupResult = LoanCalculator.XLookupLists((double)totalScore, lookupArray, -1);
						if (lookupResult != null)
						{
							var scoreClosest = creditScoreList.Items.FirstOrDefault(x => x.Id == lookupResult.ID);
							if (scoreClosest != null)
							{
								cr_Level = scoreClosest.Level;
								cr_CreditScore = scoreClosest.CreditScore;
								cr_Grade = scoreClosest.Grade;
								cr_LimitMultiplier = scoreClosest.LimitMultiplier;
								cr_RateMultiplier = scoreClosest.RateMultiplier;
							}
						}
					}


					var chancePassList = await _repo.PreChancePass.GetList(new() { page = 1, pagesize = 300 });
					if (chancePassList.Items.Count > 0)
					{
						//chancePercent
						//=1-XLOOKUP(ICO!N74,PD!B:B,PD!C:C,"error",-1)
						//N74 = totalScore
						var lookupArray = chancePassList.Items.Select(s =>
						{
							if (double.TryParse(s.CreditScore, out double checkValue))
							{
								return new XLookUpModel
								{
									ID = s.Id,
									CheckValue = checkValue,
									ReturnValue = s.Prob
								};
							}
							else
							{
								return new();
							}
						}).ToList();

						lookupArray = lookupArray.OrderByDescending(x => x.CheckValue).ToList();

						// ค้นหาค่าที่ระบุ ถ้าไม่พบจะคืนค่าลำดับที่น้อยกว่า	
						var lookupResult = LoanCalculator.XLookupLists((double)totalScore, lookupArray, -1);
						if (lookupResult != null)
						{
							var scoreClosest = chancePassList.Items.FirstOrDefault(x => x.Id == lookupResult.ID);
							if (scoreClosest != null)
							{
								if (double.TryParse(scoreClosest.Prob, out double prob))
								{
									chanceNumber = 1 - prob;
									if (chanceNumber.HasValue)
									{
										chanceNumber2digit = (double)Math.Round((decimal)chanceNumber.Value * 100, 2);
										chancePercent = $"{chanceNumber2digit} %";
									}
								}
							}
						}

					}

				}

				var pre_Result = new Data.Entity.Pre_Result();
				pre_Result.Status = StatusModel.Active;
				pre_Result.CreateDate = _dateNow;
				pre_Result.CreateBy = model.CurrentUserId;
				pre_Result.Pre_FactorId = pre_Factor.Id;
				pre_Result.TotalScore = totalScore;
				pre_Result.Cr_Level = cr_Level;
				pre_Result.Cr_CreditScore = cr_CreditScore;
				pre_Result.Cr_Grade = cr_Grade;
				pre_Result.Cr_LimitMultiplier = cr_LimitMultiplier;
				pre_Result.Cr_RateMultiplier = cr_RateMultiplier;
				pre_Result.ResultLoan = resultLoan;
				pre_Result.ChanceNumber = chanceNumber;
				pre_Result.ChancePercent = chancePercent;
				pre_Result.DisplayResultType = pre_Cal.DisplayResultType;
				await _db.InsterAsync(pre_Result);
				await _db.SaveAsync();

				if (pre_Result_Items.Count > 0)
				{
					int sequenceNo = 1;
					foreach (var item in pre_Result_Items)
					{
						var pre_Result_Item = new Data.Entity.Pre_Result_Item();
						pre_Result_Item.Status = StatusModel.Active;
						pre_Result_Item.CreateDate = _dateNow;
						pre_Result_Item.Pre_ResultId = pre_Result.Id;
						pre_Result_Item.SequenceNo = sequenceNo;
						pre_Result_Item.Type = item.Type;
						pre_Result_Item.AnalysisFactor = item.AnalysisFactor;
						pre_Result_Item.Feature = item.Feature;
						pre_Result_Item.Score = item.Score;
						pre_Result_Item.Ratio = item.Ratio;
						pre_Result_Item.ScoreResult = item.ScoreResult;
						await _db.InsterAsync(pre_Result_Item);
						await _db.SaveAsync();
						sequenceNo++;
					}
				}

				sales.Pre_FactorId = pre_Factor.Id;
				sales.ProjectLoanName = loanIName;
				sales.LoanPeriod = loanPeriod;
				sales.PercentChanceLoanPass = chanceNumber2digit;
				sales.PercentChanceLoanPassName = $"มีโอกาสขอสินเชื่อผ่าน {chancePercent}";
				_db.Update(sales);
				await _db.SaveAsync();

				_transaction.Commit();

				return _mapper.Map<Pre_FactorCustom>(pre_Factor);
			}
		}

		public async Task<Pre_FactorCustom> GetById(Guid id)
		{
			var query = await _repo.Context.Pre_Factors
				.AsNoTracking()
				.Include(x => x.Pre_Cal)
				.Include(x => x.Pre_Factor_Infos)
				.Include(x => x.Pre_Factor_Stans)
				.Include(x => x.Pre_Factor_Apps)
				.Include(x => x.Pre_Factor_Bus)
				.Include(x => x.Pre_Results).ThenInclude(x => x.Pre_Result_Items.OrderBy(o => o.SequenceNo))
				.OrderByDescending(o => o.CreateDate)
				.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.Id == id);

			if (query != null && query.Pre_Cal != null)
			{
				var result = query.Pre_Results.FirstOrDefault();
				if (result != null)
				{
					result.DisplayResultType = query.Pre_Cal.DisplayResultType;
				}
			}

			return _mapper.Map<Pre_FactorCustom>(query);
		}

		public async Task<Pre_FactorCustom> GetLastProcessBySaleId(Guid id)
		{
			var query = await _repo.Context.Pre_Factors
				.AsNoTracking()
				.Include(x => x.Pre_Cal)
				.Include(x => x.Pre_Factor_Infos)
				.Include(x => x.Pre_Factor_Stans)
				.Include(x => x.Pre_Factor_Apps)
				.Include(x => x.Pre_Factor_Bus)
				.Include(x => x.Pre_Results).ThenInclude(x => x.Pre_Result_Items.OrderBy(o => o.SequenceNo))
				.OrderByDescending(o => o.CreateDate)
				.FirstOrDefaultAsync(x => x.Status != StatusModel.Delete && x.SaleId == id);

			if (query != null && query.Pre_Cal != null)
			{
				var result = query.Pre_Results.FirstOrDefault();
				if (result != null)
				{
					result.DisplayResultType = query.Pre_Cal.DisplayResultType;
				}
			}

			return _mapper.Map<Pre_FactorCustom>(query);
		}

		public async Task<Pre_ResultCustom> UpdateEvaluateAppLoan(Pre_ResultCustom model)
		{
			using (var _transaction = _repo.BeginTransaction())
			{
				var pre_Result = await _repo.Context.Pre_Results.Where(x => x.Pre_FactorId == model.Pre_FactorId).FirstOrDefaultAsync();
				if (pre_Result != null)
				{
					pre_Result.InstallmentAll = model.InstallmentAll;
					pre_Result.IncomeTotal = model.IncomeTotal;
					pre_Result.RatioInstallmentIncome = model.RatioInstallmentIncome;
					pre_Result.PresSave = 1;
					_db.Update(pre_Result);
					await _db.SaveAsync();

					_transaction.Commit();
				}

				return _mapper.Map<Pre_ResultCustom>(pre_Result);
			}
		}

		public async Task<PaySchedule> PaymentSchedule(PayScheduleFactor model)
		{
			var response = new PaySchedule();

			response.Factor = model;

			response.ScheduleItem = new();

			var loan = await _repo.Loan.GetById(model.LoanId);
			if (loan == null) throw new ExceptionCustom("ไม่พบสินเชื่อ");

			//อัตราดอกเบี้ย(ต่ำสุด)รายปี period 1
			double rateValue = 0; //F7 = 0.06125

			//Risk Premium รายปี
			double riskPremium = 0; //F10 = 0.03

			// ตัวหารดอกเบี้ย/ตัวคูณจำนวนงวด
			double interestDivisorTermMultiplier = 12; //F11 = 12 ****ยังไม่รู้ที่มา

			// จำนวนเงินต้น
			double principal = model.Principal; //F79

			//Premium Rate Multiplier
			double premiumRateMultiplier = 0.285714286; //F80 = 0.285714286  ****ยังไม่รู้ที่มา

			// อัตราดอกเบี้ย **ดอกเบี้ยรายปี period 1
			double annualInterestRate = 0; //F81 = 0.069821429 สูตร  =F7+(F$10*F$80)

			// อัตราดอกเบี้ย **ดอกเบี้ยรายงวด period 1
			double monthlyInterestRate = 0; //F84 = 0.005818452 สูตร  =F81/F$11

			if (loan.RiskPremiumYear.HasValue)
			{
				riskPremium = (double)loan.RiskPremiumYear.Value / 100;
			}

			//if (loan.Master_Pre_Interest_PayTypeId == PrePayTypeIdModel.PayType1)
			//{
			if (loan.Loan_Periods?.Count > 0)
			{
				var period = loan.Loan_Periods.FirstOrDefault();
				if (period != null && period.RateValue.HasValue)
				{
					rateValue = (double)period.RateValue.Value / 100; //0.06125
					annualInterestRate = rateValue + (riskPremium * premiumRateMultiplier);
					monthlyInterestRate = annualInterestRate / interestDivisorTermMultiplier;
				}
			}
			//}

			// จำนวนงวด
			int numberOfPayments = model.NumberOfPayments;

			// คำนวณยอดชำระเงินรายงวด
			double monthlyPayment = LoanCalculator.CalculateMonthlyPayment(principal, monthlyInterestRate, numberOfPayments);

			double? Rate = null;
			double? Payment = null;
			var balance = principal;
			double interest = 0;
			double principalPayment = 0;
            const int MAX_PAYMENTS = 600;
            if (model.NumberOfPayments <= 0 || model.NumberOfPayments > MAX_PAYMENTS)
            {
                throw new ExceptionCustom($"จำนวนงวดต้องอยู่ระหว่าง 1-{MAX_PAYMENTS} งวด");
            }

            int safeNumberOfPayments = Math.Min(Math.Max(model.NumberOfPayments, 1), MAX_PAYMENTS);
            for (int i = 0; i <= safeNumberOfPayments; i++)
            {
                if (i != 0)
				{
					interest = Math.Round(balance * monthlyInterestRate, 2);
					principalPayment = Math.Round(monthlyPayment - interest, 2);
					balance = Math.Round(balance - principalPayment, 2);

					if (i == model.NumberOfPayments)
					{
						balance = 0;
					}

					Rate = monthlyInterestRate;
					Payment = monthlyPayment;
				}

				response.ScheduleItem.Add(new()
				{
					Period = i,
					Rate = Rate,
					RateStr = Rate.HasValue ? $"{Rate.Value.ToString("P2")}" : string.Empty,
					Payment = Payment,
					PaymentStr = Payment.HasValue ? $"฿{Payment.Value.ToString(GeneralTxt.FormatDecimal2)}" : string.Empty,
					Interest = interest,
					InterestStr = interest.ToString(GeneralTxt.FormatDecimal2),
					Principle = principalPayment,
					PrincipleStr = principalPayment.ToString(GeneralTxt.FormatDecimal2),
					Balance = balance,
					BalanceStr = balance.ToString(GeneralTxt.FormatDecimal2),
				});
			}

			return response;
		}

	}
}
