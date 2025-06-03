using global::Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Sales;

namespace SalesPipeline.Pages.Settings.ProcessSales
{
    public partial class ProcessSalesReplyView
    {
        [Parameter]
        public Guid id { get; set; }

        string? _errorMessage = null;
        private bool isLoading = false;
        private User_PermissionCustom _permission = new();
        private Sale_ReplyCustom formModel = new();

        protected override async Task OnInitializedAsync()
        {
            _permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.SetProcessSales) ?? new User_PermissionCustom();
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
            var data = await _processSaleViewModel.GetReplyById(id);
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

        private void ShowLoading()
        {
            isLoading = true;
            StateHasChanged();
        }

        private void HideLoading()
        {
            isLoading = false;
            StateHasChanged();
        }

    }
}