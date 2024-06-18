using Microsoft.AspNetCore.Components;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils;
using Microsoft.JSInterop;
using SalesPipeline.Utils.Resources.Customers;
using SalesPipeline.Shared.Modals;
using SixLabors.ImageSharp.Metadata.Profiles.Iptc;
using BlazorBootstrap;
using NPOI.SS.Formula.Functions;
using SalesPipeline.Utils.ConstTypeModel;

namespace SalesPipeline.Pages.ApproveLoans
{
    public partial class ApproveLoanView
	{
		[Parameter]
		public Guid id { get; set; }

		string? _errorMessage = null;
		private bool isLoading = false;
		private User_PermissionCustom _permission = new();
		private LookUpResource LookUp = new();
		private SaleCustom formModel = new();
		ModalConfirm modalConfirmApprove = default!;
		ModalNotApprove modalNotApprove = default!;
		private bool IsToCancel = false;

		protected override async Task OnInitializedAsync()
		{
			_permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.ApproveLoan) ?? new User_PermissionCustom();
			StateHasChanged();

			await SetModel();
		}

		protected async override Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				StateHasChanged();
				firstRender = false;
			}
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

		protected async Task NotApprove(SelectModel model)
		{
			_errorMessage = null;
			ShowLoading();

			var response = await _salesViewModel.UpdateStatusOnly(new()
			{
				SaleId = id,
				StatusId = StatusSaleModel.NotApproveLoanRequest,
				CreateBy = UserInfo.Id,
				Description = model.Name
			});

			if (response.Status)
			{
				HideLoading();
				await _jsRuntimes.InvokeVoidAsync("SuccessAlert");
				IsToCancel = true;
				await modalNotApprove.OnHideConfirm();
			}
			else
			{
				HideLoading();
				_errorMessage = response.errorMessage;
				await _jsRuntimes.InvokeVoidAsync("WarningAlert", _errorMessage);
			}
		}

		protected async Task Approve()
		{
			_errorMessage = null;
			ShowLoading();

			//ผู้จัดการศูนย์ตรวจสอบและอนุมัติลงนาม และส่ง API ส่งไประบบวิเคราะห์สินเชื่อ (PHOENIX/LPS) ไป รอวิเคราะห์สินเชื่อ(LPS)
			var response = await _salesViewModel.UpdateStatusOnly(new()
			{
				SaleId = id,
				StatusId = StatusSaleModel.WaitAPIPHOENIX,
				CreateBy = UserInfo.Id,
			});

			if (response.Status)
			{
				await _jsRuntimes.InvokeVoidAsync("SuccessAlert");
				IsToCancel = true;
				await modalConfirmApprove.OnHideConfirm();
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
			_Navs.NavigateTo("/approveloan");
		}

		protected async Task InitShowConfirmApprove()
		{
			await ShowConfirmApprove(id.ToString(), "กรุณากด ยืนยัน เพื่ออนุมัติ", "<img src=\"/image/icon/checkapprove.png\" width=\"65\" />");
		}

		protected async Task ShowConfirmApprove(string? id, string? txt, string? icon = null)
		{
			IsToCancel = false;
			await modalConfirmApprove.OnShowConfirm(id, $"{txt}", icon);
		}

		protected async Task ConfirmApprove(string id)
		{
			await Approve();
		}

		protected async Task InitShowNotApprove()
		{
			await ShowNotApprove(id.ToString(), "กรุณาระบุเหตุผลการไม่อนุมัติ", "<img src=\"/image/icon/notapprove.png\" width=\"65\" />");
		}

		protected async Task ShowNotApprove(string? id, string? txt, string? icon = null)
		{
			IsToCancel = false;
			await modalNotApprove.OnShowConfirm(id, $"{txt}", icon);
		}

		protected async Task NotApproveModal(SelectModel model)
		{
			if (String.IsNullOrEmpty(model.Name))
			{
				_utilsViewModel.AlertWarning("ระบุเหตุผลการไม่อนุมัติ");
			}
			else
			{
				await NotApprove(model);
			}
		}

		private void OnModalHidden()
		{
			if (IsToCancel)
			{
				Cancel();
			}
		}

		protected async Task ShowTabContactInfo()
		{
			if (formModel.Sale_Contact_Infos == null || formModel.Sale_Contact_Infos.Count == 0)
			{
				var data = await _salesViewModel.GetListInfo(new() { id = id, pagesize = 200, saleid = formModel.Id });
				if (data != null && data.Status && data.Data != null)
				{
					formModel.Sale_Contact_Infos = data.Data.Items;
				}
				else
				{
					_errorMessage = data?.errorMessage;
					_utilsViewModel.AlertWarning(_errorMessage);
				}
			}
		}

		protected async Task ShowTabDocument()
		{
			if (formModel.Sale_Documents == null || formModel.Sale_Documents.Count == 0)
			{
				var data = await _processSaleViewModel.GetListDocument(new() { id = id, pagesize = 50 });
				if (data != null && data.Status && data.Data != null)
				{
					formModel.Sale_Documents = data.Data;
				}
				else
				{
					_errorMessage = data?.errorMessage;
					_utilsViewModel.AlertWarning(_errorMessage);
				}
			}
		}

		protected async Task ShowTabContactHistory()
		{
			if (formModel.Sale_Contact_Histories == null || formModel.Sale_Contact_Histories.Count == 0)
			{
				var data = await _processSaleViewModel.GetListContactHistory(new() { id = id, pagesize = 50 });
				if (data != null && data.Status && data.Data != null)
				{
					formModel.Sale_Contact_Histories = data.Data.Items;
				}
				else
				{
					_errorMessage = data?.errorMessage;
					_utilsViewModel.AlertWarning(_errorMessage);
				}
			}
		}

		protected async Task ShowTabPartneInfo()
		{
			if (formModel.Sale_Partners == null || formModel.Sale_Partners.Count == 0)
			{
				var data = await _salesViewModel.GetListPartner(new() { id = id, pagesize = 200, saleid = formModel.Id });
				if (data != null && data.Status && data.Data != null)
				{
					formModel.Sale_Partners = data.Data.Items;
				}
				else
				{
					_errorMessage = data?.errorMessage;
					_utilsViewModel.AlertWarning(_errorMessage);
				}
			}
		}

	}
}