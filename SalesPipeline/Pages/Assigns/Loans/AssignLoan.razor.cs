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
		private allFilter filter = new();
		private LookUpResource LookUp = new();
		private List<AssignmentCustom>? Items;
		private List<Assignment_SaleCustom>? Assignment_Sale_Original;
		private List<Assignment_SaleCustom>? AssignedModel;
		private Pager? Pager;
		private SaleCustom? formView = null;
		private int stepAssign = StepAssignLoanModel.Home;
		private Guid? employeeIdPrevious = null;

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

		protected async Task SetModel()
		{
			filter.pagesize = 100;
			var data = await _assignmentViewModel.GetListAutoAssign(filter);
			if (data != null && data.Status)
			{
				Items = data.Data?.Items;
				Pager = data.Data?.Pager;
				if (Pager != null)
				{
					Pager.UrlAction = "/assign/loan";
				}
			}
			else
			{
				_errorMessage = data?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
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

		protected async Task Assign()
		{
			_errorMessage = null;

			if (Items != null)
			{
				ClearDeleteAssigned();

				var aaa = 11;

				//var response = await _assignmentViewModel.Assign(Items);

				//if (response.Status)
				//{
				//	IsToClose = true;
				//	await modalConfirmAssign.OnHideConfirm();
				//	await ShowSuccessfulAssign(null, "เสร็จสิ้นการมอบหมายงาน");
				//	await SetModel();
				//	HideLoading();
				//}
				//else
				//{
				//	HideLoading();
				//	_errorMessage = response.errorMessage;
				//	await _jsRuntimes.InvokeVoidAsync("WarningAlert", _errorMessage);
				//}
			}

		}

		protected async Task Search()
		{
			await SetModel();
			StateHasChanged();
			_Navs.NavigateTo($"{Pager?.UrlAction}?{filter.SetParameter(true)}");
		}

		protected async Task GotoStep(int step, Guid? _id = null)
		{
			bool isNext = true;
			ClearSearchAssigned();
			ClearSearchCustomer();

			if (step == StepAssignLoanModel.Home)
			{
				employeeIdPrevious = null;
				ClearItemsIsSelectAll();
			}

			if (_id.HasValue)
			{
				employeeIdPrevious = _id;
			}

			if (step == StepAssignLoanModel.Assigned)
			{
				SetOriginalAssigned();
				isNext = CheckSelectCustomer();
				if (!isNext)
				{
					_utilsViewModel.AlertWarning("เลือกลูกค้า");
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

			if (isNext)
			{
				stepAssign = step;
				StateHasChanged();
			}

			await Task.Delay(10);
			await SetInitManual();
			//await _jsRuntimes.InvokeVoidAsync("selectPickerInitialize");
		}

		protected void OnCheckCustomer(Assignment_SaleCustom model, object? checkedValue)
		{

			//เก็บ object เดิม
			//if (Assignment_Sale_Original == null && _items?.Assignment_Sales != null)
			//{
			//	Assignment_Sale_Original = new(_items.Assignment_Sales);
			//}
			//else
			//{
			//	//ถ้า object เดิมมีข้อมูลให้นำมายัดใส่ item ที่จะค้นหาก่อนค้นหา
			//	if (Assignment_Sale_Original != null && _items != null)
			//	{
			//		_items.Assignment_Sales = new(Assignment_Sale_Original);
			//	}
			//}

			if (checkedValue != null && (bool)checkedValue)
			{
				model.IsSelectMove = true;
			}
			else
			{
				model.IsSelectMove = false;
			}
		}

		protected void OnCheckEmployee(AssignmentCustom model, object? checkedValue)
		{
			if (Items?.Count > 0)
			{
				foreach (var item in Items.Where(x => x.IsSelectMove))
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

		protected void ClearItemsIsSelectAll()
		{
			if (Items?.Count > 0)
			{
				foreach (var item in Items)
				{
					item.IsSelectMove = false;
					if (item.Assignment_Sales?.Count(x => x.IsSelectMove) > 0)
					{
						foreach (var item_sale in item.Assignment_Sales.Where(x => x.IsSelectMove))
						{
							item_sale.IsSelectMove = false;
						}
					}
				}
			}
		}

		protected bool CheckSelectCustomer()
		{
			if (Items?.Count > 0)
			{
				var _items = Items.FirstOrDefault(x => x.Id == employeeIdPrevious);
				if (_items != null && _items.Assignment_Sales != null)
				{
					var _itemsCheck = _items.Assignment_Sales.Any(s => s.IsSelectMove);
					return _itemsCheck;
				}
			}
			return false;
		}

		//protected bool Summary()
		//{
		//	if (Items?.Count > 0)
		//	{
		//		//_itemsAssign ผู้รับผิดชอบใหม่ที่ถูกมอบหมาย
		//		var _itemsAssign = Items.Where(x => x.IsSelectMove).FirstOrDefault();
		//		if (_itemsAssign != null && _itemsAssign.Assignment_Sales != null)
		//		{
		//			//_itemsCustomerMove ผู้รับผิดชอบเดิม
		//			var _itemsCustomerMove = Items?.Where(x => x.Id == employeeIdPrevious).FirstOrDefault();
		//			if (_itemsCustomerMove != null && _itemsCustomerMove.Assignment_Sales != null)
		//			{
		//				var itemAssign = _itemsCustomerMove.Assignment_Sales.ToList();
		//				foreach (var item in itemAssign)
		//				{
		//					if (item.IsSelectMove)
		//					{
		//						//เพิ่มข้อมูลลูกค้าจากผู้รับผิดชอบเดิมไปยังผู้รับผิดชอบใหม่
		//						if (!_itemsAssign.Assignment_Sales.Select(x => x.Id).Contains(item.Id))
		//						{
		//							//_itemsAssign.Assignment_Sales.Add(item);
		//							_itemsAssign.Assignment_Sales.Add(new()
		//							{
		//								Id = item.Id,
		//								Status = item.Status,
		//								CreateDate = item.CreateDate,
		//								CreateBy = item.CreateBy,
		//								CreateByName = item.CreateByName,
		//								AssignmentId = item.AssignmentId,
		//								IsActive = item.IsActive,
		//								SaleId = item.SaleId,
		//								Description = item.Description,
		//								Sale = item.Sale,
		//								IsSelect = item.IsSelect,
		//								IsSelectMove = item.IsSelectMove,
		//								IsShow = item.IsShow
		//							});
		//						}
		//						//ลบข้อมูลลูกค้าผู้รับผิดชอบเดิม
		//						var _itemsRemove = _itemsCustomerMove.Assignment_Sales.FirstOrDefault(x => x.Id == item.Id);
		//						if (_itemsRemove != null)
		//						{
		//							_itemsRemove.Status = StatusModel.Delete;
		//							//_itemsCustomerMove.Assignment_Sales.Remove(_itemsRemove);
		//						}
		//					}
		//					else
		//					{
		//						//เพิ่มข้อมูลลูกค้าไปยังผู้รับผิดชอบเดิม
		//						var _itemsAdd = _itemsCustomerMove.Assignment_Sales.FirstOrDefault(x => x.Id == item.Id);
		//						if (_itemsAdd != null)
		//						{
		//							_itemsAdd.Status = StatusModel.Active;
		//						}
		//						//if (!_itemsCustomerMove.Assignment_Sales.Select(x => x.Id).Contains(item.Id))
		//						//{
		//						//	_itemsCustomerMove.Assignment_Sales.Add(item);
		//						//}
		//						//ลบข้อมูลลูกค้าจากผู้รับผิดชอบใหม่
		//						var _itemsRemove = _itemsAssign.Assignment_Sales.FirstOrDefault(x => x.Id == item.Id);
		//						if (_itemsRemove != null)
		//						{
		//							_itemsRemove.Status = StatusModel.Delete;
		//							//_itemsAssign.Assignment_Sales.Remove(_itemsRemove);
		//						}
		//					}
		//				}
		//			}

		//			return true;
		//		}
		//		else
		//		{
		//			return false;
		//		}
		//	}
		//	return false;
		//}

		protected bool Summary()
		{
			if (Items?.Count > 0)
			{
				//_itemsAssign ผู้รับผิดชอบใหม่ที่ถูกมอบหมาย
				var _itemsAssign = Items.Where(x => x.IsSelectMove).FirstOrDefault();
				if (_itemsAssign != null && _itemsAssign.Assignment_Sales != null)
				{
					//_itemsCustomerMove ผู้รับผิดชอบเดิม
					var _itemsCustomerMove = Items?.Where(x => x.Id == employeeIdPrevious).FirstOrDefault();
					if (_itemsCustomerMove != null && _itemsCustomerMove.Assignment_Sales != null)
					{
						var itemAssign = _itemsCustomerMove.Assignment_Sales.ToList();
						foreach (var item in itemAssign)
						{
							if (item.IsSelectMove)
							{
								//เพิ่มข้อมูลลูกค้าจากผู้รับผิดชอบเดิมไปยังผู้รับผิดชอบใหม่
								if (!_itemsAssign.Assignment_Sales.Select(x => x.Id).Contains(item.Id))
								{
									//_itemsAssign.Assignment_Sales.Add(item);
									_itemsAssign.Assignment_Sales.Add(new()
									{
										Id = item.Id,
										Status = item.Status,
										CreateDate = item.CreateDate,
										CreateBy = item.CreateBy,
										CreateByName = item.CreateByName,
										AssignmentId = item.AssignmentId,
										IsActive = item.IsActive,
										SaleId = item.SaleId,
										Description = item.Description,
										Sale = item.Sale,
										IsSelect = item.IsSelect,
										IsSelectMove = item.IsSelectMove,
										IsShow = item.IsShow
									});
								}
								//ลบข้อมูลลูกค้าผู้รับผิดชอบเดิม
								var _itemsRemove = _itemsCustomerMove.Assignment_Sales.FirstOrDefault(x => x.Id == item.Id);
								if (_itemsRemove != null)
								{
									_itemsRemove.Status = StatusModel.Delete;
									//_itemsCustomerMove.Assignment_Sales.Remove(_itemsRemove);
								}
							}
							else
							{
								//เพิ่มข้อมูลลูกค้าไปยังผู้รับผิดชอบเดิม
								var _itemsAdd = _itemsCustomerMove.Assignment_Sales.FirstOrDefault(x => x.Id == item.Id);
								if (_itemsAdd != null)
								{
									_itemsAdd.Status = StatusModel.Active;
								}
								//if (!_itemsCustomerMove.Assignment_Sales.Select(x => x.Id).Contains(item.Id))
								//{
								//	_itemsCustomerMove.Assignment_Sales.Add(item);
								//}
								//ลบข้อมูลลูกค้าจากผู้รับผิดชอบใหม่
								var _itemsRemove = _itemsAssign.Assignment_Sales.FirstOrDefault(x => x.Id == item.Id);
								if (_itemsRemove != null)
								{
									_itemsRemove.Status = StatusModel.Delete;
									//_itemsAssign.Assignment_Sales.Remove(_itemsRemove);
								}
							}
						}
					}

					return true;
				}
				else
				{
					return false;
				}
			}
			return false;
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
			if (Items?.Count > 0)
			{
				var _items = Items.FirstOrDefault(x => x.Id == employeeIdPrevious);
				//เก็บ object เดิม
				//if (Assignment_Sale_Original == null && _items?.Assignment_Sales != null)
				//{
				//	Assignment_Sale_Original = new(_items.Assignment_Sales);
				//}
				//else
				//{
				//	//ถ้า object เดิมมีข้อมูลให้นำมายัดใส่ item ที่จะค้นหาก่อนค้นหา
				//	if (Assignment_Sale_Original != null && _items != null)
				//	{
				//		_items.Assignment_Sales = new(Assignment_Sale_Original);
				//	}
				//}

				string valSearch = filter.searchtxt ?? string.Empty;
				string valbusinesstype = businesstype?.ToString() ?? string.Empty;
				if (!String.IsNullOrEmpty(valSearch) || !String.IsNullOrEmpty(valbusinesstype))
				{
					if (_items != null && _items.Assignment_Sales != null)
					{
						foreach (var item in _items.Assignment_Sales)
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
			if (Items?.Count > 0)
			{
				var _items = Items.FirstOrDefault(x => x.Id == employeeIdPrevious);
				if (_items != null && _items.Assignment_Sales != null)
				{
					foreach (var item in _items.Assignment_Sales.Where(x => !x.IsShow))
					{
						item.IsShow = true;
					}
				}
			}
		}

		protected void SearchStepAssigned()
		{
			if (Items?.Count > 0)
			{
				var _items = Items?.Where(x => x.Id != employeeIdPrevious).ToList();

				if (!String.IsNullOrEmpty(filter.emp_id) || !String.IsNullOrEmpty(filter.emp_name))
				{
					if (_items != null)
					{
						foreach (var item in _items)
						{
							if (!String.IsNullOrEmpty(filter.emp_id) && String.IsNullOrEmpty(filter.emp_name))
							{
								item.IsShow = item.EmployeeId != null && item.EmployeeId.Contains(filter.emp_id);
							}

							if (String.IsNullOrEmpty(filter.emp_id) && !String.IsNullOrEmpty(filter.emp_name))
							{
								item.IsShow = item.EmployeeName != null && item.EmployeeName.Contains(filter.emp_name);
							}

							if (!String.IsNullOrEmpty(filter.emp_id) && !String.IsNullOrEmpty(filter.emp_name))
							{
								item.IsShow = (item.EmployeeId != null && item.EmployeeId.Contains(filter.emp_id))
										   && (item.EmployeeName != null && item.EmployeeName.Contains(filter.emp_name));
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
			if (Items?.Count > 0)
			{
				var _items = Items?.Where(x => x.Id != employeeIdPrevious).ToList();
				if (_items != null)
				{
					foreach (var item in _items)
					{
						item.IsShow = true;
					}
				}
			}
		}

		protected void ClearDeleteAssigned()
		{
			if (Items?.Count > 0)
			{
				foreach (var item in Items)
				{
					if (item.Assignment_Sales != null)
					{
						var saleRemove = item.Assignment_Sales.Where(x => x.Status == StatusModel.Delete).ToList();
						if (saleRemove.Count > 0)
						{
							foreach (var item_remove in saleRemove)
							{
								item.Assignment_Sales.Remove(item_remove);
							}
						}
					}
				}
			}
		}

		protected void SetOriginalAssigned()
		{
			if (Items?.Count > 0)
			{
				var _items = Items.Where(x => x.IsSelectMove).FirstOrDefault();


				var _itemsSale = _items?.Assignment_Sales?.Where(x => x.IsSelectMove).ToList();

				foreach (var item in Items)
				{
					if (item.Assignment_Sales != null)
					{
						var saleAdd = item.Assignment_Sales.Where(x => x.Status == StatusModel.Delete || x.IsSelectMove).ToList();
						if (saleAdd.Count > 0)
						{
							foreach (var item_handle in saleAdd)
							{
								if (item_handle.Status == StatusModel.Delete)
								{
									item_handle.Status = StatusModel.Active;
								}
								if (item_handle.AssignmentId != employeeIdPrevious && item_handle.IsSelectMove)
								{
									item.Assignment_Sales.Remove(item_handle);
								}
							}
						}
					}
				}
			}
		}


	}
}