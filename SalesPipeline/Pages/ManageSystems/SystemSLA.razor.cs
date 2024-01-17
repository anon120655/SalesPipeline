using BlazorBootstrap;
using Microsoft.JSInterop;
using SalesPipeline.Shared.Modals;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Customers;
using SalesPipeline.Utils.Resources.ManageSystems;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Pages.ManageSystems
{
	public partial class SystemSLA
	{
		public Guid? id { get; set; }

		string? _errorMessage = null;
		private bool isLoading = false;
		private User_PermissionCustom _permission = new();
		private List<System_SLACustom>? Items;
		private System_SLACustom formModel = new();

		Modal modalSLA = default!;
		ModalConfirm modalConfirm = default!;

		protected override async Task OnInitializedAsync()
		{
			_permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.Systems) ?? new User_PermissionCustom();
			StateHasChanged();

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

		protected async Task SetModel()
		{
			var data = await _systemViewModel.GetListSLA(new allFilter()
			{
				pagesize = 30
			});
			if (data != null && data.Status)
			{
				Items = data.Data?.Items;
			}
			else
			{
				_errorMessage = data?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

			StateHasChanged();
		}

		protected async Task SetModelById(Guid id)
		{
			var data = await _systemViewModel.GetSLAById(id);
			if (data != null && data.Status)
			{
				if (data.Data != null)
				{
					formModel = data.Data;
				}
			}
			else
			{
				_errorMessage = data?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

			StateHasChanged();
		}

		private async Task Save()
		{
			_errorMessage = null;
			ShowLoading();

			ResultModel<System_SLACustom> response;

			formModel.CurrentUserId = UserInfo.Id;

			if (id.HasValue && id != Guid.Empty)
			{
				response = await _systemViewModel.UpdateSLA(formModel);
			}
			else
			{
				response = await _systemViewModel.CreateSLA(formModel);
			}

			if (response.Status)
			{
				await _jsRuntimes.InvokeVoidAsync("SuccessAlert");
				HideLoading();
				await OnHideSLA();
				await SetModel();
			}
			else
			{
				HideLoading();
				_errorMessage = response.errorMessage;
				await _jsRuntimes.InvokeVoidAsync("WarningAlert", _errorMessage);
			}

		}

		protected async Task ConfirmDelete(string? id, string? txt)
		{
			await modalConfirm.OnShowConfirm(id, $"คุณต้องการลบข้อมูล <span class='text-primary'>{txt}</span>");
		}

		protected async Task Delete(string id)
		{
			await modalConfirm.OnHideConfirm();

			var data = await _systemViewModel.DeleteSLAById(new UpdateModel() { id = id, userid = UserInfo.Id });
			if (data != null && !data.Status && !String.IsNullOrEmpty(data.errorMessage))
			{
				_errorMessage = data?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}
			await SetModel();
		}

		private async Task OnShowSLA(Guid? _id = null)
		{
			if (_id.HasValue && _id.Value != Guid.Empty)
			{
				id = _id;
				await SetModelById(_id.Value);
			}
			else
			{
				id = null;
				formModel = new();
			}
			await modalSLA.ShowAsync();
		}

		private async Task OnHideSLA()
		{
			id = null;
			await modalSLA.HideAsync();
		}

		private void ShowLoading()
		{
			isLoading = true;
			StateHasChanged();
		}

		private void HideLoading()
		{
			isLoading = false;
			StateHasChanged();
		}


	}
}