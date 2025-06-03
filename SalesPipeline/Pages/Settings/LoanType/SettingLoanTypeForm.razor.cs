using global::Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils;

namespace SalesPipeline.Pages.Settings.LoanType
{
    public partial class SettingLoanTypeForm
    {
        [Parameter]
        public Guid? id { get; set; }

        string? _errorMessage = null;
        private bool isLoading = false;
        private LookUpResource LookUp = new();
        private User_PermissionCustom _permission = new();
        private Master_LoanTypeCustom formModel = new();

        protected override async Task OnInitializedAsync()
        {
            _permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.SetLoanType) ?? new User_PermissionCustom();
            StateHasChanged();

            await SetModel();
        }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await _jsRuntimes.InvokeVoidAsync("selectPickerInitialize");
                StateHasChanged();
                firstRender = false;
            }
        }

        protected async Task SetModel()
        {
            if (id.HasValue)
            {
                var data = await _masterViewModel.GetLoanTypeById(id.Value);
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

        protected async Task OnInvalidSubmit()
        {
            await Task.Delay(100);
            await _jsRuntimes.InvokeVoidAsync("scrollToElement", "validation-message");
        }

        protected async Task Save()
        {
            _errorMessage = null;
            ShowLoading();

            ResultModel<Master_LoanTypeCustom> response;

            formModel.CurrentUserId = UserInfo.Id;

            if (id.HasValue)
            {
                response = await _masterViewModel.UpdateLoanType(formModel);
            }
            else
            {
                response = await _masterViewModel.CreateLoanType(formModel);
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

        protected void Cancel()
        {
            _Navs.NavigateTo("/setting/loantype");
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

    }
}