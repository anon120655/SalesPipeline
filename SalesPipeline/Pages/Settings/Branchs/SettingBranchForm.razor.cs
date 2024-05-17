using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils.Resources.Thailands;

namespace SalesPipeline.Pages.Settings.Branchs
{
	public partial class SettingBranchForm
	{
		[Parameter]
		public int? id { get; set; }

		string? _errorMessage = null;
		private bool isLoading = false;
		private User_PermissionCustom _permission = new();
		private InfoBranchCustom formModel = new();

		protected override async Task OnInitializedAsync()
		{
			_permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.SetBranch) ?? new User_PermissionCustom();
			StateHasChanged();
			await Task.Delay(1);
		}

		protected async override Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				await SetModel();
				StateHasChanged();

				await _jsRuntimes.InvokeVoidAsync("selectPickerInitialize");
				firstRender = false;
			}
		}

		protected async Task OnInvalidSubmit()
		{
			await Task.Delay(100);
			await _jsRuntimes.InvokeVoidAsync("scrollToElement", "validation-message");
		}

		protected async Task SetModel()
		{
			if (id.HasValue)
			{
				var data = await _masterViewModel.GetBranchById(id.Value);
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

		protected async Task Save()
		{
			_errorMessage = null;
			ShowLoading();

			ResultModel<InfoBranchCustom> response;

			formModel.CurrentUserId = UserInfo.Id;

			if (id.HasValue)
			{
				response = await _masterViewModel.UpdateBranch(formModel);
			}
			else
			{
				response = await _masterViewModel.CreateBranch(formModel);
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
			_Navs.NavigateTo("/setting/branch");
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