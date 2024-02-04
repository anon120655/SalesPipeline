using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Assignments;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Pages.Assigns.Loans
{
	public partial class AssignLoan
	{
		string? _errorMessage = null;
		private bool isLoading = false;
		private User_PermissionCustom _permission = new();
		private allFilter filter = new();
		private LookUpResource LookUp = new();
		private List<AssignmentCustom>? Items;
		public Pager? Pager;
		int stepAssign = StepAssignLoanModel.Home;

		protected override async Task OnInitializedAsync()
		{
			stepAssign = StepAssignLoanModel.Home;
			_permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.AssignLoan) ?? new User_PermissionCustom();
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
			await _jsRuntimes.InvokeVoidAsync("BootSelectId", "BusinessType");
			await _jsRuntimes.InvokeVoidAsync("BootSelectId", "Province");
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
			filter.pagesize = 100;
			var data = await _assignmentViewModel.GetListAutoAssign(filter);
			if (data != null && data.Status)
			{
				Items = data.Data?.Items;
				Pager = data.Data?.Pager;
				if (Pager != null)
				{
					Pager.UrlAction = "/assign/loan";
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

		protected async Task Assign()
		{
			_errorMessage = null;
			ShowLoading();

			if (Items != null)
			{
				var response = await _assignmentViewModel.Assign(Items);

				if (response.Status)
				{
					await _jsRuntimes.InvokeVoidAsync("SuccessAlert");
					await SetModel();
					HideLoading();
				}
				else
				{
					HideLoading();
					_errorMessage = response.errorMessage;
					await _jsRuntimes.InvokeVoidAsync("WarningAlert", _errorMessage);
				}
			}

		}

		protected async Task Search()
		{
			await SetModel();
			StateHasChanged();
			_Navs.NavigateTo($"{Pager?.UrlAction}?{filter.SetParameter(true)}");
		}

		protected async Task OnProvince(ChangeEventArgs e)
		{
			filter.province = null;
			if (e.Value != null)
			{
				filter.province = e.Value.ToString();

				await SetModel();
				StateHasChanged();
				_Navs.NavigateTo($"{Pager?.UrlAction}?{filter.SetParameter(true)}");
			}
		}

		protected async Task GotoStep(int step)
		{
			stepAssign = step;
			StateHasChanged();
			await Task.Delay(10);
		}


	}
}