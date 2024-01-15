using global::Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Masters;

namespace SalesPipeline.Pages.Settings.Division
{
	public partial class SettingDivLoanForm
	{
		[Parameter]
		public Guid? id { get; set; }

		string? _errorMessage = null;
		private bool isLoading = false;
		private LookUpResource LookUp = new();
		private User_PermissionCustom _permission = new();
		private Master_Division_LoanCustom formModel = new();

		protected override async Task OnInitializedAsync()
		{
			_permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.SetDivLoan) ?? new User_PermissionCustom();
			StateHasChanged();

			await SetInitManual();
			await SetModel();
		}

		protected async override Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				await _jsRuntimes.InvokeVoidAsync("selectPickerInitialize");
				StateHasChanged();
				firstRender = false;
			}
		}

		protected async Task SetInitManual()
		{
			var dataPosition = await _masterViewModel.GetDivBranchs(new allFilter() { status = StatusModel.Active });
			if (dataPosition != null && dataPosition.Status)
			{
				LookUp.DivisionBranch = dataPosition.Data?.Items;
			}
			else
			{
				_errorMessage = dataPosition?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}
			StateHasChanged();
		}

		protected async Task SetModel()
		{
			if (id.HasValue)
			{
				var data = await _masterViewModel.GetDivLoansById(id.Value);
				if (data != null && data.Status && data.Data != null)
				{
					formModel = data.Data;
				}
				else
				{
					_errorMessage = data?.errorMessage;
					_utilsViewModel.AlertWarning(_errorMessage);
				}
			}
		}

		protected async Task OnInvalidSubmit()
		{
			await Task.Delay(100);
			await _jsRuntimes.InvokeVoidAsync("scrollToElement", "validation-message");
		}

		protected async Task Save()
		{
			_errorMessage = null;
			ShowLoading();

			ResultModel<Master_Division_LoanCustom> response;

			formModel.CurrentUserId = UserInfo.Id;

			if (id.HasValue)
			{
				response = await _masterViewModel.UpdateDivLoans(formModel);
			}
			else
			{
				response = await _masterViewModel.CreateDivLoans(formModel);
			}

			if (response.Status)
			{
				await _jsRuntimes.InvokeVoidAsync("SuccessAlert");
				Cancel();
			}
			else
			{
				HideLoading();
				_errorMessage = response.errorMessage;
				await _jsRuntimes.InvokeVoidAsync("WarningAlert", _errorMessage);
			}
		}

		public void Cancel()
		{
			_Navs.NavigateTo("/setting/div/loan");
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
	
	}
}