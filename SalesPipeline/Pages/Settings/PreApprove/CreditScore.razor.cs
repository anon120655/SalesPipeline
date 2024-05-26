using SalesPipeline.Utils;
using SalesPipeline.Utils.DataCustom;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.PreApprove;
using SalesPipeline.Utils.Resources.Sales;

namespace SalesPipeline.Pages.Settings.PreApprove
{
	public partial class CreditScore
	{

		string? _errorMessage = null;
		private User_PermissionCustom _permission = new();
		private List<CreditScoreModel>? Items;

		protected override async Task OnInitializedAsync()
		{
			_permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.SetPreApprove) ?? new User_PermissionCustom();
			StateHasChanged();

			SetModel();
			await Task.Delay(1);
		}

		public void SetModel()
		{
			Items = MoreDataModel.CreditScore();
		}

	}
}