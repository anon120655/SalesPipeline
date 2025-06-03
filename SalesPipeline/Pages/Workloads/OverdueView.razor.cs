using Microsoft.AspNetCore.Components;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Sales;

namespace SalesPipeline.Pages.Workloads
{
    public partial class OverdueView
    {
        [Parameter]
        public Guid id { get; set; }

        string? _errorMessage = null;
        private User_PermissionCustom _permission = new();
        private SaleCustom formModel = new();

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
                await SetModel();
                StateHasChanged();
                await Task.Delay(1);
                firstRender = false;
            }
        }

        protected async Task SetModel()
        {
            if (id != Guid.Empty)
            {
                var data = await _processSaleViewModel.GetListContactHistory(new() { id = id, pagesize = 50 });
                if (data != null && data.Status && data.Data != null)
                {
                    formModel.Sale_Contact_Histories = data.Data.Items;
                }
                else
                {
                    _errorMessage = data?.errorMessage;
                    _utilsViewModel.AlertWarning(_errorMessage);
                }
            }
        }

        protected void Cancel()
        {
            _Navs.NavigateTo("/workload/overdue");
        }


    }
}