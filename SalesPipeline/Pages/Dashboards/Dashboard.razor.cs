using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using SalesPipeline.Utils;
using SalesPipeline.Utils.ConstTypeModel;
using SalesPipeline.Utils.Resources.Assignments;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Dashboards;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils.Resources.Thailands;
using SalesPipeline.ViewModels;

namespace SalesPipeline.Pages.Dashboards
{
    public partial class Dashboard
	{
		string? _errorMessage = null;
		private User_PermissionCustom _permission = new();
		private LookUpResource LookUp = new();
		private allFilter filter = new();
		private Dash_Status_TotalCustom status_TotalModel = new();
		private Dash_SalesPipelineModel salesPipelineModel = new();
		private Dash_AvgTop_NumberCustom avgTop_NumberModel = new();
		private Dash_AvgBottom_NumberCustom avgBottom_NumberModel = new();
		private List<Dash_Map_ThailandCustom> topSaleModel = new();
		private List<Dash_Map_ThailandCustom> lostSaleModel = new();

		protected override async Task OnInitializedAsync()
		{
			_permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.Dashboard) ?? new User_PermissionCustom();
			StateHasChanged();
			await Task.Delay(1);
		}

		protected async override Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				filter.userid = UserInfo.Id;
				filter.status = StatusModel.Active;

				await SetInitManual();
				await _jsRuntimes.InvokeVoidAsync("BootSelectClass", "selectInit");

				var UrlJs = $"/js/dashboards/dashboard.js?v={_appSet.Value.Version}";
				var iSloadJs = await _jsRuntimes.InvokeAsync<bool>("loadJs", UrlJs, "/dashboard.js");
				if (iSloadJs)
				{
					var UrlJs2 = $"/js/html2canvas.js?v={_appSet.Value.Version}";
					var iSloadJs2 = await _jsRuntimes.InvokeAsync<bool>("loadJs", UrlJs2, "/html2canvas.js");

					await SetModelAll();
				}

				StateHasChanged();

