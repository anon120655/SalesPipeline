
using global::Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Forms;
using SalesPipeline.Utils.ConstTypeModel;
using SalesPipeline.Utils.Resources.Thailands;
using System.Collections.Generic;

namespace SalesPipeline.Pages.Users.Admin
{
    public partial class AdminUserForm
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
        private string? department_BranchName = null;

        protected override async Task OnInitializedAsync()
        {
            isLoadingContent = true;
            _permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.LoanUser) ?? new User_PermissionCustom();
            StateHasChanged();

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
            var dataPosition = await _masterViewModel.Positions(new allFilter() { status = StatusModel.Active, type = UserTypes.Admin });
            if (dataPosition != null && dataPosition.Status)
            {
                LookUp.Positions = dataPosition.Data;
            }
            else
            {
                _errorMessage = dataPosition?.errorMessage;
                _utilsViewModel.AlertWarning(_errorMessage);
            }

            var dataDepartments = await _masterViewModel.GetDepartments(new allFilter() { status = StatusModel.Active });
            if (dataDepartments != null && dataDepartments.Status)
            {
                LookUp.Departments = dataDepartments.Data?.Items;
            }
            else
            {
                _errorMessage = dataDepartments?.errorMessage;
                _utilsViewModel.AlertWarning(_errorMessage);
            }

            var data = await _userViewModel.GetListRole(new allFilter() { pagesize = 50, status = StatusModel.Active });
            if (data != null && data.Status)
            {
                //*** ไม่มีส่วนนี้ ยุบเมนูจัดการ user มารวมกับ จัดการระบบผู้ใช้งาน
                //ItemsUserRole = data.Data?.Items.Where(x => x.Code.Contains(RoleCodes.LOAN)).ToList();
                ItemsUserRole = data.Data?.Items;
            }
            else
            {
                _errorMessage = data?.errorMessage;
                _utilsViewModel.AlertWarning(_errorMessage);
            }
            var dataLevels = await _userViewModel.GetListLevel(new allFilter() { status = StatusModel.Active });
            if (dataLevels != null && dataLevels.Status)
            {
                if (dataLevels.Data != null && dataLevels.Data.Count > 0)
                {
                    LookUp.UserLevels = dataLevels.Data;
                }
            }
            else
            {
                _errorMessage = dataLevels?.errorMessage;
                _utilsViewModel.AlertWarning(_errorMessage);
            }

            var dataGetDivBranchs = await _masterViewModel.GetDepBranchs(new allFilter() { status = StatusModel.Active });
            if (dataGetDivBranchs != null && dataGetDivBranchs.Status)
            {
                LookUp.DepartmentBranch = new() { new() { Id = Guid.Parse("99999999-9999-9999-9999-999999999999"), Name = "ทั้งหมด" } };
                if (dataGetDivBranchs.Data?.Items.Count > 0)
                {
                    LookUp.DepartmentBranch.AddRange(dataGetDivBranchs.Data.Items);
                }
            }
            else
            {
                _errorMessage = dataGetDivBranchs?.errorMessage;
                _utilsViewModel.AlertWarning(_errorMessage);
            }

