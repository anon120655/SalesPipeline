using SalesPipeline.Utils;
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
			Items = new() {
				new() {Level = "" ,CreditScore = "0" ,Grade = "D",LimitMultiplier = "Reject",RateMultiplier = "Reject" ,CreditScoreColor="#f8696b" },
				new() {Level = "" ,CreditScore = "53" ,Grade = "C",LimitMultiplier = "Reject",RateMultiplier = "Reject",CreditScoreColor="#fdd17f" },
				new() {Level = "0" ,CreditScore = "59" ,Grade = "CC",LimitMultiplier = "1.000",RateMultiplier = "1.000",CreditScoreColor="#fedd81" },
				new() {Level = "1" ,CreditScore = "61" ,Grade = "CCC",LimitMultiplier = "0.857",RateMultiplier = "0.857" ,CreditScoreColor="#fee182"},
				new() {Level = "2" ,CreditScore = "64" ,Grade = "B",LimitMultiplier = "0.714",RateMultiplier = "0.714",CreditScoreColor="#fee783" },
				new() {Level = "3" ,CreditScore = "66" ,Grade = "BB",LimitMultiplier = "0.571",RateMultiplier = "0.571",CreditScoreColor="#ffeb84" },
				new() {Level = "4" ,CreditScore = "69" ,Grade = "BBB",LimitMultiplier = "0.429",RateMultiplier = "0.429",CreditScoreColor="#f2e884" },
				new() {Level = "5" ,CreditScore = "75" ,Grade = "A",LimitMultiplier = "0.286",RateMultiplier = "0.286",CreditScoreColor="#d7e082" },
				new() {Level = "6" ,CreditScore = "80" ,Grade = "AA",LimitMultiplier = "0.143",RateMultiplier = "0.143",CreditScoreColor="#c1da81" },
				new() {Level = "7" ,CreditScore = "86" ,Grade = "AAA",LimitMultiplier = "0.000",RateMultiplier = "0.000",CreditScoreColor="#a6d27f" },
				new() {Level = "" ,CreditScore = "101" ,Grade = "Error",LimitMultiplier = "Error",RateMultiplier = "Error",CreditScoreColor="#63be7b" }
			};
		}

	}
}