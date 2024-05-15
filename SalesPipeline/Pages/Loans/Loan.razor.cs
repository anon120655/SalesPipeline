using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Authorizes.Users;

namespace SalesPipeline.Pages.Loans
{
	public partial class Loan
	{
		string? _errorMessage = null;
		private User_PermissionCustom _permission = new();

		protected override async Task OnInitializedAsync()
		{
			_permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.Loan) ?? new User_PermissionCustom();
			StateHasChanged();
			await Task.Delay(1);
		}


	}
}