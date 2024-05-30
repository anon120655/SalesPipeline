using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SalesPipeline.Shared.Modals;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.PreApprove;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Pages.Settings.PreApprove
{
	public partial class Calculated
	{
		public Guid? id { get; set; }

		string? _errorMessage = null;
		private User_PermissionCustom _permission = new();
		private allFilter filter = new();
		private LookUpResource LookUp = new();
		private bool isLoading = false;
		private List<Pre_Cal_InfoCustom>? Items;
		private Pre_Cal_InfoCustom formModel = new();

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

		protected async Task SetInitManual()
		{
			await _jsRuntimes.InvokeVoidAsync("BootSelectClass", "selectInit");

			filter.pagesize = 100;
			var dataPre_App_Loan = await _masterViewModel.GetPre_App_Loan(filter);
			if (dataPre_App_Loan != null && dataPre_App_Loan.Status)
			{
				LookUp.Pre_Applicant_Loan = dataPre_App_Loan.Data?.Items;
				StateHasChanged();
				await Task.Delay(10);
				await _jsRuntimes.InvokeVoidAsync("BootDestroyAndNewSelectId", "Pre_Applicant_Loan");
			}
			else
			{
				_errorMessage = dataPre_App_Loan?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

			var dataPre_BusType = await _masterViewModel.GetPre_BusType(filter);
			if (dataPre_BusType != null && dataPre_BusType.Status)
			{
				LookUp.Pre_BusinessType = dataPre_BusType.Data?.Items;
				StateHasChanged();
				await Task.Delay(10);
				await _jsRuntimes.InvokeVoidAsync("BootDestroyAndNewSelectId", "Pre_BusinessType");
			}
			else
			{
				_errorMessage = dataPre_BusType?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}
		}

		protected async Task SetModel()
		{
			//Items = new() { new() { Master_Pre_Applicant_LoanName = "Micro", Master_Pre_BusinessTypeName = "เกษตรกร", Status = StatusModel.Active } };
			var data = await _preCalInfoViewModel.GetList(filter);
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
			var data = await _preCalInfoViewModel.GetById(id);
			if (data != null && data.Status)
			{
				if (data.Data != null)
				{
					formModel = data.Data;
					StateHasChanged();
				}
			}
			else
			{
				_errorMessage = data?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}
		}

		private async Task Save()
		{
			_errorMessage = null;
			ShowLoading();

			ResultModel<Pre_Cal_InfoCustom> response;

			formModel.CheckPage = 1;
			formModel.CurrentUserId = UserInfo.Id;

			if (id.HasValue && id != Guid.Empty)
			{
				response = await _preCalInfoViewModel.Update(formModel);
			}
			else
			{
				response = await _preCalInfoViewModel.Create(formModel);
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

		protected async Task ConfirmDelete(string? id, string? txt)
		{
			await modalConfirm.OnShowConfirm(id, $"คุณต้องการลบข้อมูล <span class='text-primary'>{txt}</span>");
		}

		protected async Task Delete(string id)
		{
			await modalConfirm.OnHideConfirm();

			var data = await _preCalInfoViewModel.DeleteById(new UpdateModel() { id = id, userid = UserInfo.Id });
			if (data != null && !data.Status && !String.IsNullOrEmpty(data.errorMessage))
			{
				_errorMessage = data?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}
			await SetModel();
		}

		protected async Task StatusChanged(ChangeEventArgs e, Guid id)
		{
			if (e.Value != null && Boolean.TryParse(e.Value.ToString(), out bool val))
			{
				var data = await _preCalInfoViewModel.UpdateStatusById(new UpdateModel() { id = id.ToString(), userid = UserInfo.Id, value = val.ToString() });
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
					await SetModel();
				}
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

		private async Task OnModalShown()
		{
			await SetInitManual();
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