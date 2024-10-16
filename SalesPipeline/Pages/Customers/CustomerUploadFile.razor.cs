using BlazorBootstrap;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Customers;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;
using System.Collections.Generic;
using System.Xml.Linq;

namespace SalesPipeline.Pages.Customers
{
	public partial class CustomerUploadFile
	{

		string? _errorMessage = null;
		private bool isLoading = false;
		private User_PermissionCustom _permission = new();
		private allFilter filter = new();
		List<CustomerCustom>? customerImportList = new();

		private SaleCustom saleCurrentModel = new();
		private SaleCustom saleImportModel = new();

		//ใส่กรณี clear file แล้ว input ไม่ update
		private string _inputFileId = Guid.NewGuid().ToString();
		private bool bClearInputFile = false;
		private Modal modalResult = default!;
		List<ResultImport> resultImport = new();
		private string dropClass = "";
		private string? _fileName = null;
		private List<string> header_list_key = new();

		private Modal modalVersion = default!;

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

		//protected async Task OnChooseFile(InputFileChangeEventArgs inputFileChangeEvent)
		//{
		//	_fileName = null;
		//	dropClass = "";
		//	_errorMessage = null;
		//	CustomerList = null;
		//	var file = inputFileChangeEvent.File;

		//	int _SizeLimit = 10; //MB

		//	int TenMegaBytes = _SizeLimit * 1024 * 1024;
		//	var fileSize = file.Size;
		//	if (fileSize > TenMegaBytes)
		//	{
		//		ClearInputFile();
		//		_errorMessage = $"Limited Max. {_SizeLimit} MB per file.";
		//		await _jsRuntimes.InvokeVoidAsync("WarningAlert", _errorMessage);
		//	}
		//	else
		//	{
		//		try
		//		{
		//			using (var stream = file.OpenReadStream(TenMegaBytes))
		//			{
		//				MemoryStream ms = new MemoryStream();
		//				await stream.CopyToAsync(ms);
		//				stream.Close();

		//				var bytefile = ms.ToArray();
		//				IFormFile files = new FormFile(ms, 0, bytefile.Length, "name", "CustomerLoan.xlsx");
		//				if (files == null) throw new Exception("File Not Support.");

		//				string folderName = @$"{_appSet.Value.ContentRootPath}\import\excel";

		//				if (files.Length > 0)
		//				{
		//					_fileName = files.FileName;
		//					string sFileExtension = Path.GetExtension(files.FileName).ToLower();
		//					if (sFileExtension != ".xls" && sFileExtension != ".xlsx" && sFileExtension != ".csv")
		//						throw new Exception("FileExtension Not Support.");

		//					ISheet sheet;
		//					string fullPath = Path.Combine(folderName, files.FileName);
		//					using (var streamread = new FileStream(fullPath, FileMode.Create))
		//					{
		//						files.CopyTo(streamread);
		//						streamread.Position = 0;
		//						int sheetCount = 0;
		//						if (sFileExtension == ".xls")
		//						{
		//							throw new Exception("not support  Excel 97-2000 formats.");
		//						}

		//						XSSFWorkbook hssfwb = new XSSFWorkbook(streamread); //This will read 2007 Excel format  
		//						sheetCount = hssfwb.NumberOfSheets;
		//						sheet = hssfwb.GetSheetAt(0);

		//						IRow row_header = sheet.GetRow(0);

		//						try
		//						{
		//							var row_0 = row_header.GetCell(0).ToString()?.Trim();
		//							var row_1 = row_header.GetCell(1).ToString()?.Trim();
		//							if (row_0 != "วันที่เข้ามาติดต่อ"
		//								|| row_1 != "ช่องทางการติดต่อ")
		//							{
		//								throw new Exception("Template file not support.");
		//							}
		//						}
		//						catch
		//						{
		//							throw new Exception("Template file not support.");
		//						}

		//						DateTime? DateContact = null;
		//						Guid? Master_ContactChannelId = null;
		//						Guid? Branch_RegionId = null;
		//						string? ProvincialOffice = null;
		//						string? EmployeeName = null;
		//						string? EmployeeId = null;
		//						string? ContactName = null;
		//						string? ContactTel = null;
		//						string? CompanyName = null;
		//						string? JuristicPersonRegNumber = null;
		//						Guid? Master_BusinessTypeId = null;
		//						Guid? Master_BusinessSizeId = null;
		//						Guid? Master_ISICCodeId = null;
		//						Guid? Master_YieldId = null;
		//						Guid? Master_ChainId = null;
		//						Guid? Master_LoanTypeId = null;
		//						string? CompanyEmail = null;
		//						string? CompanyTel = null;
		//						string? ParentCompanyGroup = null;
		//						string? HouseNo = null;
		//						int? VillageNo = null;
		//						int? ProvinceId = null;
		//						int? AmphurId = null;
		//						int? TambolId = null;
		//						string? ZipCode = null;
		//						List<Customer_CommitteeCustom>? Customer_Committees = null;
		//						List<Customer_ShareholderCustom>? Customer_Shareholders = null;
		//						DateTime? ShareholderMeetDay = null;
		//						string? RegisteredCapital = null;
		//						string? CreditScore = null;
		//						string? FiscalYear = null;
		//						DateTime? StatementDate = null;
		//						string? TradeAccReceivable = null;
		//						string? TradeAccRecProceedsNet = null;
		//						string? Inventories = null;
		//						decimal? LoansShort = null;
		//						decimal? TotalCurrentAssets = null;
		//						decimal? LoansLong = null;
		//						decimal? LandBuildingEquipment = null;
		//						decimal? TotalNotCurrentAssets = null;
		//						decimal? AssetsTotal = null;
		//						string? TradeAccPay = null;
		//						decimal? TradeAccPayLoansShot = null;
		//						decimal? TradeAccPayTotalCurrentLia = null;
		//						decimal? TradeAccPayLoansLong = null;
		//						decimal? TradeAccPayTotalNotCurrentLia = null;
		//						decimal? TradeAccPayForLoansShot = null;
		//						decimal? TradeAccPayTotalLiabilitie = null;
		//						decimal? RegisterCapitalOrdinary = null;
		//						decimal? RegisterCapitalPaid = null;
		//						decimal? ProfitLossAccumulate = null;
		//						decimal? TotalShareholders = null;
		//						decimal? TotalLiabilitieShareholders = null;
		//						decimal? TotalIncome = null;
		//						decimal? CostSales = null;
		//						decimal? GrossProfit = null;
		//						decimal? OperatingExpenses = null;
		//						decimal? ProfitLossBeforeDepExp = null;
		//						decimal? ProfitLossBeforeInterestTax = null;
		//						decimal? NetProfitLoss = null;
		//						string? InterestNote = null;

		//						Dictionary<string, int> header_list =
		//												row_header.Cells
		//														  .Select(x => new { x.StringCellValue, x.ColumnIndex })
		//														  .ToDictionary(x => x.StringCellValue, x => x.ColumnIndex);

		//						header_list_key = header_list.Select(x => x.Key).ToList();
		//						if (header_list_key.Count < 60)
		//						{
		//							throw new Exception("Template file not support.");
		//						}

		//						for (var rowIndex = 1; rowIndex <= sheet.LastRowNum; rowIndex++)
		//						{

