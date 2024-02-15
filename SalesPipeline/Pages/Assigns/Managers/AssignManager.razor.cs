using Microsoft.JSInterop;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Assignments;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Pages.Assigns.Managers
{
	public partial class AssignManager
	{
		string? _errorMessage = null;
		private bool isLoading = false;
		private bool isDisabled = true;
		private User_PermissionCustom _permission = new();
		private allFilter filter = new();
		private LookUpResource LookUp = new();
		private List<SaleCustom>? Items;
		private List<Guid> ItemsSelected = new();
		private int stepAssign = StepAssignManagerCenterModel.Customer;
		public Pager? Pager;
		int LimitAssign = 10;

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
			filter.pagesize = 100;
			filter.statussaleid = StatusSaleModel.WaitAssignCenter;
			var data = await _salesViewModel.GetList(filter);
			if (data != null && data.Status)
			{
				Items = data.Data?.Items;
				if (Items != null && ItemsSelected.Count > 0)
				{
					foreach (var item in Items)
					{
						if (ItemsSelected.Contains(item.Id))
						{
							item.IsSelected = true;
						}
					}
				}
				Pager = data.Data?.Pager;
				if (Pager != null)
				{
					Pager.UrlAction = "/assign/manager";
				}
			}
			else
			{
				_errorMessage = data?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

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

		protected async Task GotoStep(int step, Guid? assignmentId = null, bool? back = null)
		{
			bool isNext = true;
		
			if (step == StepAssignManagerCenterModel.Customer)
			{
				
			}
			else if (step == StepAssignManagerCenterModel.Assigned)
			{
				
			}
			else if (step == StepAssignManagerCenterModel.Summary)
			{
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
					ItemsSelected.Add(model.Id);
				}
			}
			else
			{
				model.IsSelected = false;
				ItemsSelected.Remove(model.Id);
			}

			isDisabled = ItemsSelected.Count == 0;
		}

	}
}