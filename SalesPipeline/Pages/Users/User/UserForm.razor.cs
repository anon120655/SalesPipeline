
using global::Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.ViewModels;

namespace SalesPipeline.Pages.Users.User
{
	public partial class UserForm
	{
		[Parameter]
		public int? id { get; set; }

		private string? _errorMessage = null;
		private bool isLoading = false;
		private bool isLoadingContent = false;
		private User_PermissionCustom _permission = new();
		private LookUpResource LookUp = new();
		private List<User_RoleCustom>? ItemsUserRole;
		private UserCustom formModel = new();

		protected override async Task OnInitializedAsync()
		{
			isLoadingContent = true;
			_permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.RMUser) ?? new User_PermissionCustom();
			StateHasChanged();

			await SetInitManual();
			await SetModel();
		}

		protected async override Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				await _jsRuntimes.InvokeVoidAsync("selectPickerInitialize");
				firstRender = false;
			}
		}

		protected async Task SetModel()
		{
			if (id.HasValue)
			{
				var data = await _userViewModel.GetById(id.Value);
				if (data != null && data.Status && data.Data != null)
				{
					formModel = data.Data;
					isLoadingContent = false;
					StateHasChanged();
				}
				else
				{
					_errorMessage = data?.errorMessage;
					_utilsViewModel.AlertWarning(_errorMessage);
				}
			}
			else
			{
				isLoadingContent = false;
			}
		}

		protected async Task SetInitManual()
		{
			var dataPosition = await _masterViewModel.Positions(new allFilter() { status = StatusModel.Active, type = UserTypes.User });
			if (dataPosition != null && dataPosition.Status)
			{
				LookUp.Positions = dataPosition.Data;
			}
			else
			{
				_errorMessage = dataPosition?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

			var dataGetDivLoans = await _masterViewModel.GetDepLoans(new allFilter() { status = StatusModel.Active });
			if (dataGetDivLoans != null && dataGetDivLoans.Status)
			{
				LookUp.DepartmentLoan = dataGetDivLoans.Data?.Items;
			}
			else
			{
				_errorMessage = dataGetDivLoans?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

			var dataGetDivBranchs = await _masterViewModel.GetDepBranchs(new allFilter() { status = StatusModel.Active });
			if (dataGetDivBranchs != null && dataGetDivBranchs.Status)
			{
				LookUp.DepartmentBranch = dataGetDivBranchs.Data?.Items;
			}
			else
			{
				_errorMessage = dataGetDivBranchs?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

			var dataBranchs = await _masterViewModel.Branchs(new allFilter() { status = StatusModel.Active });
			if (dataBranchs != null && dataBranchs.Status)
			{
				LookUp.Branchs = dataBranchs.Data;
			}
			else
			{
				_errorMessage = dataBranchs?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

			var dataLevels = await _userViewModel.GetListLevel(new allFilter() { status = StatusModel.Active });
			if (dataLevels != null && dataLevels.Status)
			{
				if (dataLevels.Data?.Count > 0)
				{
					LookUp.UserLevels = dataLevels.Data.Where(x => x.Id >= 4 && x.Id <= 12).ToList();
				}
			}
			else
			{
				_errorMessage = dataLevels?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}


			StateHasChanged();
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

			ResultModel<UserCustom> response;

			formModel.CurrentUserId = UserInfo.Id;

			if (id.HasValue)
			{
				response = await _userViewModel.Update(formModel);
			}
			else
			{
				response = await _userViewModel.Create(formModel);
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
			_Navs.NavigateTo("/user");
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