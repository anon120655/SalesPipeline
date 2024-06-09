using SalesPipeline.Utils;
using SalesPipeline.Utils.DataCustom;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.PreApprove;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Pages.Settings.PreApprove
{
	public partial class ChancePass
	{
		string? _errorMessage = null;
		private allFilter filter = new();
		private bool isLoading = false;
		private User_PermissionCustom _permission = new();
		private List<Pre_ChancePassCustom>? Items;

		protected override async Task OnInitializedAsync()
		{
			_permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.SetPreApprove) ?? new User_PermissionCustom();
			StateHasChanged();

			await Task.Delay(1);
		}

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
			filter.pagesize = 300;
			var data = await _preChanceViewModel.GetList(filter);
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