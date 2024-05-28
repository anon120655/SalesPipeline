using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.PreApprove;

namespace SalesPipeline.Pages.Settings.PreApprove.Calculateds
{
	public partial class Partial_Cal_Info
	{
		[Parameter]
		public Guid id { get; set; }

		private bool isLoading = false;
		private Pre_Cal_InfoCustom formModel = new();

		protected override async Task OnInitializedAsync()
		{
			await SetModel();
		}

		protected async Task SetModel()
		{
			if (id != Guid.Empty)
			{
				//var data = await _customerViewModel.GetById(id);
				//if (data != null && data.Status && data.Data != null)
				//{
				//	formModel = data.Data;
				//	if (formModel.Sales != null)
				//	{
				//		formModel.Branch_RegionId = formModel.Sales.Select(x => x.Master_Branch_RegionId).FirstOrDefault();
				//	}
				//}
				//else
				//{
				//	_errorMessage = data?.errorMessage;
				//	_utilsViewModel.AlertWarning(_errorMessage);
				//}
			}

			if (formModel.Pre_Cal_Info_Scores == null || formModel.Pre_Cal_Info_Scores.Count == 0)
			{
				formModel.Pre_Cal_Info_Scores = new() { new() { Id = Guid.NewGuid() } };
			}

			await Task.Delay(1);
		}

		private async Task Seve()
		{
			await Task.Delay(300);
			await _jsRuntimes.InvokeVoidAsync("SuccessAlert");
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