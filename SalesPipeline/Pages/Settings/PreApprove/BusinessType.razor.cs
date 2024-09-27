using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SalesPipeline.Shared.Modals;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Pages.Settings.PreApprove
{
	public partial class BusinessType
	{
		public Guid? id { get; set; }

		string? _errorMessage = null;
		private User_PermissionCustom _permission = new();
		private allFilter filter = new();
		private bool isLoading = false;
		private List<Master_Pre_BusinessTypeCustom>? Items;
		public Pager? Pager;
		private Master_Pre_BusinessTypeCustom formModel = new();

		Modal modalForm = default!;
		ModalConfirm modalConfirm = default!;

		protected override async Task OnInitializedAsync()
		{
			_permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.SetPreApprove) ?? new User_PermissionCustom();
			StateHasChanged();
			await Task.Delay(1);
		}

		protected async override Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				await SetQuery();
				StateHasChanged();

				firstRender = false;
			}
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
			var data = await _masterViewModel.GetPre_BusType(filter);
			if (data != null && data.Status)
			{
				Items = data.Data?.Items;
				Pager = data.Data?.Pager;
				if (Pager != null)
				{
					Pager.UrlAction = "/setting/pre/bustype";
				}
			}
			else
			{
				_errorMessage = data?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}
		}

		protected async Task SetModelById(Guid id)
		{
			var data = await _masterViewModel.GetPre_BusTypeById(id);
			if (data != null && data.Status)
			{
				if (data.Data != null)
				{
					formModel = data.Data;
				}
			}
			else
			{
				_errorMessage = data?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}
			StateHasChanged();
		}

		private async Task Save()
		{
			_errorMessage = null;
			ShowLoading();

			ResultModel<Master_Pre_BusinessTypeCustom> response;

			formModel.CurrentUserId = UserInfo.Id;

			if (id.HasValue && id != Guid.Empty)
			{
				response = await _masterViewModel.UpdatePre_BusType(formModel);
			}
			else
			{
				response = await _masterViewModel.CreatePre_BusType(formModel);
			}

			if (response.Status)
			{
				await _jsRuntimes.InvokeVoidAsync("SuccessAlert");
				HideLoading();
				await OnHide();
				await SetModel();
			}
			else
			{
				HideLoading();
				_errorMessage = response.errorMessage;
				await _jsRuntimes.InvokeVoidAsync("WarningAlert", _errorMessage);
			}
		}

		protected async Task ConfirmDelete(string? id, string? txt)
		{
			await modalConfirm.OnShowConfirm(id, $"คุณต้องการลบข้อมูล <span class='text-primary'>{txt}</span>");
		}

		protected async Task Delete(string id)
		{
			await modalConfirm.OnHideConfirm();

			var data = await _masterViewModel.DeletePre_BusTypeById(new UpdateModel() { id = id, userid = UserInfo.Id });
			if (data != null && !data.Status && !String.IsNullOrEmpty(data.errorMessage))
			{
				_errorMessage = data?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}
			await SetModel();
		}

		protected async Task StatusChanged(ChangeEventArgs e, Guid id)
		{
			if (e.Value != null && Boolean.TryParse(e.Value.ToString(), out bool val))
			{
				var data = await _masterViewModel.UpdateStatusPre_BusTypeById(new UpdateModel() { id = id.ToString(), userid = UserInfo.Id, value = val.ToString() });
				if (data != null && !data.Status && !String.IsNullOrEmpty(data.errorMessage))
				{
					_errorMessage = data?.errorMessage;
					_utilsViewModel.AlertWarning(_errorMessage);
				}
				else
				{
					string? actiontxt = val ? "<i class=\"fa-regular fa-circle-check\"></i> เปิด" : "<i class=\"fa-solid fa-circle-xmark\"></i> ปิด";
					string fulltxt = $"{actiontxt}การใช้งานเรียบร้อย";
					await _jsRuntimes.InvokeVoidAsync("SuccessAlert", fulltxt);
					await SetModel();
				}
			}
		}

		private async Task OnShow(Guid? _id = null)
		{
			if (_id.HasValue && _id.Value != Guid.Empty)
			{
				id = _id;
				await SetModelById(_id.Value);
			}
			else
			{
				id = null;
				formModel = new();
			}
			await modalForm.ShowAsync();
		}

		private async Task OnHide()
		{
			id = null;
			await modalForm.HideAsync();
		}

		private void ShowLoading()
		{
			isLoading = true;
			StateHasChanged();
		}

		private void HideLoading()
		{
			isLoading = false;
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
	}
}