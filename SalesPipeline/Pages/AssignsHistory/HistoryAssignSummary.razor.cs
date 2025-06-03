using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SalesPipeline.Shared.Modals;
using SalesPipeline.Utils;
using SalesPipeline.Utils.ConstTypeModel;
using SalesPipeline.Utils.Resources.Assignments;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils.Resources.Thailands;

namespace SalesPipeline.Pages.AssignsHistory
{
    public partial class HistoryAssignSummary
    {
        [Parameter]
        public Guid id { get; set; }

        string? _errorMessage = null;
        private bool isLoading = false;
        private allFilter filter = new();
        private User_PermissionCustom _permission = new();
        private LookUpResource LookUp = new();
        private SaleCustom? formModel;
        private List<Assignment_RMCustom>? Items;
        private int stepAssign = StepAssignLoanModel.Assigned;

        ModalConfirm modalConfirmAssign = default!;
        ModalSuccessful modalSuccessfulAssign = default!;
        private bool IsToClose = false;

        protected override async Task OnInitializedAsync()
        {
            _permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.HistoryAssign) ?? new User_PermissionCustom();
            StateHasChanged();

            await SetModel();
            await SetModelAssigned();
        }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await SetInitManual();
                await Task.Delay(10);

                await _jsRuntimes.InvokeVoidAsync("selectPickerInitialize");
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
            await _jsRuntimes.InvokeVoidAsync("BootSelectId", "BusinessType");
            await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "ProvinceChange", "#Province");
        }

        protected async Task SetModel()
        {
            if (id != Guid.Empty)
            {
                var data = await _salesViewModel.GetById(id);
                if (data != null && data.Status && data.Data != null)
                {
                    formModel = data.Data;
                }
                else
                {
                    _errorMessage = data?.errorMessage;
                    _utilsViewModel.AlertWarning(_errorMessage);
                }
            }
        }

        protected async Task SetModelAssigned()
        {
            if (UserInfo.RoleCode != null)
            {
                filter.assigncenter = UserInfo.Id;
            }

            filter.userid = UserInfo.Id;
            filter.pagesize = 100;
            var data = await _assignmentRMViewModel.GetListRM(filter);
            if (data != null && data.Status)
            {
                Items = data.Data?.Items;
            }
            else
            {
                _errorMessage = data?.errorMessage;
                _utilsViewModel.AlertWarning(_errorMessage);
            }

            StateHasChanged();
        }

        protected async Task GotoStep(int step)
        {
            bool isNext = true;
            ClearSearchAssigned();

            if (step == StepAssignLoanModel.Assigned)
            {

            }
            else if (step == StepAssignLoanModel.Summary)
            {
                isNext = Summary();
                if (!isNext)
                {
                    _utilsViewModel.AlertWarning("เลือกผู้รับผิดชอบใหม่");
                }
            }

            if (isNext)
            {
                stepAssign = step;
                StateHasChanged();
            }

            await Task.Delay(10);
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

        protected void Cancel()
        {
            _Navs.NavigateTo("/historyassign");
        }

        protected void OnCheckEmployee(Assignment_RMCustom model, object? checkedValue)
        {
            if (Items?.Count > 0)
            {
                foreach (var item in Items.Where(x => x.IsSelect))
                {
                    item.IsSelect = false;
                }
            }

            if (checkedValue != null && (bool)checkedValue)
            {
                model.IsSelect = true;
            }
            else
            {
                model.IsSelect = false;
            }
        }

        protected void ClearSearchAssigned()
        {
            filter = new();
            if (Items?.Count > 0)
            {
                foreach (var item in Items)
                {
                    item.IsShow = true;
                }
            }
        }

        protected bool Summary()
        {
            if (Items?.Count > 0)
            {
                //_itemsAssign ผู้รับผิดชอบใหม่ที่ถูกมอบหมายใหม่
                var _itemsAssign = Items.Where(x => x.IsSelect).FirstOrDefault();
                if (_itemsAssign != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        protected async Task InitShowConfirmAssign()
        {
            await ShowConfirmAssign(null, "กรุณากด ยืนยัน แก้ไขมอบหมายงาน", "<img src=\"/image/icon/do.png\" width=\"65\" />");
        }

        protected async Task ShowConfirmAssign(string? id, string? txt, string? icon = null)
        {
            IsToClose = false;
            await modalConfirmAssign.OnShowConfirm(id, $"{txt}", icon);
        }

        protected async Task ConfirmAssign(string id)
        {
            ShowLoading();
            await Task.Delay(10);
            await AssignChange();
        }

        protected async Task ShowSuccessfulAssign(string? id, string? txt)
        {
            await modalSuccessfulAssign.OnShow(id, $"{txt}");
        }

        private async Task OnModalHidden()
        {
            if (IsToClose)
            {
                await Task.Delay(1);
                Cancel();
            }
        }

        protected async Task AssignChange()
        {
            _errorMessage = null;

            if (Items != null && formModel != null)
            {
                var _itemsNew = Items.FirstOrDefault(x => x.IsSelect);

                if (_itemsNew != null)
                {
                    AssignChangeModel model = new();
                    model.CurrentUserId = UserInfo.Id;
                    model.Original = formModel;
                    model.New = _itemsNew;

                    var response = await _assignmentRMViewModel.AssignChange(model);

                    if (response.Status)
                    {
                        IsToClose = true;
                        await modalConfirmAssign.OnHideConfirm();
                        await ShowSuccessfulAssign(null, "เสร็จสิ้นการแก้ไขมอบหมายงาน");
                        await SetModel();
                        HideLoading();
                    }
                    else
                    {
                        HideLoading();
                        _errorMessage = response.errorMessage;
                        await _jsRuntimes.InvokeVoidAsync("WarningAlert", _errorMessage);
                    }
                }
                else
                {
                    HideLoading();
                    _errorMessage = "เกิดข้อผิดพลาด กรุณาทำรายการอีกครั้ง";
                    await _jsRuntimes.InvokeVoidAsync("WarningAlert", _errorMessage);
                }
            }

        }

        protected async Task SearchStepAssigned()
        {
            await SetModelAssigned();
            StateHasChanged();
        }

        [JSInvokable]
        public async Task ProvinceChange(string _provinceID, string _provinceName)
        {
            filter.provinceid = null;
            filter.amphurid = null;
            LookUp.Amphurs = new();
            StateHasChanged();
            await _jsRuntimes.InvokeVoidAsync("BootSelectEmptyID", "Amphur");

            if (_provinceID != null && int.TryParse(_provinceID, out int provinceID))
            {
                filter.provinceid = provinceID;

                var amphurs = await _masterViewModel.GetAmphur(provinceID);
                if (amphurs != null && amphurs.Data?.Count > 0)
                {
                    LookUp.Amphurs = new List<InfoAmphurCustom>() { new InfoAmphurCustom() { AmphurID = 0, AmphurName = "--เลือก--" } };
                    LookUp.Amphurs.AddRange(amphurs.Data);

                    StateHasChanged();
                    await Task.Delay(10);
                    await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "AmphurChange", "#Amphur");
                    await _jsRuntimes.InvokeVoidAsync("BootSelectRefreshID", "Amphur", 100);
                }
                else
                {
                    _errorMessage = amphurs?.errorMessage;
                    _utilsViewModel.AlertWarning(_errorMessage);
                }
            }

            await SetModelAssigned();
            StateHasChanged();

        }

        [JSInvokable]
        public async Task AmphurChange(string _amphurID, string _amphurName)
        {
            filter.amphurid = null;
            if (_amphurID != null && int.TryParse(_amphurID, out int amphurID))
            {
                filter.amphurid = amphurID;
            }

            await SetModelAssigned();
            StateHasChanged();
        }

    }
}