				firstRender = false;
			}
		}

		protected async Task SetInitManual()
		{
			var dataDepBranchs = await _masterViewModel.GetDepBranchs(new allFilter() { status = StatusModel.Active });
			if (dataDepBranchs != null && dataDepBranchs.Status)
			{
				LookUp.DepartmentBranch = new() { new() { Id = Guid.Empty, Name = "ทั้งหมด" } };
				if (dataDepBranchs.Data?.Items.Count > 0)
				{
					LookUp.DepartmentBranch.AddRange(dataDepBranchs.Data.Items);
					StateHasChanged();
					await Task.Delay(1);
					await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "OnDepBranch", "#DepBranch");
				}
			}
			else
			{
				_errorMessage = dataDepBranchs?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}
		}

		protected async Task SetModelAll()
		{
			await Status_Total();
			await Get_SalesPipeline();
			await AvgTop_Number();

			if (UserInfo.RoleCode != RoleCodes.CENTER)
			{
				//10 อันดับ
				await TopSaleMap_Thailand();
				StateHasChanged();
				await LostSaleMap_Thailand();
				StateHasChanged();
				//ระยะเวลาที่ใช้ในแต่ละสเตจ
				await DurationOnStage();
				//StateHasChanged();
			}

			//การปิดการขาย เหตุผลไม่ประสงค์ขอสินเชื่อ
			await CloseSaleAndReasonNotLoan();
			//เป้ายอดการขาย
			await TargetSales();
			//จำนวนลูกค้าตาม...
			await NumberCustomer();
			//มูลค่าสินเชื่อตาม...
			await LoanValue();

			StateHasChanged();

			await AvgBottom_Number();
			StateHasChanged();
		}

		protected async Task Status_Total()
		{
			if (UserInfo.Id > 0)
			{
				var data = await _dashboarViewModel.GetStatus_TotalById(filter);
				if (data != null && data.Status && data.Data != null)
				{
					status_TotalModel = data.Data;
				}
				else
				{
					_errorMessage = data?.errorMessage;
					_utilsViewModel.AlertWarning(_errorMessage);
				}
			}

		}

		protected async Task Get_SalesPipeline()
		{
			if (UserInfo.Id > 0)
			{
				var data = await _dashboarViewModel.Get_SalesPipelineById(filter);
				if (data != null && data.Status && data.Data != null)
				{
					salesPipelineModel = data.Data;
				}
				else
				{
					_errorMessage = data?.errorMessage;
					_utilsViewModel.AlertWarning(_errorMessage);
				}
			}

		}

		protected async Task AvgTop_Number()
		{
			if (UserInfo.Id > 0)
			{
				var data = await _dashboarViewModel.GetAvgTop_NumberById(filter);
				if (data != null && data.Status && data.Data != null)
				{
					avgTop_NumberModel = data.Data;
				}
				else
				{
					_errorMessage = data?.errorMessage;
					_utilsViewModel.AlertWarning(_errorMessage);
				}
			}
		}

		protected async Task AvgBottom_Number()
		{
			if (UserInfo.Id > 0)
			{
				var data = await _dashboarViewModel.GetAvgBottom_NumberById(filter);
				if (data != null && data.Status && data.Data != null)
				{
					avgBottom_NumberModel = data.Data;
				}
				else
				{
					_errorMessage = data?.errorMessage;
					_utilsViewModel.AlertWarning(_errorMessage);
				}
			}
		}

		protected async Task TopSaleMap_Thailand()
		{
			if (UserInfo.Id > 0)
			{
				var data = await _dashboarViewModel.GetTopSale(filter);
				if (data != null && data.Status && data.Data != null)
				{
					topSaleModel = data.Data.Items;
					StateHasChanged();
					await Task.Delay(10);

					await TopSalesCenter();
				}
				else
				{
					_errorMessage = data?.errorMessage;
					_utilsViewModel.AlertWarning(_errorMessage);
				}
			}
		}

		protected async Task LostSaleMap_Thailand()
		{
			if (UserInfo.Id > 0)
			{
				var data = await _dashboarViewModel.GetLostSale(filter);
				if (data != null && data.Status && data.Data != null)
				{
					lostSaleModel = data.Data.Items;
					StateHasChanged();
					await Task.Delay(10);

					await CenterLost();
				}
				else
				{
					_errorMessage = data?.errorMessage;
					_utilsViewModel.AlertWarning(_errorMessage);
				}
			}
		}

		protected async Task CloseSaleAndReasonNotLoan()
		{
			var data = await _dashboarViewModel.GetPieCloseSaleReason(filter);
			if (data != null && data.Status && data.Data != null)
			{
				var closesale = data.Data.Where(x => x.Code == Dash_PieCodeModel.ClosingSale).ToList();
				await _jsRuntimes.InvokeVoidAsync("closesale", closesale);


				//var labels = new[] { "ใช้เวลานาน ", "ขาดการติดต่อ ", "กู้ธนาคารอื่นแล้ว ", "ดอกเบี้ยสูง " };
				//var datas = new[] { 10, 20, 30, 40 };
				var labels = new List<string?>();
				var datas = new List<decimal>();

				var reasonnotloan = data.Data.Where(x => x.Code == Dash_PieCodeModel.ReasonNotLoan).ToList();
				if (reasonnotloan.Count > 0)
				{
					foreach (var item in reasonnotloan)
					{
						labels.Add(item.Name);
						datas.Add(item.Value ?? 0);
					}
				}
				await _jsRuntimes.InvokeVoidAsync("reasonnotloan", datas.ToArray(), labels.ToArray());
			}
			else
			{
				_errorMessage = data?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}
		}

		protected async Task TargetSales()
		{
			var data = await _dashboarViewModel.GetSumTargetActual(filter);
			if (data != null && data.Status && data.Data != null)
			{
				await _jsRuntimes.InvokeVoidAsync("targetsales", data.Data);
			}
			else
			{
				_errorMessage = data?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}
		}

		protected async Task NumberCustomer()
		{
			var data = await _dashboarViewModel.GetPieNumberCustomer(filter);
			if (data != null && data.Status && data.Data != null)
			{
				//var labels = new[] { "ใช้เวลานาน ", "ขาดการติดต่อ ", "กู้ธนาคารอื่นแล้ว ", "ดอกเบี้ยสูง " };
				//var datas = new[] { 10, 20, 30, 40 };
				var chartModel = new ChartJsDataLabelsModel();

				var numCusSizeBusiness = data.Data.Where(x => x.Code == Dash_PieCodeModel.NumCusSizeBusiness).ToList();
				if (numCusSizeBusiness.Count > 0)
				{
					foreach (var item in numCusSizeBusiness)
					{
						chartModel.labels.Add(item.Name);
						chartModel.datas.Add(item.Value ?? 0);
					}
				}
				await _jsRuntimes.InvokeVoidAsync("numcussizebusiness", chartModel.datas.ToArray(), chartModel.labels.ToArray());

				chartModel = new();
				var numCusTypeBusiness = data.Data.Where(x => x.Code == Dash_PieCodeModel.NumCusTypeBusiness).ToList();
				if (numCusTypeBusiness.Count > 0)
				{
					foreach (var item in numCusTypeBusiness)
					{
						chartModel.labels.Add(item.Name);
						chartModel.datas.Add(item.Value ?? 0);
					}
				}
				await _jsRuntimes.InvokeVoidAsync("numcustypebusiness", chartModel.datas.ToArray(), chartModel.labels.ToArray());

				chartModel = new();
				var numCusISICCode = data.Data.Where(x => x.Code == Dash_PieCodeModel.NumCusISICCode).ToList();
				if (numCusISICCode.Count > 0)
				{
					foreach (var item in numCusISICCode)
					{
						chartModel.labels.Add(item.Name);
						chartModel.datas.Add(item.Value ?? 0);
					}
				}
				await _jsRuntimes.InvokeVoidAsync("numcusisiccode", chartModel.datas.ToArray(), chartModel.labels.ToArray());

				chartModel = new();
				var numCusLoanType = data.Data.Where(x => x.Code == Dash_PieCodeModel.NumCusLoanType).ToList();
				if (numCusLoanType.Count > 0)
				{
					foreach (var item in numCusLoanType)
					{
						chartModel.labels.Add(item.Name);
						chartModel.datas.Add(item.Value ?? 0);
					}
				}
				await _jsRuntimes.InvokeVoidAsync("numcusloantype", chartModel.datas.ToArray(), chartModel.labels.ToArray());

			}
			else
			{
				_errorMessage = data?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}
		}

		protected async Task LoanValue()
		{
			var data = await _dashboarViewModel.GetPieLoanValue(filter);
			if (data != null && data.Status && data.Data != null)
			{
				var chartModel = new ChartJsDataLabelsModel();

				var valueSizeBusiness = data.Data.Where(x => x.Code == Dash_PieCodeModel.ValueSizeBusiness).ToList();
				if (valueSizeBusiness.Count > 0)
				{
					foreach (var item in valueSizeBusiness)
					{
						chartModel.labels.Add(item.Name);
						chartModel.datas.Add(item.Value ?? 0);
					}
				}
				await _jsRuntimes.InvokeVoidAsync("valuesizebusiness", chartModel.datas.ToArray(), chartModel.labels.ToArray());

				chartModel = new();
				var valueTypeBusiness = data.Data.Where(x => x.Code == Dash_PieCodeModel.ValueTypeBusiness).ToList();
				if (valueTypeBusiness.Count > 0)
				{
					foreach (var item in valueTypeBusiness)
					{
						chartModel.labels.Add(item.Name);
						chartModel.datas.Add(item.Value ?? 0);
					}
				}
				await _jsRuntimes.InvokeVoidAsync("valuetypebusiness", chartModel.datas.ToArray(), chartModel.labels.ToArray());

				chartModel = new();
				var valueISICCode = data.Data.Where(x => x.Code == Dash_PieCodeModel.ValueISICCode).ToList();
				if (valueISICCode.Count > 0)
				{
					foreach (var item in valueISICCode)
					{
						chartModel.labels.Add(item.Name);
						chartModel.datas.Add(item.Value ?? 0);
					}
				}
				await _jsRuntimes.InvokeVoidAsync("valueisiccode", chartModel.datas.ToArray(), chartModel.labels.ToArray());

				chartModel = new();
				var valueLoanType = data.Data.Where(x => x.Code == Dash_PieCodeModel.ValueLoanType).ToList();
				if (valueLoanType.Count > 0)
				{
					foreach (var item in valueLoanType)
					{
						chartModel.labels.Add(item.Name);
						chartModel.datas.Add(item.Value ?? 0);
					}
				}
				await _jsRuntimes.InvokeVoidAsync("valueloantype", chartModel.datas.ToArray(), chartModel.labels.ToArray());
			}
			else
			{
				_errorMessage = data?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}
		}

		protected async Task TopSalesCenter()
		{
			await _jsRuntimes.InvokeVoidAsync("topsalescenter", null);
		}

		protected async Task CenterLost()
		{
			await _jsRuntimes.InvokeVoidAsync("centerlost", null);
		}

		protected async Task DurationOnStage()
		{
			var data = await _dashboarViewModel.GetAvgOnStage(filter);
			if (data != null && data.Status && data.Data != null)
			{
				await _jsRuntimes.InvokeVoidAsync("durationonstage", data.Data);
			}
			else
			{
				_errorMessage = data?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}
		}

		protected async Task Search()
		{
			await SetModelAll();
			StateHasChanged();
		}

		[JSInvokable]
		public async Task OnDepBranch(string _ids, string _name)
		{
			LookUp.Provinces = new();
			LookUp.Branchs = new();
			filter.DepBranchs = new();
			filter.Provinces = new();
			filter.Branchs = new();
			StateHasChanged();

			await _jsRuntimes.InvokeVoidAsync("BootSelectEmptyID", "Province");
			await _jsRuntimes.InvokeVoidAsync("BootSelectEmptyID", "Branch");

			if (_ids != null)
			{
				filter.DepBranchs.Add(_ids);
			}

			if (filter.DepBranchs.Count > 0)
			{
				if (Guid.TryParse(_ids, out Guid depBranchId))
				{
					await _jsRuntimes.InvokeVoidAsync("AddCursorWait");

					var dataProvince = await _masterViewModel.GetProvince(depBranchId);
					if (dataProvince != null && dataProvince.Status)
					{
						if (dataProvince.Data?.Count > 0)
						{
							LookUp.Provinces = new() { new() { ProvinceID = 0, ProvinceName = "ทั้งหมด" } };
							LookUp.Provinces.AddRange(dataProvince.Data);
							StateHasChanged();
							await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "OnProvince", "#Province");
							await _jsRuntimes.InvokeVoidAsync("BootSelectRefreshID", "Province", 100);
							await _jsRuntimes.InvokeVoidAsync("BootSelectRefreshID", "Branch", 100);
						}
					}
					else
					{
						_errorMessage = dataProvince?.errorMessage;
						_utilsViewModel.AlertWarning(_errorMessage);
					}
					await _jsRuntimes.InvokeVoidAsync("RemoveCursorWait");
				}
			}
			else
			{
				await _jsRuntimes.InvokeVoidAsync("RemoveCursorWait");
			}
		}

		[JSInvokable]
		public async Task OnProvince(string _ids, string _provinceName)
		{
			LookUp.Branchs = new();
			filter.Provinces = new();
			filter.Branchs = new();
			StateHasChanged();
			await _jsRuntimes.InvokeVoidAsync("BootSelectEmptyID", "Branch");

			if (_ids != null)
			{
				filter.Provinces.Add(_ids);
			}

			if (filter.Provinces.Count > 0)
			{
				if (_ids != null && int.TryParse(_ids, out int provinceID))
				{
					var branch = await _masterViewModel.GetBranch(provinceID);
					if (branch != null && branch.Data?.Count > 0)
					{
						LookUp.Branchs = new() { new() { BranchID = 0, BranchName = "ทั้งหมด" } };
						LookUp.Branchs.AddRange(branch.Data);
						StateHasChanged();
						await Task.Delay(10);
						await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "OnBranch", "#Branch");
						await _jsRuntimes.InvokeVoidAsync("BootSelectRefreshID", "Branch", 100);
					}
					else
					{
						_errorMessage = branch?.errorMessage;
						_utilsViewModel.AlertWarning(_errorMessage);
					}
				}
			}
		}

		[JSInvokable]
		public async Task OnBranch(string _branchID, string _branchName)
		{
			await Task.Delay(1);
			filter.Branchs = new();
			if (_branchID != null && int.TryParse(_branchID, out int branchID))
			{
				filter.Branchs.Add(branchID.ToString());
			}
		}

		protected void OnDateStart(ChangeEventArgs e)
		{
			if (e != null && e.Value != null && filter != null)
			{
				if (!String.IsNullOrEmpty(e.Value.ToString()))
				{
					filter.startdate = GeneralUtils.DateNotNullToEn(e.Value.ToString(), "yyyy-MM-dd", Culture: "en-US");
				}
				else
				{
					filter.startdate = null;
				}
			}
		}

		protected void OnDateEnd(ChangeEventArgs e)
		{
			if (e != null && e.Value != null && filter != null)
			{
				if (!String.IsNullOrEmpty(e.Value.ToString()))
				{
					filter.enddate = GeneralUtils.DateNotNullToEn(e.Value.ToString(), "yyyy-MM-dd", Culture: "en-US");
				}
				else
				{
					filter.enddate = null;
				}
			}
		}

		protected async Task CaptureDashboard()
		{
			await _jsRuntimes.InvokeVoidAsync("captureDashboard", "Dashboard");
		}

	}
}