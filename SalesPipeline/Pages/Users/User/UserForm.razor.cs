
using global::Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.ViewModels;
using SalesPipeline.Utils.Resources.Thailands;

namespace SalesPipeline.Pages.Users.User
{
	public partial class UserForm
	{
		[Parameter]
		public int? id { get; set; }

		private string? _errorMessage = null;
		private bool isLoading = false;
		private bool isLoadingContent = false;
		private User_PermissionCustom _permission = new();
		private LookUpResource LookUp = new();
		private List<User_RoleCustom>? ItemsUserRole;
		private UserCustom formModel = new();

		protected override async Task OnInitializedAsync()
		{
			isLoadingContent = true;
			_permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.RMUser) ?? new User_PermissionCustom();
			StateHasChanged();

			//await SetInitManual();
			await SetModel();
		}

		protected async override Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				await BootSelectInit();
				firstRender = false;
			}
		}

		protected async Task SetInitManual()
		{
			var dataPosition = await _masterViewModel.Positions(new allFilter() { status = StatusModel.Active, type = UserTypes.User });
			if (dataPosition != null && dataPosition.Status)
			{
				LookUp.Positions = dataPosition.Data;
			}
			else
			{
				_errorMessage = dataPosition?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

			var dataGetDivLoans = await _masterViewModel.GetDepCenter(new allFilter() { status = StatusModel.Active });
			if (dataGetDivLoans != null && dataGetDivLoans.Status)
			{
				LookUp.DepartmentCenter = dataGetDivLoans.Data?.Items;
			}
			else
			{
				_errorMessage = dataGetDivLoans?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

			var dataBranchs = await _masterViewModel.Branchs(new allFilter() { status = StatusModel.Active });
			if (dataBranchs != null && dataBranchs.Status)
			{
				LookUp.Branchs = dataBranchs.Data;
			}
			else
			{
				_errorMessage = dataBranchs?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

			var data = await _userViewModel.GetListRole(new allFilter() { pagesize = 50, status = StatusModel.Active });
			if (data != null && data.Status)
			{
				ItemsUserRole = data.Data?.Items.Where(x => !x.Code.Contains(RoleCodes.LOAN)).ToList();
			}
			else
			{
				_errorMessage = data?.errorMessage;
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

			StateHasChanged();
			await Task.Delay(1);
			await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "OnDepartmentBranch", "#DepartmentBranch");

			await Task.Delay(1);
			await SetAddress();
			isLoadingContent = false;
			StateHasChanged();
		}

		protected async Task SetModel()
		{
			if (id.HasValue)
			{
				var data = await _userViewModel.GetById(id.Value);
				if (data != null && data.Status && data.Data != null)
				{
					formModel = data.Data;
					if (formModel.RoleId.HasValue)
					{
						await OnRoles(formModel.RoleId, formModel.LevelId);
					}
					StateHasChanged();
				}
				else
				{
					_errorMessage = data?.errorMessage;
					_utilsViewModel.AlertWarning(_errorMessage);
				}
			}
			else
			{
				isLoadingContent = false;
			}
		}

		protected async Task SetAddress()
		{
			if (formModel.Master_Department_BranchId.HasValue)
			{
				LookUp.Provinces = new();
				StateHasChanged();
				await Task.Delay(10);
				await _jsRuntimes.InvokeVoidAsync("BootSelectDestroy", "Province");

				var dataProvince = await _masterViewModel.GetProvince(formModel.Master_Department_BranchId);
				if (dataProvince != null && dataProvince.Status)
				{
					if (dataProvince.Data != null && dataProvince.Data.Count > 0)
					{
						LookUp.Provinces = new () { new() { ProvinceID = 0, ProvinceName = "--เลือก--" } };
						LookUp.Provinces.AddRange(dataProvince.Data);
						StateHasChanged();
						await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "ProvinceChange", "#Province");

						if (formModel.ProvinceId.HasValue)
						{
							LookUp.Amphurs = new();
							StateHasChanged();
							await Task.Delay(10);
							await _jsRuntimes.InvokeVoidAsync("BootSelectDestroy", "Amphur");

							var amphur = await _masterViewModel.GetAmphur(formModel.ProvinceId.Value);
							if (amphur != null && amphur.Data != null)
							{
								LookUp.Amphurs = new() { new() { AmphurID = 0, AmphurName = "--เลือก--" } };
								LookUp.Amphurs?.AddRange(amphur.Data);
								StateHasChanged();
								await Task.Delay(10);
								await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "AmphurChange", $"#Amphur");

							}
						}
					}

				}
			}

		}

		protected async Task BootSelectInit()
		{
			await Task.Delay(10);
			await SetInitManual();
			await _jsRuntimes.InvokeVoidAsync("BootSelectClass", "selectInit");
		}
		protected async Task OnInvalidSubmit()
		{
			await Task.Delay(100);
			await _jsRuntimes.InvokeVoidAsync("scrollToElement", "validation-message");
		}

		protected async Task Save()
		{
			_errorMessage = null;
			ShowLoading();

			ResultModel<UserCustom> response;

			formModel.CurrentUserId = UserInfo.Id;

			if (id.HasValue)
			{
				response = await _userViewModel.Update(formModel);
			}
			else
			{
				response = await _userViewModel.Create(formModel);
			}

			if (response.Status)
			{
				await _jsRuntimes.InvokeVoidAsync("SuccessAlert");
				Cancel();
			}
			else
			{
				HideLoading();
				_errorMessage = response.errorMessage;
				await _jsRuntimes.InvokeVoidAsync("WarningAlert", _errorMessage);
			}
		}

		public void Cancel()
		{
			_Navs.NavigateTo("/user");
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

		protected async Task OnRoles(object? val, int? levelId = null)
		{
			formModel.LevelId = levelId;
			formModel.RoleId = null;
			LookUp.UserLevels = new();
			StateHasChanged();

			if (val != null && int.TryParse(val.ToString(), out int roleid))
			{
				formModel.RoleId = roleid;
				StateHasChanged();

				var dataLevels = await _userViewModel.GetListLevel(new allFilter() { status = StatusModel.Active });
				if (dataLevels != null && dataLevels.Status)
				{
					if (dataLevels.Data != null && dataLevels.Data.Count > 0)
					{
						if (formModel.RoleId == 5) //สายงานกิจการสาขาภาค  10-12
						{
							LookUp.UserLevels = dataLevels.Data.Where(x => x.Id >= 10 && x.Id <= 12).ToList();
						}
						else if (formModel.RoleId == 6) //สายงานกิจการสาขาภาค  4-9
						{
							LookUp.UserLevels = dataLevels.Data.Where(x => x.Id >= 4 && x.Id <= 9).ToList();
						}

						StateHasChanged();
					}
				}
				else
				{
					_errorMessage = dataLevels?.errorMessage;
					_utilsViewModel.AlertWarning(_errorMessage);
				}

			}
		}

		[JSInvokable]
		public async Task OnDepartmentBranch(string _id, string _name)
		{
			formModel.ProvinceId = null;
			formModel.AmphurId = null;
			LookUp.Provinces = new();
			LookUp.Amphurs = new();
			StateHasChanged();

			await _jsRuntimes.InvokeVoidAsync("BootSelectEmptyID", "Province");
			await _jsRuntimes.InvokeVoidAsync("BootSelectEmptyID", "Amphur");

			if (_id != null && Guid.TryParse(_id, out Guid department_BranchId))
			{
				formModel.Master_Department_BranchId = department_BranchId;

				var dataProvince = await _masterViewModel.GetProvince(department_BranchId);
				if (dataProvince != null && dataProvince.Status)
				{
					if (dataProvince.Data != null && dataProvince.Data.Count > 0)
					{
						LookUp.Provinces = new List<InfoProvinceCustom>() { new InfoProvinceCustom() { ProvinceID = 0, ProvinceName = "--เลือก--" } };
						LookUp.Provinces.AddRange(dataProvince.Data);
						StateHasChanged();
						await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "ProvinceChange", "#Province");
						await _jsRuntimes.InvokeVoidAsync("BootSelectRefreshID", "Province", 100);
						await _jsRuntimes.InvokeVoidAsync("BootSelectRefreshID", "Amphur", 100);
					}
				}
				else
				{
					_errorMessage = dataProvince?.errorMessage;
					_utilsViewModel.AlertWarning(_errorMessage);
				}
			}
		}

		[JSInvokable]
		public async Task ProvinceChange(string _provinceID, string _provinceName)
		{
			LookUp.Amphurs = new List<InfoAmphurCustom>();
			StateHasChanged();

			await _jsRuntimes.InvokeVoidAsync("BootSelectEmptyID", "Amphur");

			if (_provinceID != null && int.TryParse(_provinceID, out int provinceID))
			{
				formModel.ProvinceId = provinceID;

				var amphurs = await _masterViewModel.GetAmphur(provinceID);
				if (amphurs != null && amphurs.Data?.Count > 0)
				{
					LookUp.Amphurs = new List<InfoAmphurCustom>() { new InfoAmphurCustom() { AmphurID = 0, AmphurName = "--เลือก--" } };
					LookUp.Amphurs.AddRange(amphurs.Data);

					StateHasChanged();
					await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "AmphurChange", "#Amphur");
					await _jsRuntimes.InvokeVoidAsync("BootSelectRefreshID", "Amphur", 100);
				}
			}
		}





	}
}