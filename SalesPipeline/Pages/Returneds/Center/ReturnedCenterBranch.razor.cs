using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SalesPipeline.Shared.Modals;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils;
using SalesPipeline.Utils.ConstTypeModel;

namespace SalesPipeline.Pages.Returneds.Center
{
    public partial class ReturnedCenterBranch
	{
		[Parameter]
		public Guid id { get; set; }

		string? _errorMessage = null;
		string? _errorMessageModal = null;
		private bool isLoading = false;
		private User_PermissionCustom _permission = new();
		private LookUpResource LookUp = new();
		private SaleCustom? formModel;

		ModalReturnReason modalReturnReason = default!;
		ModalSuccessful modalSuccessfulAssign = default!;
		private bool IsToClose = false;

		protected override async Task OnInitializedAsync()
		{
			_permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.ReturnedCenter) ?? new User_PermissionCustom();
			StateHasChanged();
			await SetModel();
		}

		protected async override Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				await SetInitManual();
				await Task.Delay(10);

				await _jsRuntimes.InvokeVoidAsync("selectPickerInitialize");
				firstRender = false;
			}
		}

		protected async Task SetInitManual()
		{
			var businessType = await _masterViewModel.GetBusinessType(new() { status = StatusModel.Active });
			if (businessType != null && businessType.Status)
			{
				LookUp.BusinessType = businessType.Data?.Items;
			}
			else
			{
				_errorMessage = businessType?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

			var province = await _masterViewModel.GetProvince();
			if (province != null && province.Status)
			{
				LookUp.Provinces = province.Data;
			}
			else
			{
				_errorMessage = province?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

			var reasonReturn = await _masterViewModel.GetReasonReturns(new() { pagesize = 50 });
			if (reasonReturn != null && reasonReturn.Status)
			{
				LookUp.ReasonReturn = reasonReturn.Data?.Items;
			}
			else
			{
				_errorMessage = reasonReturn?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

			StateHasChanged();
			await Task.Delay(10);
			await _jsRuntimes.InvokeVoidAsync("BootSelectId", "BusinessType");
			await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "ProvinceChange", "#Province");
		}

		protected async Task SetModel()
		{
			if (id != Guid.Empty)
			{
				var data = await _salesViewModel.GetById(id);
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

		protected void Cancel()
		{
			_Navs.NavigateTo("/return/center");
		}

		protected async Task ShowSuccessfulAssign(string? id, string? txt)
		{
			await modalSuccessfulAssign.OnShow(id, $"{txt}");
		}

		private async Task OnModalHidden()
		{
			if (IsToClose)
			{
				await Task.Delay(1);
				Cancel();
			}
		}

		protected async Task ShowReturnReason()
		{
			await modalReturnReason.OnShowConfirm();
		}

		protected async Task CenBranchToBranchReg(string? id)
		{
			_errorMessageModal = null;

			if (Guid.TryParse(id, out Guid _id) && formModel != null)
			{
				var response = await _returnViewModel.CenBranchToBranchReg(new()
				{
					CurrentUserId = UserInfo.Id,
					Master_ReasonReturnId = _id,
					ListSale = new()
					{
						new(){ ID = formModel.Id.ToString() }
					}
				});

				if (response.Status)
				{
					IsToClose = true;
					await modalReturnReason.OnHide();
					await ShowSuccessfulAssign(null, "เสร็จสิ้นการส่งคืน");
					await SetModel();
					HideLoading();
				}
				else
				{
					HideLoading();
					_errorMessageModal = response.errorMessage;
				}
			}
			else
			{
				_errorMessageModal = "ระบุเหตุผลในการส่งคืน";
			}
		}


	}
}