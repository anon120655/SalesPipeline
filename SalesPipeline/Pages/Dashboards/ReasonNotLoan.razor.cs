using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Dashboards;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Pages.Dashboards
{
	public partial class ReasonNotLoan
	{
		string? _errorMessage = null;
		private User_PermissionCustom _permission = new();
		private allFilter filter = new();
		private LookUpResource LookUp = new();
		private List<Dash_PieCustom>? ItemsReason;
		private List<SaleCustom>? Items;
		public Pager? Pager;

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
				await SetModelReason();
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
			var dataLevels = await _userViewModel.GetListLevel(new() { status = StatusModel.Active });
			if (dataLevels != null && dataLevels.Status)
			{
				LookUp.UserLevels = dataLevels.Data;
			}
			else
			{
				_errorMessage = dataLevels?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

			var businessType = await _masterViewModel.GetBusinessType(new() { status = StatusModel.Active });
			if (businessType != null && businessType.Status)
			{
				LookUp.BusinessType = businessType.Data?.Items;
			}
			else
			{
				_errorMessage = businessType?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

			var chain = await _masterViewModel.GetChains(new() { status = StatusModel.Active });
			if (chain != null && chain.Status)
			{
				LookUp.Chain = chain.Data?.Items;
			}
			else
			{
				_errorMessage = chain?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

			var province = await _masterViewModel.GetProvince();
			if (province != null && province.Status)
			{
				LookUp.Provinces = province.Data;
			}
			else
			{
				_errorMessage = province?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

			var statusSale = await _masterViewModel.GetStatusSale(new() { pagesize = 20, status = StatusModel.Active, isshow = 1 });
			if (statusSale != null && statusSale.Status)
			{
				LookUp.StatusSale = statusSale.Data?.Items;
			}
			else
			{
				_errorMessage = statusSale?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

			var userRM = await _masterViewModel.GetListRM(new allFilter() { status = StatusModel.Active, pagesize = 100 });
			if (userRM != null && userRM.Status)
			{
				LookUp.AssignmentUser = userRM.Data?.Items;
			}
			else
			{
				_errorMessage = userRM?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

			StateHasChanged();
			await Task.Delay(10);
			await _jsRuntimes.InvokeVoidAsync("BootSelectId", "BusinessType");
			await _jsRuntimes.InvokeVoidAsync("BootSelectId", "Chain");
			await _jsRuntimes.InvokeVoidAsync("BootSelectId", "Province");
			await _jsRuntimes.InvokeVoidAsync("BootSelectId", "StatusSale");
			await _jsRuntimes.InvokeVoidAsync("BootSelectId", "AssignmentUser");
		}

		protected async Task SetModelReason()
		{
			var data = await _dashboarViewModel.GetGroupReasonNotLoan(new() { userid = UserInfo.Id });
			if (data != null && data.Status)
			{
				ItemsReason = data.Data;
			}
			else
			{
				_errorMessage = data?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

			StateHasChanged();
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
				if (UserInfo.RoleCode == RoleCodes.CEN_BRANCH)
				{
					filter.assigncenter = UserInfo.Id;
				}
				else if (UserInfo.RoleCode.StartsWith(RoleCodes.BRANCH_REG))
				{

				}

			}

			filter.statussaleid = StatusSaleModel.CloseSaleNotLoan;
			var data = await _salesViewModel.GetList(filter);
			if (data != null && data.Status)
			{
				Items = data.Data?.Items;
				Pager = data.Data?.Pager;
				if (Pager != null)
				{
					Pager.UrlAction = "/dashboard/reasonnotloan";
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