		//							DateContact = null;
		//							Master_ContactChannelId = null;
		//							Branch_RegionId = null;
		//							ProvincialOffice = null;
		//							EmployeeName = null;
		//							EmployeeId = null;
		//							ContactName = null;
		//							ContactTel = null;
		//							CompanyName = null;
		//							JuristicPersonRegNumber = null;
		//							Master_BusinessTypeId = null;
		//							Master_BusinessSizeId = null;
		//							Master_ISICCodeId = null;
		//							Master_YieldId = null;
		//							Master_ChainId = null;
		//							Master_LoanTypeId = null;
		//							CompanyEmail = null;
		//							CompanyTel = null;
		//							ParentCompanyGroup = null;
		//							HouseNo = null;
		//							VillageNo = null;
		//							ProvinceId = null;
		//							AmphurId = null;
		//							TambolId = null;
		//							ZipCode = null;
		//							Customer_Committees = new();
		//							Customer_Shareholders = new();
		//							ShareholderMeetDay = null;
		//							RegisteredCapital = null;
		//							CreditScore = null;
		//							FiscalYear = null;
		//							StatementDate = null;
		//							TradeAccReceivable = null;
		//							TradeAccRecProceedsNet = null;
		//							Inventories = null;
		//							LoansShort = null;
		//							TotalCurrentAssets = null;
		//							LoansLong = null;
		//							LandBuildingEquipment = null;
		//							TotalNotCurrentAssets = null;
		//							AssetsTotal = null;
		//							TradeAccPay = null;
		//							TradeAccPayLoansShot = null;
		//							TradeAccPayTotalCurrentLia = null;
		//							TradeAccPayLoansLong = null;
		//							TradeAccPayTotalNotCurrentLia = null;
		//							TradeAccPayForLoansShot = null;
		//							TradeAccPayTotalLiabilitie = null;
		//							RegisterCapitalOrdinary = null;
		//							RegisterCapitalPaid = null;
		//							ProfitLossAccumulate = null;
		//							TotalShareholders = null;
		//							TotalLiabilitieShareholders = null;
		//							TotalIncome = null;
		//							CostSales = null;
		//							GrossProfit = null;
		//							OperatingExpenses = null;
		//							ProfitLossBeforeDepExp = null;
		//							ProfitLossBeforeInterestTax = null;
		//							NetProfitLoss = null;
		//							InterestNote = null;

		//							var row = sheet.GetRow(rowIndex);
		//							int cellIndex = 0;
		//							int idMaster = 0;
		//							Guid guidMaster = Guid.Empty;
		//							DateTime dateTimeMaster = DateTime.MinValue;
		//							decimal decimalMaster = 0;

		//							if (header_list.TryGetValue("วันที่เข้ามาติดต่อ", out cellIndex))
		//							{
		//								if (DateTime.TryParse(row.GetCell(cellIndex).ToString(), out dateTimeMaster))
		//								{
		//									DateContact = dateTimeMaster;
		//								}
		//							}
		//							if (header_list.TryGetValue("ช่องทางการติดต่อ", out cellIndex))
		//							{
		//								if (Guid.TryParse(row.GetCell(cellIndex).ToString(), out guidMaster))
		//								{
		//									Master_ContactChannelId = guidMaster;
		//								}
		//							}
		//							if (header_list.TryGetValue("กิจการสาขาภาค", out cellIndex))
		//							{
		//								if (Guid.TryParse(row.GetCell(cellIndex).ToString(), out guidMaster))
		//								{
		//									Branch_RegionId = guidMaster;
		//								}
		//							}
		//							if (header_list.TryGetValue("สนจ.", out cellIndex))
		//							{
		//								ProvincialOffice = row.GetCell(cellIndex).ToString();
		//							}
		//							if (header_list.TryGetValue("ชื่อพนักงาน", out cellIndex))
		//							{
		//								EmployeeName = row.GetCell(cellIndex).ToString();
		//							}
		//							if (header_list.TryGetValue("รหัสพนักงาน", out cellIndex))
		//							{
		//								EmployeeId = row.GetCell(cellIndex).ToString();
		//							}
		//							if (header_list.TryGetValue("ชื่อผู้ติดต่อ", out cellIndex))
		//							{
		//								ContactName = row.GetCell(cellIndex).ToString();
		//							}
		//							if (header_list.TryGetValue("โทรศัพท์", out cellIndex))
		//							{
		//								ContactTel = row.GetCell(cellIndex).ToString();
		//							}
		//							if (header_list.TryGetValue("ชื่อบริษัท", out cellIndex))
		//							{
		//								CompanyName = row.GetCell(cellIndex).ToString();
		//							}
		//							if (header_list.TryGetValue("เลขนิติบุคคล", out cellIndex))
		//							{
		//								JuristicPersonRegNumber = row.GetCell(cellIndex).ToString();
		//							}
		//							if (header_list.TryGetValue("ประเภทธุรกิจ", out cellIndex))
		//							{
		//								if (Guid.TryParse(row.GetCell(cellIndex).ToString(), out guidMaster))
		//								{
		//									Master_BusinessTypeId = guidMaster;
		//								}
		//							}
		//							if (header_list.TryGetValue("ขนาดธุรกิจ", out cellIndex))
		//							{
		//								if (Guid.TryParse(row.GetCell(cellIndex).ToString(), out guidMaster))
		//								{
		//									Master_BusinessSizeId = guidMaster;
		//								}
		//							}
		//							if (header_list.TryGetValue("ISIC Code", out cellIndex))
		//							{
		//								if (Guid.TryParse(row.GetCell(cellIndex).ToString(), out guidMaster))
		//								{
		//									Master_ISICCodeId = guidMaster;
		//								}
		//							}
		//							if (header_list.TryGetValue("ผลผลิตหลัก", out cellIndex))
		//							{
		//								if (Guid.TryParse(row.GetCell(cellIndex).ToString(), out guidMaster))
		//								{
		//									Master_YieldId = guidMaster;
		//								}
		//							}
		//							if (header_list.TryGetValue("ห่วงโซ่คุณค่า", out cellIndex))
		//							{
		//								if (Guid.TryParse(row.GetCell(cellIndex).ToString(), out guidMaster))
		//								{
		//									Master_ChainId = guidMaster;
		//								}
		//							}
		//							if (header_list.TryGetValue("ประเภทสินเชื่อ", out cellIndex))
		//							{
		//								if (Guid.TryParse(row.GetCell(cellIndex).ToString(), out guidMaster))
		//								{
		//									Master_LoanTypeId = guidMaster;
		//								}
		//							}
		//							if (header_list.TryGetValue("อีเมล", out cellIndex))
		//							{
		//								CompanyEmail = row.GetCell(cellIndex).ToString();
		//							}
		//							if (header_list.TryGetValue("โทรศัพท์", out cellIndex))
		//							{
		//								CompanyTel = row.GetCell(cellIndex).ToString();
		//							}
		//							if (header_list.TryGetValue("กลุ่มบริษัทแม่", out cellIndex))
		//							{
		//								ParentCompanyGroup = row.GetCell(cellIndex).ToString();
		//							}
		//							if (header_list.TryGetValue("บ้านเลขที่", out cellIndex))
		//							{
		//								HouseNo = row.GetCell(cellIndex).ToString();
		//							}
		//							if (header_list.TryGetValue("หมู่ที่", out cellIndex))
		//							{
		//								if (int.TryParse(row.GetCell(cellIndex).ToString(), out idMaster))
		//								{
		//									VillageNo = idMaster;
		//								}
		//							}
		//							if (header_list.TryGetValue("จังหวัด", out cellIndex))
		//							{
		//								if (int.TryParse(row.GetCell(cellIndex).ToString(), out idMaster))
		//								{
		//									ProvinceId = idMaster;
		//								}
		//							}
		//							if (header_list.TryGetValue("เขต/อำเภอ", out cellIndex))
		//							{
		//								if (int.TryParse(row.GetCell(cellIndex).ToString(), out idMaster))
		//								{
		//									AmphurId = idMaster;
		//								}
		//							}
		//							if (header_list.TryGetValue("แขวง/ตำบล", out cellIndex))
		//							{
		//								if (int.TryParse(row.GetCell(cellIndex).ToString(), out idMaster))
		//								{
		//									TambolId = idMaster;
		//								}
		//							}
		//							if (header_list.TryGetValue("รหัสไปรษณีย์", out cellIndex))
		//							{
		//								ZipCode = row.GetCell(cellIndex).ToString();
		//							}

