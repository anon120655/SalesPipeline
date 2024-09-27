using Microsoft.JSInterop;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Customers;
using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Pages.Settings.PreApprove
{
	public partial class RateType
	{

		string? _errorMessage = null;
		private User_PermissionCustom _permission = new();
		private bool isLoading = false;
		private allFilter filter = new();
		private List<Master_Pre_Interest_RateTypeCustom>? Items;
		public Pager? Pager;

		protected override async Task OnInitializedAsync()
		{
			_permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.SetPreApprove) ?? new User_PermissionCustom();
			StateHasChanged();
			await Task.Delay(1);
		}

		protected async override Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				await SetQuery();
				StateHasChanged();

				firstRender = false;
			}
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
			filter.id = Guid.Parse("11e23023-18cd-11ef-93aa-30e37aef72fb");
			var data = await _masterViewModel.GetPre_RateType(filter);
			if (data != null && data.Status)
			{
				Items = data.Data?.Items;
				Pager = data.Data?.Pager;
				if (Pager != null)
				{
					Pager.UrlAction = "/setting/pre/ratetype";
				}
			}
			else
			{
				_errorMessage = data?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

			StateHasChanged();
		}

		protected async Task Save()
		{
			_errorMessage = null;
			ShowLoading();

			if (Items == null || Items.Count == 0)
			{
				_errorMessage = "ไม่พบข้อมูลประเภทอัตราดอกเบี้ย";
				await _jsRuntimes.InvokeVoidAsync("WarningAlert", _errorMessage);
			}

			if (Items != null && Items.Count > 0)
			{
				foreach (var item in Items)
				{
					item.CurrentUserId = UserInfo.Id;
				}
			}

			var response = await _masterViewModel.UpdatePre_RateType(Items);

			if (response.Status)
			{
				HideLoading();
				await _jsRuntimes.InvokeVoidAsync("SuccessAlert");

				Items = new();
				StateHasChanged();
				await SetModel();
				StateHasChanged();
			}
			else
			{
				HideLoading();
				_errorMessage = response.errorMessage;
				await _jsRuntimes.InvokeVoidAsync("WarningAlert", _errorMessage);
			}
		}

		protected void ShowLoading()
		{
			isLoading = true;
			StateHasChanged();
		}

		protected void HideLoading()
		{
			isLoading = false;
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

	}
}