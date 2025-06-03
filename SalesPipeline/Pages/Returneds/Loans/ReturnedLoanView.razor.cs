using Microsoft.AspNetCore.Components;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Pages.Returneds.Loans
{
    public partial class ReturnedLoanView
    {
        [Parameter]
        public Guid id { get; set; }

        string? _errorMessage = null;
        private bool isLoading = false;
        private User_PermissionCustom _permission = new();
        private LookUpResource LookUp = new();
        private SaleCustom formModel = new();

        protected override async Task OnInitializedAsync()
        {
            _permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.ReturnedLoan) ?? new User_PermissionCustom();
            StateHasChanged();

            await SetModel();
        }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                StateHasChanged();
                firstRender = false;
            }
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

    }
}