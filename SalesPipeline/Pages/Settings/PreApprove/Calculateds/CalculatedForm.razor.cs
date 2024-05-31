using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.PreApprove;

namespace SalesPipeline.Pages.Settings.PreApprove.Calculateds
{
	public partial class CalculatedForm
	{
		[Parameter]
		public Guid id { get; set; }

		string? _errorMessage = null;
		private User_PermissionCustom _permission = new();
		private Pre_CalCustom formModel = new();
		private Pre_Cal_InfoCustom formModelInfo = new();
		private Pre_Cal_Fetu_StanCustom formModelStan = new();

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
				await _jsRuntimes.InvokeVoidAsync("BootSelectClass", "selectInit");

				firstRender = false;
			}

		}

		protected async Task ShowTabInfo()
		{
			//var data = await _preCalInfoViewModel.GetById(pre_CalId);
			//if (data != null && data.Status)
			//{
			//	if (data.Data != null)
			//	{
			//		formModel = data.Data;
			//		StateHasChanged();
			//	}
			//}

			//if (formModelInfo.Pre_Cal_Info_Scores == null || formModelInfo.Pre_Cal_Info_Scores.Count == 0)
			//{
			//	formModelInfo.Pre_Cal_Info_Scores = new() { new() { Id = Guid.NewGuid() } };
			//}
		}


	}
}