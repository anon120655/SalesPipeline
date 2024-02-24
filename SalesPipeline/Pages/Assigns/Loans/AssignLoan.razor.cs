using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using NPOI.OpenXmlFormats.Spreadsheet;
using SalesPipeline.Shared.Modals;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Assignments;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Customers;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils.Resources.Thailands;
using System.Linq;

namespace SalesPipeline.Pages.Assigns.Loans
{
	public partial class AssignLoan
	{
		string? _errorMessage = null;
		private bool isLoading = false;
		private User_PermissionCustom _permission = new();
		private LookUpResource LookUp = new();
		private allFilter filter = new();
		private allFilter filterRM = new();
		private allFilter filterRMNew = new();
		private List<Assignment_RMCustom>? ItemsAll;
		private List<Assignment_RMCustom>? ItemsRM;
		private Pager? PagerRM;
		private List<Assignment_RMCustom>? ItemsRMNew;
		private Pager? PagerRMNew;
		private List<Assignment_RM_SaleCustom>? SaleMoveAssigned;
		private Pager? Pager;
		private SaleCustom? formView = null;
		private int stepAssign = StepAssignLoanModel.Home;
		private Guid? assignmentIdPrevious = null;

		ModalConfirm modalConfirmAssign = default!;
		ModalSuccessful modalSuccessfulAssign = default!;
		private bool IsToClose = false;

		protected override async Task OnInitializedAsync()
		{
			stepAssign = StepAssignLoanModel.Home;
			_permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.AssignLoan) ?? new User_PermissionCustom();
			StateHasChanged();
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

		protected async Task SetQuery(string? parematerAll = null)
		{
			string uriQuery = _Navs.ToAbsoluteUri(_Navs.Uri).Query;

			if (parematerAll != null)
				uriQuery = $"?{parematerAll}";

			filter.SetUriQuery(uriQuery);

			await SetModel();
			StateHasChanged();
		}

		protected async Task SetQueryRM(string? parematerAll = null)
		{
			string uriQuery = _Navs.ToAbsoluteUri(_Navs.Uri).Query;

			if (parematerAll != null)
				uriQuery = $"?{parematerAll}";

			filterRM.SetUriQuery(uriQuery);

			SetModelRM();
			StateHasChanged();
			await Task.Delay(1);
		}

		protected async Task SetQueryRMNew(string? parematerAll = null)
		{
			string uriQuery = _Navs.ToAbsoluteUri(_Navs.Uri).Query;

			if (parematerAll != null)
				uriQuery = $"?{parematerAll}";

			filterRMNew.SetUriQuery(uriQuery);

			SetModelRMNew();
			StateHasChanged();
			await Task.Delay(1);
		}

