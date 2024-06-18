
using global::Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Forms;
using SalesPipeline.Utils.ConstTypeModel;

namespace SalesPipeline.Pages.Users.Admin
{
    public partial class AdminUserForm
	{
		[Parameter]
		public int? id { get; set; }

		private string? _errorMessage = null;
		private bool isLoading = false;
		private User_PermissionCustom _permission = new();
		private LookUpResource LookUp = new();
		private List<User_RoleCustom>? ItemsUserRole;
		private UserCustom formModel = new();

		protected override async Task OnInitializedAsync()
		{
			_permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.LoanUser) ?? new User_PermissionCustom();
			StateHasChanged();

			await SetInitManual();
		}

		protected async override Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				await SetModel();
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
					if (formModel.RoleId.HasValue)
					{
						await OnRoles(formModel.RoleId,formModel.LevelId);
					}
				}
				else
				{
					_errorMessage = data?.errorMessage;
					_utilsViewModel.AlertWarning(_errorMessage);
				}
			}
		}

		protected async Task SetInitManual()
		{
			var dataPosition = await _masterViewModel.Positions(new allFilter() { status = StatusModel.Active, type = UserTypes.Admin });
			if (dataPosition != null && dataPosition.Status)
			{
				LookUp.Positions = dataPosition.Data;
			}
			else
			{
				_errorMessage = dataPosition?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

			var dataDepartments = await _masterViewModel.GetDepartments(new allFilter() { status = StatusModel.Active });
			if (dataDepartments != null && dataDepartments.Status)
			{
				LookUp.Departments = dataDepartments.Data?.Items;
			}
			else
			{
				_errorMessage = dataDepartments?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

			var data = await _userViewModel.GetListRole(new allFilter() { pagesize = 50, status = StatusModel.Active });
			if (data != null && data.Status)
			{
				ItemsUserRole = data.Data?.Items.Where(x => x.Code.Contains(RoleCodes.LOAN)).ToList();
			}
			else
			{
				_errorMessage = data?.errorMessage;
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
			_Navs.NavigateTo("/admin");
		}

		protected async Task OnRoles(object? val, int? levelId = null)
		{
			formModel.RoleId = null;
			formModel.LevelId = levelId;
			LookUp.UserLevels = new();
			StateHasChanged();

			if (val != null && int.TryParse(val.ToString(), out int roleid))
			{
				formModel.RoleId = roleid;
				StateHasChanged();

				var dataLevels = await _userViewModel.GetListLevel(new allFilter() { status = StatusModel.Active });
				if (dataLevels != null && dataLevels.Status)
				{
					if (dataLevels.Data != null && dataLevels.Data.Count > 0)
					{
						if (formModel.RoleId == 3) //สายงานธุรกิจสินเชื่อ 10-12
						{
							LookUp.UserLevels = dataLevels.Data.Where(x => x.Id >= 10 && x.Id <= 12).ToList();
						}
						else if (formModel.RoleId == 4) //สายงานธุรกิจสินเชื่อ 4-9
						{
							LookUp.UserLevels = dataLevels.Data.Where(x => x.Id >= 4 && x.Id <= 9).ToList();
						}

						StateHasChanged();
					}
				}
				else
				{
					_errorMessage = dataLevels?.errorMessage;
					_utilsViewModel.AlertWarning(_errorMessage);
				}

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

		private void OnInputTel(KeyboardEventArgs e)
		{
			if (formModel.Tel != null)
			{
				formModel.Tel = formModel.Tel.Substring(0, 10);
			}
		}

	}
}