using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils.Resources.Dashboards;
using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Thailands;
using SalesPipeline.Utils.Resources.Assignments;

namespace SalesPipeline.Pages.Dashboards
{
	public partial class AvgPerDeal
	{
		string? _errorMessage = null;
		private User_PermissionCustom _permission = new();
		private LookUpResource LookUp = new();
		private FilterAvgPerDeal filterAvg = new();
		private bool isLoadingDepBranchs = false;

		public string filterRegionsTitle = "เลือก";
		public string filterBranchsTitle = "เลือก";
		public string filterRMUserTitle = "เลือก";

		protected override async Task OnInitializedAsync()
		{
			_permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.Dashboard) ?? new User_PermissionCustom();
			StateHasChanged();

			await SetInitManual();
		}

		protected async override Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				var UrlJs = $"/js/dashboards/avgeperdeal.js?v={_appSet.Value.Version}";
				var iSloadJs = await _jsRuntimes.InvokeAsync<bool>("loadJs", UrlJs, "/avgeperdeal.js");
				if (iSloadJs)
				{
					await SetModelAll();
				}

				await _jsRuntimes.InvokeVoidAsync("selectPickerInitialize");
				StateHasChanged();
				firstRender = false;
			}
		}

		protected async Task SetInitManual()
		{
			var dataDepBranchs = await _masterViewModel.GetDepBranchs(new allFilter() { status = StatusModel.Active });
			if (dataDepBranchs != null && dataDepBranchs.Status)
			{
				LookUp.DepartmentBranch = new() { new() { Name = "ทั้งหมด" } };
				if (dataDepBranchs.Data?.Items.Count > 0)
				{
					LookUp.DepartmentBranch.AddRange(dataDepBranchs.Data.Items);
				}
			}
			else
			{
				_errorMessage = dataDepBranchs?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

			StateHasChanged();
		}

		public async Task OnDepBranchs(object? valChecked, Master_Department_BranchCustom model)
		{
			if (valChecked != null)
			{
				isLoadingDepBranchs = true;

				bool isChecked = (bool)valChecked ? true : false;

				filterRegionsTitle = "เลือก";
				LookUp.Branchs = new();
				LookUp.RMUser = new();
				filterAvg.Branchs = new();
				filterAvg.RMUser = new();

				if (filterAvg.DepartmentBranch == null) filterAvg.DepartmentBranch = new();

				if (model.Id == Guid.Empty)
				{
					filterAvg.DepartmentBranch.Clear();
					if (isChecked)
					{
						foreach (var item in LookUp.DepartmentBranch ?? new())
						{
							filterAvg.DepartmentBranch.Add(new SelectModel() { Value = item.Id.ToString(), Name = item.Name });
						}
					}
				}
				else
				{
					var data = filterAvg.DepartmentBranch.FirstOrDefault(x => x.Value == model.Id.ToString());
					if (data == null)
					{
						filterAvg.DepartmentBranch.Add(new SelectModel() { Value = model.Id.ToString(), Name = model.Name });
					}
					else
					{
						filterAvg.DepartmentBranch.Remove(data);
					}
				}


				if (filterAvg.DepartmentBranch.Count > 0)
				{
					var removeAll = filterAvg.DepartmentBranch.FirstOrDefault(x => x.Value == "0");
					if (removeAll != null)
						filterAvg.DepartmentBranch.Remove(removeAll);

					if (filterAvg.DepartmentBranch.Count == LookUp.DepartmentBranch?.Count(x => x.Id != Guid.Empty))
					{
						filterRegionsTitle = $"ทั้งหมด ({filterAvg.DepartmentBranch.Count})";
						filterAvg.DepartmentBranch.Add(new SelectModel() { Value = "0" });
					}
					else if (filterAvg.DepartmentBranch.Count <= 1)
					{
						filterRegionsTitle = string.Join(", ", filterAvg.DepartmentBranch.Select(x => x.Name).ToList());
						filterRegionsTitle = GeneralUtils.LimitTo(filterRegionsTitle, 20) ?? string.Empty;
					}
					else
					{
						filterRegionsTitle = $"เลือก ({filterAvg.DepartmentBranch.Count})";
					}

					var dataBranchs = await _masterViewModel.GetBranchByDepBranchId(new allFilter()
					{
						status = StatusModel.Active,
						Selecteds = filterAvg.DepartmentBranch.Select(x => x.Value).ToList()
					});
					if (dataBranchs != null && dataBranchs.Status)
					{
						if (dataBranchs.Data?.Count > 0)
						{
							LookUp.Branchs = new() { new() { BranchID = 0, BranchName = "ทั้งหมด" } };
							LookUp.Branchs.AddRange(dataBranchs.Data);
						}
					}
					else
					{
						_errorMessage = dataBranchs?.errorMessage;
						_utilsViewModel.AlertWarning(_errorMessage);
					}
				}

				isLoadingDepBranchs = false;
				StateHasChanged();
			}
		}

		public async Task OnBranch(object? valChecked, InfoBranchCustom model)
		{
			if (valChecked != null)
			{
				isLoadingDepBranchs = true;

				bool isChecked = (bool)valChecked ? true : false;

				filterBranchsTitle = "เลือก";
				LookUp.RMUser = new();
				filterAvg.RMUser = new();

				if (filterAvg.Branchs == null) filterAvg.Branchs = new();

				if (model.BranchID == 0)
				{
					filterAvg.Branchs.Clear();
					if (isChecked)
					{
						foreach (var item in LookUp.Branchs ?? new())
						{
							filterAvg.Branchs.Add(new SelectModel() { Value = item.BranchID.ToString(), Name = item.BranchName });
						}
					}
				}
				else
				{
					var data = filterAvg.Branchs.FirstOrDefault(x => x.Value == model.BranchID.ToString());
					if (data == null)
					{
						filterAvg.Branchs.Add(new SelectModel() { Value = model.BranchID.ToString(), Name = model.BranchName });
					}
					else
					{
						filterAvg.Branchs.Remove(data);
					}
				}

				if (filterAvg.Branchs.Count > 0)
				{
					var removeAll = filterAvg.Branchs.FirstOrDefault(x => x.Value == "0");
					if (removeAll != null)
						filterAvg.Branchs.Remove(removeAll);

					if (filterAvg.Branchs.Count == LookUp.Branchs?.Count(x => x.BranchID != 0))
					{
						filterBranchsTitle = $"ทั้งหมด ({filterAvg.Branchs.Count})";
						filterAvg.Branchs.Add(new SelectModel() { Value = "0" });
					}
					else if (filterAvg.Branchs.Count <= 1)
					{
						filterBranchsTitle = string.Join(", ", filterAvg.Branchs.Select(x => x.Name).ToList());
						filterBranchsTitle = GeneralUtils.LimitTo(filterBranchsTitle, 20) ?? string.Empty;
					}
					else
					{
						filterBranchsTitle = $"เลือก ({filterAvg.Branchs.Count})";
					}

					var dataUsersRM = await _assignmentRMViewModel.GetListRM(new allFilter()
					{
						pagesize = 100,
						status = StatusModel.Active,
						Selecteds = filterAvg.Branchs.Select(x => x.Value).ToList()
					});
					if (dataUsersRM != null && dataUsersRM.Status)
					{
						if (dataUsersRM.Data?.Items.Count > 0)
						{
							if (dataUsersRM.Data.Items?.Count > 0)
							{
								LookUp.RMUser = new() { new() { User = new() { FullName = "ทั้งหมด" } } };
								LookUp.RMUser.AddRange(dataUsersRM.Data.Items);
							}
						}
					}
					else
					{
						_errorMessage = dataUsersRM?.errorMessage;
						_utilsViewModel.AlertWarning(_errorMessage);
					}
				}

				isLoadingDepBranchs = false;
				StateHasChanged();

			}
		}

		public async Task OnRMUser(object? valChecked, Assignment_RMCustom model)
		{
			await Task.Delay(1);
			if (valChecked != null)
			{
				bool isChecked = (bool)valChecked ? true : false;

				filterRMUserTitle = "เลือก";

				if (filterAvg.RMUser == null) filterAvg.RMUser = new();

				if (model.Id == Guid.Empty)
				{
					filterAvg.RMUser.Clear();
					if (isChecked)
					{
						foreach (var item in LookUp.RMUser ?? new())
						{
							filterAvg.RMUser.Add(new SelectModel() { Value = item.Id.ToString(), Name = item.User?.FirstName });
						}
					}
				}
				else
				{
					var data = filterAvg.RMUser.FirstOrDefault(x => x.Value == model.Id.ToString());
					if (data == null)
					{
						filterAvg.RMUser.Add(new SelectModel() { Value = model.Id.ToString(), Name = model.User?.FirstName });
					}
					else
					{
						filterAvg.RMUser.Remove(data);
					}
				}

				if (filterAvg.RMUser.Count > 0)
				{
					var removeAll = filterAvg.RMUser.FirstOrDefault(x => x.Value == "0");
					if (removeAll != null)
						filterAvg.RMUser.Remove(removeAll);

					if (filterAvg.RMUser.Count == LookUp.RMUser?.Count(x => x.Id != Guid.Empty))
					{
						filterRMUserTitle = $"ทั้งหมด ({filterAvg.RMUser.Count})";
						filterAvg.RMUser.Add(new SelectModel() { Value = "0" });
					}
					else if (filterAvg.RMUser.Count <= 2)
					{
						filterRMUserTitle = string.Join(", ", filterAvg.RMUser.Select(x => x.Name).ToList());
					}
					else
					{
						filterRMUserTitle = $"เลือก ({filterAvg.RMUser.Count})";
					}
				}

				StateHasChanged();
			}


		}

		protected async Task SetModelAll()
		{
			await AvgDeal_Bar1();
			await AvgDeal_Bar2();
			await AvgDeal_Bar3();
			await AvgDeal_Bar4();
			await Dropdown_Level();
		}

		protected async Task AvgDeal_Bar1()
		{
			var data = await _dashboarViewModel.GetAvgTopBar(new() { userid = UserInfo.Id });
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
			var data = await _dashboarViewModel.GetAvgRegionBar(new() { userid = UserInfo.Id });
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
			var data = await _dashboarViewModel.GetAvgBranchBar(new()
			{
				userid = UserInfo.Id,
				//Selecteds = department_BranchList,
				//Selecteds2 = branchList
			});
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
			var data = await _dashboarViewModel.GetAvgRMBar(new()
			{
				userid = UserInfo.Id,
				//Selecteds = branchList,
				//Selecteds2 = rmList
			});
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


	}
}