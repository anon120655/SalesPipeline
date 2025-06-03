using Microsoft.AspNetCore.Components;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Customers;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils.Resources.Sales;

namespace SalesPipeline.Pages.Customers
{
    public partial class PartialSale
    {
        [Parameter]
        public Guid customerid { get; set; }

        string? _errorMessage = null;
        private bool isLoading = false;
        private LookUpResource LookUp = new();
        private allFilter filter = new();
        private List<SaleCustom>? Items;

        //protected override async Task OnInitializedAsync()
        //{
        //	await SetModel();
        //}

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await SetModel();
                StateHasChanged();
                firstRender = false;
            }
        }

        protected async Task SetModel()
        {
            if (customerid != Guid.Empty)
            {
                filter.userid = UserInfo.Id;
                filter.customerid = customerid;
                filter.pagesize = 100;
                var data = await _salesViewModel.GetList(filter);
                if (data != null && data.Status)
                {
                    Items = data.Data?.Items;
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