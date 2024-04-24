using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Pages.Dashboards
{
	public partial class AvgDurationCloseSale
	{
		string? _errorMessage = null;
		private User_PermissionCustom _permission = new();
		private allFilter filter = new();
		private LookUpResource LookUp = new();
		private List<Sale_DurationCustom>? Items;
		public Pager? Pager;

		protected override async Task OnInitializedAsync()
		{
			_permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.Dashboard) ?? new User_PermissionCustom();
			StateHasChanged();

		}

		protected async override Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				await SetQuery();
				StateHasChanged();
				await SetInitManual();
				await Task.Delay(10);

				await _jsRuntimes.InvokeVoidAsync("selectPickerInitialize");
				firstRender = false;
			}
		}

		protected async Task SetInitManual()
		{
			await Task.Delay(10);
			//var businessType = await _masterViewModel.GetBusinessType(new() { status = StatusModel.Active });
			//if (businessType != null && businessType.Status)
			//{
			//	LookUp.BusinessType = businessType.Data?.Items;
			//}
			//else
			//{
			//	_errorMessage = businessType?.errorMessage;
			//	_utilsViewModel.AlertWarning(_errorMessage);
			//}


			//StateHasChanged();
			//await Task.Delay(10);
			//await _jsRuntimes.InvokeVoidAsync("BootSelectId", "BusinessType");
		}

		protected async Task SetQuery(string? parematerAll = null)
		{
			string uriQuery = _Navs.ToAbsoluteUri(_Navs.Uri).Query;

			if (parematerAll != null)
				uriQuery = $"?{parematerAll}";

			filter.SetUriQuery(uriQuery);

			await SetModel();
			StateHasChanged();
		}

		protected async Task SetModel()
		{
			if (UserInfo.RoleCode != null)
			{
				if (UserInfo.RoleCode == RoleCodes.MCENTER)
				{
					filter.assigncenter = UserInfo.Id;
				}
				else if (UserInfo.RoleCode.StartsWith(RoleCodes.BRANCH))
				{

				}

			}

			filter.type = "avgdurationclosesale";
			var data = await _dashboarViewModel.GetDuration(filter);
			if (data != null && data.Status)
			{
				Items = data.Data?.Items;
				Pager = data.Data?.Pager;
				if (Pager != null)
				{
					Pager.UrlAction = "/dashboard/avgdurationclosesale";
				}
			}
			else
			{
				_errorMessage = data?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

			StateHasChanged();
		}

		protected async Task OnSelectPagesize(int _number)
		{
			Items = null;
			StateHasChanged();
			filter.page = 1;
			filter.pagesize = _number;
			await SetModel();
			_Navs.NavigateTo($"{Pager?.UrlAction}?{filter.SetParameter(true)}");
		}

		protected async Task OnSelectPage(string parematerAll)
		{
			await SetQuery(parematerAll);
			StateHasChanged();
		}

		protected async Task Search()
		{
			await SetModel();
			StateHasChanged();
			_Navs.NavigateTo($"{Pager?.UrlAction}?{filter.SetParameter(true)}");
		}

		protected void OnContactDateStart(ChangeEventArgs e)
		{
			if (e != null && e.Value != null && filter != null)
			{
				if (!String.IsNullOrEmpty(e.Value.ToString()))
				{
					filter.contactstartdate = GeneralUtils.DateNotNullToEn(e.Value.ToString(), "yyyy-MM-dd", Culture: "en-US");
				}
				else
				{
					filter.contactstartdate = null;
				}
			}
		}


	}
}