		protected async Task SetModel()
		{
			allFilter filter = new();
			if (UserInfo.RoleCode != null)
			{
				filter.assigncenter = UserInfo.Id;

				if (UserInfo.RoleCode == RoleCodes.SUPERADMIN)
				{
					filter.assigncenter = null;
					filter.assignrm = null;
				}
			}

			filter.pagesize = 1000;
			var data = await _assignmentRMViewModel.GetListAutoAssign(filter);
			if (data != null && data.Status)
			{
				ItemsAll = data.Data?.Items;
				Pager = data.Data?.Pager;
				if (Pager != null)
				{
					Pager.UrlAction = "/assign/loan";
				}
				SetModelRM();
			}
			else
			{
				_errorMessage = data?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

			StateHasChanged();
		}

		protected void SetModelRM()
		{
			if (ItemsAll?.Count > 0)
			{
				ItemsRM = GeneralUtils.DeepCopyJson(ItemsAll);
				//ItemsRM = new(ItemsAll);

				if (!String.IsNullOrEmpty(filterRM.emp_id))
				{
					ItemsRM = ItemsRM.Where(x => x.EmployeeId != null && x.EmployeeId.Contains(filterRM.emp_id)).ToList();
				}

				if (!String.IsNullOrEmpty(filterRM.emp_name))
				{
					ItemsRM = ItemsRM.Where(x => x.EmployeeName != null && x.EmployeeName.Contains(filterRM.emp_name)).ToList();
				}

				if (stepAssign == StepAssignLoanModel.Customer && assignmentIdPrevious.HasValue)
				{
					ItemsRM = ItemsRM.Where(x => x.Id != assignmentIdPrevious).ToList();
				}

				PagerRM = new Pager(ItemsRM.Count(), filterRM.page, filterRM.pagesize, null);
				if (PagerRM != null)
				{
					PagerRM.UrlAction = "/assign/loan";
					ItemsRM = ItemsRM.Skip((PagerRM.CurrentPage - 1) * PagerRM.PageSize).Take(PagerRM.PageSize).ToList();
				}
			}

			StateHasChanged();
		}

		protected void SetModelRMNew()
		{
			if (ItemsAll?.Count > 0)
			{
				ItemsRMNew = GeneralUtils.DeepCopyJson(ItemsAll);
				//ItemsRMNew = new(ItemsAll);

				if (!String.IsNullOrEmpty(filterRMNew.emp_id))
				{
					ItemsRMNew = ItemsRMNew.Where(x => x.EmployeeId != null && x.EmployeeId.Contains(filterRMNew.emp_id)).ToList();
				}

				if (!String.IsNullOrEmpty(filterRMNew.emp_name))
				{
					ItemsRMNew = ItemsRMNew.Where(x => x.EmployeeName != null && x.EmployeeName.Contains(filterRMNew.emp_name)).ToList();
				}

				if (assignmentIdPrevious.HasValue)
				{
					ItemsRMNew = ItemsRMNew.Where(x => x.Id != assignmentIdPrevious).ToList();
				}

				PagerRMNew = new Pager(ItemsRMNew.Count(), filterRMNew.page, filterRMNew.pagesize, null);
				if (PagerRMNew != null)
				{
					PagerRMNew.UrlAction = "/assign/loan";
					ItemsRMNew = ItemsRMNew.Skip((PagerRMNew.CurrentPage - 1) * PagerRMNew.PageSize).Take(PagerRMNew.PageSize).ToList();
				}
			}

			StateHasChanged();
		}

		[JSInvokable]
		public async Task ProvinceChange(string _provinceID, string _provinceName)
		{
			filter.province = null;
			filter.amphur = null;
			LookUp.Amphurs = new();
			StateHasChanged();
			await _jsRuntimes.InvokeVoidAsync("BootSelectEmptyID", "Amphur");

			if (_provinceID != null && int.TryParse(_provinceID, out int provinceID))
			{
				filter.province = provinceID.ToString();

				if (int.TryParse(filter.province, out int id))
				{
					var amphurs = await _masterViewModel.GetAmphur(id);
					if (amphurs != null && amphurs.Data?.Count > 0)
					{
						LookUp.Amphurs = new List<InfoAmphurCustom>() { new InfoAmphurCustom() { AmphurID = 0, AmphurName = "--เลือก--" } };
						LookUp.Amphurs.AddRange(amphurs.Data);

						StateHasChanged();
						await Task.Delay(10);
						await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "AmphurChange", "#Amphur");
						await _jsRuntimes.InvokeVoidAsync("BootSelectRefreshID", "Amphur", 100);
					}
					else
					{
						_errorMessage = amphurs?.errorMessage;
						_utilsViewModel.AlertWarning(_errorMessage);
					}
				}
			}

			await SetModel();
			StateHasChanged();
			_Navs.NavigateTo($"{Pager?.UrlAction}?{filter.SetParameter(true)}");

		}

		[JSInvokable]
		public async Task AmphurChange(string _amphurID, string _amphurName)
		{
			filter.amphur = null;
			if (_amphurID != null && int.TryParse(_amphurID, out int amphurID))
			{
				filter.amphur = amphurID.ToString();
			}

			await SetModel();
			StateHasChanged();
			_Navs.NavigateTo($"{Pager?.UrlAction}?{filter.SetParameter(true)}");
		}

		protected async Task OnSelectPageRM(string parematerAll)
		{
			await SetQueryRM(parematerAll);
			StateHasChanged();
		}

