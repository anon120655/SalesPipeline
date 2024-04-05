using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Dashboards;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.ViewModels;

namespace SalesPipeline.Pages.Dashboards
{
	public partial class Dashboard
	{
		string? _errorMessage = null;
		private User_PermissionCustom _permission = new();
		private Dash_Status_TotalCustom status_TotalModel = new();
		private Dash_Avg_NumberCustom avg_NumberModel = new();
		private List<Dash_Map_ThailandCustom> map_ThailandModel = new();

		protected override async Task OnInitializedAsync()
		{
			_permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.Dashboard) ?? new User_PermissionCustom();
			StateHasChanged();


		}

		protected async override Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				await Status_Total();
				await AvgTop_Number();

				await _jsRuntimes.InvokeVoidAsync("selectPickerInitialize");

				var UrlJs = $"/js/dashboards/dashboard.js?v={_appSet.Value.Version}";
				var iSloadJs = await _jsRuntimes.InvokeAsync<bool>("loadJs", UrlJs, "/dashboard.js");
				if (iSloadJs)
				{
					await SetModelAll();
				}

				await _jsRuntimes.InvokeVoidAsync("selectPickerInitialize");
				StateHasChanged();

				firstRender = false;
			}
		}

		protected async Task SetModelAll()
		{
			if (UserInfo.RoleCode != RoleCodes.MCENTER)
			{
				//10 อันดับ
				await Map_Thailand();
				StateHasChanged();
			}

			//การปิดการขาย เหตุผลไม่ประสงค์ขอสินเชื่อ
			await CloseSaleAndReasonNotLoan();
			//เป้ายอดการขาย
			await TargetSales();
			//จำนวนลูกค้าตาม...
			await NumberCustomer();
			//มูลค่าสินเชื่อตาม...
			await LoanValue();

			if (UserInfo.RoleCode != RoleCodes.MCENTER)
			{
				//ระยะเวลาที่ใช้ในแต่ละสเตจ
				await DurationOnStage();
			}

			StateHasChanged();
		}

		protected async Task Status_Total()
		{
			if (UserInfo.Id > 0)
			{
				var data = await _dashboarViewModel.GetStatus_TotalById(UserInfo.Id);
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

		protected async Task AvgTop_Number()
		{
			if (UserInfo.Id > 0)
			{
				var data = await _dashboarViewModel.GetAvgTop_NumberById(UserInfo.Id);
				if (data != null && data.Status && data.Data != null)
				{
					avg_NumberModel = data.Data;
				}
				else
				{
					_errorMessage = data?.errorMessage;
					_utilsViewModel.AlertWarning(_errorMessage);
				}
			}
		}

		protected async Task Map_Thailand()
		{
			if (UserInfo.Id > 0)
			{
				var data = await _dashboarViewModel.GetMap_ThailandById(UserInfo.Id);
				if (data != null && data.Status && data.Data != null)
				{
					map_ThailandModel = data.Data;
					StateHasChanged();
					await Task.Delay(10);

					await TopSalesCenter();
					await CenterLost();
					StateHasChanged();
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
			var data = await _dashboarViewModel.GetPieCloseSaleReason(UserInfo.Id);
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
			await _jsRuntimes.InvokeVoidAsync("targetsales", null);
		}

		protected async Task NumberCustomer()
		{
			var data = await _dashboarViewModel.GetPieNumberCustomer(UserInfo.Id);
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

					await _jsRuntimes.InvokeVoidAsync("numcussizebusiness", chartModel.datas.ToArray(), chartModel.labels.ToArray());
				}

				chartModel = new();
				var numCusTypeBusiness = data.Data.Where(x => x.Code == Dash_PieCodeModel.NumCusTypeBusiness).ToList();
				if (numCusTypeBusiness.Count > 0)
				{
					foreach (var item in numCusTypeBusiness)
					{
						chartModel.labels.Add(item.Name);
						chartModel.datas.Add(item.Value ?? 0);
					}

					await _jsRuntimes.InvokeVoidAsync("numcustypebusiness", chartModel.datas.ToArray(), chartModel.labels.ToArray());
				}

				chartModel = new();
				var numCusISICCode = data.Data.Where(x => x.Code == Dash_PieCodeModel.NumCusISICCode).ToList();
				if (numCusISICCode.Count > 0)
				{
					foreach (var item in numCusISICCode)
					{
						chartModel.labels.Add(item.Name);
						chartModel.datas.Add(item.Value ?? 0);
					}

					await _jsRuntimes.InvokeVoidAsync("numcusisiccode", chartModel.datas.ToArray(), chartModel.labels.ToArray());
				}

				chartModel = new();
				var numCusLoanType = data.Data.Where(x => x.Code == Dash_PieCodeModel.NumCusLoanType).ToList();
				if (numCusLoanType.Count > 0)
				{
					foreach (var item in numCusLoanType)
					{
						chartModel.labels.Add(item.Name);
						chartModel.datas.Add(item.Value ?? 0);
					}

					await _jsRuntimes.InvokeVoidAsync("numcusloantype", chartModel.datas.ToArray(), chartModel.labels.ToArray());
				}

			}
			else
			{
				_errorMessage = data?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}
		}

		protected async Task LoanValue()
		{
			var data = await _dashboarViewModel.GetPieLoanValue(UserInfo.Id);
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

					await _jsRuntimes.InvokeVoidAsync("valuesizebusiness", chartModel.datas.ToArray(), chartModel.labels.ToArray());
				}

				chartModel = new();
				var valueTypeBusiness = data.Data.Where(x => x.Code == Dash_PieCodeModel.ValueTypeBusiness).ToList();
				if (valueTypeBusiness.Count > 0)
				{
					foreach (var item in valueTypeBusiness)
					{
						chartModel.labels.Add(item.Name);
						chartModel.datas.Add(item.Value ?? 0);
					}

					await _jsRuntimes.InvokeVoidAsync("valuetypebusiness", chartModel.datas.ToArray(), chartModel.labels.ToArray());
				}

				chartModel = new();
				var valueISICCode = data.Data.Where(x => x.Code == Dash_PieCodeModel.ValueISICCode).ToList();
				if (valueISICCode.Count > 0)
				{
					foreach (var item in valueISICCode)
					{
						chartModel.labels.Add(item.Name);
						chartModel.datas.Add(item.Value ?? 0);
					}

					await _jsRuntimes.InvokeVoidAsync("valueisiccode", chartModel.datas.ToArray(), chartModel.labels.ToArray());
				}

				chartModel = new();
				var valueLoanType = data.Data.Where(x => x.Code == Dash_PieCodeModel.ValueLoanType).ToList();
				if (valueLoanType.Count > 0)
				{
					foreach (var item in valueLoanType)
					{
						chartModel.labels.Add(item.Name);
						chartModel.datas.Add(item.Value ?? 0);
					}

					await _jsRuntimes.InvokeVoidAsync("valueloantype", chartModel.datas.ToArray(), chartModel.labels.ToArray());
				}
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
			await _jsRuntimes.InvokeVoidAsync("durationonstage", null);
		}


	}
}