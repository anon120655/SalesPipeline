using Microsoft.AspNetCore.Components;
using SalesPipeline.Shared.Modals;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils;
using SalesPipeline.ViewModels;
using Microsoft.JSInterop;

namespace SalesPipeline.Pages.Users.User
{
	public partial class User
	{
		string? _errorMessage = null;
		private User_PermissionCustom _permission = new();
		private UserFilter filter = new();
		private LookUpResource LookUp = new();
		private List<UserCustom>? Items;
		public Pager? Pager;

		ModalConfirm modalConfirm = default!;
		protected bool IsStatus = true;

		protected override async Task OnInitializedAsync()
		{
			_permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.RMUser) ?? new User_PermissionCustom();
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

				await _assignmentCenterViewModel.CreateAssignmentCenterAll();

				firstRender = false;
			}
		}

		protected async Task SetInitManual()
		{
			var dataPosition = await _masterViewModel.Positions(new allFilter() { status = StatusModel.Active });
			if (dataPosition != null && dataPosition.Status)
			{
				LookUp.Positions = dataPosition.Data;
			}
			else
			{
				_errorMessage = dataPosition?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

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

			var dataGetDivBranchs = await _masterViewModel.GetDepBranchs(new allFilter() { status = StatusModel.Active });
			if (dataGetDivBranchs != null && dataGetDivBranchs.Status)
			{
				LookUp.DepartmentBranch = dataGetDivBranchs.Data?.Items;
			}
			else
			{
				_errorMessage = dataGetDivBranchs?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

			var dataGetDivCenter = await _masterViewModel.GetDepCenter(new allFilter() { status = StatusModel.Active });
			if (dataGetDivCenter != null && dataGetDivCenter.Status)
			{
				LookUp.DepartmentCenter = dataGetDivCenter.Data?.Items;
			}
			else
			{
				_errorMessage = dataGetDivCenter?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

			StateHasChanged();
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
			filter.type = UserTypes.User;
			filter.createby = UserInfo.Id;
			var data = await _userViewModel.GetList(filter);
			if (data != null && data.Status)
			{
				Items = data.Data?.Items;
				Pager = data.Data?.Pager;
				if (Pager != null)
				{
					Pager.UrlAction = "/user";
				}
			}
			else
			{
				_errorMessage = data?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}
			StateHasChanged();

		}

		protected string? GetPositionName(int? id)
		{
			if (LookUp.Positions != null && id.HasValue)
			{
				return LookUp.Positions.FirstOrDefault(x => x.Id == id)?.Name;
			}

			return null;
		}

		protected string? GetDepartmentBranchName(Guid? id)
		{
			if (LookUp.DepartmentBranch != null && id.HasValue)
			{
				return LookUp.DepartmentBranch.FirstOrDefault(x => x.Id == id)?.Name;
			}

			return null;
		}

		protected string? GetDepartmentCenterName(Guid? id)
		{
			if (LookUp.DepartmentCenter != null && id.HasValue)
			{
				return LookUp.DepartmentCenter.FirstOrDefault(x => x.Id == id)?.Name;
			}

			return null;
		}

		protected string? GetLevelName(int? id)
		{
			if (LookUp.UserLevels != null && id.HasValue)
			{
				return LookUp.UserLevels.FirstOrDefault(x => x.Id == id)?.Name;
			}

			return null;
		}

		protected string? GetBranchName(Guid? id)
		{
			if (LookUp.Branchs != null && id.HasValue)
			{
				return LookUp.Branchs.FirstOrDefault(x => x.Id == id)?.Name;
			}

			return null;
		}

		protected async Task Search()
		{
			await SetModel();
			StateHasChanged();
			_Navs.NavigateTo($"{Pager?.UrlAction}?{filter.SetParameter(true)}");
		}

		protected async Task OnPositions(ChangeEventArgs e)
		{
			filter.PositionsList = new();
			if (e.Value != null)
			{
				filter.PositionsList.Add(e.Value.ToString());
			}

			await SetModel();
			StateHasChanged();
			_Navs.NavigateTo($"{Pager?.UrlAction}?{filter.SetParameter(true)}");
		}

		protected async Task OnUserLevels(ChangeEventArgs e)
		{
			filter.SUserLevelsList = new();
			if (e.Value != null)
			{
				filter.SUserLevelsList.Add(e.Value.ToString());
			}

			await SetModel();
			StateHasChanged();
			_Navs.NavigateTo($"{Pager?.UrlAction}?{filter.SetParameter(true)}");
		}

		protected async Task OnStatus(ChangeEventArgs e)
		{
			filter.status = null;
			if (e.Value != null)
			{
				if (short.TryParse(e.Value.ToString(), out short _status))
				{
					filter.status = _status;
				}

				await SetModel();
				StateHasChanged();
				_Navs.NavigateTo($"{Pager?.UrlAction}?{filter.SetParameter(true)}");
			}
		}

		protected async Task ConfirmDelete(string? id, string? txt)
		{
			await modalConfirm.OnShowConfirm(id, $"คุณต้องการลบข้อมูล <span class='text-primary'>{txt}</span>");
		}

		protected async Task Delete(string id)
		{
			await modalConfirm.OnHideConfirm();

			var data = await _userViewModel.DeleteById(new UpdateModel() { id = id, userid = UserInfo.Id });
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
				var data = await _userViewModel.UpdateStatusById(new UpdateModel() { id = id.ToString(), userid = UserInfo.Id, value = val.ToString() });
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