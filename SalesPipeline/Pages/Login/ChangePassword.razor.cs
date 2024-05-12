
using Microsoft.AspNetCore.Http.Features;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.ViewModels;

namespace SalesPipeline.Pages.Login
{
	public partial class ChangePassword
	{
		string? _errorMessage = null;
		bool isLoading = false;
		ChangePasswordModel changePassword = new ChangePasswordModel();

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				firstRender = false;
			}
		}

		protected async Task SubmitChange()
		{
			
		}

	}
}