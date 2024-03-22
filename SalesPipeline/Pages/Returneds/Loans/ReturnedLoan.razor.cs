using Microsoft.JSInterop;
using SalesPipeline.Shared.Modals;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Pages.Returneds.Loans
{
	public partial class ReturnedLoan
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
			_permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.ReturnedLoan) ?? new User_PermissionCustom();
			StateHasChanged();

		}

		protected async override Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				await SetQuery();
				StateHasChanged();

				await _jsRuntimes.InvokeVoidAsync("selectPickerInitialize");
				StateHasChanged();
				firstRender = false;
			}
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
			filter.statussaleid = StatusSaleModel.BranchReturnLCenter;
			var data = await _salesViewModel.GetList(filter);
			if (data != null && data.Status)
			{
				Items = data.Data?.Items;
				Pager = data.Data?.Pager;
				if (Pager != null)
				{
					Pager.UrlAction = "/return/loan";
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
				//pathToNext = $"/return/branch/loan/{model.ID}";
			}
			else if (model.Value == "1")
			{
				pathToNext = $"/return/loan/summary/{model.ID}";
			}
			await modalReturnAssign.OnHide();
		}


	}
}