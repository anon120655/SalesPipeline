using Microsoft.JSInterop;
using SalesPipeline.Shared.Modals;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils;
using Microsoft.AspNetCore.Components;
using NPOI.SS.Formula.Functions;
using SalesPipeline.Utils.Resources.Thailands;
using SalesPipeline.Utils.ConstTypeModel;

namespace SalesPipeline.Pages.ApproveLoans
{
    public partial class ApproveLoan
    {
        string? _errorMessage = null;
        private User_PermissionCustom _permission = new();
        private allFilter filter = new();
        private LookUpResource LookUp = new();
        private List<SaleCustom>? Items;
        public Pager? Pager;

        protected override async Task OnInitializedAsync()
        {
            _permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.ApproveLoan) ?? new User_PermissionCustom();
            StateHasChanged();
            await Task.Delay(1);

            filter.sort = OrderByModel.ASC;
        }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await SetInitManual();
                await Task.Delay(10);
                await _jsRuntimes.InvokeVoidAsync("BootSelectClass", "selectInit");

                await SetQuery();
                StateHasChanged();
                firstRender = false;
            }
        }

        protected async Task SetInitManual()
        {
            await _jsRuntimes.InvokeVoidAsync("BootSelectId", "DisplaySort");

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
            await Task.Delay(10);
            await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "OnProvince", "#Province");

        }

        protected async Task SetQuery(string? parematerAll = null)
        {
            string uriQuery = _Navs.ToAbsoluteUri(_Navs.Uri).Query;

            if (parematerAll != null)
                uriQuery = $"?{parematerAll}";

            filter.SetUriQuery(uriQuery);

            await SetModel(!true);
            StateHasChanged();
        }

        protected async Task SetModel(bool resetPage = true)
        {
            if (resetPage) filter.page = 1;

            filter.userid = UserInfo.Id;
            filter.statussaleid = StatusSaleModel.WaitApproveLoanRequest;
            var data = await _salesViewModel.GetList(filter);
            if (data != null && data.Status)
            {
                //อนุมัติคำขอสินเชื่อของลูกค้า
                //จะเป็นขั้นตอนหลังจากที่ลูกค้ายื่นเอกสารคำขอสินเชื่อ
                //หลังจากนั้นผู้จัดการศูนย์จะตรวจสอบและอนุมัติลงนาม เพื่อ API ส่งไประบบวิเคราะห์สินเชื่อ (PHOENIX/LPS) อีกที

                Items = data.Data?.Items;
                Pager = data.Data?.Pager;
                if (Pager != null)
                {
                    Pager.UrlAction = "/approveloan";
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

        protected async Task OnSort(ChangeEventArgs e)
        {
            filter.sort = null;
            if (e.Value != null)
            {
                filter.sort = e.Value.ToString();

                await SetModel();
                StateHasChanged();
                _Navs.NavigateTo($"{Pager?.UrlAction}?{filter.SetParameter(true)}");
            }
        }

        [JSInvokable]
        public async Task OnProvince(string _ids, string _provinceName)
        {
            LookUp.Branchs = new();
            LookUp.RMUser = new();
            filter.provinceid = null;
            filter.Provinces = null;
            filter.Branchs = new();
            filter.RMUsers = new();
            StateHasChanged();
            await Task.Delay(1);

            await _jsRuntimes.InvokeVoidAsync("BootSelectEmptyID", "Branch");
            await _jsRuntimes.InvokeVoidAsync("BootSelectEmptyID", "RMUser");

            if (_ids != null && int.TryParse(_ids, out int provinceID) && provinceID > 0)
            {
                filter.provinceid = provinceID;

                var dataBranchs = await _masterViewModel.GetBranch(provinceID);
                if (dataBranchs != null && dataBranchs.Status)
                {
                    if (dataBranchs.Data?.Count > 0)
                    {
                        LookUp.Branchs = new() { new() { BranchID = 0, BranchName = "ทั้งหมด" } };
                        //LookUp.Branchs = new();
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
            }
        }

        [JSInvokable]
        public async Task OnBranch(string _ids, string _name)
        {
            LookUp.RMUser = new();
            filter.Branchs = new();
            filter.RMUsers = new();
            StateHasChanged();

            await _jsRuntimes.InvokeVoidAsync("BootSelectEmptyID", "RMUser");

            if (_ids != null)
            {
                filter.Branchs.Add(_ids);
            }

            if (filter.Branchs.Count > 0)
            {
                var dataUsersRM = await _assignmentRMViewModel.GetListRM(new allFilter()
                {
                    userid = UserInfo.Id,
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
            }
        }

        [JSInvokable]
        public async Task OnRMUser(string _ids, string _name)
        {
            filter.RMUsers = new();

            if (_ids != null)
            {
                filter.RMUsers.Add(_ids);
            }

            await Task.Delay(1);
        }

    }
}