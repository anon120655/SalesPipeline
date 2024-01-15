using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SalesPipeline.Shared.Modals;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Pages.Users.Loans
{
	public partial class LoanPermission
	{
		public int? id { get; set; }
		string? _errorMessage = null;
		bool isLoading = false;
		bool agree1;
		private User_PermissionCustom _permission = new();
		protected allFilter filter = new();
		private LookUpResource LookUp = new();
		private List<User_RoleCustom>? Items;
		private User_RoleCustom? formModel;
		//private UserRoleCustom formModel = new();
		public Pager? Pager;

		ModalConfirm modalConfirm = default!;

		protected override async Task OnInitializedAsync()
		{
			_permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.LoanPermission) ?? new User_PermissionCustom();
			StateHasChanged();
			await SetInitManual();
		}

		protected async override Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				await SetModel();
				StateHasChanged();
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
			var data = await _userViewModel.GetRoles(filter);
			if (data != null && data.Status)
			{
				Items = data.Data?.Items;
				Pager = data.Data?.Pager;
				if (Pager != null)
				{
					Pager.UrlAction = "/loans/permission";
				}
			}
			else
			{
				_errorMessage = data?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

			StateHasChanged();
		}

		protected async Task ConfirmDelete(string? id, string? txt)
		{
			await modalConfirm.OnShowConfirm(id, $"คุณต้องการลบข้อมูล <span class='text-primary'>{txt}</span>");
		}

		protected async Task Delete(string id)
		{
			await modalConfirm.OnHideConfirm();

			var data = await _userViewModel.DeleteRoleById(new UpdateModel() { id = id, userid = UserInfo.Id });
			if (data != null && !data.Status && !String.IsNullOrEmpty(data.errorMessage))
			{
				_errorMessage = data?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}
			await SetModel();
		}

		protected async Task ModifyRoleChanged(ChangeEventArgs e, int id)
		{
			if (e.Value != null && Boolean.TryParse(e.Value.ToString(), out bool parsedValue))
			{
				var data = await _userViewModel.UpdateIsModifyRoleById(new UpdateModel() { id = id.ToString(), userid = UserInfo.Id, value = parsedValue.ToString() });
				if (data != null && !data.Status && !String.IsNullOrEmpty(data.errorMessage))
				{
					_errorMessage = data?.errorMessage;
					_utilsViewModel.AlertWarning(_errorMessage);
				}
				else
				{
					string? actiontxt = parsedValue ? "<i class=\"fa-regular fa-circle-check\"></i> เปิด" : "<i class=\"fa-solid fa-circle-xmark\"></i> ปิด";
					string fulltxt = $"{actiontxt}อนุญาติให้แก้ไขเรียบร้อย";
					//_utilsViewModel.AlertSuccess(fulltxt);
					await _jsRuntimes.InvokeVoidAsync("SuccessAlert", fulltxt);
					await SetModel();
				}
			}
		}

		protected async Task OnAccess(int _id)
		{
			id = _id;
			if (Items != null)
			{
				foreach (var item in Items)
				{
					if (item.Id == _id)
						item.IsAccess = true;
					else
						item.IsAccess = false;
				}

				formModel = null;
				var data = await _userViewModel.GetRoleById(_id);
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

		protected void CheckChanged(object? valChecked, int menuNumber, int type)
		{
			if (id > 0 && formModel != null && valChecked != null)
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
				this.StateHasChanged();
			}
		}

		protected async Task Save()
		{
			_errorMessage = null;
			ShowLoading();

			ResultModel<User_RoleCustom> response;

			if (formModel != null)
			{
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
				}
				else
				{
					HideLoading();
					_errorMessage = response.errorMessage;
					await _jsRuntimes.InvokeVoidAsync("WarningAlert", _errorMessage);
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


	}
}