            StateHasChanged();
            await Task.Delay(1);
            await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "OnRoles", "#Roles");
            await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "OnDepBranch", "#DepBranch");

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
                    //if (formModel.RoleId.HasValue)
                    //{
                    //	await OnRoles(formModel.RoleId,formModel.LevelId);
                    //}
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
                formModel.Status = StatusModel.Active;
            }
        }

        protected async Task SetAddress()
        {
            Guid? department_BranchId = null;
            department_BranchName = null;

            if (formModel.Master_Branch_RegionId.HasValue)
            {
                department_BranchId = formModel.Master_Branch_RegionId.Value;
            }

            if (department_BranchId.HasValue)
            {
                LookUp.Provinces = new();
                StateHasChanged();
                await Task.Delay(10);
                await _jsRuntimes.InvokeVoidAsync("BootSelectDestroy", "Provinces");

                var dataProvince = await _masterViewModel.GetProvince(department_BranchId);
                if (dataProvince != null && dataProvince.Status)
                {
                    if (dataProvince.Data != null && dataProvince.Data.Count > 0)
                    {
                        LookUp.Provinces = new() { new() { ProvinceID = 9999, ProvinceName = "ทั้งหมด" } };
                        LookUp.Provinces.AddRange(dataProvince.Data);
                        StateHasChanged();
                        await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "OnProvinces", "#Provinces");

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
            _Navs.NavigateTo("/admin");
        }

        [JSInvokable]
        public async Task OnRoles(string _id, string _name)
        {
            formModel.RoleId = null;
            LookUp.UserLevels = new();
            StateHasChanged();

            if (_id != null && int.TryParse(_id.ToString(), out int roleid))
            {
                formModel.RoleId = roleid;
                StateHasChanged();

                //await SetLevels(formModel.RoleId.Value);
                //var dataLevels = await _userViewModel.GetListLevel(new allFilter() { status = StatusModel.Active });
                //if (dataLevels != null && dataLevels.Status)
                //{
                //	if (dataLevels.Data != null && dataLevels.Data.Count > 0)
                //	{
                //		if (formModel.RoleId == 5) //สายงานกิจการสาขาภาค  10-12
                //		{
                //			LookUp.UserLevels = dataLevels.Data.Where(x => x.Id >= 10 && x.Id <= 12).ToList();
                //		}
                //		else if (formModel.RoleId == 6) //สายงานกิจการสาขาภาค  4-9
                //		{
                //			LookUp.UserLevels = dataLevels.Data.Where(x => x.Id >= 4 && x.Id <= 9).ToList();
                //		}
                //		else if (formModel.RoleId == 8) //RM
                //		{
                //			LookUp.UserLevels = dataLevels.Data;
                //		}

                //		StateHasChanged();
                //	}
                //}
                //else
                //{
                //	_errorMessage = dataLevels?.errorMessage;
                //	_utilsViewModel.AlertWarning(_errorMessage);
                //}
            }

            await Task.Delay(10);
        }

        [JSInvokable]
        public async Task OnDepBranch(string _id, string _name)
        {
            formModel.ProvinceId = null;
            formModel.BranchId = null;
            LookUp.Provinces = new();
            StateHasChanged();

            await _jsRuntimes.InvokeVoidAsync("BootSelectEmptyID", "Provinces");

            if (_id != null && Guid.TryParse(_id, out Guid department_BranchId))
            {
                formModel.Master_Branch_RegionId = department_BranchId;

                var dataProvince = await _masterViewModel.GetProvince(department_BranchId);
                if (dataProvince != null && dataProvince.Status)
                {
                    if (dataProvince.Data != null && dataProvince.Data.Count > 0)
                    {
                        LookUp.Provinces = new() { new() { ProvinceID = 0, ProvinceName = "เลือก" } };
                        LookUp.Provinces = new() { new() { ProvinceID = 9999, ProvinceName = "ทั้งหมด" } };
                        LookUp.Provinces.AddRange(dataProvince.Data);
                        StateHasChanged();
                        await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "OnProvinces", "#Provinces");
                        await _jsRuntimes.InvokeVoidAsync("BootSelectRefreshID", "Provinces", 100);
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
        public async Task OnProvince(string _provinceID, string _provinceName)
        {
            formModel.BranchId = null;
            LookUp.Branchs = new List<InfoBranchCustom>();
            StateHasChanged();

            await _jsRuntimes.InvokeVoidAsync("BootSelectEmptyID", "Branch");

            if (_provinceID != null && int.TryParse(_provinceID, out int provinceID))
            {
                formModel.ProvinceId = provinceID;

                var branch = await _masterViewModel.GetBranch(provinceID);
                if (branch != null && branch.Data?.Count > 0)
                {
                    LookUp.Branchs = new List<InfoBranchCustom>() { new InfoBranchCustom() { BranchID = 0, BranchName = "--เลือก--" } };
                    LookUp.Branchs.AddRange(branch.Data);

                    StateHasChanged();
                    await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "OnBranch", "#Branch");
                    await _jsRuntimes.InvokeVoidAsync("BootSelectRefreshID", "Branch", 100);
                }
            }
        }

        [JSInvokable]
        public async Task OnBranch(string _branchID, string _branchName)
        {
            await Task.Delay(100);
            if (_branchID != null && int.TryParse(_branchID, out int branchID))
            {
                formModel.BranchId = branchID;
            }
            else
            {
                formModel.BranchId = null;
            }
        }

        [JSInvokable]
        public async Task OnProvinces(string[] _ids, string _name)
        {
            formModel.User_Areas = new();
            await Task.Delay(1);

            if (_ids != null)
            {
                var selection = (_ids as string[])?.Select(x => x).ToList() ?? new();
                if (selection != null)
                {
                    foreach (var item in selection)
                    {
                        if (int.TryParse(item, out int provinceId))
                        {
                            formModel.User_Areas.Add(new()
                            {
                                ProvinceId = provinceId,
                            });
                        }
                    }
                }
            }
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

        private void OnInputTel(KeyboardEventArgs e)
        {
            if (formModel.Tel != null)
            {
                formModel.Tel = formModel.Tel.Substring(0, 10);
            }
        }

    }
}