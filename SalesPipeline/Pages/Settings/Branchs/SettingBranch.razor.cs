using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SalesPipeline.Shared.Modals;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils.Resources.Thailands;

namespace SalesPipeline.Pages.Settings.Branchs
{
	public partial class SettingBranch
	{
		string? _errorMessage = null;
		private User_PermissionCustom _permission = new();
		private allFilter filter = new();
		private LookUpResource LookUp = new();
		private List<InfoBranchCustom>? Items;
		public Pager? Pager;

		ModalConfirm modalConfirm = default!;

		protected override async Task OnInitializedAsync()
		{
			_permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.SetBranch) ?? new User_PermissionCustom();
			StateHasChanged();
			await Task.Delay(1);
		}

		protected async override Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				await SetInitManual();

				await SetQuery();
				StateHasChanged();

				await _jsRuntimes.InvokeVoidAsync("selectPickerInitialize");
				StateHasChanged();
				firstRender = false;
			}
		}

		protected async Task SetInitManual()
		{
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
			await Task.Delay(1);
		}

		protected string? GetProvinceNameById(int id)
		{
			if (LookUp.Provinces?.Count > 0)
			{
				return LookUp.Provinces.FirstOrDefault(x=>x.ProvinceID == id)?.ProvinceName;
			}
			return string.Empty;
		}

		protected async Task SetQuery(string? parematerAll = null)
		{
			string uriQuery = String.Empty;

			uriQuery = _Navs.ToAbsoluteUri(_Navs.Uri).Query;

			if (parematerAll != null)
				uriQuery = $"?{parematerAll}";

			filter.SetUriQuery(uriQuery);

			await SetModel();
			StateHasChanged();
		}

		protected async Task SetModel()
		{
			var data = await _masterViewModel.GetBranchs(filter);
			if (data != null && data.Status)
			{
				Items = data.Data?.Items;
				Pager = data.Data?.Pager;
				if (Pager != null)
				{
					Pager.UrlAction = "/setting/branch";
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
			_Navs.NavigateTo($"{Pager?.UrlAction}?{filter.SetParameter(true)}");
		}

		protected async Task ConfirmDelete(string? id, string? txt)
		{
			await modalConfirm.OnShowConfirm(id, $"คุณต้องการลบข้อมูล <span class='text-primary'>{txt}</span>");
		}

		protected async Task Delete(string id)
		{
			await modalConfirm.OnHideConfirm();

			var data = await _masterViewModel.DeleteBranchById(new UpdateModel() { id = id, userid = UserInfo.Id });
			if (data != null && !data.Status && !String.IsNullOrEmpty(data.errorMessage))
			{
				_errorMessage = data?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}
			await SetModel();
		}

		protected async Task StatusChanged(ChangeEventArgs e, int id)
		{
			if (e.Value != null && Boolean.TryParse(e.Value.ToString(), out bool val))
			{
				var data = await _masterViewModel.UpdateStatusBranchById(new UpdateModel() { id = id.ToString(), userid = UserInfo.Id, value = val.ToString() });
				if (data != null && !data.Status && !String.IsNullOrEmpty(data.errorMessage))
				{
					_errorMessage = data?.errorMessage;
					_utilsViewModel.AlertWarning(_errorMessage);
				}
				else
				{
					string? actiontxt = val ? "<i class=\"fa-regular fa-circle-check\"></i> เปิด" : "<i class=\"fa-solid fa-circle-xmark\"></i> ปิด";
					string fulltxt = $"{actiontxt}การใช้งานเรียบร้อย";
					//_utilsViewModel.AlertSuccess(fulltxt);
					await _jsRuntimes.InvokeVoidAsync("SuccessAlert", fulltxt);
					await SetModel();
				}
			}
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