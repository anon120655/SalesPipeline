using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils;

namespace SalesPipeline.Pages.Users.Admin
{
	public partial class AdminPermissionForm
	{
		[Parameter]
		public int? id { get; set; }

		private string? _errorMessage = null;
		private bool isLoading = false;
		private User_PermissionCustom _permission = new();
		private LookUpResource LookUp = new();
		private User_RoleCustom formModel = new();

		protected override async Task OnInitializedAsync()
		{
			_permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.LoanPermission) ?? new User_PermissionCustom();
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

		protected async Task SetInitManual()
		{
			var dataMenuItem = await _masterViewModel.MenuItem(new allFilter() { status = StatusModel.Active });
			if (dataMenuItem != null && dataMenuItem.Status)
			{
				LookUp.MenuItem = dataMenuItem.Data;
			}
			else
			{
				_errorMessage = dataMenuItem?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

			StateHasChanged();
		}

		protected async Task SetModel()
		{
			if (id.HasValue)
			{
				var data = await _userViewModel.GetRoleById(id.Value);
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

			ResultModel<User_RoleCustom> response;

			formModel.CurrentUserId = UserInfo.Id;

			if (id.HasValue)
			{
				response = await _userViewModel.UpdateRole(formModel);
			}
			else
			{
				response = await _userViewModel.CreateRole(formModel);
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

		protected void Cancel()
		{
			_Navs.NavigateTo("/permission");
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

		protected void CheckChanged(object? valChecked, int menuNumber, int type)
		{
			if (valChecked != null)
			{
				bool isChecked = (bool)valChecked ? true : false;

				int roleId = 0;
				int.TryParse(id.ToString(), out roleId);

				if (formModel.User_Permissions == null) formModel.User_Permissions = new List<User_PermissionCustom>();

				var model = formModel.User_Permissions.FirstOrDefault(x => x.MenuNumber == menuNumber);
				if (model == null)
				{
					var permission = new User_PermissionCustom();
					permission.Status = StatusModel.Active;
					permission.MenuNumber = menuNumber;
					permission.RoleId = roleId;
					if (type == 1) permission.IsView = isChecked;

					formModel.User_Permissions.Add(permission);
				}
				else
				{
					if (type == 1) model.IsView = isChecked;
				}

				if (LookUp.MenuItem?.Count > 0)
				{
					var menuItem = LookUp.MenuItem.Where(x => x.ParentNumber == menuNumber).ToList();
					if (menuItem.Count > 0)
					{
						foreach (var item in menuItem)
						{
							var permission = new User_PermissionCustom();
							permission.Status = StatusModel.Active;
							permission.MenuNumber = item.MenuNumber;
							permission.RoleId = roleId;
							if (type == 1) permission.IsView = isChecked;
							formModel.User_Permissions.Add(permission);

							var menuItemL3 = LookUp.MenuItem.Where(x => x.ParentNumber == item.MenuNumber).ToList();
							if (menuItemL3.Count > 0)
							{
								foreach (var itemL3 in menuItemL3)
								{
									var permission3 = new User_PermissionCustom();
									permission3.Status = StatusModel.Active;
									permission3.MenuNumber = itemL3.MenuNumber;
									permission3.RoleId = roleId;
									if (type == 1) permission3.IsView = isChecked;
									formModel.User_Permissions.Add(permission3);
								}
							}
						}
					}
				}

				//UpdateIsAll(type);
				this.StateHasChanged();
			}
		}


	}
}