using Microsoft.JSInterop;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Shares;
using Microsoft.AspNetCore.Components;

namespace SalesPipeline.Pages.Dashboards
{
	public partial class AvgPerDeal
	{
		string? _errorMessage = null;
		private User_PermissionCustom _permission = new();
		private LookUpResource LookUp = new();
		private LookUpResource LookUpBottom = new();
		private LookUpResource LookUpBottomEnd = new();
		private allFilter filter = new();
		private allFilter filterBottom = new();
		private allFilter filterBottomEnd = new();

		protected override async Task OnInitializedAsync()
		{
			_permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.Dashboard) ?? new User_PermissionCustom();
			StateHasChanged();

		}

		protected async override Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				filter.userid = UserInfo.Id;
				filterBottom.userid = UserInfo.Id;
				filterBottomEnd.userid = UserInfo.Id;

				var UrlJs = $"/js/dashboards/avgeperdeal.js?v={_appSet.Value.Version}";
				var iSloadJs = await _jsRuntimes.InvokeAsync<bool>("loadJs", UrlJs, "/avgeperdeal.js");
				if (iSloadJs)
				{
					await SetInitManual();
					await _jsRuntimes.InvokeVoidAsync("BootSelectClass", "selectInit");

					await SetModelTop();
					await SetModelBottom();
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
				LookUpBottom.DepartmentBranch = new() { new() { Id = Guid.Empty, Name = "เลือก" } };
				if (dataDepBranchs.Data?.Items.Count > 0)
				{
					LookUp.DepartmentBranch.AddRange(dataDepBranchs.Data.Items);
					LookUpBottom.DepartmentBranch.AddRange(dataDepBranchs.Data.Items);
					StateHasChanged();
					await Task.Delay(1);
					await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "OnDepBranchs", "#DepBranchs");
					await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "OnDepBranchsBottom", "#DepBranchsBottom");
				}
			}
			else
			{
				_errorMessage = dataDepBranchs?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

			var dataBranchs = await _masterViewModel.GetBranch(0);
			if (dataBranchs != null && dataBranchs.Status)
			{
				LookUpBottomEnd.Branchs = new() { new() { BranchID = 0, BranchName = "เลือก" } };
				if (dataBranchs.Data?.Count > 0)
				{
					LookUpBottomEnd.Branchs.AddRange(dataBranchs.Data);
					StateHasChanged();
					await Task.Delay(1);
					await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "OnBranchBottomEnd", "#BranchBottomEnd");
				}
			}
			else
			{
				_errorMessage = dataDepBranchs?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

		}

		//TOP
		[JSInvokable]
		public async Task OnDepBranchs(string[] _ids, string _name)
		{
			LookUp.Branchs = new();
			LookUp.RMUser = new();
			filter.DepBranch = new();
			filter.Branchs = new();
			filter.RMUser = new();
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
						filter.DepBranch.Add(item);
					}
				}
			}

			if (filter.DepBranch.Count > 0)
			{
				await _jsRuntimes.InvokeVoidAsync("AddCursorWait");

				var dataBranchs = await _masterViewModel.GetBranchByDepBranchId(new allFilter()
				{
					status = StatusModel.Active,
					DepBranch = filter.DepBranch
				});
				if (dataBranchs != null && dataBranchs.Status)
				{
					if (dataBranchs.Data?.Count > 0)
					{
						LookUp.Branchs = new();
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
			filter.Branchs = new();
			filter.RMUser = new();
			StateHasChanged();

			await _jsRuntimes.InvokeVoidAsync("BootSelectEmptyID", "RMUser");

			if (_ids != null)
			{
				var selection = (_ids as string[])?.Select(x => x).ToList() ?? new();
				if (selection != null)
				{
					foreach (var item in selection)
					{
						filter.Branchs.Add(item);
					}
				}
			}

			if (filter.Branchs.Count > 0)
			{
				await _jsRuntimes.InvokeVoidAsync("AddCursorWait");

				var dataUsersRM = await _assignmentRMViewModel.GetListRM(new allFilter()
				{
					pagesize = 100,
					status = StatusModel.Active,
					Branchs = filter.Branchs
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
			filter.RMUser = new();

			if (_ids != null)
			{
				var selection = (_ids as string[])?.Select(x => x).ToList() ?? new();
				if (selection != null)
				{
					foreach (var item in selection)
					{
						filter.RMUser.Add(item);
					}
				}
			}

			await Task.Delay(1);
		}

		//Bottom
		[JSInvokable]
		public async Task OnDepBranchsBottom(string _ids, string _name)
		{
			LookUpBottom.Branchs = new();
			filterBottom.DepBranch = new();
			filterBottom.Branchs = new();
			StateHasChanged();

			await _jsRuntimes.InvokeVoidAsync("BootSelectEmptyID", "BranchBottom");

			if (_ids != null)
			{
				filterBottom.DepBranch.Add(_ids);
			}

			if (filterBottom.DepBranch.Count > 0)
			{
				await _jsRuntimes.InvokeVoidAsync("AddCursorWait");

				var dataBranchs = await _masterViewModel.GetBranchByDepBranchId(new allFilter()
				{
					status = StatusModel.Active,
					DepBranch = filterBottom.DepBranch
				});
				if (dataBranchs != null && dataBranchs.Status)
				{
					if (dataBranchs.Data?.Count > 0)
					{
						LookUpBottom.Branchs = new();
						LookUpBottom.Branchs.AddRange(dataBranchs.Data);
						StateHasChanged();
						await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "OnBranchBottom", "#BranchBottom");
						await _jsRuntimes.InvokeVoidAsync("BootSelectRefreshID", "BranchBottom", 100);
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
		public async Task OnBranchBottom(string[] _ids, string _name)
		{
			filterBottom.Branchs = new();
			StateHasChanged();

			if (_ids != null)
			{
				var selection = (_ids as string[])?.Select(x => x).ToList() ?? new();
				if (selection != null)
				{
					foreach (var item in selection)
					{
						filterBottom.Branchs.Add(item);
					}
				}
			}

			if (filterBottom.Branchs.Count > 0)
			{
				await _jsRuntimes.InvokeVoidAsync("AddCursorWait");

				await _jsRuntimes.InvokeVoidAsync("RemoveCursorWait");
			}
			else
			{
				await _jsRuntimes.InvokeVoidAsync("RemoveCursorWait");
			}
		}

		//BottomEnd
		[JSInvokable]
		public async Task OnBranchBottomEnd(string _ids, string _name)
		{
			LookUpBottomEnd.RMUser = new();
			filterBottomEnd.Branchs = new();
			filterBottomEnd.RMUser = new();
			filterBottomEnd.Branchs = new();
			StateHasChanged();

			await _jsRuntimes.InvokeVoidAsync("BootSelectEmptyID", "RMUserBottomEnd");

			if (_ids != null)
			{
				filterBottomEnd.Branchs.Add(_ids);
			}

			if (filterBottomEnd.Branchs.Count > 0)
			{
				await _jsRuntimes.InvokeVoidAsync("AddCursorWait");

				var dataUsersRM = await _assignmentRMViewModel.GetListRM(new allFilter()
				{
					pagesize = 100,
					status = StatusModel.Active,
					Branchs = filterBottomEnd.Branchs
				});
				if (dataUsersRM != null && dataUsersRM.Status)
				{
					if (dataUsersRM.Data?.Items.Count > 0)
					{
						if (dataUsersRM.Data.Items?.Count > 0)
						{
							LookUpBottomEnd.RMUser = new();
							LookUpBottomEnd.RMUser.AddRange(dataUsersRM.Data.Items);
							StateHasChanged();
							await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "OnRMUserBottomEnd", "#RMUserBottomEnd");
							await _jsRuntimes.InvokeVoidAsync("BootSelectRefreshID", "RMUserBottomEnd", 100);
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
		public async Task OnRMUserBottomEnd(string[] _ids, string _name)
		{
			filterBottomEnd.RMUser = new();

			if (_ids != null)
			{
				var selection = (_ids as string[])?.Select(x => x).ToList() ?? new();
				if (selection != null)
				{
					foreach (var item in selection)
					{
						filterBottomEnd.RMUser.Add(item);
					}
				}
			}

			if (filterBottomEnd.RMUser.Count > 0)
			{

			}
			else
			{
				await _jsRuntimes.InvokeVoidAsync("RemoveCursorWait");
			}
		}


		protected async Task SetModelTop()
		{
			await AvgDeal_Bar1();
			await AvgDeal_Bar2();
		}

		protected async Task SetModelBottom()
		{
			await AvgDeal_Bar3();
			await AvgDeal_Bar4();
		}

		protected async Task AvgDeal_Bar1()
		{
			var data = await _dashboarViewModel.GetAvgTopBar(filter);
			if (data != null && data.Status)
			{
				var datas = new List<ChartJSDataModel>();

				if (data.Data?.Count > 0)
				{
					foreach (var item in data.Data)
					{
						datas.Add(new()
						{
							id = item.GroupID,
							x = item.Name ?? string.Empty,
							y = item.Value
						});
					}
				}
				await _jsRuntimes.InvokeVoidAsync("avgdeal_bar1", datas);
			}
			else
			{
				_errorMessage = data?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}
		}

		protected async Task AvgDeal_Bar2()
		{
			var data = await _dashboarViewModel.GetAvgRegionBar(filter);
			if (data != null && data.Status)
			{
				var datas = new List<ChartJSDataModel>();

				if (data.Data?.Count > 0)
				{
					foreach (var item in data.Data)
					{
						datas.Add(new()
						{
							id = item.GroupID,
							x = item.Name ?? string.Empty,
							y = item.Value
						});
					}
				}
				await _jsRuntimes.InvokeVoidAsync("avgdeal_bar2", datas);
			}
			else
			{
				_errorMessage = data?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}
		}

		protected async Task AvgDeal_Bar3()
		{
			List<string?> department_BranchList = new() { "788b8633-cd3b-11ee-ac10-30e37aef72fb", "7550476c-c1b1-11ee-bf19-0205965f5884" };
			List<string?> branchList = new() { "3", "143", "62" };
			var data = await _dashboarViewModel.GetAvgBranchBar(filterBottom);
			if (data != null && data.Status)
			{
				var datas = new List<ChartJSDataModel>();

				if (data.Data?.Count > 0)
				{
					foreach (var item in data.Data)
					{
						datas.Add(new()
						{
							id = item.GroupID,
							x = item.Name ?? string.Empty,
							y = item.Value
						});
					}
				}
				await _jsRuntimes.InvokeVoidAsync("avgdeal_bar3", datas);
			}
			else
			{
				_errorMessage = data?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}
		}

		protected async Task AvgDeal_Bar4()
		{
			List<string?> branchList = new() { "3", "143", "62" };
			List<string?> rmList = new() { "13", "14" };
			var data = await _dashboarViewModel.GetAvgRMBar(filterBottomEnd);
			if (data != null && data.Status)
			{
				var datas = new List<ChartJSDataModel>();

				if (data.Data?.Count > 0)
				{
					foreach (var item in data.Data)
					{
						datas.Add(new()
						{
							id = item.GroupID,
							x = item.Name ?? string.Empty,
							y = item.Value
						});
					}
				}
				await _jsRuntimes.InvokeVoidAsync("avgdeal_bar4", datas);
			}
			else
			{
				_errorMessage = data?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}
		}

		protected async Task Dropdown_Level()
		{
			await _jsRuntimes.InvokeVoidAsync("dropdown_level", null);
		}

		protected async Task Search()
		{
			await SetModelTop();
			StateHasChanged();
		}

		protected async Task SearchBottom()
		{
			await SetModelBottom();
			StateHasChanged();
		}

		protected void OnDateStart(ChangeEventArgs e)
		{
			if (e != null && e.Value != null && filter != null)
			{
				if (!String.IsNullOrEmpty(e.Value.ToString()))
				{
					filter.startdate = GeneralUtils.DateNotNullToEn(e.Value.ToString(), "yyyy-MM-dd", Culture: "en-US");
				}
				else
				{
					filter.startdate = null;
				}
			}
		}

		protected void OnDateEnd(ChangeEventArgs e)
		{
			if (e != null && e.Value != null && filter != null)
			{
				if (!String.IsNullOrEmpty(e.Value.ToString()))
				{
					filter.enddate = GeneralUtils.DateNotNullToEn(e.Value.ToString(), "yyyy-MM-dd", Culture: "en-US");
				}
				else
				{
					filter.enddate = null;
				}
			}
		}

	}
}