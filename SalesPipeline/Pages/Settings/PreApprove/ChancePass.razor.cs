using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.PreApprove;

namespace SalesPipeline.Pages.Settings.PreApprove
{
	public partial class ChancePass
	{

		string? _errorMessage = null;
		private User_PermissionCustom _permission = new();
		private List<ChancePassModel>? Items;

		protected override async Task OnInitializedAsync()
		{
			_permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.SetPreApprove) ?? new User_PermissionCustom();
			StateHasChanged();

			SetModel();
			await Task.Delay(1);
		}

		public void SetModel()
		{
			Items = new() {
				new() {Z = "-6" ,CreditScore = "100" ,Prob = "0.002472623"},
				new() {Z = "-5.9" ,CreditScore = "99" ,Prob = "0.002731961"},
				new() {Z = "-5.8" ,CreditScore = "98" ,Prob = "0.003018416"},
				new() {Z = "-5.7" ,CreditScore = "97" ,Prob = "0.003334807"},
				new() {Z = "-5.6" ,CreditScore = "96" ,Prob = "0.00368424"},
				new() {Z = "-5.5" ,CreditScore = "95" ,Prob = "0.004070138"},
				new() {Z = "-5.4" ,CreditScore = "94" ,Prob = "0.004496273"},
				new() {Z = "-5.3" ,CreditScore = "93" ,Prob = "0.004966802"},
				new() {Z = "-5.2" ,CreditScore = "92" ,Prob = "0.005486299"},
				new() {Z = "-5.1" ,CreditScore = "91" ,Prob = "0.006059801"},
				new() {Z = "5" ,CreditScore = "90" ,Prob = "0.006692851"}
			};
		}


	}
}