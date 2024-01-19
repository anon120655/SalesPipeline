using global::Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Customers;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Pages.Customers
{
	public partial class CustomerForm
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

			await SetInitManual();
			await SetModel();
		}

		protected async override Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				await _jsRuntimes.InvokeVoidAsync("selectPickerInitialize");
				StateHasChanged();
				firstRender = false;
			}
		}

		protected async Task SetInitManual()
		{
			var dataLevels = await _userViewModel.GetListLevel(new allFilter() { status = StatusModel.Active });
			if (dataLevels != null && dataLevels.Status)
			{
				LookUp.UserLevels = dataLevels.Data;
			}
			else
			{
				_errorMessage = dataLevels?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

			StateHasChanged();
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

		protected async Task OnInvalidSubmit()
		{
			await Task.Delay(100);
			await _jsRuntimes.InvokeVoidAsync("scrollToElement", "validation-message");
		}

		protected async Task Save()
		{
			_errorMessage = null;
			ShowLoading();

			ResultModel<CustomerCustom> response;

			formModel.CurrentUserId = UserInfo.Id;

			if (id != Guid.Empty)
			{
				response = await _customerViewModel.Update(formModel);
			}
			else
			{
				response = await _customerViewModel.Create(formModel);
			}

			if (response.Status)
			{
				await _jsRuntimes.InvokeVoidAsync("SuccessAlert");
				Cancel();
			}
			else
			{
				HideLoading();
				_errorMessage = response.errorMessage;
				await _jsRuntimes.InvokeVoidAsync("WarningAlert", _errorMessage);
			}
		}

		public void Cancel()
		{
			_Navs.NavigateTo("/customer/target");
		}

		protected async Task InsertCommittee()
		{
			if (formModel.Customer_Committees == null) formModel.Customer_Committees = new();

			formModel.Customer_Committees.Add(new()
			{
				Id = Guid.NewGuid()
			});

			await Task.Delay(1);
			StateHasChanged();
		}

		protected async Task RemoveCommittee(Guid ID)
		{
			var itemToRemove = formModel.Customer_Committees?.FirstOrDefault(r => r.Id == ID);
			if (itemToRemove != null)
				formModel.Customer_Committees?.Remove(itemToRemove);

			await Task.Delay(1);
			StateHasChanged();
		}

		protected async Task InsertShareholder()
		{
			if (formModel.Customer_Shareholders == null) formModel.Customer_Shareholders = new();

			formModel.Customer_Shareholders.Add(new()
			{
				Id = Guid.NewGuid()
			});

			await Task.Delay(1);
			StateHasChanged();
		}

		protected async Task RemoveShareholder(Guid ID)
		{
			var itemToRemove = formModel.Customer_Shareholders?.FirstOrDefault(r => r.Id == ID);
			if (itemToRemove != null)
				formModel.Customer_Shareholders?.Remove(itemToRemove);

			await Task.Delay(1);
			StateHasChanged();
		}

		protected void ShowLoading()
		{
			isLoading = true;
			StateHasChanged();
		}

		protected void HideLoading()
		{
			isLoading = false;
			StateHasChanged();
		}

	}
}