		//							int i = 1;
		//							while (header_list.Keys.Any(key => key.Equals($"ชื่อกรรมการ{i}")))
		//							{
		//								if (header_list.TryGetValue($"ชื่อกรรมการ{i}", out cellIndex))
		//								{
		//									Customer_Committees.Add(new() { Name = row.GetCell(cellIndex).ToString() });
		//								}
		//								i++;
		//							}

		//							i = 1;
		//							while (header_list.Keys.Any(key => key.Equals($"ชื่อผู้ถือหุ้น{i}")))
		//							{
		//								string? _Name = null;
		//								string? _Nationality = null;
		//								string? _Proportion = null;
		//								int? _NumberShareholder = null;
		//								decimal? _TotalShareValue = null;
		//								if (header_list.TryGetValue($"ชื่อผู้ถือหุ้น{i}", out cellIndex))
		//								{
		//									_Name = row.GetCell(cellIndex).ToString();
		//								}
		//								if (header_list.TryGetValue($"สัญชาติ{i}", out cellIndex))
		//								{
		//									_Nationality = row.GetCell(cellIndex).ToString();
		//								}
		//								if (header_list.TryGetValue($"สัดส่วนการถือหุ้น{i}", out cellIndex))
		//								{
		//									_Proportion = row.GetCell(cellIndex).ToString();
		//								}
		//								if (header_list.TryGetValue($"จำนวนหุ้นที่ถือ{i}", out cellIndex))
		//								{
		//									if (int.TryParse(row.GetCell(cellIndex).ToString(), out idMaster))
		//									{
		//										_NumberShareholder = idMaster;
		//									}
		//								}
		//								if (header_list.TryGetValue($"มูลค่าหุ้นทั้งหมด{i}", out cellIndex))
		//								{
		//									if (decimal.TryParse(row.GetCell(cellIndex).ToString(), out decimalMaster))
		//									{
		//										_TotalShareValue = decimalMaster;
		//									}
		//								}
		//								Customer_Shareholders.Add(new()
		//								{
		//									Name = _Name,
		//									Nationality = _Nationality,
		//									Proportion = _Proportion,
		//									NumberShareholder = _NumberShareholder,
		//									TotalShareValue = _TotalShareValue
		//								});
		//								i++;
		//							}

