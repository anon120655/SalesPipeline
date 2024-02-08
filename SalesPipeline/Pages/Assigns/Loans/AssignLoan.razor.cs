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
			await _jsRuntimes.InvokeVoidAsync("BootSelectId", "Province");
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
			await Task.Delay(3000);
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
			}
		}

		protected async Task Assign()
		{
			_errorMessage = null;

			if (Items != null)
			{
				var response = await _assignmentViewModel.Assign(Items);

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

		protected async Task Search()
		{
			await SetModel();
			StateHasChanged();
			_Navs.NavigateTo($"{Pager?.UrlAction}?{filter.SetParameter(true)}");
		}

		protected async Task OnProvince(ChangeEventArgs e)
		{
			filter.province = null;
			if (e.Value != null)
			{
				filter.province = e.Value.ToString();

				await SetModel();
				StateHasChanged();
				_Navs.NavigateTo($"{Pager?.UrlAction}?{filter.SetParameter(true)}");
			}
		}

		protected async Task GotoStep(int step, Guid? _id = null)
		{
			bool isNext = true;
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
				var _items = Items.Any(x => x.Id == employeeIdPrevious && x.Assignment_Sales != null && x.Assignment_Sales.Any(s => s.IsSelectMove));
				return _items;
			}
			return false;
		}

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
									_itemsAssign.Assignment_Sales.Add(item);
								}
								//ลบข้อมูลลูกค้าผู้รับผิดชอบเดิม
								var _itemsRemove = _itemsCustomerMove.Assignment_Sales.FirstOrDefault(x => x.Id == item.Id);
								if (_itemsRemove != null)
								{
									_itemsCustomerMove.Assignment_Sales.Remove(_itemsRemove);
								}
							}
							else
							{
								//เพิ่มข้อมูลลูกค้าไปยังผู้รับผิดชอบเดิม
								if (!_itemsCustomerMove.Assignment_Sales.Select(x => x.Id).Contains(item.Id))
								{
									_itemsCustomerMove.Assignment_Sales.Add(item);
								}
								//ลบข้อมูลลูกค้าจากผู้รับผิดชอบใหม่
								var _itemsRemove = _itemsAssign.Assignment_Sales.FirstOrDefault(x => x.Id == item.Id);
								if (_itemsRemove != null)
								{
									_itemsAssign.Assignment_Sales.Remove(_itemsRemove);
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
				if (Assignment_Sale_Original == null && _items?.Assignment_Sales != null)
				{
					Assignment_Sale_Original = new(_items.Assignment_Sales);
				}
				else
				{
					//ถ้า object เดิมมีข้อมูลให้นำมายัดใส่ item ที่จะค้นหาก่อนค้นหา
					if (Assignment_Sale_Original != null && _items != null)
					{
						_items.Assignment_Sales = new(Assignment_Sale_Original);
					}
				}

				string valSearch = filter.searchtxt ?? string.Empty;
				string valbusinesstype = businesstype?.ToString() ?? string.Empty;
				if (!String.IsNullOrEmpty(valSearch) || !String.IsNullOrEmpty(valbusinesstype))
				{
					if (_items != null && _items.Assignment_Sales != null)
					{
						if (!String.IsNullOrEmpty(valSearch))
						{
							//_items.Assignment_Sales = _items.Assignment_Sales.Where(x => x.Sale != null
							//&& x.Sale.CompanyName != null
							//&& x.Sale.CompanyName.Contains(valSearch)).ToList();

							foreach (var item in _items.Assignment_Sales)
							{
								if (item.Sale != null && item.Sale.CompanyName != null && item.Sale.CompanyName.Contains(valSearch))
								{
									item.IsShow = false;
								}
							}

						}

						if (Guid.TryParse(valbusinesstype, out Guid businesstypeid))
						{
							if (_items != null && _items.Assignment_Sales != null)
							{
								//_items.Assignment_Sales = _items.Assignment_Sales.Where(x => x.Sale != null
								//&& x.Sale.Customer != null
								//&& x.Sale.Customer.Master_BusinessTypeId != null
								//&& x.Sale.Customer.Master_BusinessTypeId == businesstypeid).ToList();

								foreach (var item in _items.Assignment_Sales)
								{
									if (item.Sale != null
										&& item.Sale.Customer != null
										&& item.Sale.Customer.Master_BusinessTypeId != null
										&& item.Sale.Customer.Master_BusinessTypeId == businesstypeid)
									{
										item.IsShow = false;
									}
								}

							}
						}
					}
				}

			}
		}

		protected void ClearSearchCustomer()
		{
			if (Items?.Count > 0)
			{
				var _items = Items.FirstOrDefault(x => x.Id == employeeIdPrevious);
				if (_items != null && _items.Assignment_Sales != null)
				{
					foreach (var item in _items.Assignment_Sales.Where(x=> !x.IsShow))
					{
						item.IsShow = true;
					}
				}
			}
		}


	}
}