using global::Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SalesPipeline.Shared.Modals;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.ManageSystems;
using BlazorBootstrap;
using SalesPipeline.Utils;

namespace SalesPipeline.Pages.Settings.YieldChain
{
	public partial class SettingYieldChain
	{
		public Guid? idYield { get; set; }
		public Guid? idChain { get; set; }

		string? _errorMessage = null;
		private bool isLoading = false;
		private User_PermissionCustom _permission = new();
		private allFilter filter = new();
		private LookUpResource LookUp = new();
		private List<Master_YieldCustom>? ItemsYield;
		private List<Master_ChainCustom>? ItemsChain;
		private Master_YieldCustom formModelYield = new();
		private Master_ChainCustom formModelChain = new();

		Modal  modalFormYield = default!;
		Modal  modalFormChain = default!;

		ModalConfirm modalConfirmYield = default!;
		ModalConfirm modalConfirmChain = default!;

		protected override async Task OnInitializedAsync()
		{
			_permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.SetYieldChain) ?? new User_PermissionCustom();
			StateHasChanged();

			await SetModelYield();
			await SetModelChain();
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

		//ผลผลิต
		protected async Task SetModelYield()
		{
			var data = await _masterViewModel.GetYields(filter);
			if (data != null && data.Status)
			{
				ItemsYield = data.Data?.Items;
			}
			else
			{
				_errorMessage = data?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

		}

		protected async Task SetModelYieldById(Guid id)
		{
			var data = await _masterViewModel.GetYieldById(id);
			if (data != null && data.Status)
			{
				if (data.Data != null)
				{
					formModelYield = data.Data;
				}
			}
			else
			{
				_errorMessage = data?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

			StateHasChanged();
		}

		private async Task SaveYield()
		{
			_errorMessage = null;
			ShowLoading();

			ResultModel<Master_YieldCustom> response;

			formModelYield.CurrentUserId = UserInfo.Id;

			if (idYield.HasValue && idYield != Guid.Empty)
			{
				response = await _masterViewModel.UpdateYield(formModelYield);
			}
			else
			{
				response = await _masterViewModel.CreateYield(formModelYield);
			}

			if (response.Status)
			{
				await _jsRuntimes.InvokeVoidAsync("SuccessAlert");
				HideLoading();
				await OnHideYield();
				await SetModelYield();
			}
			else
			{
				HideLoading();
				_errorMessage = response.errorMessage;
				await _jsRuntimes.InvokeVoidAsync("WarningAlert", _errorMessage);
			}
		}

		protected async Task ConfirmDeleteYield(string? id, string? txt)
		{
			await modalConfirmYield.OnShowConfirm(id, $"คุณต้องการลบข้อมูล <span class='text-primary'>{txt}</span>");
		}

		protected async Task DeleteYield(string id)
		{
			await modalConfirmYield.OnHideConfirm();

			var data = await _masterViewModel.DeleteYieldById(new UpdateModel() { id = id, userid = UserInfo.Id });
			if (data != null && !data.Status && !String.IsNullOrEmpty(data.errorMessage))
			{
				_errorMessage = data?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}
			await SetModelYield();
		}

		protected async Task StatusYieldChanged(ChangeEventArgs e, Guid id)
		{
			if (e.Value != null && Boolean.TryParse(e.Value.ToString(), out bool val))
			{
				var data = await _masterViewModel.UpdateStatusYieldById(new UpdateModel() { id = id.ToString(), userid = UserInfo.Id, value = val.ToString() });
				if (data != null && !data.Status && !String.IsNullOrEmpty(data.errorMessage))
				{
					_errorMessage = data?.errorMessage;
					_utilsViewModel.AlertWarning(_errorMessage);
				}
				else
				{
					string? actiontxt = val ? "<i class=\"fa-regular fa-circle-check\"></i> เปิด" : "<i class=\"fa-solid fa-circle-xmark\"></i> ปิด";
					string fulltxt = $"{actiontxt}การใช้งานเรียบร้อย";
					await _jsRuntimes.InvokeVoidAsync("SuccessAlert", fulltxt);
					await SetModelYield();
				}
			}
		}

		private async Task OnShowYield(Guid? _id = null)
		{
			if (_id.HasValue && _id.Value != Guid.Empty)
			{
				idYield = _id;
				await SetModelYieldById(_id.Value);
			}
			else
			{
				idYield = null;
				formModelYield = new();
			}
			await modalFormYield.ShowAsync();
		}

		private async Task OnHideYield()
		{
			idYield = null;
			await modalFormYield.HideAsync();
		}

		//ห่วงโซ่
		protected async Task SetModelChain()
		{
			var data = await _masterViewModel.GetChains(filter);
			if (data != null && data.Status)
			{
				ItemsChain = data.Data?.Items;
			}
			else
			{
				_errorMessage = data?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

		}

		protected async Task SetModelChainById(Guid id)
		{
			var data = await _masterViewModel.GetChainById(id);
			if (data != null && data.Status)
			{
				if (data.Data != null)
				{
					formModelChain = data.Data;
				}
			}
			else
			{
				_errorMessage = data?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

			StateHasChanged();
		}

		private async Task SaveChain()
		{
			_errorMessage = null;
			ShowLoading();

			ResultModel<Master_ChainCustom> response;

			formModelChain.CurrentUserId = UserInfo.Id;

			if (idChain.HasValue && idChain != Guid.Empty)
			{
				response = await _masterViewModel.UpdateChain(formModelChain);
			}
			else
			{
				response = await _masterViewModel.CreateChain(formModelChain);
			}

			if (response.Status)
			{
				await _jsRuntimes.InvokeVoidAsync("SuccessAlert");
				HideLoading();
				await OnHideChain();
				await SetModelChain();
			}
			else
			{
				HideLoading();
				_errorMessage = response.errorMessage;
				await _jsRuntimes.InvokeVoidAsync("WarningAlert", _errorMessage);
			}
		}

		protected async Task ConfirmDeleteChain(string? id, string? txt)
		{
			await modalConfirmChain.OnShowConfirm(id, $"คุณต้องการลบข้อมูล <span class='text-primary'>{txt}</span>");
		}

		protected async Task DeleteChain(string id)
		{
			await modalConfirmChain.OnHideConfirm();

			var data = await _masterViewModel.DeleteChainById(new UpdateModel() { id = id, userid = UserInfo.Id });
			if (data != null && !data.Status && !String.IsNullOrEmpty(data.errorMessage))
			{
				_errorMessage = data?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}
			await SetModelChain();
		}

		protected async Task StatusChainChanged(ChangeEventArgs e, Guid id)
		{
			if (e.Value != null && Boolean.TryParse(e.Value.ToString(), out bool val))
			{
				var data = await _masterViewModel.UpdateStatusChainById(new UpdateModel() { id = id.ToString(), userid = UserInfo.Id, value = val.ToString() });
				if (data != null && !data.Status && !String.IsNullOrEmpty(data.errorMessage))
				{
					_errorMessage = data?.errorMessage;
					_utilsViewModel.AlertWarning(_errorMessage);
				}
				else
				{
					string? actiontxt = val ? "<i class=\"fa-regular fa-circle-check\"></i> เปิด" : "<i class=\"fa-solid fa-circle-xmark\"></i> ปิด";
					string fulltxt = $"{actiontxt}การใช้งานเรียบร้อย";
					await _jsRuntimes.InvokeVoidAsync("SuccessAlert", fulltxt);
					await SetModelYield();
				}
			}
		}

		private async Task OnShowChain(Guid? _id = null)
		{
			if (_id.HasValue && _id.Value != Guid.Empty)
			{
				idChain = _id;
				await SetModelChainById(_id.Value);
			}
			else
			{
				idChain = null;
				formModelChain = new();
			}
			await modalFormChain.ShowAsync();
		}

		private async Task OnHideChain()
		{
			idChain = null;
			await modalFormChain.HideAsync();
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