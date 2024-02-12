using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils.Resources.Thailands;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Assignments;
using SalesPipeline.Shared.Modals;

namespace SalesPipeline.Pages.ApproveTargets
{
	public partial class ApproveTarget
	{
		string? _errorMessage = null;
		private bool isLoading = false;
		private User_PermissionCustom _permission = new();
		private allFilter filter = new();
		private LookUpResource LookUp = new();
		private List<SaleCustom>? Items;
		public Pager? Pager;
		ModalConfirm modalConfirmApprove = default!;
		ModalSuccessful modalSuccessfulApprove = default!;
		ModalNotApprove modalNotApprove = default!;

		protected override async Task OnInitializedAsync()
		{
			_permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.ApproveTarget) ?? new User_PermissionCustom();
			StateHasChanged();

			filter.sort = OrderByModel.ASC;
		}

		protected async override Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				await SetQuery();
				StateHasChanged();
				await SetInitManual();
				await Task.Delay(10);

				firstRender = false;
			}
		}

		protected async Task SetInitManual()
		{
			await _jsRuntimes.InvokeVoidAsync("BootSelectId", "DisplaySort");

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
			await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "ProvinceChange", "#Province");


			await _jsRuntimes.InvokeVoidAsync("BootSelectId", "Responsible");
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
			filter.idnumber = StatusSaleModel.WaitApproveCenter;
			var data = await _salesViewModel.GetList(filter);
			if (data != null && data.Status)
			{
				Items = data.Data?.Items;
				Pager = data.Data?.Pager;
				if (Pager != null)
				{
					Pager.UrlAction = "/approvetarget";
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

		protected async Task Search()
		{
			await SetModel();
			StateHasChanged();
			_Navs.NavigateTo($"{Pager?.UrlAction}?{filter.SetParameter(true)}");
		}

		protected async Task OnSort(ChangeEventArgs e)
		{
			filter.sort = null;
			if (e.Value != null)
			{
				filter.sort = e.Value.ToString();

				await SetModel();
				StateHasChanged();
				_Navs.NavigateTo($"{Pager?.UrlAction}?{filter.SetParameter(true)}");
			}
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

		protected void OnCheckCustomer(SaleCustom model, object? checkedValue)
		{
			if (checkedValue != null && (bool)checkedValue)
			{
				model.IsSelected = true;
			}
			else
			{
				model.IsSelected = false;
			}
		}

		protected async Task InitShowConfirmApprove()
		{
			if (!CheckSelectCustomer())
			{
				_utilsViewModel.AlertWarning("เลือกลูกค้า");
			}
			else
			{
				await ShowConfirmApprove(null, "กรุณากด ยืนยัน เพื่ออนุมัติ", "<img src=\"/image/icon/do.png\" width=\"65\" />");
			}
		}

		protected async Task ShowConfirmApprove(string? id, string? txt, string? icon = null)
		{
			await modalConfirmApprove.OnShowConfirm(id, $"{txt}", icon);
		}

		protected async Task ConfirmApprove(string id)
		{
			ShowLoading();
			await Task.Delay(10);
			await Approve();
		}

		protected async Task ShowSuccessfulApprove(string? id, string? txt)
		{
			await modalSuccessfulApprove.OnShow(id, $"{txt}");
		}

		protected bool CheckSelectCustomer()
		{
			if (Items?.Count > 0)
			{
				var _items = Items.Any(x => x.IsSelected);
				return _items;
			}
			return false;
		}

		protected async Task Approve()
		{
			_errorMessage = null;

			if (Items != null)
			{
				var _itemSelected = Items.Where(x => x.IsSelected).ToList();
				if (_itemSelected.Count > 0)
				{
					List<Sale_StatusCustom> modal = new();

					foreach (var item in _itemSelected)
					{
						modal.Add(new()
						{
							SaleId = item.Id,
							StatusId = StatusSaleModel.WaitAssign,
							CreateBy = UserInfo.Id
						});
					}

					//ผู้จัดการศูนย์สาขาอนุมัติกลุ่มเป้าหมายจากกิจการสาขาภาค ไปรอมอบหมาย
					var response = await _salesViewModel.UpdateStatusOnlyList(modal);

					if (response.Status)
					{
						await modalConfirmApprove.OnHideConfirm();
						await ShowSuccessfulApprove(null, "เสร็จสิ้นการอนุมัติกลุ่มเป้าหมาย");
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

		protected async Task InitShowNotApprove()
		{
			_errorMessage = null;
			if (!CheckSelectCustomer())
			{
				_utilsViewModel.AlertWarning("เลือกลูกค้า");
			}
			else
			{
				await ShowNotApprove(null, "กรุณาระบุเหตุผลการไม่อนุมัติ", "<img src=\"/image/icon/notapprove.png\" width=\"65\" />");
			}
		}

		protected async Task ShowNotApprove(string? id, string? txt, string? icon = null)
		{
			await modalNotApprove.OnShowConfirm(id, $"{txt}", icon);
		}

		protected async Task NotApproveModal(SelectModel model)
		{
			if (String.IsNullOrEmpty(model.Name))
			{
				_errorMessage = "ระบุเหตุผลการไม่อนุมัติ";
			}
			else
			{
				await NotApprove(model);
			}
		}

		protected async Task NotApprove(SelectModel model)
		{
			_errorMessage = null;

			if (Items != null)
			{
				var _itemSelected = Items.Where(x => x.IsSelected).ToList();
				if (_itemSelected.Count > 0)
				{
					List<Sale_StatusCustom> modal = new();

					foreach (var item in _itemSelected)
					{
						modal.Add(new()
						{
							SaleId = item.Id,
							StatusId = StatusSaleModel.NotApproveCenter,
							CreateBy = UserInfo.Id,
							Description = model.Name
						});
					}

					//ผู้จัดการศูนย์สาขาไม่อนุมัติกลุ่มเป้าหมาย
					var response = await _salesViewModel.UpdateStatusOnlyList(modal);

					if (response.Status)
					{
						await modalNotApprove.OnHideConfirm();
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
}