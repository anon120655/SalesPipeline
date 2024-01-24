using global::Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Customers;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Pages.Customers
{
	public partial class CustomerView
	{
		[Parameter]
		public Guid id { get; set; }

		string? _errorMessage = null;
		private bool isLoading = false;
		private User_PermissionCustom _permission = new();
		private LookUpResource LookUp = new();
		private CustomerCustom formModel = new();

		protected override async Task OnInitializedAsync()
		{
			_permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.Customers) ?? new User_PermissionCustom();
			StateHasChanged();

			await SetModel();
		}

		protected async override Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				await _jsRuntimes.InvokeVoidAsync("BootSelectClass", "selectInit");
				StateHasChanged();
				firstRender = false;
			}
		}

		protected async Task SetModel()
		{
			if (id != Guid.Empty)
			{
				var data = await _customerViewModel.GetById(id);
				if (data != null && data.Status && data.Data != null)
				{
					formModel = data.Data;
				}
				else
				{
					_errorMessage = data?.errorMessage;
					_utilsViewModel.AlertWarning(_errorMessage);
				}
			}

			if (formModel.Customer_Committees == null || formModel.Customer_Committees.Count == 0)
			{
				formModel.Customer_Committees = new() { new() { Id = Guid.NewGuid() } };
			}

			if (formModel.Customer_Shareholders == null || formModel.Customer_Shareholders.Count == 0)
			{
				formModel.Customer_Shareholders = new() { new() { Id = Guid.NewGuid() } };
			}
		}

	}
}