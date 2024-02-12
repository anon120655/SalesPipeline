using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SalesPipeline.Shared.Modals;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Assignments;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Pages.AssignsHistory
{
	public partial class HistoryAssignSummary
	{
		[Parameter]
		public Guid id { get; set; }

		string? _errorMessage = null;
		private bool isLoading = false;
		private User_PermissionCustom _permission = new();
		private LookUpResource LookUp = new();
		private int stepAssign = StepAssignLoanModel.Assigned;

		ModalConfirm modalConfirmAssign = default!;
		ModalSuccessful modalSuccessfulAssign = default!;
		private bool IsToClose = false;

		protected override async Task OnInitializedAsync()
		{
			_permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.HistoryAssign) ?? new User_PermissionCustom();
			StateHasChanged();

		}

		protected async override Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				firstRender = false;
			}
		}

		protected async Task GotoStep(int step)
		{
			bool isNext = true;

			if (step == StepAssignLoanModel.Customer)
			{

			}
			else if (step == StepAssignLoanModel.Summary)
			{

			}

			if (isNext)
			{
				stepAssign = step;
				StateHasChanged();
			}

			await Task.Delay(10);
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
			_Navs.NavigateTo("/historyassign");
		}

		protected async Task InitShowConfirmAssign()
		{
			await ShowConfirmAssign(null, "กรุณากด ยืนยัน แก้ไขมอบหมายงาน", "<img src=\"/image/icon/do.png\" width=\"65\" />");
		}

		protected async Task ShowConfirmAssign(string? id, string? txt, string? icon = null)
		{
			IsToClose = false;
			await modalConfirmAssign.OnShowConfirm(id, $"{txt}", icon);
		}

		protected async Task ConfirmAssign(string id)
		{
			ShowLoading();
			await Task.Delay(10);
			await Assign();
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

		protected async Task Assign()
		{
			_errorMessage = null;

			//if (Items != null)
			//{
			//	var response = await _assignmentViewModel.Assign(Items);

			//	if (response.Status)
			//	{
			IsToClose = true;
			await modalConfirmAssign.OnHideConfirm();
			await ShowSuccessfulAssign(null, "เสร็จสิ้นการแก้ไขมอบหมายงาน");
			//await SetModel();
			HideLoading();
			//	}
			//	else
			//	{
			//		HideLoading();
			//		_errorMessage = response.errorMessage;
			//		await _jsRuntimes.InvokeVoidAsync("WarningAlert", _errorMessage);
			//	}
			//}

		}



	}
}