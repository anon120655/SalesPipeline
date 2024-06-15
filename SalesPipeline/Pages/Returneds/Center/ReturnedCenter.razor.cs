using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SalesPipeline.Shared.Modals;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Assignments;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Customers;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Pages.Returneds.Center
{
	public partial class ReturnedCenter
	{
		string? _errorMessage = null;
		string? _errorMessageModal = null;
		private bool isLoading = false;
		private User_PermissionCustom _permission = new();
		private LookUpResource LookUp = new();
		private allFilter filter = new();
		private List<SaleCustom>? Items;
		public Pager? Pager;

		ModalReturnAssign modalReturnAssign = default!;
		private bool IsToClose = false;
		private string? pathToNext = null;


		protected override async Task OnInitializedAsync()
		{
			_permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.ReturnedCenter) ?? new User_PermissionCustom();
			StateHasChanged();
			await Task.Delay(1);
		}

		protected async override Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				await SetInitManual();

				await SetQuery();
				StateHasChanged();

				await _jsRuntimes.InvokeVoidAsync("selectPickerInitialize");
				StateHasChanged();
				firstRender = false;
			}
		}

		protected async Task SetInitManual()
		{
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
			await _jsRuntimes.InvokeVoidAsync("BootSelectId", "Chain");
			await _jsRuntimes.InvokeVoidAsync("BootSelectId", "BusinessType");
			await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "OnProvince", "#Province");
		}

		protected async Task SetQuery(string? parematerAll = null)
		{
			var uriQuery = _Navs.ToAbsoluteUri(_Navs.Uri).Query;

			if (parematerAll != null)
				uriQuery = $"?{parematerAll}";

			filter.SetUriQuery(uriQuery);

			await SetModel();
			StateHasChanged();
		}

		protected async Task SetModel()
        {
            filter.page = 1;
            filter.userid = UserInfo.Id;
			filter.statussaleid = StatusSaleModel.RMReturnMCenter;
			var data = await _salesViewModel.GetList(filter);
			if (data != null && data.Status)
			{
				Items = data.Data?.Items;
				Pager = data.Data?.Pager;
				if (Pager != null)
				{
					Pager.UrlAction = "/return/center";
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

		private async Task OnModalReturnHidden()
		{
			await Task.Delay(1);
			if (!String.IsNullOrEmpty(pathToNext))
			{
				_Navs.NavigateTo(pathToNext);
			}
		}

		protected async Task InitShowReturnAssign(string? id)
		{
			pathToNext = null;
			_errorMessageModal = null;
			await ShowReturnAssign(id, "ท่านต้องการ \"ส่งคืน\" หรือ \"มอบหมาย\" ");
		}

		protected async Task ShowReturnAssign(string? id, string? txt, string? icon = null)
		{
			//IsToClose = false;
			await modalReturnAssign.OnShowConfirm(id, $"{txt}", icon);
		}

		protected async Task ReturnAssign(SelectModel model)
		{
			pathToNext = null;
			if (model.Value == "0")
			{
				pathToNext = $"/return/center/branch/{model.ID}";
			}
			else if (model.Value == "1")
			{
				pathToNext = $"/return/center/summary/{model.ID}";
			}
			await modalReturnAssign.OnHide();
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
			filter.RMUsers = new();
			StateHasChanged();
			await Task.Delay(1);

			if (_ids != null)
			{
				filter.Branchs.Add(_ids);
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
			}
		}

		protected void OnReturnDate(ChangeEventArgs e)
		{
			if (e != null && e.Value != null && filter != null)
			{
				if (!String.IsNullOrEmpty(e.Value.ToString()))
				{
					filter.returndate = GeneralUtils.DateNotNullToEn(e.Value.ToString(), "yyyy-MM-dd", Culture: "en-US");
				}
				else
				{
					filter.returndate = null;
				}
			}
		}

		protected async Task Search()
		{
			await SetModel();
			StateHasChanged();
		}

	}
}