		//							if (header_list.TryGetValue("วันประชุมผู้ถือหุ้น", out cellIndex))
		//							{
		//								if (DateTime.TryParse(row.GetCell(cellIndex).ToString(), out dateTimeMaster))
		//								{
		//									ShareholderMeetDay = dateTimeMaster;
		//								}
		//							}
		//							if (header_list.TryGetValue("ทุนจดทะเบียน", out cellIndex))
		//							{
		//								RegisteredCapital = row.GetCell(cellIndex).ToString();
		//							}
		//							if (header_list.TryGetValue("Credit Score", out cellIndex))
		//							{
		//								CreditScore = row.GetCell(cellIndex).ToString();
		//							}
		//							if (header_list.TryGetValue("ปีงบการเงิน", out cellIndex))
		//							{
		//								FiscalYear = row.GetCell(cellIndex).ToString();
		//							}
		//							if (header_list.TryGetValue("วันเดือนปีงบการเงิน", out cellIndex))
		//							{
		//								if (DateTime.TryParse(row.GetCell(cellIndex).ToString(), out dateTimeMaster))
		//								{
		//									StatementDate = dateTimeMaster;
		//								}
		//							}
		//							if (header_list.TryGetValue("ลูกหนี้การค้า", out cellIndex))
		//							{
		//								TradeAccReceivable = row.GetCell(cellIndex).ToString();
		//							}
		//							if (header_list.TryGetValue("ลูกหนี้การค้าและตั่วเงินรับ-สุทธิ", out cellIndex))
		//							{
		//								TradeAccRecProceedsNet = row.GetCell(cellIndex).ToString();
		//							}
		//							if (header_list.TryGetValue("สินค้าคงเหลือ", out cellIndex))
		//							{
		//								Inventories = row.GetCell(cellIndex).ToString();
		//							}
		//							if (header_list.TryGetValue("เงินให้กู้ยืมระยะสั้น(ลูกหนี้)", out cellIndex))
		//							{
		//								if (decimal.TryParse(row.GetCell(cellIndex).ToString(), out decimalMaster))
		//								{
		//									LoansShort = decimalMaster;
		//								}
		//							}
		//							if (header_list.TryGetValue("รวมสินทรัพย์หมุนเวียน", out cellIndex))
		//							{
		//								if (decimal.TryParse(row.GetCell(cellIndex).ToString(), out decimalMaster))
		//								{
		//									TotalCurrentAssets = decimalMaster;
		//								}
		//							}
		//							if (header_list.TryGetValue("เงินให้กู้ยืมระยะยาว", out cellIndex))
		//							{
		//								if (decimal.TryParse(row.GetCell(cellIndex).ToString(), out decimalMaster))
		//								{
		//									LoansLong = decimalMaster;
		//								}
		//							}
		//							if (header_list.TryGetValue("ที่ดิน อาคาร และอุปกรณ์", out cellIndex))
		//							{
		//								if (decimal.TryParse(row.GetCell(cellIndex).ToString(), out decimalMaster))
		//								{
		//									LandBuildingEquipment = decimalMaster;
		//								}
		//							}
		//							if (header_list.TryGetValue("รวมสินทรัพย์ไม่หมุนเวียน", out cellIndex))
		//							{
		//								if (decimal.TryParse(row.GetCell(cellIndex).ToString(), out decimalMaster))
		//								{
		//									TotalNotCurrentAssets = decimalMaster;
		//								}
		//							}
		//							if (header_list.TryGetValue("รวมสินทรัพย์", out cellIndex))
		//							{
		//								if (decimal.TryParse(row.GetCell(cellIndex).ToString(), out decimalMaster))
		//								{
		//									AssetsTotal = decimalMaster;
		//								}
		//							}
		//							if (header_list.TryGetValue("เจ้าหนี้การค้า", out cellIndex))
		//							{
		//								TradeAccPay = row.GetCell(cellIndex).ToString();
		//							}
		//							if (header_list.TryGetValue("เงินกู้ระยะสั้น", out cellIndex))
		//							{
		//								if (decimal.TryParse(row.GetCell(cellIndex).ToString(), out decimalMaster))
		//								{
		//									TradeAccPayLoansShot = decimalMaster;
		//								}
		//							}
		//							if (header_list.TryGetValue("รวมหนี้สินหมุนเวียน", out cellIndex))
		//							{
		//								if (decimal.TryParse(row.GetCell(cellIndex).ToString(), out decimalMaster))
		//								{
		//									TradeAccPayTotalCurrentLia = decimalMaster;
		//								}
		//							}
		//							if (header_list.TryGetValue("เงินกู้ระยะยาว", out cellIndex))
		//							{
		//								if (decimal.TryParse(row.GetCell(cellIndex).ToString(), out decimalMaster))
		//								{
		//									TradeAccPayLoansLong = decimalMaster;
		//								}
		//							}
		//							if (header_list.TryGetValue("รวมหนี้สินไม่หมุนเวียน", out cellIndex))
		//							{
		//								if (decimal.TryParse(row.GetCell(cellIndex).ToString(), out decimalMaster))
		//								{
		//									TradeAccPayTotalNotCurrentLia = decimalMaster;
		//								}
		//							}
		//							if (header_list.TryGetValue("เงินให้กู้ยืมระยะสั้น(เจ้าหนี้)", out cellIndex))
		//							{
		//								if (decimal.TryParse(row.GetCell(cellIndex).ToString(), out decimalMaster))
		//								{
		//									TradeAccPayForLoansShot = decimalMaster;
		//								}
		//							}
		//							if (header_list.TryGetValue("รวมหนี้สิน", out cellIndex))
		//							{
		//								if (decimal.TryParse(row.GetCell(cellIndex).ToString(), out decimalMaster))
		//								{
		//									TradeAccPayTotalLiabilitie = decimalMaster;
		//								}
		//							}
		//							if (header_list.TryGetValue("ทุนจดทะเบียนสามัญ", out cellIndex))
		//							{
		//								if (decimal.TryParse(row.GetCell(cellIndex).ToString(), out decimalMaster))
		//								{
		//									RegisterCapitalOrdinary = decimalMaster;
		//								}
		//							}
		//							if (header_list.TryGetValue("ทุนจดทะเบียนที่ชำระแล้ว", out cellIndex))
		//							{
		//								if (decimal.TryParse(row.GetCell(cellIndex).ToString(), out decimalMaster))
		//								{
		//									RegisterCapitalPaid = decimalMaster;
		//								}
		//							}
		//							if (header_list.TryGetValue("กำไร (ขาดทุน) สะสม", out cellIndex))
		//							{
		//								if (decimal.TryParse(row.GetCell(cellIndex).ToString(), out decimalMaster))
		//								{
		//									ProfitLossAccumulate = decimalMaster;
		//								}
		//							}
		//							if (header_list.TryGetValue("รวมส่วนของผู้ถือหุ้น", out cellIndex))
		//							{
		//								if (decimal.TryParse(row.GetCell(cellIndex).ToString(), out decimalMaster))
		//								{
		//									TotalShareholders = decimalMaster;
		//								}
		//							}
		//							if (header_list.TryGetValue("รวมหนี้สินและส่วนของผู้ถือหุ้น", out cellIndex))
		//							{
		//								if (decimal.TryParse(row.GetCell(cellIndex).ToString(), out decimalMaster))
		//								{
		//									TotalLiabilitieShareholders = decimalMaster;
		//								}
		//							}
		//							if (header_list.TryGetValue("รายได้รวม", out cellIndex))
		//							{
		//								if (decimal.TryParse(row.GetCell(cellIndex).ToString(), out decimalMaster))
		//								{
		//									TotalIncome = decimalMaster;
		//								}
		//							}
		//							if (header_list.TryGetValue("ต้นทุนขาย", out cellIndex))
		//							{
		//								if (decimal.TryParse(row.GetCell(cellIndex).ToString(), out decimalMaster))
		//								{
		//									CostSales = decimalMaster;
		//								}
		//							}
		//							if (header_list.TryGetValue("กำไรขั้นต้น", out cellIndex))
		//							{
		//								if (decimal.TryParse(row.GetCell(cellIndex).ToString(), out decimalMaster))
		//								{
		//									GrossProfit = decimalMaster;
		//								}
		//							}
		//							if (header_list.TryGetValue("ค่าใช้จ่ายในการดำเนินงาน", out cellIndex))
		//							{
		//								if (decimal.TryParse(row.GetCell(cellIndex).ToString(), out decimalMaster))
		//								{
		//									OperatingExpenses = decimalMaster;
		//								}
		//							}
		//							if (header_list.TryGetValue("กำไร (ขาดทุน) ก่อนหักค่าเสื่อมและค่าใช้จ่าย", out cellIndex))
		//							{
		//								if (decimal.TryParse(row.GetCell(cellIndex).ToString(), out decimalMaster))
		//								{
		//									ProfitLossBeforeDepExp = decimalMaster;
		//								}
		//							}
		//							if (header_list.TryGetValue("กำไร (ขาดทุน) ก่อนหักดอกเบี้ยและภาษี", out cellIndex))
		//							{
		//								if (decimal.TryParse(row.GetCell(cellIndex).ToString(), out decimalMaster))
		//								{
		//									ProfitLossBeforeInterestTax = decimalMaster;
		//								}
		//							}
		//							if (header_list.TryGetValue("กำไร (ขาดทุน) สุทธิ", out cellIndex))
		//							{
		//								if (decimal.TryParse(row.GetCell(cellIndex).ToString(), out decimalMaster))
		//								{
		//									NetProfitLoss = decimalMaster;
		//								}
		//							}
		//							if (header_list.TryGetValue("หมายเหตุ", out cellIndex))
		//							{
		//								InterestNote = row.GetCell(cellIndex).ToString();
		//							}

		//							if (CustomerList == null) CustomerList = new();

		//							if (CustomerList.Select(x => x.JuristicPersonRegNumber).Any(x => x == JuristicPersonRegNumber))
		//								throw new Exception("มีเลขนิติบุคคลซ้ำ กรุณาตรวจสอบอีกครั้ง");

		//							if (string.IsNullOrEmpty(JuristicPersonRegNumber))
		//								throw new Exception("ระบุเลขนิติบุคคลไม่ครบ");

		//							if (JuristicPersonRegNumber != null && JuristicPersonRegNumber.Length != 13)
		//								throw new Exception("ระบุเลขนิติบุคคลไม่ครบ 13 หลัก");


		//							CustomerList.Add(new()
		//							{
		//								DateContact = DateContact,
		//								Master_ContactChannelId = Master_ContactChannelId,
		//								Branch_RegionId = Branch_RegionId,
		//								ProvincialOffice = ProvincialOffice,
		//								EmployeeName = EmployeeName,
		//								EmployeeId = EmployeeId,
		//								ContactName = ContactName,
		//								ContactTel = ContactTel,
		//								JuristicPersonRegNumber = JuristicPersonRegNumber,
		//								CompanyName = CompanyName,
		//								Master_BusinessTypeId = Master_BusinessTypeId,
		//								Master_BusinessSizeId = Master_BusinessSizeId,
		//								Master_ISICCodeId = Master_ISICCodeId,
		//								Master_YieldId = Master_YieldId,
		//								Master_ChainId = Master_ChainId,
		//								Master_LoanTypeId = Master_LoanTypeId,
		//								CompanyEmail = CompanyEmail,
		//								CompanyTel = CompanyTel,
		//								ParentCompanyGroup = ParentCompanyGroup,
		//								HouseNo = HouseNo,
		//								VillageNo = VillageNo,
		//								ProvinceId = ProvinceId,
		//								AmphurId = AmphurId,
		//								TambolId = TambolId,
		//								ZipCode = ZipCode,
		//								Customer_Committees = Customer_Committees,
		//								ShareholderMeetDay = ShareholderMeetDay,
		//								Customer_Shareholders = Customer_Shareholders,
		//								RegisteredCapital = RegisteredCapital,
		//								CreditScore = CreditScore,
		//								FiscalYear = FiscalYear,
		//								StatementDate = StatementDate,
		//								TradeAccReceivable = TradeAccReceivable,
		//								TradeAccRecProceedsNet = TradeAccRecProceedsNet,
		//								Inventories = Inventories,
		//								LoansShort = LoansShort,
		//								TotalCurrentAssets = TotalCurrentAssets,
		//								LoansLong = LoansLong,
		//								LandBuildingEquipment = LandBuildingEquipment,
		//								TotalNotCurrentAssets = TotalNotCurrentAssets,
		//								AssetsTotal = AssetsTotal,
		//								TradeAccPay = TradeAccPay,
		//								TradeAccPayLoansShot = TradeAccPayLoansShot,
		//								TradeAccPayTotalCurrentLia = TradeAccPayTotalCurrentLia,
		//								TradeAccPayLoansLong = TradeAccPayLoansLong,
		//								TradeAccPayTotalNotCurrentLia = TradeAccPayTotalNotCurrentLia,
		//								TradeAccPayForLoansShot = TradeAccPayForLoansShot,
		//								TradeAccPayTotalLiabilitie = TradeAccPayTotalLiabilitie,
		//								RegisterCapitalOrdinary = RegisterCapitalOrdinary,
		//								RegisterCapitalPaid = RegisterCapitalPaid,
		//								ProfitLossAccumulate = ProfitLossAccumulate,
		//								TotalShareholders = TotalShareholders,
		//								TotalLiabilitieShareholders = TotalLiabilitieShareholders,
		//								TotalIncome = TotalIncome,
		//								CostSales = CostSales,
		//								GrossProfit = GrossProfit,
		//								OperatingExpenses = OperatingExpenses,
		//								ProfitLossBeforeDepExp = ProfitLossBeforeDepExp,
		//								ProfitLossBeforeInterestTax = ProfitLossBeforeInterestTax,
		//								NetProfitLoss = NetProfitLoss,
		//								InterestNote = InterestNote
		//							});
		//						}

