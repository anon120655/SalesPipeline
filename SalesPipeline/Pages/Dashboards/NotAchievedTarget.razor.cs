using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Pages.Dashboards
{
	public partial class NotAchievedTarget
	{
		string? _errorMessage = null;
		private User_PermissionCustom _permission = new();
		private allFilter filter = new();
		private LookUpResource LookUp = new();
		private List<User_Target_SaleCustom>? Items;
		public Pager? Pager;

		protected override async Task OnInitializedAsync()
		{
			_permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.Dashboard) ?? new User_PermissionCustom();
			StateHasChanged();
			await Task.Delay(1);
		}

		protected async override Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				filter.userid = UserInfo.Id;
				filter.achieve_goal = 2;

				await SetInitManual();
				await Task.Delay(10);
				await _jsRuntimes.InvokeVoidAsync("BootSelectClass", "selectInit");

				await SetQuery();
				StateHasChanged();

				StateHasChanged();
				firstRender = false;
			}
		}

		protected async Task SetInitManual()
		{
			var dataDepBranchs = await _masterViewModel.GetDepBranchs(new allFilter() { status = StatusModel.Active });
			if (dataDepBranchs != null && dataDepBranchs.Status)
			{
				LookUp.DepartmentBranch = new() { new() { Id = Guid.Empty, Name = "ทั้งหมด" } };
				if (dataDepBranchs.Data?.Items.Count > 0)
				{
					LookUp.DepartmentBranch.AddRange(dataDepBranchs.Data.Items);
					StateHasChanged();
					await Task.Delay(1);
					await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "OnDepBranch", "#DepBranch");
				}
			}
			else
			{
				_errorMessage = dataDepBranchs?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

			var dataYear = await _masterViewModel.GetYear(new allFilter() { status = StatusModel.Active });
			if (dataYear != null && dataYear.Status)
			{
				//LookUp.Years = new() { new() { Id = Guid.Empty, Name = "ทั้งหมด" } };
				LookUp.Years = new();
				if (dataYear.Data?.Count > 0)
				{
					LookUp.Years.AddRange(dataYear.Data);
					StateHasChanged();
					await Task.Delay(1);
					//await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "OnYear", "#Year");
				}
			}
			else
			{
				_errorMessage = dataYear?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
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
			filter.userid = UserInfo.Id;
			filter.type = "avgdurationclosesale";
			var data = await _dashboarViewModel.GetListTarget_SaleById(filter);
			if (data != null && data.Status)
			{
				Items = data.Data?.Items;
				Pager = data.Data?.Pager;
				if (Pager != null)
				{
					Pager.UrlAction = "/dashboard/notachievedtarget";
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

		protected string? GetDepBranchName(Guid? id)
		{
			if (LookUp.DepartmentBranch != null && id.HasValue)
			{
				return LookUp.DepartmentBranch.FirstOrDefault(x => x.Id == id)?.Name;
			}

			return null;
		}


		[JSInvokable]
		public async Task OnDepBranch(string _ids, string _name)
		{
			LookUp.Branchs = new();
			LookUp.RMUser = new();
			filter.DepBranchs = new();
			filter.Branchs = new();
			filter.RMUsers = new();
			StateHasChanged();

			await _jsRuntimes.InvokeVoidAsync("BootSelectEmptyID", "Branch");
			await _jsRuntimes.InvokeVoidAsync("BootSelectEmptyID", "RMUser");

			if (_ids != null)
			{
				filter.DepBranchs.Add(_ids);
			}

			if (filter.DepBranchs.Count > 0)
			{
				await _jsRuntimes.InvokeVoidAsync("AddCursorWait");

				var dataBranchs = await _masterViewModel.GetBranchByDepBranchId(new allFilter()
				{
					status = StatusModel.Active,
					DepBranchs = filter.DepBranchs
				});
				if (dataBranchs != null && dataBranchs.Status)
				{
					if (dataBranchs.Data?.Count > 0)
					{
						LookUp.Branchs = new() { new() { BranchID = 0, BranchName = "ทั้งหมด" } };
						LookUp.Branchs.AddRange(dataBranchs.Data);
						StateHasChanged();
						await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "OnBranch", "#Branch");
						await _jsRuntimes.InvokeVoidAsync("BootSelectRefreshID", "Branch", 100);
						await _jsRuntimes.InvokeVoidAsync("BootSelectRefreshID", "RMUser", 100);
					}
				}
				else
				{
					_errorMessage = dataBranchs?.errorMessage;
					_utilsViewModel.AlertWarning(_errorMessage);
				}
				await _jsRuntimes.InvokeVoidAsync("RemoveCursorWait");
			}
			else
			{
				await _jsRuntimes.InvokeVoidAsync("RemoveCursorWait");
			}

		}

		[JSInvokable]
		public async Task OnBranch(string _ids, string _name)
		{
			await Task.Delay(1);
			LookUp.RMUser = new();
			filter.Branchs = new();
			filter.RMUsers = new();
			StateHasChanged();

			if (_ids != null)
			{
				filter.Branchs.Add(_ids);
			}
		}

		public async Task OnYear(ChangeEventArgs e)
		{
			filter.year = null;
			StateHasChanged();
			await Task.Delay(1);

			if (e.Value != null)
			{
				if (int.TryParse(e.Value.ToString(), out int year))
				{
					filter.year = year.ToString();
				}
			}

			await SetModel();
			StateHasChanged();
			_Navs.NavigateTo($"{Pager?.UrlAction}?{filter.SetParameter(true)}");
		}

		protected async Task ExportExcel()
		{
			var data = await _exportViewModel.ExcelNotAchievedTarget(filter);
			if (data != null && data.Status && data.Data != null)
			{
				await _jsRuntimes.InvokeAsync<object>("saveAsFile", "รายงานพนักงานที่ไม่บรรลุเป้าหมาย.xlsx", Convert.ToBase64String(data.Data));
			}
			else
			{
				_errorMessage = data?.errorMessage;
				await _jsRuntimes.InvokeVoidAsync("WarningAlert", _errorMessage);
			}
		}


	}
}