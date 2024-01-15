using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using SalesPipeline.Utils;
using SalesPipeline.ViewModels;

namespace SalesPipeline.PagesCode.Dashboards
{
	public class DashboardCode : ComponentBase
	{
		[Inject] protected IJSRuntime _jsRuntimes { get; set; } = null!;
		[Inject] protected IOptions<AppSettings> _appSet { get; set; } = null!;
		[Inject] public MasterViewModel Master { get; set; } = null!;

		string? _errorMessage = null;


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
			await SizeBusinessClosedSales();
			await TypeBusinessClosedSales();
			await TopSalesCenter();
			await CenterLost();
			await PeriodOnStage();
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

		protected async Task SizeBusinessClosedSales()
		{
			await _jsRuntimes.InvokeVoidAsync("sizebusinessclosedsales", null);
		}

		protected async Task TypeBusinessClosedSales()
		{
			await _jsRuntimes.InvokeVoidAsync("typebusinessclosedsales", null);
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
