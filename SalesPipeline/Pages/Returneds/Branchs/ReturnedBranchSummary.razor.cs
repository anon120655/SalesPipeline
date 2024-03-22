using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SalesPipeline.Shared.Modals;
using SalesPipeline.Utils.Resources.Assignments;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils.Resources.Thailands;
using SalesPipeline.Utils;

namespace SalesPipeline.Pages.Returneds.Branchs
{
	public partial class ReturnedBranchSummary
	{
		[Parameter]
		public Guid id { get; set; }

		string? _errorMessage = null;
		private bool isLoading = false;
		private bool isDisabled = true;
		private allFilter filter = new();
		private User_PermissionCustom _permission = new();
		private LookUpResource LookUp = new();
		private SaleCustom? formModel;
		private List<Assignment_MCenterCustom>? Items;
		private int stepAssign = StepAssignLoanModel.Assigned;
		private AssignCenterModel AssignModel = new();

		ModalConfirm modalConfirmAssign = default!;
		ModalSuccessful modalSuccessfulAssign = default!;
		private bool IsToClose = false;

		protected override async Task OnInitializedAsync()
		{
			_permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.ReturnedBranch) ?? new User_PermissionCustom();
			StateHasChanged();

			await SetModel();
			await SetModelAssigned();
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

		protected async Task SetModelAssigned()
		{
			filter.pagesize = 100;
			var data = await _assignmentCenterViewModel.GetListCenter(filter);
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

		protected async Task GotoStep(int step)
		{
			bool isNext = true;

			if (step == StepAssignLoanModel.Assigned)
			{

			}
			else if (step == StepAssignLoanModel.Summary)
			{
				isNext = Summary();
				if (!isNext)
				{
					_utilsViewModel.AlertWarning("เลือกผู้รับผิดชอบใหม่");
				}
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
			_Navs.NavigateTo("/return/branch");
		}

		protected void OnCheckEmployee(Assignment_MCenterCustom model, object? checkedValue)
		{
			if (Items?.Count > 0)
			{
				foreach (var item in Items.Where(x => x.IsSelected))
				{
					item.IsSelected = false;
				}
			}

			if (checkedValue != null && (bool)checkedValue)
			{
				model.IsSelected = true;
			}
			else
			{
				model.IsSelected = false;
			}

			isDisabled = !model.IsSelected;
		}

		protected bool Summary()
		{
			if (Items?.Count > 0 && formModel != null)
			{
				//_itemsAssign ผู้รับผิดชอบใหม่ที่ถูกมอบหมายใหม่
				var _itemsAssign = Items.Where(x => x.IsSelected).FirstOrDefault();
				if (_itemsAssign != null)
				{
					var saleModel = GeneralUtils.DeepCopyJson(formModel);
					saleModel.Customer = null;

					AssignModel.Assign = _itemsAssign;
					AssignModel.Sales = new()
					{
						saleModel
					};
					return true;
				}
				else
				{
					return false;
				}
			}
			return false;
		}

		protected async Task InitShowConfirmAssign()
		{
			await ShowConfirmAssign(null, "กรุณากด ยืนยัน มอบหมายใหม่", "<img src=\"/image/icon/do.png\" width=\"65\" />");
		}

		protected async Task ShowConfirmAssign(string? id, string? txt, string? icon = null)
		{
			IsToClose = false;
			await modalConfirmAssign.OnShowConfirm(id, $"{txt}", icon);
		}

		protected async Task ConfirmAssign(string id)
		{
			ShowLoading();
			await Task.Delay(1);
			await AssignChange();
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

		protected async Task AssignChange()
		{
			await Task.Delay(1);
			_errorMessage = null;
			ShowLoading();

			AssignModel.CurrentUserId = UserInfo.Id;

			if (Items != null)
			{
				var response = await _assignmentCenterViewModel.Assign(AssignModel);

				if (response.Status)
				{
					IsToClose = true;
					await modalConfirmAssign.OnHideConfirm();
					await modalSuccessfulAssign.OnShow(null, "เสร็จสิ้นการมอบหมายงาน");
					HideLoading();
				}
				else
				{
					HideLoading();
					_errorMessage = response.errorMessage;
					await _jsRuntimes.InvokeVoidAsync("WarningAlert", _errorMessage);
				}
			}
		}

		protected async Task SearchStepAssigned()
		{
			await SetModelAssigned();
			StateHasChanged();
		}

		[JSInvokable]
		public async Task ProvinceChange(string _provinceID, string _provinceName)
		{
			filter.provinceid = null;
			filter.branch = null;
			LookUp.Branchs = new();
			StateHasChanged();
			await _jsRuntimes.InvokeVoidAsync("BootSelectEmptyID", "Branch");

			if (_provinceID != null && int.TryParse(_provinceID, out int provinceID))
			{
				filter.provinceid = provinceID;

				var branchs = await _masterViewModel.GetBranch(provinceID);
				if (branchs != null && branchs.Data?.Count > 0)
				{
					LookUp.Branchs = new List<InfoBranchCustom>() { new InfoBranchCustom() { BranchID = 0, BranchName = "--เลือก--" } };
					LookUp.Branchs.AddRange(branchs.Data);

					StateHasChanged();
					await Task.Delay(10);
					await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "BranchChange", "#Branch");
					await _jsRuntimes.InvokeVoidAsync("BootSelectRefreshID", "Branch", 100);
				}
				else
				{
					_errorMessage = branchs?.errorMessage;
					_utilsViewModel.AlertWarning(_errorMessage);
				}
			}

			await SetModelAssigned();
			StateHasChanged();

		}

		[JSInvokable]
		public async Task BranchChange(string _branchID, string _branchName)
		{
			filter.branch = null;
			if (_branchID != null && int.TryParse(_branchID, out int branchID))
			{
				filter.branch = branchID.ToString();
			}

			await SetModelAssigned();
			StateHasChanged();
		}

	}
}