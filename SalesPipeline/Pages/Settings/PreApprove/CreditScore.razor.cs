using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SalesPipeline.Shared.Modals;
using SalesPipeline.Utils;
using SalesPipeline.Utils.ConstTypeModel;
using SalesPipeline.Utils.DataCustom;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.ManageSystems;
using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.PreApprove;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Pages.Settings.PreApprove
{
	public partial class CreditScore
	{
		public Guid? id { get; set; }

		string? _errorMessage = null;
		private allFilter filter = new();
		private bool isLoading = false;
		private User_PermissionCustom _permission = new();
		private List<Pre_CreditScoreCustom>? Items;
		private Pre_CreditScoreCustom formModel = new();
		private System_ConfigCustom configModel = new();

		Modal modalForm = default!;
		ModalConfirm modalConfirm = default!;

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
				await SetModel();
				StateHasChanged();
				firstRender = false;
			}
		}

		protected async Task SetModel()
		{
			var data_config = await _systemViewModel.GetConfigByCode(ConfigCode.CREDITSCORE_LM_MT);
			if (data_config != null && data_config.Status && data_config.Data != null)
			{
				configModel = data_config.Data;
			}

			filter.pagesize = 300;
			var data = await _preCreditViewModel.GetList(filter);
			if (data != null && data.Status)
			{
				Items = data.Data?.Items;
			}
			else
			{
				_errorMessage = data?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}
		}

		protected async Task SetModelById(Guid id)
		{
			var data = await _preCreditViewModel.GetById(id);
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

			ResultModel<Pre_CreditScoreCustom> response;

			formModel.CurrentUserId = UserInfo.Id;

			if (id.HasValue && id.Value != Guid.Empty)
			{
				response = await _preCreditViewModel.Update(formModel);
			}
			else
			{
				response = await _preCreditViewModel.Create(formModel);
			}

			if (response.Status)
			{
				await _jsRuntimes.InvokeVoidAsync("SuccessAlert");
				HideLoading();
				await OnHide();
				await SetModel();
			}
			else
			{
				HideLoading();
				_errorMessage = response.errorMessage;
				await _jsRuntimes.InvokeVoidAsync("WarningAlert", _errorMessage);
			}
		}

		private async Task OnShow(Guid? _id = null)
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
			await modalForm.ShowAsync();
		}

		private async Task OnHide()
		{
			id = null;
			await modalForm.HideAsync();
		}

		protected async Task ConfirmDelete(string? id, string? txt)
		{
			await modalConfirm.OnShowConfirm(id, $"คุณต้องการลบข้อมูล <span class='text-primary'>{txt}</span>");
		}

		protected async Task Delete(string id)
		{
			await modalConfirm.OnHideConfirm();

			var data = await _preCreditViewModel.DeleteById(new UpdateModel() { id = id, userid = UserInfo.Id });
			if (data != null && !data.Status && !String.IsNullOrEmpty(data.errorMessage))
			{
				_errorMessage = data?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}
			await SetModel();
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