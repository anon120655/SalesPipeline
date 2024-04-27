using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Pages.Dashboards
{
	public partial class ReturnedItems
    {
		string? _errorMessage = null;
		private User_PermissionCustom _permission = new();
		private allFilter filter = new();
		private LookUpResource LookUp = new();
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

			StateHasChanged();
			await Task.Delay(10);
			await _jsRuntimes.InvokeVoidAsync("BootSelectId", "BusinessType");
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
					filter.statussaleid = StatusSaleModel.RMReturnMCenter;
				}
				else if (UserInfo.RoleCode.StartsWith(RoleCodes.BRANCH))
				{
					filter.statussaleid = StatusSaleModel.MCenterReturnBranch;
				}
				else if (UserInfo.RoleCode.StartsWith(RoleCodes.LOAN) || UserInfo.RoleCode.Contains(RoleCodes.ADMIN))
				{
					filter.statussaleid = StatusSaleModel.BranchReturnLCenter;
				}
			}

			var data = await _salesViewModel.GetList(filter);
			if (data != null && data.Status)
			{
				Items = data.Data?.Items;
				Pager = data.Data?.Pager;
				if (Pager != null)
				{
					Pager.UrlAction = "/dashboard/returneditems";
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

		protected async Task OnChain(ChangeEventArgs e)
		{
			filter.chain = null;
			if (e.Value != null)
			{
				filter.chain = e.Value.ToString();

				await SetModel();
				StateHasChanged();
				_Navs.NavigateTo($"{Pager?.UrlAction}?{filter.SetParameter(true)}");
			}
		}

		protected async Task OnBusinessType(ChangeEventArgs e)
		{
			filter.businesstype = null;
			if (e.Value != null)
			{
				filter.businesstype = e.Value.ToString();

				await SetModel();
				StateHasChanged();
				_Navs.NavigateTo($"{Pager?.UrlAction}?{filter.SetParameter(true)}");
			}
		}

		protected async Task OnProvince(ChangeEventArgs e)
		{
			filter.provinceid = null;
			if (e.Value != null)
			{
				if (int.TryParse(e.Value.ToString(), out int id))
				{
					filter.provinceid = id;
				}

				await SetModel();
				StateHasChanged();
				_Navs.NavigateTo($"{Pager?.UrlAction}?{filter.SetParameter(true)}");
			}
		}

		protected async Task OnStatusSale(ChangeEventArgs e)
		{
			filter.statussaleid = null;
			if (e.Value != null)
			{
				if (int.TryParse(e.Value.ToString(), out int saleid))
				{
					filter.statussaleid = saleid;
				}
			}

			await SetModel();
			StateHasChanged();
			_Navs.NavigateTo($"{Pager?.UrlAction}?{filter.SetParameter(true)}");
		}

		[JSInvokable]
		public async Task OnDepBranch(string _id, string _name)
		{
			filter.DepBranch = new();
			filter.provinceid = null;
			LookUp.Provinces = new();
			LookUp.Branchs = new();
			StateHasChanged();

			await _jsRuntimes.InvokeVoidAsync("BootSelectEmptyID", "Province");
			await _jsRuntimes.InvokeVoidAsync("BootSelectEmptyID", "Branch");

			if (_id != null && Guid.TryParse(_id, out Guid dep_BranchId))
			{
				filter.DepBranch.Add(dep_BranchId.ToString());

				var dataProvince = await _masterViewModel.GetProvince(dep_BranchId);
				if (dataProvince != null && dataProvince.Status)
				{
					if (dataProvince.Data != null && dataProvince.Data.Count > 0)
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
			}
		}

		[JSInvokable]
		public async Task OnProvince(string _provinceID, string _provinceName)
		{
			LookUp.Branchs = new();
			filter.provinceid = null;
			filter.Branchs = new();
			StateHasChanged();
			await Task.Delay(1);

			await _jsRuntimes.InvokeVoidAsync("BootSelectEmptyID", "Branch");

			if (_provinceID != null && int.TryParse(_provinceID, out int provinceID) && provinceID > 0)
			{
				filter.provinceid = provinceID;

				var dataBranchs = await _masterViewModel.GetBranch(provinceID);
				if (dataBranchs != null && dataBranchs.Status)
				{
					if (dataBranchs.Data?.Count > 0)
					{
						LookUp.Branchs = new() { new() { BranchID = 0, BranchName = "ทั้งหมด" } };
						LookUp.Branchs.AddRange(dataBranchs.Data);
						StateHasChanged();
						await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "OnBranch", "#Branch");
						await _jsRuntimes.InvokeVoidAsync("BootSelectRefreshID", "Branch", 100);
					}
				}
				else
				{
					_errorMessage = dataBranchs?.errorMessage;
					_utilsViewModel.AlertWarning(_errorMessage);
				}
			}
		}

		[JSInvokable]
		public async Task OnBranch(string _ids, string _name)
		{
			LookUp.RMUser = new();
			filter.Branchs = new();
			filter.RMUser = new();
			StateHasChanged();
			await Task.Delay(1);

			if (_ids != null)
			{
				filter.Branchs.Add(_ids);
			}
		}

	}
}