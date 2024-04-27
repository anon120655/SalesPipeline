using Microsoft.JSInterop;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Pages.Dashboards
{
	public partial class NotAchievedTarget
	{
		string? _errorMessage = null;
		private User_PermissionCustom _permission = new();
		private LookUpResource LookUp = new();
		private allFilter filter = new();

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

				await SetInitManual();
				await _jsRuntimes.InvokeVoidAsync("BootSelectClass", "selectInit");

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
			LookUp.RMUser = new();
			filter.Branchs = new();
			filter.RMUsers = new();
			StateHasChanged();

			if (_ids != null)
			{
				filter.Branchs.Add(_ids);
			}
		}


	}
}