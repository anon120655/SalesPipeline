using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.PreApprove;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Pages.Settings.PreApprove.Calculateds
{
	public partial class Partial_Cal_Info
	{
		[Parameter]
		public Guid id { get; set; }

		string? _errorMessage = null;
		private bool isLoading = false;
		private Pre_Cal_InfoCustom formModel = new();

		protected override async Task OnInitializedAsync()
		{
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
			var data = await _preCalInfoViewModel.GetById(id);
			if (data != null && data.Status)
			{
				if (data.Data != null)
				{
					formModel = data.Data;
					StateHasChanged();
				}
			}
			else
			{
				_errorMessage = data?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

			if (formModel.Pre_Cal_Info_Scores == null || formModel.Pre_Cal_Info_Scores.Count == 0)
			{
				formModel.Pre_Cal_Info_Scores = new() { new() { Id = Guid.NewGuid() } };
			}

			await Task.Delay(1);
		}

		private async Task Seve()
		{
			formModel.CurrentUserId = UserInfo.Id;

			var response = await _preCalInfoViewModel.Update(formModel);
			if (response.Status)
			{
				await _jsRuntimes.InvokeVoidAsync("SuccessAlert");
				await SetModel();
			}
			else
			{
				_errorMessage = response.errorMessage;
				await _jsRuntimes.InvokeVoidAsync("WarningAlert", _errorMessage);
			}
		}

		private void Cancel()
		{
			_Navs.NavigateTo("/setting/pre/calculated");
		}

		protected async Task InsertScore()
		{
			if (formModel.Pre_Cal_Info_Scores == null) formModel.Pre_Cal_Info_Scores = new();

			formModel.Pre_Cal_Info_Scores.Add(new()
			{
				Id = Guid.NewGuid(),
				Status = StatusModel.Active
			});

			await Task.Delay(1);
			StateHasChanged();
		}

		protected async Task RemoveScore(Guid ID)
		{
			var itemToRemove = formModel.Pre_Cal_Info_Scores?.FirstOrDefault(r => r.Id == ID);
			if (itemToRemove != null)
				formModel.Pre_Cal_Info_Scores?.Remove(itemToRemove);

			await Task.Delay(1);
			StateHasChanged();
		}

	}
}