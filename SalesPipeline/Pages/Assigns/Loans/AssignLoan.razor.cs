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
		string? _errorMessageModal = null;
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
		//IsSelectNewAssign ใช้เช็คตอนกด back กลับจากหน้า Summary
		private Guid? IsSelectNewAssign = null;
		private Pager? Pager;
		private CustomerCustom? formView = null;
		private int stepAssign = StepAssignLoanModel.Home;
		private Guid? assignmentIdPrevious = null;

		ModalConfirm modalConfirmAssign = default!;
		ModalReturnAssign modalReturnAssign = default!;
		ModalReturnReason modalReturnReason = default!;
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
			ItemsRM = new();
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

				if (!String.IsNullOrEmpty(filterRM.province))
				{
					ItemsRM = ItemsRM.Where(x => x.ProvinceName != null && x.ProvinceName.Contains(filterRM.province)).ToList();
				}

				if (!String.IsNullOrEmpty(filterRM.amphur))
				{
					ItemsRM = ItemsRM.Where(x => x.BranchName != null && x.BranchName.Contains(filterRM.amphur)).ToList();
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

				if (IsSelectNewAssign.HasValue && IsSelectNewAssign != Guid.Empty)
				{
					var isSelect = ItemsRMNew.FirstOrDefault(x => x.Id == IsSelectNewAssign);
					if (isSelect != null)
					{
						isSelect.IsSelect = true;
					}
				}

				if (assignmentIdPrevious.HasValue)
				{
					ItemsRMNew = ItemsRMNew.Where(x => x.Id != assignmentIdPrevious).ToList();
				}

				if (!String.IsNullOrEmpty(filterRMNew.emp_id))
				{
					ItemsRMNew = ItemsRMNew.Where(x => x.EmployeeId != null && x.EmployeeId.Contains(filterRMNew.emp_id)).ToList();
				}

				if (!String.IsNullOrEmpty(filterRMNew.emp_name))
				{
					ItemsRMNew = ItemsRMNew.Where(x => x.EmployeeName != null && x.EmployeeName.Contains(filterRMNew.emp_name)).ToList();
				}

				if (!String.IsNullOrEmpty(filterRMNew.province))
				{
					ItemsRMNew = ItemsRMNew.Where(x => x.ProvinceName != null && x.ProvinceName.Contains(filterRMNew.province)).ToList();
				}

				if (!String.IsNullOrEmpty(filterRMNew.amphur))
				{
					ItemsRMNew = ItemsRMNew.Where(x => x.BranchName != null && x.BranchName.Contains(filterRMNew.amphur)).ToList();
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
			filterRM.province = null;
			filterRM.amphur = null;
			filterRMNew.province = null;
			filterRMNew.amphur = null;
			LookUp.Amphurs = new();
			StateHasChanged();
			await _jsRuntimes.InvokeVoidAsync("BootSelectEmptyID", "Amphur");

			if (_provinceID != null && int.TryParse(_provinceID, out int provinceID))
			{
				filterRM.province = provinceID.ToString();
				filterRMNew.province = provinceID.ToString();

				if (int.TryParse(filterRM.province, out int id))
				{
					//เปลี่ยนเป็น name เพื่อเช็คใน Items
					filterRM.province = _provinceName;
					filterRMNew.province = _provinceName;

					var amphurs = await _masterViewModel.GetAmphur(id);
					if (amphurs != null && amphurs.Data?.Count > 0)
					{
						LookUp.Amphurs = new List<InfoAmphurCustom>() { new InfoAmphurCustom() { AmphurID = 0, AmphurName = "--ทั้งหมด--" } };
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

			if (stepAssign == StepAssignLoanModel.Home)
			{
				SetModelRM();
			}
			else if (stepAssign == StepAssignLoanModel.Customer)
			{
				SearchStepCustomer();
			}
			else if (stepAssign == StepAssignLoanModel.Assigned)
			{
				SetModelRMNew();
			}
			StateHasChanged();

		}

		[JSInvokable]
		public async Task AmphurChange(string _amphurID, string _amphurName)
		{
			await Task.Delay(1);
			filterRM.amphur = null;
			filterRMNew.amphur = null;
			if (_amphurID != null && int.TryParse(_amphurID, out int amphurID))
			{
				if (amphurID != 0)
				{
					//เปลี่ยนเป็น name เพื่อเช็คใน Items
					filterRM.amphur = _amphurName;
					filterRMNew.amphur = _amphurName;
				}
				else
				{
					filterRM.amphur = null;
					filterRMNew.amphur = null;
				}
			}

			if (stepAssign == StepAssignLoanModel.Home)
			{
				SetModelRM();
			}
			else if (stepAssign == StepAssignLoanModel.Customer)
			{
				SearchStepCustomer();
			}
			else if (stepAssign == StepAssignLoanModel.Assigned)
			{
				SetModelRMNew();
			}
			StateHasChanged();
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

		protected async Task InitShowReturnAssign()
		{
			_errorMessageModal = null;
			await ShowReturnAssign(null, "ท่านต้องการ \"ส่งคืน\" หรือ \"มอบหมาย\" ");
		}

		protected async Task ShowReturnAssign(string? id, string? txt, string? icon = null)
		{
			var isNext = CheckSelectCustomer();
			if (!isNext)
			{
				_utilsViewModel.AlertWarning("เลือกลูกค้า");
			}
			else
			{
				IsToClose = false;
				await modalReturnAssign.OnShowConfirm(id, $"{txt}", icon);
			}
		}

		protected async Task ReturnAssign(SelectModel model)
		{
			if (model.Value == "0")
			{
				await GotoStep(StepAssignLoanModel.Return);
			}
			else if (model.Value == "1")
			{
				await GotoStep(StepAssignLoanModel.Assigned);
			}
			await modalReturnAssign.OnHide();
		}

		protected async Task ShowReturnReason()
		{
			await modalReturnReason.OnShowConfirm();
		}

		protected async Task Return(string? id)
		{
			_errorMessageModal = null;

			if (Guid.TryParse(id, out Guid _id) && SaleMoveAssigned?.Count > 0)
			{
				var response = await _assignmentRMViewModel.Return(new()
				{
					CurrentUserId = UserInfo.Id,
					Master_ReasonReturnId = _id,
					RM_Sale = SaleMoveAssigned
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

		private async Task OnModalHidden()
		{
			if (IsToClose)
			{
				await Task.Delay(1);
			}
		}

		protected async Task SearchRM()
		{
			SetModelRM();
			StateHasChanged();
			await Task.Delay(1);
		}

		protected async Task SearchRMNew()
		{
			SetModelRMNew();
			StateHasChanged();
			await Task.Delay(1);
		}

		protected async Task GotoStep(int step, Guid? assignmentId = null, bool? back = null)
		{
			bool isNext = true;
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

			}
			else if (step == StepAssignLoanModel.Return)
			{
				isNext = CheckSelectCustomer();
				if (!isNext)
				{
					_utilsViewModel.AlertWarning("เลือกลูกค้า");
				}
				else
				{
					//เก็บกลุ่มลูกค้าที่ถูกเลือกไว้ใน SaleMoveAssigned ใช้แสดงตอน Sammary
					KeepSaleMove();
				}
			}
			else if (step == StepAssignLoanModel.Assigned)
			{
				isNext = CheckSelectCustomer();
				if (!isNext)
				{
					_utilsViewModel.AlertWarning("เลือกลูกค้า");
				}
				else
				{
					//set model ผู้รับผิดชอบใหม่  ใช้แสดงตอนเลือกผู้รับผิดชอบ Assigned
					SetModelRMNew();

					//เก็บกลุ่มลูกค้าที่ถูกเลือกไว้ใน SaleMoveAssigned ใช้แสดงตอน Sammary
					KeepSaleMove();
				}
			}
			else if (step == StepAssignLoanModel.Summary)
			{
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
				SetModelRM();
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

		//เลือกลูกค้า
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

		protected void OnViewCustomer(CustomerCustom? model)
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
				string valProvince = filterRM.province ?? string.Empty;
				string valAmphur = filterRM.amphur ?? string.Empty;
				if (!String.IsNullOrEmpty(valSearch) || !String.IsNullOrEmpty(valbusinesstype) || !String.IsNullOrEmpty(valProvince) || !String.IsNullOrEmpty(valAmphur))
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

							if (!String.IsNullOrEmpty(valProvince))
							{
								item.IsShow = item.Sale != null
										&& item.Sale.Customer != null
										&& item.Sale.Customer.ProvinceName != null
										&& item.Sale.Customer.ProvinceName.Contains(valProvince);
							}

							if (!String.IsNullOrEmpty(valAmphur))
							{
								item.IsShow = item.Sale != null
										&& item.Sale.Customer != null
										&& item.Sale.Customer.AmphurName != null
										&& item.Sale.Customer.AmphurName.Contains(valAmphur);
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

		protected void KeepSaleMove()
		{
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

		//เลือกผู้รับผิดชอบ
		protected void OnCheckEmployee(Assignment_RMCustom model, object? checkedValue)
		{
			if (ItemsRMNew?.Count > 0)
			{
				foreach (var item in ItemsRMNew.Where(x => x.IsSelect))
				{
					item.IsSelect = false;
				}
			}

			if (checkedValue != null && (bool)checkedValue)
			{
				model.IsSelect = true;
				IsSelectNewAssign = model.Id;
			}
			else
			{
				model.IsSelect = false;
				IsSelectNewAssign = null;
			}
		}

		protected void ClearDataCustomerMove()
		{
			if (ItemsRM?.Count > 0 && SaleMoveAssigned != null)
			{
				var _itemsAssign = ItemsRM.Where(x => x.IsSelect).FirstOrDefault();
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
			if (ItemsAll?.Count > 0)
			{
				foreach (var item in ItemsAll)
				{
					item.IsSelect = false;
					if (item.Assignment_RM_Sales?.Count(x => x.IsSelectMove) > 0)
					{
						foreach (var item_sale in item.Assignment_RM_Sales.Where(x => x.IsSelectMove))
						{
							item_sale.IsSelectMove = false;
						}
					}
				}
			}
			ItemsRM = GeneralUtils.DeepCopyJson(ItemsAll);
			ItemsRMNew = GeneralUtils.DeepCopyJson(ItemsAll);
			IsSelectNewAssign = null;
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

		//สรุปผู้รับผิดชอบและลูกค้าที่ได้รับมอบหมาย
		protected bool Summary()
		{
			if (ItemsRMNew?.Count > 0)
			{
				//_itemsAssign ผู้รับผิดชอบใหม่ที่ถูกมอบหมาย
				var _itemsAssign = ItemsRMNew.Where(x => x.IsSelect).FirstOrDefault();
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

		//เพิ่มลูกค้าไปผู้รับผิดชอบใหม่ ลบลูกค้าผู้ับผิดชอบเดิม ใน model หลัก
		protected void SetDataCustomerMove()
		{
			if (ItemsRMNew?.Count > 0 && ItemsAll?.Count > 0 && SaleMoveAssigned?.Count > 0)
			{
				//ผู้รับผิดชอบเดิม
				var _itemsRemove = ItemsAll.FirstOrDefault(x => x.Id == assignmentIdPrevious);
				//ผู้รับผิดชอบใหม่
				var _itemsRMNew = ItemsRMNew.FirstOrDefault(x => x.IsSelect);

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
					}
				}
			}
		}

		protected async Task Assign()
		{
			_errorMessageModal = null;

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
					_errorMessageModal = response.errorMessage;
				}
			}

		}


	}
}