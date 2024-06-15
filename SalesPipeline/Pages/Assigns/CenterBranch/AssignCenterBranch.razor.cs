using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SalesPipeline.Shared.Modals;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Assignments;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Pages.Assigns.CenterBranch
{
	public partial class AssignCenterBranch
	{
		string? _errorMessage = null;
		private bool isLoading = false;
		private bool isDisabled = true;
		private bool isDisabledAssignment = true;
		private User_PermissionCustom _permission = new();
		private allFilter filter = new();
		private LookUpResource LookUp = new();
		private SaleCustom? formView = null;
		private List<SaleCustom>? Items;
		private List<SaleCustom> ItemsSelected = new();
		private List<Assignment_CenterBranchCustom>? ItemsAssignment;
		private List<Assignment_CenterBranchCustom> ItemsAssignmentSelected = new();
		private AssignModel AssignModel = new();
		private int stepAssign = StepAssignManagerCenterModel.Customer;
		public Pager? Pager;
		private int LimitAssign = 10;
		ModalSuccessful modalSuccessfulAssign = default!;
		private bool IsToClose = false;

		protected override async Task OnInitializedAsync()
		{
			_permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.AssignManager) ?? new User_PermissionCustom();
			StateHasChanged();

			await SetInitManual();
		}

		protected async override Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				await SetQuery();
				StateHasChanged();

				await _jsRuntimes.InvokeVoidAsync("selectPickerInitialize");
				StateHasChanged();
				firstRender = false;
			}
		}

		protected async Task SetInitManual()
		{
			var dataLevels = await _userViewModel.GetListLevel(new allFilter() { status = StatusModel.Active });
			if (dataLevels != null && dataLevels.Status)
			{
				LookUp.UserLevels = dataLevels.Data;
			}
			else
			{
				_errorMessage = dataLevels?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

			var userCenter = await _masterViewModel.GetListCenter(new allFilter() { status = StatusModel.Active, pagesize = 100 });
			if (userCenter != null && userCenter.Status)
			{
				LookUp.AssignmentCenter = userCenter.Data?.Items;
			}
			else
			{
				_errorMessage = userCenter?.errorMessage;
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

			await _jsRuntimes.InvokeVoidAsync("BootSelectId", "Assignment");
			await _jsRuntimes.InvokeVoidAsync("BootSelectId", "BusinessType");
			await _jsRuntimes.InvokeVoidAsync("BootSelectId", "Province");
		}

		protected async Task SetQuery(string? parematerAll = null)
		{
			var uriQuery = _Navs.ToAbsoluteUri(_Navs.Uri).Query;

			if (parematerAll != null)
				uriQuery = $"?{parematerAll}";

			filter.SetUriQuery(uriQuery);

			await SetModel();
			StateHasChanged();
		}

		protected async Task SetModel()
        {
            filter.page = 1;
            filter.userid = UserInfo.Id;
			//filter.pagesize = 100;
			filter.StatusSales = new()
			{
				StatusSaleModel.WaitAssignCenter.ToString(),
				StatusSaleModel.WaitAssignCenterREG.ToString()
			};
			var data = await _salesViewModel.GetList(filter);
			if (data != null && data.Status)
			{
				Items = data.Data?.Items;
				if (Items != null && ItemsSelected.Count > 0)
				{
					foreach (var item in Items)
					{
						if (ItemsSelected.Select(x => x.Id).Contains(item.Id))
						{
							item.IsSelected = true;
						}
					}
				}
				Pager = data.Data?.Pager;
				if (Pager != null)
				{
					Pager.UrlAction = "/assign/cbranch";
				}
			}
			else
			{
				_errorMessage = data?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

			StateHasChanged();
		}

		protected async Task SetModelAssignment()
		{
			filter.pagesize = 100;
			var data = await _assignmentCenterViewModel.GetListCenter(filter);
			if (data != null && data.Status)
			{
				ItemsAssignment = data.Data?.Items;
				if (ItemsAssignment != null && ItemsAssignmentSelected.Count > 0)
				{
					foreach (var item in ItemsAssignment)
					{
						if (ItemsAssignmentSelected.Select(x => x.Id).Contains(item.Id))
						{
							item.IsSelected = true;
						}
					}
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

		protected async Task SearchAssignment()
		{
			await SetModelAssignment();
			StateHasChanged();
		}

		protected async Task OnAssignment(object? val)
		{
			filter.assignmentid = null;
			StateHasChanged();

			if (val != null && Guid.TryParse(val.ToString(), out Guid _id))
			{
				filter.assignmentid = _id.ToString();
			}

			await SetModelAssignment();
			StateHasChanged();
		}

		protected async Task OnSelectPagesize(int _number)
		{
			Items = null;
			StateHasChanged();
			filter.page = 1;
			filter.pagesize = _number;
			await SetModel();
			_Navs.NavigateTo($"{Pager?.UrlAction}?{filter.SetParameter(true)}");
		}

		protected async Task OnSelectPage(string parematerAll)
		{
			await SetQuery(parematerAll);
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

		protected async Task GotoStep(int step, Guid? assignmentId = null, bool? back = null)
		{
			bool isNext = true;

			if (step == StepAssignManagerCenterModel.Customer)
			{
				await SetModel();
			}
			else if (step == StepAssignManagerCenterModel.Assigned)
			{
				await SetModelAssignment();
			}
			else if (step == StepAssignManagerCenterModel.Summary)
			{
				isNext = Summary();
				if (!isNext)
				{
					_utilsViewModel.AlertWarning("เลือกผู้รับผิดชอบ");
				}
			}

			if (isNext)
			{
				stepAssign = step;
				StateHasChanged();
			}

			await Task.Delay(10);
			await SetInitManual();
		}

		//เลือกลูกค้า
		protected void OnCheckCustomer(SaleCustom model, object? checkedValue)
		{
			if (checkedValue != null && (bool)checkedValue)
			{
				if (ItemsSelected.Count > LimitAssign)
				{
					_utilsViewModel.AlertWarning("เลือกลูกค้าครบแล้ว");
				}
				else
				{
					model.IsSelected = true;
					ItemsSelected.Add(model);
				}
			}
			else
			{
				model.IsSelected = false;
				ItemsSelected.Remove(model);
			}

			isDisabled = ItemsSelected.Count == 0;
		}

		protected void OnViewCustomer(SaleCustom? model)
		{
			if (model != null)
			{
				formView = model;
			}
		}

		protected void OnViewCustomerBack()
		{
			formView = null;
		}

		//เลือกผู้รับผิดชอบ
		protected void OnCheckEmployee(Assignment_CenterBranchCustom model, object? checkedValue)
		{
			if (ItemsAssignment?.Count > 0)
			{
				foreach (var item in ItemsAssignment.Where(x => x.IsSelected))
				{
					item.IsSelected = false;
				}
			}

			ItemsAssignmentSelected = new();

			if (checkedValue != null && (bool)checkedValue)
			{
				model.IsSelected = true;
				ItemsAssignmentSelected.Add(model);
			}

			isDisabledAssignment = ItemsAssignmentSelected.Count == 0;
		}

		//สรุปผู้รับผิดชอบและผู้จัดการศูนย์ที่ได้รับมอบหมาย
		protected bool Summary()
		{
			if (ItemsSelected.Count > 0 && ItemsAssignmentSelected.Count > 0)
			{
				AssignModel.AssignMCenter = ItemsAssignmentSelected.FirstOrDefault() ?? new();
				AssignModel.Sales = ItemsSelected;
				return true;
			}
			return false;
		}

		protected async Task Assign()
		{
			_errorMessage = null;
			ShowLoading();

			AssignModel.CurrentUserId = UserInfo.Id;

			if (Items != null)
			{
				var response = await _assignmentCenterViewModel.Assign(AssignModel);

				if (response.Status)
				{
					IsToClose = true;
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

		private async Task OnModalHidden()
		{
			if (IsToClose)
			{
				isDisabled = true;
				isDisabledAssignment = true;
				ItemsSelected = new();
				ItemsAssignmentSelected = new();
				await GotoStep(StepAssignManagerCenterModel.Customer);
			}
		}

		protected async Task OnBusinessType(ChangeEventArgs e)
		{
			filter.businesstype = null;
			if (e.Value != null)
			{
				filter.businesstype = e.Value.ToString();
			}

			StateHasChanged();
			await Task.Delay(1);
		}

		public async Task OnProvince(ChangeEventArgs e)
		{
			filter.provinceid = null;
			if (e.Value != null && int.TryParse(e.Value.ToString(),out int _id))
			{
				filter.provinceid = _id;
			}

			StateHasChanged();
			await Task.Delay(1);
		}


	}
}