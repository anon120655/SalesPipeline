using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Authorizes.Users;

namespace SalesPipeline.Pages.Settings.PreApprove
{
	public partial class LoanApplicant
	{

		string? _errorMessage = null;
		private User_PermissionCustom _permission = new();

		protected override async Task OnInitializedAsync()
		{
			_permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.SetPreApprove) ?? new User_PermissionCustom();
			StateHasChanged();
			await Task.Delay(1);
		}


	}
}