		//						if (CustomerList?.Count > 0)
		//						{
		//							var response = await _customerViewModel.ValidateUpload(CustomerList);

		//							if (response.Status)
		//							{
		//								CustomerList = response.Data?.OrderBy(x => x.IsValidate == true).ToList();
		//							}
		//							else
		//							{
		//								HideLoading();
		//								_errorMessage = response.errorMessage;
		//								await _jsRuntimes.InvokeVoidAsync("WarningAlert", _errorMessage);
		//							}
		//						}
		//					}
		//				}
		//			}
		//		}
		//		catch (Exception ex)
		//		{
		//			ClearInputFile();
		//			_errorMessage = ex.Message;
		//			await _jsRuntimes.InvokeVoidAsync("WarningAlert", _errorMessage);
		//		}
		//	}

		//}

		protected async Task OnChooseFileTemplateFCC(InputFileChangeEventArgs inputFileChangeEvent)
		{
			_fileName = null;
			dropClass = "";
			_errorMessage = null;
			customerImportList = null;
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

								IRow row_header = sheet.GetRow(1);

								try
								{
									var row_0 = row_header.GetCell(0).ToString()?.Trim();
									var row_1 = row_header.GetCell(1).ToString()?.Trim();
									var row_2 = row_header.GetCell(2).ToString()?.Trim();
									if (row_0 != "เลขทะเบียนนิติบุคคล" || row_1 != "CIF" || row_2 != "ชื่อบริษัท")
									{
										throw new Exception("Template file not support.");
									}
								}
								catch
								{
									throw new Exception("Template file not support.");
								}

								DateTime? DateContact = null;
								Guid? Master_ContactChannelId = null;
								Guid? Branch_RegionId = null;
								string? ProvincialOffice = null;
								string? EmployeeName = null;
								string? EmployeeId = null;
								string? CIF = null;
								string? ContactName = null;
								string? ContactTel = null;
								string? JuristicPersonRegNumber = null;
								string? CompanyName = null;
								Guid? Master_BusinessTypeId = null;
								string? Master_BusinessTypeName = null;
								Guid? Master_BusinessSizeId = null;
								string? Master_BusinessSizeName = null;
								Guid? Master_ISICCodeId = null;
								Guid? Master_TSICId = null;
								string? Master_TSICName = null;
								Guid? Master_YieldId = null;
								Guid? Master_ChainId = null;
								Guid? Master_LoanTypeId = null;
								string? CompanyEmail = null;
								string? CompanyTel = null;
								string? ParentCompanyGroup = null;
								string? HouseNo = null;
								int? VillageNo = null;
								string? Road_Soi_Village = null;
								int? ProvinceId = null;
								int? AmphurId = null;
								int? TambolId = null;
								string? ProvinceName = null;
								string? AmphurName = null;
								string? TambolName = null;
								string? ZipCode = null;
								List<Customer_CommitteeCustom>? Customer_Committees = null;
								List<Customer_ShareholderCustom>? Customer_Shareholders = null;
								DateTime? ShareholderMeetDay = null;
								string? RegisteredCapital = null;
								string? CreditScore = null;
								string? FiscalYear = null;
								DateTime? StatementDate = null;
								decimal? TradeAccReceivable = null;
								decimal? TradeAccRecProceedsNet = null;
								decimal? Inventories = null;
								decimal? LoansShort = null;
								decimal? TotalCurrentAssets = null;
								decimal? LoansLong = null;
								decimal? LandBuildingEquipment = null;
								decimal? TotalNotCurrentAssets = null;
								decimal? AssetsTotal = null;
								decimal? TradeAccPay = null;
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
								decimal? ProfitLossBeforeIncomeTaxExpense = null;
								decimal? NetProfitLoss = null;
								string? InterestNote = null;

								Dictionary<string, int> header_list =
														row_header.Cells
																  .Select(x => new { x.StringCellValue, x.ColumnIndex })
																  .ToDictionary(x => x.StringCellValue, x => x.ColumnIndex);

								header_list_key = header_list.Select(x => x.Key.Trim()).ToList();
								if (header_list_key.Count < 150)
								{
									throw new Exception("Template file not support.");
								}

								for (var rowIndex = 2; rowIndex <= sheet.LastRowNum; rowIndex++)
								{

									DateContact = null;
									Master_ContactChannelId = null;
									Branch_RegionId = null;
									ProvincialOffice = null;
									EmployeeName = null;
									EmployeeId = null;
									CIF = null;
									ContactName = null;
									ContactTel = null;
									JuristicPersonRegNumber = null;
									CompanyName = null;
									Master_BusinessTypeId = null;
									Master_BusinessTypeName = null;
									Master_BusinessSizeId = null;
									Master_BusinessSizeName = null;
									Master_ISICCodeId = null;
									Master_TSICId = null;
									Master_TSICName = null;
									Master_TSICId = null;
									Master_YieldId = null;
									Master_ChainId = null;
									Master_LoanTypeId = null;
									CompanyEmail = null;
									CompanyTel = null;
									ParentCompanyGroup = null;
									HouseNo = null;
									VillageNo = null;
									Road_Soi_Village = null;
									ProvinceId = null;
									AmphurId = null;
									TambolId = null;
									ProvinceName = null;
									AmphurName = null;
									TambolName = null;
									ZipCode = null;
									Customer_Committees = new();
									Customer_Shareholders = new();
									ShareholderMeetDay = null;
									RegisteredCapital = null;
									CreditScore = null;
									FiscalYear = null;
									StatementDate = null;
									TradeAccReceivable = null;
									TradeAccRecProceedsNet = null;
									Inventories = null;
									LoansShort = null;
									TotalCurrentAssets = null;
									LoansLong = null;
									LandBuildingEquipment = null;
									TotalNotCurrentAssets = null;
									AssetsTotal = null;
									TradeAccPay = null;
									TradeAccPayLoansShot = null;
									TradeAccPayTotalCurrentLia = null;
									TradeAccPayLoansLong = null;
									TradeAccPayTotalNotCurrentLia = null;
									TradeAccPayForLoansShot = null;
									TradeAccPayTotalLiabilitie = null;
									RegisterCapitalOrdinary = null;
									RegisterCapitalPaid = null;
									ProfitLossAccumulate = null;
									TotalShareholders = null;
									TotalLiabilitieShareholders = null;
									TotalIncome = null;
									CostSales = null;
									GrossProfit = null;
									OperatingExpenses = null;
									ProfitLossBeforeDepExp = null;
									ProfitLossBeforeInterestTax = null;
									ProfitLossBeforeIncomeTaxExpense = null;
									NetProfitLoss = null;
									InterestNote = null;

									var row = sheet.GetRow(rowIndex);
									int cellIndex = 0;
									int idMaster = 0;
									Guid guidMaster = Guid.Empty;
									DateTime dateTimeMaster = DateTime.MinValue;
									decimal decimalMaster = 0;

									//Default 6eaca010-3e6e-11ef-931d-30e37aef72fb = ข้อมูลจากระบบ
									Master_ContactChannelId = Guid.Parse("6eaca010-3e6e-11ef-931d-30e37aef72fb");
									EmployeeId = UserInfo.EmployeeId;
									EmployeeName = UserInfo.FullName;

									if (header_list.TryGetValue("เลขทะเบียนนิติบุคคล", out cellIndex))
									{
										JuristicPersonRegNumber = row.GetCell(cellIndex)?.ToString();
									}
									if (header_list.TryGetValue("CIF", out cellIndex))
									{
										CIF = row.GetCell(cellIndex)?.ToString();
									}
									if (header_list.TryGetValue("ชื่อบริษัท", out cellIndex))
									{
										CompanyName = row.GetCell(cellIndex)?.ToString();
									}
									if (header_list.TryGetValue("เลขที่", out cellIndex))
									{
										HouseNo = row.GetCell(cellIndex)?.ToString();
									}
									if (header_list.TryGetValue("หมู่", out cellIndex))
									{
										if (int.TryParse(row.GetCell(cellIndex)?.ToString(), out idMaster))
										{
											VillageNo = idMaster;
										}
									}
									if (header_list.TryGetValue("ถนน/ซอย/หมู่บ้าน", out cellIndex))
									{
										Road_Soi_Village = row.GetCell(cellIndex)?.ToString();
									}
									if (header_list.TryGetValue("ตำบล", out cellIndex))
									{
										TambolName = row.GetCell(cellIndex)?.ToString();
										//if (int.TryParse(row.GetCell(cellIndex).ToString(), out idMaster))
										//{
										//	TambolId = idMaster;
										//}
									}
									if (header_list.TryGetValue("อำเภอ", out cellIndex))
									{
										AmphurName = row.GetCell(cellIndex)?.ToString();
										//if (int.TryParse(row.GetCell(cellIndex).ToString(), out idMaster))
										//{
										//	AmphurId = idMaster;
										//}
									}
									if (header_list.TryGetValue("จังหวัด", out cellIndex))
									{
										ProvinceName = row.GetCell(cellIndex)?.ToString();
										//if (int.TryParse(row.GetCell(cellIndex).ToString(), out idMaster))
										//{
										//	ProvinceId = idMaster;
										//}
									}
									if (header_list.TryGetValue("รหัสไปรษณีย์", out cellIndex))
									{
										ZipCode = row.GetCell(cellIndex)?.ToString();
									}


									//กรรมการ
									int i = 1;
									while (header_list.Keys.Any(key => key.Equals($"กรรมการ คนที่ {i}")))
									{
										if (header_list.TryGetValue($"กรรมการ คนที่ {i}", out cellIndex))
										{
											string? _name = row.GetCell(cellIndex)?.ToString();
											//if (!string.IsNullOrEmpty(_name))
											//{
											Customer_Committees.Add(new() { Name = row.GetCell(cellIndex)?.ToString() });
											//}
										}
										i++;
									}

									if (header_list.TryGetValue("วันที่ประชุมผู้ถือหุ้น", out cellIndex))
									{
										if (DateTime.TryParse(row.GetCell(cellIndex)?.ToString(), out dateTimeMaster))
										{
											var ShareholderMeetDayStr = dateTimeMaster.ToString("dd/MM/yyyy");
											ShareholderMeetDay = GeneralUtils.DateToEn(ShareholderMeetDayStr);
											//ShareholderMeetDay = dateTimeMaster;
										}
									}

									//ผู้ถือหุ้น
									i = 1;
									while (header_list.Keys.Any(key => key.Equals($"ผู้ถือหุ้น คนที่ {i}")))
									{
										string? _Name = null;
										string? _Nationality = null;
										string? _Proportion = null;
										int? _NumberShareholder = null;
										decimal? _TotalShareValue = null;
										if (header_list.TryGetValue($"ผู้ถือหุ้น คนที่ {i}", out cellIndex))
										{
											_Name = row.GetCell(cellIndex)?.ToString();
										}

										//if (!string.IsNullOrEmpty(_Name))
										//{
										if (header_list.TryGetValue($"สัญชาติ คนที่ {i}", out cellIndex))
										{
											_Nationality = row.GetCell(cellIndex)?.ToString();
										}
										if (header_list.TryGetValue($"สัดส่วนการถือหุ้น คนที่ {i}", out cellIndex))
										{
											_Proportion = row.GetCell(cellIndex)?.ToString();
										}
										if (header_list.TryGetValue($"จำนวนหุ้นที่ถือ คนที่ {i}", out cellIndex))
										{
											if (int.TryParse(row.GetCell(cellIndex)?.ToString(), out idMaster))
											{
												_NumberShareholder = idMaster;
											}
										}
										if (header_list.TryGetValue($"มูลค่าหุ้นทั้งหมด คนที่ {i}", out cellIndex))
										{
											if (decimal.TryParse(row.GetCell(cellIndex)?.ToString(), out decimalMaster))
											{
												_TotalShareValue = decimalMaster;
											}
										}
										Customer_Shareholders.Add(new()
										{
											Name = _Name,
											Nationality = _Nationality,
											Proportion = _Proportion,
											NumberShareholder = _NumberShareholder,
											TotalShareValue = _TotalShareValue
										});
										//}

										i++;
									}

									if (header_list.TryGetValue("ประเภทกิจการ", out cellIndex))
									{
										Master_BusinessTypeName = row.GetCell(cellIndex)?.ToString()?.Trim();
										//if (Guid.TryParse(row.GetCell(cellIndex).ToString(), out guidMaster))
										//{
										//	Master_BusinessTypeId = guidMaster;
										//}
									}
									if (header_list.TryGetValue("ทุนจดทะเบียนล่าสุด", out cellIndex))
									{
										RegisteredCapital = row.GetCell(cellIndex)?.ToString();
									}
									if (header_list.TryGetValue("ขนาดธุรกิจ", out cellIndex))
									{
										Master_BusinessSizeName = row.GetCell(cellIndex)?.ToString()?.Trim();
										//if (Guid.TryParse(row.GetCell(cellIndex).ToString(), out guidMaster))
										//{
										//	Master_BusinessSizeId = guidMaster;
										//}
									}
									if (header_list.TryGetValue("ประเภทธุรกิจ (TSIC) รหัสที่ 1", out cellIndex))
									{
										Master_TSICName = row.GetCell(cellIndex)?.ToString()?.Trim();
										if (Master_TSICName?.Length >= 8)
										{
											Master_TSICName = Master_TSICName?.Substring(8);
										}
										//if (Guid.TryParse(row.GetCell(cellIndex).ToString(), out guidMaster))
										//{
										//	Master_TSICId = guidMaster;
										//}
									}

									if (header_list.TryGetValue("Credit Score", out cellIndex))
									{
										CreditScore = row.GetCell(cellIndex)?.ToString();
									}
									if (header_list.TryGetValue("ปีงบการเงิน", out cellIndex))
									{
										FiscalYear = row.GetCell(cellIndex)?.ToString();
									}
									if (header_list.TryGetValue("วันเดือนปีงบการเงิน", out cellIndex))
									{
										if (DateTime.TryParse(row.GetCell(cellIndex)?.ToString(), out dateTimeMaster))
										{
											var StatementDateStr = dateTimeMaster.ToString("dd/MM/yyyy");
											StatementDate = GeneralUtils.DateToEn(StatementDateStr);
											//ShareholderMeetDay = dateTimeMaster;
										}
									}
									if (header_list.TryGetValue("ลูกหนี้การค้า", out cellIndex))
									{
										if (decimal.TryParse(row.GetCell(cellIndex)?.ToString(), out decimalMaster))
										{
											TradeAccReceivable = decimalMaster;
										}
									}
									if (header_list.TryGetValue("ลูกหนี้การค้าและตั่วเงินรับ-สุทธิ", out cellIndex))
									{
										if (decimal.TryParse(row.GetCell(cellIndex)?.ToString(), out decimalMaster))
										{
											TradeAccRecProceedsNet = decimalMaster;
										}
									}
									if (header_list.TryGetValue("สินค้าคงเหลือ", out cellIndex))
									{
										if (decimal.TryParse(row.GetCell(cellIndex)?.ToString(), out decimalMaster))
										{
											Inventories = decimalMaster;
										}
									}
									if (header_list.TryGetValue("เงินให้กู้ยืมระยะสั้น(ลูกหนี้)", out cellIndex))
									{
										if (decimal.TryParse(row.GetCell(cellIndex)?.ToString(), out decimalMaster))
										{
											LoansShort = decimalMaster;
										}
									}
									if (header_list.TryGetValue("รวมสินทรัพย์หมุนเวียน", out cellIndex))
									{
										if (decimal.TryParse(row.GetCell(cellIndex)?.ToString(), out decimalMaster))
										{
											TotalCurrentAssets = decimalMaster;
										}
									}
									if (header_list.TryGetValue("เงินให้กู้ยืมระยะยาว", out cellIndex))
									{
										if (decimal.TryParse(row.GetCell(cellIndex)?.ToString(), out decimalMaster))
										{
											LoansLong = decimalMaster;
										}
									}
									if (header_list.TryGetValue("ที่ดิน,อาคาร และอุปกรณ์-สุทธิ", out cellIndex))
									{
										if (decimal.TryParse(row.GetCell(cellIndex)?.ToString(), out decimalMaster))
										{
											LandBuildingEquipment = decimalMaster;
										}
									}
									if (header_list.TryGetValue("รวมสินทรัพย์ไม่หมุนเวียน", out cellIndex))
									{
										if (decimal.TryParse(row.GetCell(cellIndex)?.ToString(), out decimalMaster))
										{
											TotalNotCurrentAssets = decimalMaster;
										}
									}
									if (header_list.TryGetValue("รวมสินทรัพย์", out cellIndex))
									{
										if (decimal.TryParse(row.GetCell(cellIndex)?.ToString(), out decimalMaster))
										{
											AssetsTotal = decimalMaster;
										}
									}
									if (header_list.TryGetValue("เจ้าหนี้การค้า", out cellIndex))
									{
										if (decimal.TryParse(row.GetCell(cellIndex)?.ToString(), out decimalMaster))
										{
											TradeAccPay = decimalMaster;
										}
									}
									if (header_list.TryGetValue("เงินกู้ยืมระยะสั้น", out cellIndex))
									{
										if (decimal.TryParse(row.GetCell(cellIndex)?.ToString(), out decimalMaster))
										{
											TradeAccPayLoansShot = decimalMaster;
										}
									}
									if (header_list.TryGetValue("รวมหนี้สินหมุนเวียน", out cellIndex))
									{
										if (decimal.TryParse(row.GetCell(cellIndex)?.ToString(), out decimalMaster))
										{
											TradeAccPayTotalCurrentLia = decimalMaster;
										}
									}
									if (header_list.TryGetValue("เงินกู้ยืมระยะยาว", out cellIndex))
									{
										if (decimal.TryParse(row.GetCell(cellIndex)?.ToString(), out decimalMaster))
										{
											TradeAccPayLoansLong = decimalMaster;
										}
									}
									if (header_list.TryGetValue("รวมหนี้สินไม่หมุนเวียน", out cellIndex))
									{
										if (decimal.TryParse(row.GetCell(cellIndex)?.ToString(), out decimalMaster))
										{
											TradeAccPayTotalNotCurrentLia = decimalMaster;
										}
									}
									//ใน excel ไม่มี
									if (header_list.TryGetValue("เงินให้กู้ยืมระยะสั้น(เจ้าหนี้)", out cellIndex))
									{
										if (decimal.TryParse(row.GetCell(cellIndex)?.ToString(), out decimalMaster))
										{
											TradeAccPayForLoansShot = decimalMaster;
										}
									}
									if (header_list.TryGetValue("รวมหนี้สิน", out cellIndex))
									{
										if (decimal.TryParse(row.GetCell(cellIndex)?.ToString(), out decimalMaster))
										{
											TradeAccPayTotalLiabilitie = decimalMaster;
										}
									}
									if (header_list.TryGetValue("ทุนจดทะเบียนสามัญ", out cellIndex))
									{
										if (decimal.TryParse(row.GetCell(cellIndex)?.ToString(), out decimalMaster))
										{
											RegisterCapitalOrdinary = decimalMaster;
										}
									}
									if (header_list.TryGetValue("ทุนจดทะเบียนที่ชำระแล้ว", out cellIndex))
									{
										if (decimal.TryParse(row.GetCell(cellIndex)?.ToString(), out decimalMaster))
										{
											RegisterCapitalPaid = decimalMaster;
										}
									}
									if (header_list.TryGetValue("กำไร (ขาดทุน)สะสม", out cellIndex))
									{
										if (decimal.TryParse(row.GetCell(cellIndex)?.ToString(), out decimalMaster))
										{
											ProfitLossAccumulate = decimalMaster;
										}
									}
									if (header_list.TryGetValue("รวมส่วนของผู้ถือหุ้น", out cellIndex))
									{
										if (decimal.TryParse(row.GetCell(cellIndex)?.ToString(), out decimalMaster))
										{
											TotalShareholders = decimalMaster;
										}
									}
									if (header_list.TryGetValue("รวมหนี้สินและส่วนของผู้ถือหุ้น", out cellIndex))
									{
										if (decimal.TryParse(row.GetCell(cellIndex)?.ToString(), out decimalMaster))
										{
											TotalLiabilitieShareholders = decimalMaster;
										}
									}
									if (header_list.TryGetValue("รายได้รวม", out cellIndex))
									{
										if (decimal.TryParse(row.GetCell(cellIndex)?.ToString(), out decimalMaster))
										{
											TotalIncome = decimalMaster;
										}
									}
									if (header_list.TryGetValue("ต้นทุนขาย", out cellIndex))
									{
										if (decimal.TryParse(row.GetCell(cellIndex)?.ToString(), out decimalMaster))
										{
											CostSales = decimalMaster;
										}
									}
									if (header_list.TryGetValue("กำไรขั้นต้น", out cellIndex))
									{
										if (decimal.TryParse(row.GetCell(cellIndex)?.ToString(), out decimalMaster))
										{
											GrossProfit = decimalMaster;
										}
									}
									if (header_list.TryGetValue("ค่าใช้จ่ายในการดำเนินงาน", out cellIndex))
									{
										if (decimal.TryParse(row.GetCell(cellIndex)?.ToString(), out decimalMaster))
										{
											OperatingExpenses = decimalMaster;
										}
									}
									//if (header_list.TryGetValue("กำไร (ขาดทุน) ก่อนหักค่าเสื่อมและค่าใช้จ่าย", out cellIndex))
									//{
									//	if (decimal.TryParse(row.GetCell(cellIndex).ToString(), out decimalMaster))
									//	{
									//		ProfitLossBeforeDepExp = decimalMaster;
									//	}
									//}
									if (header_list.TryGetValue("กำไร(ขาดทุน)ก่อนหักค่าเสื่อมราคาและค่าใช้จ่ายตัดจ่าย", out cellIndex))
									{
										if (decimal.TryParse(row.GetCell(cellIndex)?.ToString(), out decimalMaster))
										{
											ProfitLossBeforeDepExp = decimalMaster;
										}
									}
									if (header_list.TryGetValue("กำไร(ขาดทุน) ก่อนหักดอกเบี้ยและภาษีเงินได้", out cellIndex))
									{
										if (decimal.TryParse(row.GetCell(cellIndex)?.ToString(), out decimalMaster))
										{
											ProfitLossBeforeInterestTax = decimalMaster;
										}
									}
									//if (header_list.TryGetValue("กำไร(ขาดทุน) ก่อนหักดอกเบี้ยและภาษีเงินได้", out cellIndex))
									//{
									//	if (decimal.TryParse(row.GetCell(cellIndex).ToString(), out decimalMaster))
									//	{
									//		ProfitLossBeforeIncomeTaxExpense = decimalMaster;
									//	}
									//}
									if (header_list.TryGetValue("กำไร(ขาดทุน)สุทธิ", out cellIndex))
									{
										if (decimal.TryParse(row.GetCell(cellIndex)?.ToString(), out decimalMaster))
										{
											NetProfitLoss = decimalMaster;
										}
									}
									if (header_list.TryGetValue("หมายเหตุ", out cellIndex))
									{
										InterestNote = row.GetCell(cellIndex)?.ToString();
									}

									if (customerImportList == null) customerImportList = new();

									if (customerImportList.Select(x => x.JuristicPersonRegNumber).Any(x => x == JuristicPersonRegNumber))
										throw new Exception("มีเลขนิติบุคคลซ้ำ กรุณาตรวจสอบอีกครั้ง");

									//if (string.IsNullOrEmpty(JuristicPersonRegNumber))
									//	throw new Exception("ระบุเลขนิติบุคคลไม่ครบ");

									//if (JuristicPersonRegNumber != null && JuristicPersonRegNumber.Length < 10)
									//	throw new Exception("ระบุเลขนิติบุคคลไม่ถูกต้อง");

									if (!string.IsNullOrEmpty(JuristicPersonRegNumber))
									{
										customerImportList.Add(new()
										{
											IsFileUpload = true,
											CurrentUserId = UserInfo.Id,
											DateContact = DateContact,
											Master_ContactChannelId = Master_ContactChannelId,
											Branch_RegionId = Branch_RegionId,
											ProvincialOffice = ProvincialOffice,
											EmployeeName = EmployeeName,
											EmployeeId = EmployeeId,
											CIF = CIF,
											ContactName = ContactName,
											ContactTel = ContactTel,
											JuristicPersonRegNumber = JuristicPersonRegNumber,
											CompanyName = CompanyName,
											Master_BusinessTypeId = Master_BusinessTypeId,
											Master_BusinessTypeName = Master_BusinessTypeName,
											Master_BusinessSizeId = Master_BusinessSizeId,
											Master_BusinessSizeName = Master_BusinessSizeName,
											Master_ISICCodeId = Master_ISICCodeId,
											Master_TSICId = Master_TSICId,
											Master_TSICName = Master_TSICName,
											Master_YieldId = Master_YieldId,
											Master_ChainId = Master_ChainId,
											Master_LoanTypeId = Master_LoanTypeId,
											CompanyEmail = CompanyEmail,
											CompanyTel = CompanyTel,
											ParentCompanyGroup = ParentCompanyGroup,
											HouseNo = HouseNo,
											VillageNo = VillageNo,
											Road_Soi_Village = Road_Soi_Village,
											ProvinceId = ProvinceId,
											ProvinceName = ProvinceName,
											AmphurId = AmphurId,
											TambolId = TambolId,
											AmphurName = AmphurName,
											TambolName = TambolName,
											ZipCode = ZipCode,
											Customer_Committees = Customer_Committees,
											ShareholderMeetDay = ShareholderMeetDay,
											Customer_Shareholders = Customer_Shareholders,
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
											ProfitLossBeforeIncomeTaxExpense = ProfitLossBeforeIncomeTaxExpense,
											NetProfitLoss = NetProfitLoss,
											InterestNote = InterestNote
										});
									}
								}

								if (customerImportList?.Count > 0)
								{
									var response = await _customerViewModel.ValidateUpload(customerImportList);

									if (response.Status)
									{
										customerImportList = response.Data?.OrderBy(x => x.IsValidate == true).ToList();
									}
									else
									{
										HideLoading();
										_errorMessage = response.errorMessage;
										await _jsRuntimes.InvokeVoidAsync("WarningAlert", _errorMessage);
									}
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

			if (customerImportList == null || customerImportList.Count(x => x.IsValidate == true || x.IsKeep) == 0)
			{
				_errorMessage = "ตรวจสอบไฟล์แนบอีกครั้ง";
				await _jsRuntimes.InvokeVoidAsync("WarningAlert", _errorMessage);
			}
			else
			{
				_errorMessage = null;
				ShowLoading();

				var userData = customerImportList.Where(x => x.IsValidate == true || x.IsKeep).ToList();
				foreach (var item in userData)
				{
					var resultModel = new ResultImport();
					resultModel.Name = item.CompanyName;

					item.CurrentUserId = UserInfo.Id;

					ResultModel<CustomerCustom> response = new();
					if (item.Id == Guid.Empty)
					{
						response = await _customerViewModel.Create(item);
					}
					else
					{
						response = await _customerViewModel.Update(item);
					}

					if (!response.Status)
					{
						resultModel.Success = false;
						resultModel.errorMessage = response.errorMessage;
					}
					resultImport.Add(resultModel);
				}

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
			customerImportList = new();
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

		public async Task OnShowVersion(Guid customerID)
		{
			saleCurrentModel = new();
			saleImportModel = new();

			var data = await _salesViewModel.GetByCustomerId(customerID);
			if (data != null && data.Status && data.Data != null)
			{
				saleCurrentModel = data.Data;
			}
			else
			{
				_errorMessage = data?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

			//saleExportModel
			if (customerImportList?.Count > 0)
			{
				var sameImport = customerImportList.FirstOrDefault(x => x.Id == customerID);
				if (sameImport != null)
				{
					saleImportModel.Customer = sameImport;
				}
			}

			StateHasChanged();
			await Task.Delay(1);
			await modalVersion.ShowAsync();
		}

		private async Task KeepCustomer(CustomerCustom? model, bool isKeep = false)
		{
			if (model != null && customerImportList != null)
			{
				var keep = customerImportList.FirstOrDefault(x => x.Id == model.Id);
				if (keep != null)
				{
					keep.IsKeep = isKeep;
					await Task.Delay(1);
					await OnHideVersion();
				}
			}
		}

		private async Task OnHideVersion()
		{
			await modalVersion.HideAsync();
		}

		private async void OnHiddenVersion()
		{
			await Task.Delay(1);
		}


	}
}