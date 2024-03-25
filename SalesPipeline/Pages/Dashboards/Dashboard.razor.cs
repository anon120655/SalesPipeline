using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Dashboards;
using SalesPipeline.ViewModels;

namespace SalesPipeline.Pages.Dashboards
{
	public partial class Dashboard
	{
		string? _errorMessage = null;
		private User_PermissionCustom _permission = new();
		private Dash_Status_TotalCustom status_TotalModel = new();
		private Dash_Avg_NumberCustom avg_NumberModel = new();

		protected override async Task OnInitializedAsync()
		{
			_permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.Dashboard) ?? new User_PermissionCustom();
			StateHasChanged();

			await Status_Total();
			await Avg_Number();
		}

		protected async override Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{



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
			await CloseSale();
			await ReasonNotLoan();
			await TargetSales();

			await NumCusSizeBusiness();
			await NumCusTypeBusiness();
			await NumCusISICCode();
			await NumCusLoanType();

			await ValueSizeBusiness();
			await ValueTypeBusiness();
			await ValueISICCode();
			await ValueLoanType();

			await TopSalesCenter();
			await CenterLost();
			await PeriodOnStage();
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

		protected async Task Avg_Number()
		{
			if (UserInfo.Id > 0)
			{
				var data = await _dashboarViewModel.GetAvg_NumberById(UserInfo.Id);
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

		protected async Task CloseSale()
		{
			await _jsRuntimes.InvokeVoidAsync("closesale", null);
		}

		protected async Task ReasonNotLoan()
		{
			await _jsRuntimes.InvokeVoidAsync("reasonnotloan", null);
		}

		protected async Task TargetSales()
		{
			await _jsRuntimes.InvokeVoidAsync("targetsales", null);
		}

		protected async Task NumCusSizeBusiness()
		{
			await _jsRuntimes.InvokeVoidAsync("numcussizebusiness", null);
		}

		protected async Task NumCusTypeBusiness()
		{
			await _jsRuntimes.InvokeVoidAsync("numcustypebusiness", null);
		}

		protected async Task NumCusISICCode()
		{
			await _jsRuntimes.InvokeVoidAsync("numcusisiccode", null);
		}

		protected async Task NumCusLoanType()
		{
			await _jsRuntimes.InvokeVoidAsync("numcusloantype", null);
		}

		protected async Task ValueSizeBusiness()
		{
			await _jsRuntimes.InvokeVoidAsync("valuesizebusiness", null);
		}

		protected async Task ValueTypeBusiness()
		{
			await _jsRuntimes.InvokeVoidAsync("valuetypebusiness", null);
		}

		protected async Task ValueISICCode()
		{
			await _jsRuntimes.InvokeVoidAsync("valueisiccode", null);
		}

		protected async Task ValueLoanType()
		{
			await _jsRuntimes.InvokeVoidAsync("valueloantype", null);
		}

		protected async Task TopSalesCenter()
		{
			await _jsRuntimes.InvokeVoidAsync("topsalescenter", null);
		}

		protected async Task CenterLost()
		{
			await _jsRuntimes.InvokeVoidAsync("centerlost", null);
		}

		protected async Task PeriodOnStage()
		{
			await _jsRuntimes.InvokeVoidAsync("periodonstage", null);
		}


	}
}