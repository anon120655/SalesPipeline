using Microsoft.JSInterop;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Pages.Dashboards
{
    public partial class AvgDeliveryDuration
    {
        string? _errorMessage = null;
        private User_PermissionCustom _permission = new();
        private allFilter filter = new();
        private LookUpResource LookUp = new();
        private List<Sale_DeliverCustom>? Items;
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
                await SetInitManual();
                await Task.Delay(10);

                await SetQuery();
                StateHasChanged();

                await _jsRuntimes.InvokeVoidAsync("selectPickerInitialize");
                firstRender = false;
            }
        }

        protected async Task SetInitManual()
        {
            await Task.Delay(10);
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
            var data = await _dashboarViewModel.GetDeliver(filter);
            if (data != null && data.Status)
            {
                Items = data.Data?.Items;
                Pager = data.Data?.Pager;
                if (Pager != null)
                {
                    Pager.UrlAction = "/dashboard/avgdeliveryduration";
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

        protected async Task ExportExcel()
        {
            var data = await _exportViewModel.ExcelTotalImport(filter);
            if (data != null && data.Status && data.Data != null)
            {
                await _jsRuntimes.InvokeAsync<object>("saveAsFile", "รายงานระยะเวลาเฉลี่ยในการส่งมอบ.xlsx", Convert.ToBase64String(data.Data));
            }
            else
            {
                _errorMessage = data?.errorMessage;
                await _jsRuntimes.InvokeVoidAsync("WarningAlert", _errorMessage);
            }
        }

    }
}