using Microsoft.JSInterop;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;
using Microsoft.AspNetCore.Components;
using SalesPipeline.Utils.ConstTypeModel;

namespace SalesPipeline.Pages.Workloads
{
    public partial class Overdue
    {
        string? _errorMessage = null;
        private User_PermissionCustom _permission = new();
        private LookUpResource LookUp = new();
        private allFilter filter = new();
        private List<SaleCustom>? Items;
        public Pager? Pager;

        protected override async Task OnInitializedAsync()
        {
            _permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.Overdue) ?? new User_PermissionCustom();
            StateHasChanged();
            await Task.Delay(1);
        }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await SetInitManual();

                await SetQuery();
                StateHasChanged();

                firstRender = false;
            }
        }

        protected async Task SetInitManual()
        {
            var businessType = await _masterViewModel.GetBusinessType(new() { status = StatusModel.Active });
            if (businessType != null && businessType.Status)
            {
                LookUp.BusinessType = businessType.Data?.Items;
            }
            else
            {
                _errorMessage = businessType?.errorMessage;
                _utilsViewModel.AlertWarning(_errorMessage);
            }

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
            await _jsRuntimes.InvokeVoidAsync("BootSelectId", "Chain");
            await _jsRuntimes.InvokeVoidAsync("BootSelectId", "BusinessType");
            await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "OnProvince", "#Province");
        }

        protected async Task SetQuery(string? parematerAll = null)
        {
            var uriQuery = _Navs.ToAbsoluteUri(_Navs.Uri).Query;

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
            filter.isoverdue = 1;
            var data = await _salesViewModel.GetList(filter);
            if (data != null && data.Status)
            {
                Items = data.Data?.Items;
                Pager = data.Data?.Pager;
                if (Pager != null)
                {
                    Pager.UrlAction = "/workload/overdue";
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

        [JSInvokable]
        public async Task OnProvince(string _provinceID, string _provinceName)
        {
            LookUp.Branchs = new();
            filter.provinceid = null;
            filter.Branchs = new();
            StateHasChanged();
            await Task.Delay(1);

            await _jsRuntimes.InvokeVoidAsync("BootSelectEmptyID", "Branch");

            if (_provinceID != null && int.TryParse(_provinceID, out int provinceID) && provinceID > 0)
            {
                filter.provinceid = provinceID;

                var dataBranchs = await _masterViewModel.GetBranch(provinceID);
                if (dataBranchs != null && dataBranchs.Status)
                {
                    if (dataBranchs.Data?.Count > 0)
                    {
                        LookUp.Branchs = new() { new() { BranchID = 0, BranchName = "ทั้งหมด" } };
                        LookUp.Branchs.AddRange(dataBranchs.Data);
                        StateHasChanged();
                        await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "OnBranch", "#Branch");
                        await _jsRuntimes.InvokeVoidAsync("BootSelectRefreshID", "Branch", 100);
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
            await Task.Delay(1);

            if (_ids != null)
            {
                filter.Branchs.Add(_ids);
            }
        }

        protected async Task OnBusinessType(ChangeEventArgs e)
        {
            filter.businesstype = null;
            if (e.Value != null)
            {
                filter.businesstype = e.Value.ToString();
                await SetModel();
                StateHasChanged();
            }
        }

        protected async Task Search()
        {
            await SetModel();
            StateHasChanged();
        }

    }
}