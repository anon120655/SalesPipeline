using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils.Resources.Dashboards;
using SalesPipeline.Utils.Resources.Masters;

namespace SalesPipeline.Pages.Dashboards
{
	public partial class AvgPerDeal
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
			var dataRegions = await _masterViewModel.Regions(new allFilter() { status = StatusModel.Active });
			if (dataRegions != null && dataRegions.Status)
			{
				LookUp.Regions = new() { new() { Id = 0, Name = "ทั้งหมด" } };
				if (dataRegions.Data?.Count > 0)
				{
					LookUp.Regions.AddRange(dataRegions.Data);
				}
			}
			else
			{
				_errorMessage = dataRegions?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

			StateHasChanged();
		}

		public async Task OnRegion(object? valChecked, Master_RegionCustom model)
		{
			if (valChecked != null)
			{
				bool isChecked = (bool)valChecked ? true : false;

				filterRegionsTitle = "เลือก";
				LookUp.Branchs = new();
				LookUp.RMUser = new();
				filterAvg.Branchs = new();
				filterAvg.RMUser = new();

				if (filterAvg.Regions == null) filterAvg.Regions = new();

				if (model.Id == 0)
				{
					filterAvg.Regions.Clear();
					if (isChecked)
					{
						foreach (var item in LookUp.Regions ?? new())
						{
							filterAvg.Regions.Add(new SelectModel() { Value = item.Id.ToString(), Name = item.Name });
						}
					}
				}
				else
				{
					var data = filterAvg.Regions.FirstOrDefault(x => x.Value == model.Id.ToString());
					if (data == null)
					{
						filterAvg.Regions.Add(new SelectModel() { Value = model.Id.ToString(), Name = model.Name });
					}
					else
					{
						filterAvg.Regions.Remove(data);
					}
				}


				if (filterAvg.Regions.Count > 0)
				{
					var removeAll = filterAvg.Regions.FirstOrDefault(x => x.Value == "0");
					if (removeAll != null)
						filterAvg.Regions.Remove(removeAll);

					if (filterAvg.Regions.Count == LookUp.Regions?.Count(x => x.Id != 0))
					{
						filterRegionsTitle = $"ทั้งหมด ({filterAvg.Regions.Count})";
						filterAvg.Regions.Add(new SelectModel() { Value = "0" });
					}
					else if (filterAvg.Regions.Count <= 2)
					{
						filterRegionsTitle = string.Join(", ", filterAvg.Regions.Select(x => x.Name).ToList());
					}
					else
					{
						filterRegionsTitle = $"เลือก ({filterAvg.Regions.Count})";
					}

					//var dataBranchs = await _masterViewModel.Branchs(new allFilter()
					//{
					//	status = StatusModel.Active,
					//	Selecteds = filterAvg.Regions.Select(x => x.Value).ToList()
					//});
					//if (dataBranchs != null && dataBranchs.Status)
					//{
					//	if (dataBranchs.Data?.Count > 0)
					//	{
					//		LookUp.Branchs = new() { new() { Id = Guid.NewGuid(), Name = "ทั้งหมด" } };
					//		LookUp.Branchs.AddRange(dataBranchs.Data);
					//	}
					//}
					//else
					//{
					//	_errorMessage = dataBranchs?.errorMessage;
					//	_utilsViewModel.AlertWarning(_errorMessage);
					//}
				}

				StateHasChanged();
			}
		}

		public async Task OnBranch(object? valChecked)
		{
			if (valChecked != null)
			{
				//bool isChecked = (bool)valChecked ? true : false;

				//filterBranchsTitle = "เลือก";
				//LookUp.RMUser = new();
				//filterAvg.RMUser = new();

				//if (filterAvg.Branchs == null) filterAvg.Branchs = new();

				//if (model.Id == 0)
				//{
				//	filterAvg.Branchs.Clear();
				//	if (isChecked)
				//	{
				//		foreach (var item in LookUp.Branchs ?? new())
				//		{
				//			filterAvg.Branchs.Add(new SelectModel() { Value = item.Id.ToString(), Name = item.Name });
				//		}
				//	}
				//}
				//else
				//{
				//	var data = filterAvg.Branchs.FirstOrDefault(x => x.Value == model.Id.ToString());
				//	if (data == null)
				//	{
				//		filterAvg.Branchs.Add(new SelectModel() { Value = model.Id.ToString(), Name = model.Name });
				//	}
				//	else
				//	{
				//		filterAvg.Branchs.Remove(data);
				//	}
				//}

				//if (filterAvg.Branchs.Count > 0)
				//{
				//	var removeAll = filterAvg.Branchs.FirstOrDefault(x => x.Value == "0");
				//	if (removeAll != null)
				//		filterAvg.Branchs.Remove(removeAll);

				//	if (filterAvg.Branchs.Count == LookUp.Branchs?.Count(x => x.Id != 0))
				//	{
				//		filterBranchsTitle = $"ทั้งหมด ({filterAvg.Branchs.Count})";
				//		filterAvg.Branchs.Add(new SelectModel() { Value = "0" });
				//	}
				//	else if (filterAvg.Branchs.Count <= 2)
				//	{
				//		filterBranchsTitle = string.Join(", ", filterAvg.Branchs.Select(x => x.Name).ToList());
				//	}
				//	else
				//	{
				//		filterBranchsTitle = $"เลือก ({filterAvg.Branchs.Count})";
				//	}

				//	var dataUsersRM = await _userViewModel.GetUsersRM(new allFilter()
				//	{
				//		pagesize = 100,
				//		status = StatusModel.Active,
				//		Selecteds = filterAvg.Branchs.Select(x => x.Value).ToList()
				//	});
				//	if (dataUsersRM != null && dataUsersRM.Status)
				//	{
				//		if (dataUsersRM.Data?.Items.Count > 0)
				//		{
				//			if (dataUsersRM.Data.Items?.Count > 0)
				//			{
				//				LookUp.RMUser = new() { new() { Id = 0, User = new() { FullName = "ทั้งหมด" } } };
				//				LookUp.RMUser.AddRange(dataUsersRM.Data.Items);
				//			}
				//		}
				//	}
				//	else
				//	{
				//		_errorMessage = dataUsersRM?.errorMessage;
				//		_utilsViewModel.AlertWarning(_errorMessage);
				//	}
				//}

				//StateHasChanged();

			}
		}

		public async Task OnRMUser(object? valChecked, User_BranchCustom model)
		{
			await Task.Delay(1);
			if (valChecked != null)
			{
				bool isChecked = (bool)valChecked ? true : false;

				filterRMUserTitle = "เลือก";

				if (filterAvg.RMUser == null) filterAvg.RMUser = new();

				if (model.Id == 0)
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

					if (filterAvg.RMUser.Count == LookUp.RMUser?.Count(x => x.Id != 0))
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
				var labels = new List<string?>();
				var datas = new List<decimal>();

				if (data.Data?.Count > 0)
				{
					foreach (var item in data.Data)
					{
						labels.Add(item.Name);
						datas.Add(item.Value);
					}
				}
				await _jsRuntimes.InvokeVoidAsync("avgdeal_bar1", datas.ToArray(), labels.ToArray());
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
			await _jsRuntimes.InvokeVoidAsync("avgdeal_bar3", null);
		}

		protected async Task AvgDeal_Bar4()
		{
			await _jsRuntimes.InvokeVoidAsync("avgdeal_bar4", null);
		}

		protected async Task Dropdown_Level()
		{
			await _jsRuntimes.InvokeVoidAsync("dropdown_level", null);
		}


	}
}