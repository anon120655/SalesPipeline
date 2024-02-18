using Microsoft.JSInterop;
using SalesPipeline.Shared.Modals;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils;
using Microsoft.AspNetCore.Components;
using NPOI.SS.Formula.Functions;
using SalesPipeline.Utils.Resources.Thailands;

namespace SalesPipeline.Pages.ApproveLoans
{
	public partial class ApproveLoan
	{
		string? _errorMessage = null;
		private User_PermissionCustom _permission = new();
		private allFilter filter = new();
		private LookUpResource LookUp = new();
		private List<SaleCustom>? Items;
		public Pager? Pager;

		protected override async Task OnInitializedAsync()
		{
			_permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.ApproveLoan) ?? new User_PermissionCustom();
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
			if (UserInfo.RoleCode != RoleCodes.SUPERADMIN)
			{
				filter.assigncenter = UserInfo.Id;
			}

			filter.statussaleid = StatusSaleModel.WaitApprove;
			var data = await _salesViewModel.GetList(filter);
			if (data != null && data.Status)
			{
				Items = data.Data?.Items;
				Pager = data.Data?.Pager;
				if (Pager != null)
				{
					Pager.UrlAction = "/approveloan";
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
			filter.province = null;
			filter.amphur = null;
			LookUp.Amphurs = new();
			StateHasChanged();
			await _jsRuntimes.InvokeVoidAsync("BootSelectEmptyID", "Amphur");

			if (_provinceID != null && int.TryParse(_provinceID, out int provinceID))
			{
				filter.province = provinceID.ToString();

				if (int.TryParse(filter.province,out int id))
				{
					var amphurs = await _masterViewModel.GetAmphur(id);
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
			}

			await SetModel();
			StateHasChanged();
			_Navs.NavigateTo($"{Pager?.UrlAction}?{filter.SetParameter(true)}");

		}

		[JSInvokable]
		public async Task AmphurChange(string _amphurID, string _amphurName)
		{
			filter.amphur = null;
			if (_amphurID != null && int.TryParse(_amphurID, out int amphurID))
			{
				filter.amphur = amphurID.ToString();
			}

			await SetModel();
			StateHasChanged();
			_Navs.NavigateTo($"{Pager?.UrlAction}?{filter.SetParameter(true)}");
		}

	}
}