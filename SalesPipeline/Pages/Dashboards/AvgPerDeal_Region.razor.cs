using Microsoft.JSInterop;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Dashboards;

namespace SalesPipeline.Pages.Dashboards
{
	public partial class AvgPerDeal_Region
    {
		string? _errorMessage = null;
		private User_PermissionCustom _permission = new();
		private LookUpResource LookUp = new();
		private FilterAvgPerDeal filterAvg = new();

		public string filterRegionsTitle = "เลือก";
		public string filterBranchsTitle = "เลือก";
		public string filterRMUserTitle = "เลือก";

		protected override async Task OnInitializedAsync()
		{
			_permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.Dashboard) ?? new User_PermissionCustom();
			StateHasChanged();
		}

		protected async override Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				var UrlJs = $"/js/dashboards/avgeperdeal_region.js?v={_appSet.Value.Version}";
				var iSloadJs = await _jsRuntimes.InvokeAsync<bool>("loadJs", UrlJs, "/avgeperdeal.js");
				if (iSloadJs)
				{
					await SetInitManual();
					await _jsRuntimes.InvokeVoidAsync("BootSelectClass", "selectInit");

					await SetModelAll();
				}

				StateHasChanged();
				firstRender = false;
			}
		}

		protected async Task SetInitManual()
		{
			var dataDepBranchs = await _masterViewModel.GetDepBranchs(new allFilter() { status = StatusModel.Active });
			if (dataDepBranchs != null && dataDepBranchs.Status)
			{
				LookUp.DepartmentBranch = new();
				if (dataDepBranchs.Data?.Items.Count > 0)
				{
					LookUp.DepartmentBranch.AddRange(dataDepBranchs.Data.Items);
					StateHasChanged();
					await Task.Delay(1);
					await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "OnDepBranchs", "#DepBranchs");
				}
			}
			else
			{
				_errorMessage = dataDepBranchs?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

			StateHasChanged();
		}

		[JSInvokable]
		public async Task OnDepBranchs(string[] _ids, string _name)
		{
			LookUp.Branchs = new();
			LookUp.RMUser = new();
			filterAvg.DepartmentBranch = new();
			filterAvg.Branchs = new();
			filterAvg.RMUser = new();
			StateHasChanged();

			await _jsRuntimes.InvokeVoidAsync("BootSelectEmptyID", "Branch");
			await _jsRuntimes.InvokeVoidAsync("BootSelectEmptyID", "RMUser");

			if (_ids != null)
			{
				var selection = (_ids as string[])?.Select(x => x).ToList() ?? new();
				if (selection != null)
				{
					foreach (var item in selection)
					{
						filterAvg.DepartmentBranch.Add(new()
						{
							ID = item
						});
					}
				}
			}

			if (filterAvg.DepartmentBranch.Count > 0)
			{
				await _jsRuntimes.InvokeVoidAsync("AddCursorWait");

				var dataBranchs = await _masterViewModel.GetBranchByDepBranchId(new allFilter()
				{
					status = StatusModel.Active,
					Selecteds = filterAvg.DepartmentBranch.Select(x => x.ID).ToList()
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
		public async Task OnBranch(string[] _ids, string _name)
		{
			LookUp.RMUser = new();
			filterAvg.Branchs = new();
			filterAvg.RMUser = new();
			StateHasChanged();

			await _jsRuntimes.InvokeVoidAsync("BootSelectEmptyID", "RMUser");

			if (_ids != null)
			{
				var selection = (_ids as string[])?.Select(x => x).ToList() ?? new();
				if (selection != null)
				{
					foreach (var item in selection)
					{
						filterAvg.Branchs.Add(new()
						{
							ID = item
						});
					}
				}
			}

			if (filterAvg.Branchs.Count > 0)
			{
				await _jsRuntimes.InvokeVoidAsync("AddCursorWait");

				var dataUsersRM = await _assignmentRMViewModel.GetListRM(new allFilter()
				{
					pagesize = 100,
					status = StatusModel.Active,
					Selecteds = filterAvg.Branchs.Select(x => x.ID).ToList()
				});
				if (dataUsersRM != null && dataUsersRM.Status)
				{
					if (dataUsersRM.Data?.Items.Count > 0)
					{
						if (dataUsersRM.Data.Items?.Count > 0)
						{
							LookUp.RMUser = new();
							LookUp.RMUser.AddRange(dataUsersRM.Data.Items);
							StateHasChanged();
							await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "OnRMUser", "#RMUser");
							await _jsRuntimes.InvokeVoidAsync("BootSelectRefreshID", "RMUser", 100);
						}
					}
				}
				else
				{
					_errorMessage = dataUsersRM?.errorMessage;
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
		public async Task OnRMUser(string[] _ids, string _name)
		{
			filterAvg.RMUser = new();

			if (_ids != null)
			{
				var selection = (_ids as string[])?.Select(x => x).ToList() ?? new();
				if (selection != null)
				{
					foreach (var item in selection)
					{
						filterAvg.RMUser.Add(new()
						{
							ID = item
						});
					}
				}
			}

			if (filterAvg.RMUser.Count > 0)
			{

			}
			else
			{
				await _jsRuntimes.InvokeVoidAsync("RemoveCursorWait");
			}
		}

		protected async Task SetModelAll()
		{
			await AvgDeal_Region_Bar();
		}

		protected async Task AvgDeal_Region_Bar()
		{
			await _jsRuntimes.InvokeVoidAsync("avgdeal_region_bar", null);
		}

	}
}