using Hangfire.MemoryStorage.Database;
using Microsoft.JSInterop;
using SalesPipeline.Shared.Modals;
using SalesPipeline.Utils;
using SalesPipeline.Utils.ConstTypeModel;
using SalesPipeline.Utils.Resources.Assignments;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;
using System.Text.Json;

namespace SalesPipeline.Pages.Assigns.CenterBranch
{
	public partial class AssignCenter
	{
		string? _errorMessage = null;
		string? _errorMessageModal = null;
		private bool isLoading = false;
		private User_PermissionCustom _permission = new();
		private LookUpResource LookUp = new();
		private allFilter filter = new();
		private List<Assignment_CenterCustom>? Items;
		private Pager? Pager;
		private SaleCustom? formView = null;
		private int stepAssign = StepAssignLoanModel.Home;
		private int? useridView;
		private Guid? assignmentIdPrevious = null;
		private bool IsToClose = false;

		ModalConfirm modalConfirmAssign = default!;
		ModalSuccessful modalSuccessfulAssign = default!;

		protected override async Task OnInitializedAsync()
		{
			stepAssign = StepAssignLoanModel.Home;
			_permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.AssignManager) ?? new User_PermissionCustom();
			StateHasChanged();
			await Task.Delay(1);
		}

		protected async override Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				await SetQuery();
				StateHasChanged();
				await SetInitManual();
				await Task.Delay(10);

				await _jsRuntimes.InvokeVoidAsync("selectPickerInitialize");
				await _jsRuntimes.InvokeVoidAsync("localStorage.removeItem", $"assignCenterData_{UserInfo.Id}");
				firstRender = false;
			}
		}

		protected async Task SetInitManual()
		{
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

			StateHasChanged();
			await Task.Delay(10);
			await _jsRuntimes.InvokeVoidAsync("BootSelectId", "BusinessType");
			await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "OnProvince", "#Province");
		}

		protected async Task SetQuery(string? parematerAll = null)
		{
			string uriQuery = _Navs.ToAbsoluteUri(_Navs.Uri).Query;

			if (parematerAll != null)
				uriQuery = $"?{parematerAll}";

			filter.SetUriQuery(uriQuery);

			await SetModel();
			StateHasChanged();
		}

		protected async Task SetModel()
		{
			allFilter filter = new();

			filter.userid = UserInfo.Id;
			filter.pagesize = 1000;
			var data = await _assignmentCenterViewModel.GetListAutoAssign(filter);
			if (data != null && data.Status)
			{
				Items = data.Data?.Items;
				Pager = data.Data?.Pager;
				if (Pager != null)
				{
					Pager.UrlAction = "/assign/center";
				}
			}
			else
			{
				_errorMessage = data?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

			StateHasChanged();
		}

		protected async Task Search()
		{
			await SetModel();
			StateHasChanged();
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

		protected async Task ToViewSummary(int userid)
		{
			stepAssign = StepAssignLoanModel.Summary;
			useridView = userid;
			//await _jsRuntimes.InvokeVoidAsync("localStorage.removeItem", $"assignCenterData_{UserInfo.Id}");
			//var jsonData = JsonSerializer.Serialize(Items);
			//await _jsRuntimes.InvokeVoidAsync("localStorage.setItem", $"assignCenterData_{UserInfo.Id}", jsonData);
			//_Navs.NavigateTo($"assign/center/customer");
			await Task.Delay(1);
		}

		protected async Task InitShowConfirmAssign()
		{
			_errorMessageModal = null;
			await ShowConfirmAssign(null, "กรุณากด ยืนยัน การมอบหมายงาน", "<img src=\"/image/icon/do.png\" width=\"65\" />");
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

		protected async Task Assign()
		{
			_errorMessageModal = null;

			if (Items != null)
			{
				foreach (var item in Items)
				{
					item.CurrentUserId = UserInfo.Id;
					foreach (var item_sale in item.Assignment_Sales ?? new())
					{
						item_sale.Sale = null;
					}
				}

				var response = await _assignmentCenterViewModel.AssignCenter(Items);
				if (response.Status)
				{
					IsToClose = true;
					await modalConfirmAssign.OnHideConfirm();
					await ShowSuccessfulAssign(null, "เสร็จสิ้นการมอบหมายงาน");
					await SetModel();
					HideLoading();
				}
				else
				{
					HideLoading();
					_errorMessageModal = response.errorMessage;
				}
			}

		}

	}
}