		protected async Task OnSelectPageRMNew(string parematerAll)
		{
			await SetQueryRMNew(parematerAll);
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

		protected async Task InitShowConfirmAssign()
		{
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

		private async Task OnModalHidden()
		{
			if (IsToClose)
			{
				await Task.Delay(1);
			}
		}

		protected async Task Search()
		{
			SetModelRM();
			StateHasChanged();
			await Task.Delay(1);
			//_Navs.NavigateTo($"{Pager?.UrlAction}?{filter.SetParameter(true)}");
		}

		protected async Task GotoStep(int step, Guid? assignmentId = null, bool? back = null)
		{
			bool isNext = true;
			ClearSearchAssigned();
			ClearSearchCustomer();

			if (step == StepAssignLoanModel.Home)
			{
				assignmentIdPrevious = null;
				SaleMoveAssigned = null;
				ClearItemsIsSelectAll();
			}

			if (assignmentId.HasValue)
			{
				assignmentIdPrevious = assignmentId;
			}

			if (step == StepAssignLoanModel.Customer)
			{
				//ถ้า SaleMoveAssigned มีค่าแสดงว่าผ่านขั้นตอนการเลือกลูกค้าและไปที่หน้าเลือกผู้รับผิดชอบแล้ว กดย้อนหลับมา
				if (back == true && SaleMoveAssigned != null)
				{
					//var _items = ItemsRM?.FirstOrDefault(x => x.Id == assignmentIdPrevious);
					//if (_items != null && _items.Assignment_RM_Sales != null)
					//{
					//	_items.Assignment_RM_Sales.AddRange(SaleMoveAssigned);
					//	_items.Assignment_RM_Sales = _items.Assignment_RM_Sales.OrderBy(x => x.CreateDate).ToList();
					//}
					//SaleMoveAssigned = null;
				}
			}
			else if (step == StepAssignLoanModel.Assigned)
			{
				if (back != true)
				{
					isNext = CheckSelectCustomer();
					if (!isNext)
					{
						_utilsViewModel.AlertWarning("เลือกลูกค้า");
					}
					else
					{
						//เก็บกลุ่มลูกค้าที่ถูกเลือกไว้ใน SaleMoveAssigned และ move ตอนเลือกผู้รับผิดชอบ
						//SetDataCustomerMove();
						//SetModelRM();

						SetModelRMNew();

						var _itemsMain = ItemsRM?.FirstOrDefault(x => x.Id == assignmentIdPrevious);
						if (_itemsMain != null && _itemsMain.Assignment_RM_Sales != null)
						{
							var _itemsMove = _itemsMain.Assignment_RM_Sales.Where(s => s.IsSelectMove).ToList();
							if (_itemsMove.Count > 0)
							{
								//ก็บกลุ่มลูกค้าที่ถูกเลือกไว้ใน SaleMoveAssigned ใช้แสดงตอน Sammary
								SaleMoveAssigned = new();
								SaleMoveAssigned = new(_itemsMove);
							}
						}
					}
				}
				else
				{
					//กรณีย้อนกลับจาก Summary ต้องลบกลุ่มลูกค้าที่ถูก move ไปก่อนหน้า
					//ClearDataCustomerMove();
				}
			}
			else if (step == StepAssignLoanModel.Summary)
			{
				//SetDataCustomerMove();
				isNext = Summary();
				if (!isNext)
				{
					_utilsViewModel.AlertWarning("เลือกผู้รับผิดชอบ");
				}
			}
			else if (step == StepAssignLoanModel.Confirm)
			{
				SetDataCustomerMove();
				assignmentIdPrevious = null;
				SaleMoveAssigned = null;
				ClearItemsIsSelectAll();
				step = StepAssignLoanModel.Home;
			}

			if (isNext)
			{
				stepAssign = step;
				StateHasChanged();
			}

			await Task.Delay(10);
			await SetInitManual();
		}

		//2. ลูกค้า
		protected void OnCheckCustomer(Assignment_RM_SaleCustom model, object? checkedValue)
		{
			if (checkedValue != null && (bool)checkedValue)
			{
				model.IsSelectMove = true;
			}
			else
			{
				model.IsSelectMove = false;
			}
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

		protected void SearchStepCustomer(object? businesstype = null)
		{
			if (ItemsRM?.Count > 0)
			{
				var _items = ItemsRM.FirstOrDefault(x => x.Id == assignmentIdPrevious);

				string valSearch = filterRM.searchtxt ?? string.Empty;
				string valbusinesstype = businesstype?.ToString() ?? string.Empty;
				if (!String.IsNullOrEmpty(valSearch) || !String.IsNullOrEmpty(valbusinesstype))
				{
					if (_items != null && _items.Assignment_RM_Sales != null)
					{
						foreach (var item in _items.Assignment_RM_Sales)
						{
							Guid businesstypeid = Guid.Empty;
							if (!String.IsNullOrEmpty(valSearch) && !Guid.TryParse(valbusinesstype, out businesstypeid))
							{
								item.IsShow = item.Sale != null && item.Sale.CompanyName != null && item.Sale.CompanyName.Contains(valSearch);
							}

							if (String.IsNullOrEmpty(valSearch) && Guid.TryParse(valbusinesstype, out businesstypeid))
							{
								item.IsShow = item.Sale != null
										&& item.Sale.Customer != null
										&& item.Sale.Customer.Master_BusinessTypeId != null
										&& item.Sale.Customer.Master_BusinessTypeId == businesstypeid;
							}

							if (!String.IsNullOrEmpty(valSearch) && Guid.TryParse(valbusinesstype, out businesstypeid))
							{
								item.IsShow = (item.Sale != null && item.Sale.CompanyName != null && item.Sale.CompanyName.Contains(valSearch))
										   && (item.Sale != null && item.Sale.Customer != null && item.Sale.Customer.Master_BusinessTypeId != null && item.Sale.Customer.Master_BusinessTypeId == businesstypeid);
							}
						}


					}
				}
				else
				{
					ClearSearchCustomer();
				}

			}
		}

		protected void ClearSearchCustomer()
		{
			filter = new();
			if (ItemsRM?.Count > 0)
			{
				var _items = ItemsRM.FirstOrDefault(x => x.Id == assignmentIdPrevious);
				if (_items != null && _items.Assignment_RM_Sales != null)
				{
					foreach (var item in _items.Assignment_RM_Sales.Where(x => !x.IsShow))
					{
						item.IsShow = true;
					}
				}
			}
		}

		//3. ผู้รับผิดชอบ
		protected void OnCheckEmployee(Assignment_RMCustom model, object? checkedValue)
		{
			if (ItemsRMNew?.Count > 0)
			{
				foreach (var item in ItemsRMNew.Where(x => x.IsSelectMove))
				{
					item.IsSelectMove = false;
				}
			}

			if (checkedValue != null && (bool)checkedValue)
			{
				model.IsSelectMove = true;
			}
			else
			{
				model.IsSelectMove = false;
			}
		}

		protected void ClearDataCustomerMove()
		{
			if (ItemsRM?.Count > 0 && SaleMoveAssigned != null)
			{
				var _itemsAssign = ItemsRM.Where(x => x.IsSelectMove).FirstOrDefault();
				if (_itemsAssign != null && _itemsAssign.Assignment_RM_Sales != null)
				{
					foreach (var item in SaleMoveAssigned)
					{
						if (_itemsAssign.Assignment_RM_Sales.Select(x => x.Id).Contains(item.Id))
						{
							_itemsAssign.Assignment_RM_Sales.Remove(item);
						}
					}
				}
			}
		}

		protected void ClearItemsIsSelectAll()
		{
			if (ItemsRM?.Count > 0)
			{
				foreach (var item in ItemsRM)
				{
					item.IsSelectMove = false;
					if (item.Assignment_RM_Sales?.Count(x => x.IsSelectMove) > 0)
					{
						foreach (var item_sale in item.Assignment_RM_Sales.Where(x => x.IsSelectMove))
						{
							item_sale.IsSelectMove = false;
						}
					}
				}
			}
		}

		protected bool CheckSelectCustomer()
		{
			if (ItemsRM?.Count > 0)
			{
				var _items = ItemsRM.FirstOrDefault(x => x.Id == assignmentIdPrevious);
				if (_items != null && _items.Assignment_RM_Sales != null)
				{
					var _itemsCheck = _items.Assignment_RM_Sales.Any(s => s.IsSelectMove);
					return _itemsCheck;
				}
			}
			return false;
		}

		//4. สรุปผู้รับผิดชอบและลูกค้าที่ได้รับมอบหมาย
		protected bool Summary()
		{
			if (ItemsRMNew?.Count > 0)
			{
				//_itemsAssign ผู้รับผิดชอบใหม่ที่ถูกมอบหมาย
				var _itemsAssign = ItemsRMNew.Where(x => x.IsSelectMove).FirstOrDefault();
				if (_itemsAssign != null)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			return false;
		}

		protected void SearchStepAssigned()
		{
			if (ItemsRM?.Count > 0)
			{
				var _items = ItemsRM?.Where(x => x.Id != assignmentIdPrevious).ToList();

				if (!String.IsNullOrEmpty(filterRMNew.emp_id) || !String.IsNullOrEmpty(filterRMNew.emp_name))
				{
					if (_items != null)
					{
						foreach (var item in _items)
						{
							if (!String.IsNullOrEmpty(filterRMNew.emp_id) && String.IsNullOrEmpty(filterRMNew.emp_name))
							{
								item.IsShow = item.EmployeeId != null && item.EmployeeId.Contains(filterRMNew.emp_id);
							}

							if (String.IsNullOrEmpty(filterRMNew.emp_id) && !String.IsNullOrEmpty(filterRMNew.emp_name))
							{
								item.IsShow = item.EmployeeName != null && item.EmployeeName.Contains(filterRMNew.emp_name);
							}

							if (!String.IsNullOrEmpty(filterRMNew.emp_id) && !String.IsNullOrEmpty(filterRMNew.emp_name))
							{
								item.IsShow = (item.EmployeeId != null && item.EmployeeId.Contains(filterRMNew.emp_id))
										   && (item.EmployeeName != null && item.EmployeeName.Contains(filterRMNew.emp_name));
							}
						}

					}
				}
				else
				{
					ClearSearchAssigned();
				}

			}
		}

		protected void ClearSearchAssigned()
		{
			filter = new();
			if (ItemsRM?.Count > 0)
			{
				var _items = ItemsRM?.Where(x => x.Id != assignmentIdPrevious).ToList();
				if (_items != null)
				{
					foreach (var item in _items)
					{
						item.IsShow = true;
					}
				}
			}
		}

		//5. เพิ่มลูกค้าไปผู้รับผิดชอบใหม่ ลบลูกค้าผู้ับผิดชอบเดิม ใน model หลัก
		protected void SetDataCustomerMove()
		{
			if (ItemsRMNew?.Count > 0 && ItemsAll?.Count > 0 && SaleMoveAssigned?.Count > 0)
			{
				//ผู้รับผิดชอบเดิม
				var _itemsRemove = ItemsAll.FirstOrDefault(x => x.Id == assignmentIdPrevious);
				//ผู้รับผิดชอบใหม่
				var _itemsRMNew = ItemsRMNew.FirstOrDefault(x => x.IsSelectMove);

				if (_itemsRMNew != null && _itemsRemove != null && _itemsRemove.Assignment_RM_Sales != null)
				{
					//ผู้รับผิดชอบใหม่ใน model หลัก
					var _itemsAssign = ItemsAll.FirstOrDefault(x => x.Id == _itemsRMNew.Id);

					if (_itemsAssign != null && _itemsAssign.Assignment_RM_Sales != null)
					{
						foreach (var item in SaleMoveAssigned)
						{
							//เพิ่มลูกค้าใหม่ไปยังผู้รับผิดชอบใหม่
							if (!_itemsAssign.Assignment_RM_Sales.Select(x => x.Id).Contains(item.Id))
							{
								_itemsAssign.Assignment_RM_Sales.Add(item);
							}
							//ลบลูกค้าออกจากผู้รับผิดชอบเดิม
							var _itemsRemoveCheck = _itemsRemove.Assignment_RM_Sales.FirstOrDefault(x => x.Id == item.Id);
							if (_itemsRemoveCheck != null)
							{
								_itemsRemove.Assignment_RM_Sales.Remove(_itemsRemoveCheck);
							}
						}

						//ทำสำเนาจาก model หลัก ไปยัง ItemsRM เพื่อแสดงผลในหน้า home
						ItemsRM = GeneralUtils.DeepCopyJson(ItemsAll);
					}
				}
			}
		}

		protected async Task Assign()
		{
			_errorMessage = null;

			if (ItemsRM != null)
			{
				var response = await _assignmentRMViewModel.Assign(ItemsRM);

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
					_errorMessage = response.errorMessage;
					await _jsRuntimes.InvokeVoidAsync("WarningAlert", _errorMessage);
				}
			}

		}


	}
}