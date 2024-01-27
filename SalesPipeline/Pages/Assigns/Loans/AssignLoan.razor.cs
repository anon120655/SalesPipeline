using Microsoft.JSInterop;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Pages.Assigns.Loans
{
	public partial class AssignLoan
	{
		string? _errorMessage = null;
		private User_PermissionCustom _permission = new();
		private allFilter filter = new();
		private LookUpResource LookUp = new();
		private List<SaleCustom>? Items;
		public Pager? Pager;

		protected async override Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{

				StateHasChanged();
				firstRender = false;
			}
		}

	}
}