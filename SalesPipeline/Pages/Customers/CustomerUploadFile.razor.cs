using BlazorBootstrap;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Customers;
using SalesPipeline.Utils.Resources.Shares;
using System.Xml.Linq;

namespace SalesPipeline.Pages.Customers
{
	public partial class CustomerUploadFile
	{

		string? _errorMessage = null;
		private bool isLoading = false;
		private User_PermissionCustom _permission = new();
		private allFilter filter = new();
		List<CustomerCustom>? CustomerList = new();

		//ใส่กรณี clear file แล้ว input ไม่ update
		private string _inputFileId = Guid.NewGuid().ToString();
		private bool bClearInputFile = false;
		private Modal modalResult = default!;
		List<ResultImport> resultImport = new();
		private string dropClass = "";
		private string? _fileName = null;

		protected override async Task OnInitializedAsync()
		{
			_permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.Customers) ?? new User_PermissionCustom();
			StateHasChanged();
			await Task.Delay(1);
		}

		protected async override Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				await Task.Delay(10);
				firstRender = false;
			}
		}

		protected async Task OnChooseFile(InputFileChangeEventArgs inputFileChangeEvent)
		{
			_fileName = null;
			dropClass = "";
			_errorMessage = null;
			CustomerList = null;
			var file = inputFileChangeEvent.File;

			int _SizeLimit = 10; //MB

			int TenMegaBytes = _SizeLimit * 1024 * 1024;
			var fileSize = file.Size;
			if (fileSize > TenMegaBytes)
			{
				ClearInputFile();
				_errorMessage = $"Limited Max. {_SizeLimit} MB per file.";
				await _jsRuntimes.InvokeVoidAsync("WarningAlert", _errorMessage);
			}
			else
			{
				try
				{
					using (var stream = file.OpenReadStream(TenMegaBytes))
					{
						MemoryStream ms = new MemoryStream();
						await stream.CopyToAsync(ms);
						stream.Close();

						var bytefile = ms.ToArray();
						IFormFile files = new FormFile(ms, 0, bytefile.Length, "name", "CustomerLoan.xlsx");
						if (files == null) throw new Exception("File Not Support.");

						string folderName = @$"{_appSet.Value.ContentRootPath}\import\excel";

						if (files.Length > 0)
						{
							_fileName = files.FileName;
							string sFileExtension = Path.GetExtension(files.FileName).ToLower();
							if (sFileExtension != ".xls" && sFileExtension != ".xlsx" && sFileExtension != ".csv")
								throw new Exception("FileExtension Not Support.");

							ISheet sheet;
							string fullPath = Path.Combine(folderName, files.FileName);
							using (var streamread = new FileStream(fullPath, FileMode.Create))
							{
								files.CopyTo(streamread);
								streamread.Position = 0;
								int sheetCount = 0;
								if (sFileExtension == ".xls")
								{
									throw new Exception("not support  Excel 97-2000 formats.");
								}

								XSSFWorkbook hssfwb = new XSSFWorkbook(streamread); //This will read 2007 Excel format  
								sheetCount = hssfwb.NumberOfSheets;
								sheet = hssfwb.GetSheetAt(0);

								IRow row_header = sheet.GetRow(0);
								
								DateTime? DateContact = null;
								Guid? Master_ContactChannelId = null;
								string? BranchName = null;
								string? ProvincialOffice = null;
								string? EmployeeName = null;
								string? EmployeeId = null;
								string? ContactName = null;
								string? ContactTel = null;
								string? CompanyName = null;
								string? JuristicPersonRegNumber = null;
								Guid? Master_BusinessTypeId = null;
								Guid? Master_BusinessSizeId = null;
								Guid? Master_ISICCodeId = null;
								Guid? Master_YieldId = null;
								Guid? Master_ChainId = null;
								Guid? Master_LoanTypeId = null;
								string? CompanyEmail = null;
								string? CompanyTel = null;
								string? ParentCompanyGroup = null;
								string? HouseNo = null;
								int? VillageNo = null;
								int? ProvinceId = null;
								int? AmphurId = null;
								int? TambolId = null;
								string? ZipCode = null;
								//string? CompanyName = null;
								//string? CompanyName = null;
								DateTime? ShareholderMeetDay = null;
								string? RegisteredCapital = null;
								string? CreditScore = null;
								string? FiscalYear = null;
								DateTime? StatementDate = null;
								string? TradeAccReceivable = null;
								string? TradeAccRecProceedsNet = null;
								string? Inventories = null;
								decimal? LoansShort = null;
								decimal? TotalCurrentAssets = null;
								decimal? LoansLong = null;
								decimal? LandBuildingEquipment = null;
								decimal? TotalNotCurrentAssets = null;
								decimal? AssetsTotal = null;
								string? TradeAccPay = null;
								decimal? TradeAccPayLoansShot = null;
								decimal? TradeAccPayTotalCurrentLia = null;
								decimal? TradeAccPayLoansLong = null;
								decimal? TradeAccPayTotalNotCurrentLia = null;
								decimal? TradeAccPayForLoansShot = null;
								decimal? TradeAccPayTotalLiabilitie = null;
								decimal? RegisterCapitalOrdinary = null;
								decimal? RegisterCapitalPaid = null;
								decimal? ProfitLossAccumulate = null;
								decimal? TotalShareholders = null;
								decimal? TotalLiabilitieShareholders = null;
								decimal? TotalIncome = null;
								decimal? CostSales = null;
								decimal? GrossProfit = null;
								decimal? OperatingExpenses = null;
								decimal? ProfitLossBeforeDepExp = null;
								decimal? ProfitLossBeforeInterestTax = null;
								decimal? NetProfitLoss = null;
								string? InterestNote = null;

								Dictionary<string, int> header_list =
														row_header.Cells
																  .Select(x => new { x.StringCellValue, x.ColumnIndex })
																  .ToDictionary(x => x.StringCellValue, x => x.ColumnIndex);

								for (var rowIndex = 1; rowIndex <= sheet.LastRowNum; rowIndex++)
								{
									var row = sheet.GetRow(rowIndex);

									int cellIndex = 0;
									Guid guidMaster = Guid.Empty;
									if (header_list.TryGetValue("ชื่อบริษัท", out cellIndex))
									{
										CompanyName = row.GetCell(cellIndex).ToString();
									}
									if (header_list.TryGetValue("เลขนิติบุคคล", out cellIndex))
									{
										JuristicPersonRegNumber = row.GetCell(cellIndex).ToString();
									}
									if (header_list.TryGetValue("ประเภทธุรกิจ", out cellIndex))
									{
										if (Guid.TryParse(row.GetCell(cellIndex).ToString(),out guidMaster))
										{
											Master_BusinessSizeId = guidMaster;
										}
									}
									if (header_list.TryGetValue("ขนาดธุรกิจ", out cellIndex))
									{
										if (Guid.TryParse(row.GetCell(cellIndex).ToString(), out guidMaster))
										{
											Master_BusinessTypeId = guidMaster;
										}
									}
									if (header_list.TryGetValue("ISIC Code", out cellIndex))
									{
										if (Guid.TryParse(row.GetCell(cellIndex).ToString(), out guidMaster))
										{
											Master_ISICCodeId = guidMaster;
										}
									}

									if (CustomerList == null) CustomerList = new();

									CustomerList.Add(new()
									{
										DateContact = DateContact,
										Master_ContactChannelId = Master_ContactChannelId,
										BranchName = BranchName,
										ProvincialOffice = ProvincialOffice,
										EmployeeName = EmployeeName,
										EmployeeId = EmployeeId,
										ContactName = ContactName,
										ContactTel = ContactTel,
										JuristicPersonRegNumber = JuristicPersonRegNumber,
										Master_BusinessTypeId = Master_BusinessTypeId,
										Master_BusinessSizeId = Master_BusinessSizeId,
										Master_ISICCodeId = Master_ISICCodeId,
										Master_YieldId = Master_YieldId,
										Master_ChainId = Master_ChainId,
										Master_LoanTypeId = Master_LoanTypeId,
										CompanyEmail = CompanyEmail,
										CompanyTel = CompanyTel,
										ParentCompanyGroup = ParentCompanyGroup,
										HouseNo = HouseNo,
										VillageNo = VillageNo,
										ProvinceId = ProvinceId,
										AmphurId = AmphurId,
										TambolId = TambolId,
										ZipCode = ZipCode,
										Customer_Committees = new List<Customer_CommitteeCustom>(),
										ShareholderMeetDay = ShareholderMeetDay,
										Customer_Shareholders = new List<Customer_ShareholderCustom>(),
										RegisteredCapital = RegisteredCapital,
										CreditScore = CreditScore,
										FiscalYear = FiscalYear,
										StatementDate = StatementDate,
										TradeAccReceivable = TradeAccReceivable,
										TradeAccRecProceedsNet = TradeAccRecProceedsNet,
										Inventories = Inventories,
										LoansShort = LoansShort,
										TotalCurrentAssets = TotalCurrentAssets,
										LoansLong = LoansLong,
										LandBuildingEquipment = LandBuildingEquipment,
										TotalNotCurrentAssets = TotalNotCurrentAssets,
										AssetsTotal = AssetsTotal,
										TradeAccPay = TradeAccPay,
										TradeAccPayLoansShot = TradeAccPayLoansShot,
										TradeAccPayTotalCurrentLia = TradeAccPayTotalCurrentLia,
										TradeAccPayLoansLong = TradeAccPayLoansLong,
										TradeAccPayTotalNotCurrentLia = TradeAccPayTotalNotCurrentLia,
										TradeAccPayForLoansShot = TradeAccPayForLoansShot,
										TradeAccPayTotalLiabilitie = TradeAccPayTotalLiabilitie,
										RegisterCapitalOrdinary = RegisterCapitalOrdinary,
										RegisterCapitalPaid = RegisterCapitalPaid,
										ProfitLossAccumulate = ProfitLossAccumulate,
										TotalShareholders = TotalShareholders,
										TotalLiabilitieShareholders = TotalLiabilitieShareholders,
										TotalIncome = TotalIncome,
										CostSales = CostSales,
										GrossProfit = GrossProfit,
										OperatingExpenses = OperatingExpenses,
										ProfitLossBeforeDepExp = ProfitLossBeforeDepExp,
										ProfitLossBeforeInterestTax = ProfitLossBeforeInterestTax,
										NetProfitLoss = NetProfitLoss,
										InterestNote = InterestNote
									});
								}

								//int firstRowNum = sheet.FirstRowNum;
								//for (int i = (firstRowNum + 1); i <= sheet.LastRowNum; i++)
								//{
								//	IRow row = sheet.GetRow(i);
								//	if (row == null) continue;
								//	if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

								//	var EmployeeId = row.GetCell(0) != null ? row.GetCell(0).ToString() : null;
								//	var FullName = row.GetCell(1) != null ? row.GetCell(1).ToString() : null;
								//	var Email = row.GetCell(2) != null ? row.GetCell(2).ToString() : null;
								//	var PositionId = row.GetCell(3) != null ? row.GetCell(3).ToString() : null;
								//	var LevelId = row.GetCell(4) != null ? row.GetCell(4).ToString() : null;
								//	var RoleId = row.GetCell(5) != null ? row.GetCell(5).ToString() : null;

								//	int.TryParse(PositionId, out int _positionId);
								//	int.TryParse(LevelId, out int _levelid);
								//	int.TryParse(RoleId, out int _roleid);

								//	if (EmployeeId != null)
								//	{
								//		var listStrLineElements = EmployeeId.Split('\n').ToList();
								//		var EmployeeIdList = new List<string?>();
								//	}

								//	if (CustomerList == null) CustomerList = new();

								//	CustomerList.Add(new()
								//	{

								//	});
								//}

								if (CustomerList?.Count > 0)
								{
									//var response = await _userViewModel.ValidateUpload(UserList);

									//if (response.Status)
									//{
									//	UserList = response.Data?.OrderBy(x => x.IsValidate == true).ToList();
									//}
									//else
									//{
									//	HideLoading();
									//	_errorMessage = response.errorMessage;
									//	await _jsRuntimes.InvokeVoidAsync("WarningAlert", _errorMessage);
									//}
								}
							}
						}
					}
				}
				catch (Exception ex)
				{
					ClearInputFile();
					_errorMessage = ex.Message;
					await _jsRuntimes.InvokeVoidAsync("WarningAlert", _errorMessage);
				}
			}

		}

		private string GetIndexRow(IRow row, string colName)
		{
			var columns = new List<string>()
			{
				"","",""
			};

			return string.Empty;
		}

		protected async Task Save()
		{
			//UserList = UserList?.Where(x => x.IsValidate == true).ToList();

			if (CustomerList == null || CustomerList.Count(x => x.IsValidate == true) == 0)
			{
				_errorMessage = "ตรวจสอบไฟล์แนบอีกครั้ง";
				await _jsRuntimes.InvokeVoidAsync("WarningAlert", _errorMessage);
			}
			else
			{
				_errorMessage = null;
				ShowLoading();

				//foreach (var item in CustomerList.Where(x => x.IsValidate == true).ToList())
				//{
				//	var resultModel = new ResultImport();
				//	resultModel.Name = item.FullName;

				//	var response = await _userViewModel.Create(item);
				//	if (!response.Status)
				//	{
				//		resultModel.Success = false;
				//		resultModel.errorMessage = response.errorMessage;
				//	}
				//	resultImport.Add(resultModel);
				//}

				await OnShowResult();
			}

		}

		public void Cancel()
		{
			_Navs.NavigateTo("/customer");
		}

		protected void ShowLoading()
		{
			isLoading = true;
			StateHasChanged();
		}

		protected void HideLoading()
		{
			isLoading = false;
			StateHasChanged();
		}

		protected void ClearInputFile()
		{
			CustomerList = new();
			bClearInputFile = true;
			StateHasChanged();
			bClearInputFile = false;
			StateHasChanged();
		}

		private async Task OnShowResult()
		{
			HideLoading();
			await modalResult.ShowAsync();
		}

		private async Task OnHideResult()
		{
			await modalResult.HideAsync();
		}

		private void OnHiddenResult()
		{
			Cancel();
		}

		private void HandleDragEnter()
		{
			dropClass = "dropzone-drag";
		}

		private void HandleDragLeave()
		{
			dropClass = "";
		}


	}
}