using Microsoft.JSInterop;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Dashboards;

namespace SalesPipeline.Pages.Dashboards
{
	public partial class AvgPerDeal_Region
    {
		string? _errorMessage = null;
		private User_PermissionCustom _permission = new();
		private LookUpResource LookUp = new();
		private FilterAvgPerDeal filterAvg = new();

		public string filterRegionsTitle = "เลือก";
		public string filterBranchsTitle = "เลือก";
		public string filterRMUserTitle = "เลือก";

		protected override async Task OnInitializedAsync()
		{
			_permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.Dashboard) ?? new User_PermissionCustom();
			StateHasChanged();

			await SetInitManual();
		}

		protected async override Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				var UrlJs = $"/js/dashboards/avgeperdeal_region.js?v={_appSet.Value.Version}";
				var iSloadJs = await _jsRuntimes.InvokeAsync<bool>("loadJs", UrlJs, "/avgeperdeal.js");
				if (iSloadJs)
				{
					await SetModelAll();
				}

				await _jsRuntimes.InvokeVoidAsync("selectPickerInitialize");
				StateHasChanged();
				firstRender = false;
			}
		}

		protected async Task SetInitManual()
		{
			var dataRegions = await _masterViewModel.Regions(new allFilter() { status = StatusModel.Active });
			if (dataRegions != null && dataRegions.Status)
			{
				LookUp.Regions = new() { new() { Id = 0, Name = "ทั้งหมด" } };
				if (dataRegions.Data?.Count > 0)
				{
					LookUp.Regions.AddRange(dataRegions.Data);
				}
			}
			else
			{
				_errorMessage = dataRegions?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

			StateHasChanged();
		}

		protected async Task SetModelAll()
		{
			await AvgDeal_Region_Bar();
		}

		protected async Task AvgDeal_Region_Bar()
		{
			await _jsRuntimes.InvokeVoidAsync("avgdeal_region_bar", null);
		}

	}
}