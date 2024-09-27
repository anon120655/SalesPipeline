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
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Pages.Settings.PreApprove
{
	public partial class ChancePass
	{
		public Guid? id { get; set; }

		string? _errorMessage = null;
		private allFilter filter = new();
		private bool isLoading = false;
		private User_PermissionCustom _permission = new();
		private List<Pre_ChancePassCustom>? Items;
		private Pre_ChancePassCustom formModel = new();
		private System_ConfigCustom configModel = new();

		Modal modalForm = default!;

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
			var data_config = await _systemViewModel.GetConfigByCode(ConfigCode.CHANCEPASS_Z);
			if (data_config != null && data_config.Status && data_config.Data != null)
			{
				configModel = data_config.Data;
			}

			filter.pagesize = 300;
			var data = await _preChanceViewModel.GetList(filter);
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
			var data = await _preChanceViewModel.GetById(id);
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

			ResultModel<Pre_ChancePassCustom> response;

			formModel.CurrentUserId = UserInfo.Id;

			response = await _preChanceViewModel.Update(formModel);

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