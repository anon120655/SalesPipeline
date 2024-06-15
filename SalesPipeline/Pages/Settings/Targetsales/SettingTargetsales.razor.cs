using Microsoft.JSInterop;
using SalesPipeline.Shared.Modals;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils;
using Microsoft.AspNetCore.Components;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Assignments;
using System.Linq;
using BlazorBootstrap;

namespace SalesPipeline.Pages.Settings.Targetsales
{
	public partial class SettingTargetsales
	{

		string? _errorMessage = null;
		private User_PermissionCustom _permission = new();
		private allFilter filter = new();
		private LookUpResource LookUp = new();
		private List<UserCustom>? Items;
		private List<UserCustom> ItemsSelected = new();
		public Pager? Pager;
		public bool isSetting = false;
		public decimal amountTarget { get; set; } = 0;
		protected Modal modalTarget = default!;

		protected override async Task OnInitializedAsync()
		{
			_permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.SetTargetsales) ?? new User_PermissionCustom();
			StateHasChanged();
			await Task.Delay(1);
		}

		protected async override Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				filter.userid = UserInfo.Id;

				await SetInitManual();
				StateHasChanged();
				await _jsRuntimes.InvokeVoidAsync("BootSelectClass", "selectInit");

				await SetQuery();
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

			var dataYear = await _masterViewModel.GetYear(new allFilter() { status = StatusModel.Active });
			if (dataYear != null && dataYear.Status)
			{
				//LookUp.Years = new() { new() { Id = Guid.Empty, Name = "ทั้งหมด" } };
				LookUp.Years = new();
				if (dataYear.Data?.Count > 0)
				{
					LookUp.Years.AddRange(dataYear.Data);
					StateHasChanged();
					await Task.Delay(1);
					//await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "OnYear", "#Year");
				}
			}
			else
			{
				_errorMessage = dataYear?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}
		}

		protected async Task SetQuery(string? parematerAll = null)
		{
			string uriQuery = String.Empty;

			uriQuery = _Navs.ToAbsoluteUri(_Navs.Uri).Query;

			if (parematerAll != null)
				uriQuery = $"?{parematerAll}";

			filter.SetUriQuery(uriQuery);

			if (String.IsNullOrEmpty(filter.year))
			{
				filter.year = DateTime.Now.Year.ToString();
			}

			await SetModel();
			StateHasChanged();
		}

		protected async Task SetModel()
        {
            filter.page = 1;
            var data = await _userViewModel.GetUserTargetList(filter);
			if (data != null && data.Status)
			{
				Items = data.Data?.Items;
				Pager = data.Data?.Pager;
				if (Pager != null)
				{
					Pager.UrlAction = "/setting/targetsales";
				}

				if (Items != null && ItemsSelected.Count > 0)
				{
					foreach (var item in Items)
					{
						if (ItemsSelected.Select(x => x.Id).Contains(item.Id))
						{
							item.IsSelected = true;
						}
					}
				}
			}
			else
			{
				_errorMessage = data?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

			StateHasChanged();
		}

		protected async Task Search()
		{
			await SetModel();
			StateHasChanged();
			_Navs.NavigateTo($"{Pager?.UrlAction}?{filter.SetParameter(true)}");
		}

		protected async Task SetSetting()
		{
			ItemsSelected = new();
			isSetting = !isSetting;
			StateHasChanged();
			await Task.Delay(1);
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

		[JSInvokable]
		public async Task OnDepBranch(string _id, string _name)
		{
			filter.DepBranchs = new();
			filter.Provinces = new();
			LookUp.Provinces = new();
			LookUp.Branchs = new();
			StateHasChanged();

			await _jsRuntimes.InvokeVoidAsync("BootSelectEmptyID", "Province");
			await _jsRuntimes.InvokeVoidAsync("BootSelectEmptyID", "Branch");

			if (_id != null && Guid.TryParse(_id, out Guid dep_BranchId))
			{
				filter.DepBranchs.Add(dep_BranchId.ToString());

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
			filter.Provinces = new();
			filter.Branchs = new();
			StateHasChanged();
			await Task.Delay(1);

			await _jsRuntimes.InvokeVoidAsync("BootSelectEmptyID", "Branch");

			if (_provinceID != null && int.TryParse(_provinceID, out int provinceID) && provinceID > 0)
			{
				filter.Provinces.Add(provinceID.ToString());

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
			filter.RMUsers = new();
			StateHasChanged();
			await Task.Delay(1);

			if (_ids != null)
			{
				filter.Branchs.Add(_ids);
			}
		}

		public async Task OnYear(ChangeEventArgs e)
		{
			filter.year = null;
			StateHasChanged();
			await Task.Delay(1);

			if (e.Value != null)
			{
				if (int.TryParse(e.Value.ToString(), out int year))
				{
					filter.year = year.ToString();
				}
			}

			await SetModel();
			StateHasChanged();
			_Navs.NavigateTo($"{Pager?.UrlAction}?{filter.SetParameter(true)}");
		}

		protected void OnCheckCustomer(UserCustom model, object? checkedValue)
		{
			if (checkedValue != null && (bool)checkedValue)
			{
				model.IsSelected = true;
				ItemsSelected.Add(model);
			}
			else
			{
				model.IsSelected = false;
				ItemsSelected.Remove(model);
			}
		}

		private async Task OnModalHidden()
		{
			await Task.Delay(1);
		}

		public async Task OnShowModal()
		{
			amountTarget = 0;
			StateHasChanged();
			await Task.Delay(1);
			await modalTarget.ShowAsync();
		}

		public async Task OnHideModal()
		{
			await modalTarget.HideAsync();
		}


		protected async Task Save()
		{
			_errorMessage = null;

			if (ItemsSelected != null && ItemsSelected.Count > 0 && int.TryParse(filter.year,out int year))
			{
				List<User_Target_SaleCustom> itemsTarget = new();
				foreach (var item in ItemsSelected)
				{
					if (!itemsTarget.Select(x => x.UserId).Contains(item.Id))
					{
						itemsTarget.Add(new()
						{
							UserId = item.Id,
							Year = year,
							AmountTarget = amountTarget
						});
					}
				}

				var response = await _userViewModel.UpdateUserTarget(new()
				{
					CurrentUserId = UserInfo.Id,
					ItemsTarget = itemsTarget
				});

				if (response.Status)
				{
					await _jsRuntimes.InvokeVoidAsync("SuccessAlert");
					await OnHideModal();
					await SetSetting();
					await SetModel();
				}
				else
				{
					_errorMessage = response.errorMessage;
					await _jsRuntimes.InvokeVoidAsync("WarningAlert", _errorMessage);
				}
			}
		}

	}
}