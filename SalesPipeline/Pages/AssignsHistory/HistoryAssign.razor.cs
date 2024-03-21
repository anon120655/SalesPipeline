using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils.Resources.Thailands;

namespace SalesPipeline.Pages.AssignsHistory
{
	public partial class HistoryAssign
	{
		string? _errorMessage = null;
		private User_PermissionCustom _permission = new();
		private allFilter filter = new();
		private LookUpResource LookUp = new();
		private List<SaleCustom>? Items;
		public Pager? Pager;

		protected override async Task OnInitializedAsync()
		{
			_permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.HistoryAssign) ?? new User_PermissionCustom();
			StateHasChanged();

			filter.sort = OrderByModel.ASC;
		}

		protected async override Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				await SetQuery();
				StateHasChanged();
				await SetInitManual();
				await Task.Delay(10);

				firstRender = false;
			}
		}

		protected async Task SetInitManual()
		{
			await _jsRuntimes.InvokeVoidAsync("BootSelectId", "DisplaySort");

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

			StateHasChanged();
			await Task.Delay(10);
			await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "ProvinceChange", "#Province");

			await _jsRuntimes.InvokeVoidAsync("BootSelectId", "Responsible");
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
				filter.assigncenter = UserInfo.Id;
				if (UserInfo.RoleCode == RoleCodes.SUPERADMIN)
				{
					filter.assigncenter = null;
					filter.assignrm = null;
				}
			}

			filter.statussaleid = StatusSaleModel.WaitContact;
			var data = await _salesViewModel.GetList(filter);
			if (data != null && data.Status)
			{
				Items = data.Data?.Items;
				Pager = data.Data?.Pager;
				if (Pager != null)
				{
					Pager.UrlAction = "/historyassign";
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

		protected async Task OnSort(ChangeEventArgs e)
		{
			filter.sort = null;
			if (e.Value != null)
			{
				filter.sort = e.Value.ToString();

				await SetModel();
				StateHasChanged();
				_Navs.NavigateTo($"{Pager?.UrlAction}?{filter.SetParameter(true)}");
			}
		}

		[JSInvokable]
		public async Task ProvinceChange(string _provinceID, string _provinceName)
		{
			filter.provinceid = null;
			filter.amphurid = null;
			LookUp.Amphurs = new();
			StateHasChanged();
			await _jsRuntimes.InvokeVoidAsync("BootSelectEmptyID", "Amphur");

			if (_provinceID != null && int.TryParse(_provinceID, out int provinceID))
			{
				filter.provinceid = provinceID;

				var amphurs = await _masterViewModel.GetAmphur(provinceID);
				if (amphurs != null && amphurs.Data?.Count > 0)
				{
					LookUp.Amphurs = new List<InfoAmphurCustom>() { new InfoAmphurCustom() { AmphurID = 0, AmphurName = "--เลือก--" } };
					LookUp.Amphurs.AddRange(amphurs.Data);

					StateHasChanged();
					await Task.Delay(10);
					await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "AmphurChange", "#Amphur");
					await _jsRuntimes.InvokeVoidAsync("BootSelectRefreshID", "Amphur", 100);
				}
				else
				{
					_errorMessage = amphurs?.errorMessage;
					_utilsViewModel.AlertWarning(_errorMessage);
				}
			}

			await SetModel();
			StateHasChanged();
			_Navs.NavigateTo($"{Pager?.UrlAction}?{filter.SetParameter(true)}");

		}

		[JSInvokable]
		public async Task AmphurChange(string _amphurID, string _amphurName)
		{
			filter.amphurid = null;
			if (_amphurID != null && int.TryParse(_amphurID, out int amphurID))
			{
				filter.amphurid = amphurID;
			}

			await SetModel();
			StateHasChanged();
			_Navs.NavigateTo($"{Pager?.UrlAction}?{filter.SetParameter(true)}");
		}



	}
}