using global::Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SalesPipeline.Shared.Modals;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Customers;
using SalesPipeline.Utils.Resources.Sales;
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
		private SaleCustom formModel = new();

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
				StateHasChanged();
				firstRender = false;
			}
		}

		protected async Task SetModel()
		{
			if (id != Guid.Empty)
			{
				var data = await _salesViewModel.GetById(id);
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
		}

		protected void Cancel()
		{
			_Navs.NavigateTo("/customer");
		}

		protected async Task UpdateStatusWaitResults()
		{
			if (id != Guid.Empty)
			{
				var response = await _salesViewModel.UpdateStatusOnly(new()
				{
					SaleId = id,
					StatusId = StatusSaleModel.WaitResults,
					CreateBy = UserInfo.Id
				});

				if (response.Status)
				{
					await _jsRuntimes.InvokeVoidAsync("SuccessAlert");
					Cancel();
				}
				else
				{
					_errorMessage = response.errorMessage;
					await _jsRuntimes.InvokeVoidAsync("WarningAlert", _errorMessage);
				}
			}
		}

		protected async Task ShowTabDocument()
		{
			if (formModel.Sale_Documents == null || formModel.Sale_Documents.Count == 0)
			{
				var data = await _processSaleViewModel.GetListDocument(new() { id = id, pagesize = 50 });
				if (data != null && data.Status && data.Data != null)
				{
					formModel.Sale_Documents = data.Data;
				}
				else
				{
					_errorMessage = data?.errorMessage;
					_utilsViewModel.AlertWarning(_errorMessage);
				}
			}
		}

		protected async Task ShowTabContactHistory()
		{
			if (formModel.Sale_Contact_Histories == null || formModel.Sale_Contact_Histories.Count == 0)
			{
				var data = await _processSaleViewModel.GetListContactHistory(new() { id = id, pagesize = 50 });
				if (data != null && data.Status && data.Data != null)
				{
					formModel.Sale_Contact_Histories = data.Data.Items;
				}
				else
				{
					_errorMessage = data?.errorMessage;
					_utilsViewModel.AlertWarning(_errorMessage);
				}
			}
		}


	}
}