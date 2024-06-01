using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SalesPipeline.Utils;
using SalesPipeline.Utils.ConstTypeModel;
using SalesPipeline.Utils.Resources.PreApprove;

namespace SalesPipeline.Pages.Settings.PreApprove.Calculateds
{
	public partial class Partial_Cal_Fetu_Stan
	{
		[Parameter]
		public Guid pre_CalId { get; set; }

		[Parameter]
		public bool IsShowTab { get; set; }

		string? _errorMessage = null;
		private bool isLoading = false;
		private Pre_Cal_Fetu_StanCustom formModel = new();
		public bool _internalIsShowTab { get; set; }

		protected override async Task OnInitializedAsync()
		{
			_internalIsShowTab = IsShowTab;
			await Task.Delay(1);
		}

		protected override async Task OnParametersSetAsync()
		{
			if (_internalIsShowTab != IsShowTab)
			{
				if (IsShowTab)
				{
					await SetModel();
				}
				_internalIsShowTab = IsShowTab;
			}
		}

		protected async override Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				await Task.Delay(1);
				StateHasChanged();
				firstRender = false;
			}
		}

		protected async Task SetModel()
		{
			var data = await _preCalStanViewModel.GetById(pre_CalId);
			if (data != null && data.Status)
			{
				if (data.Data != null)
				{
					formModel = data.Data;
					StateHasChanged();
				}
			}

			if (formModel.Pre_Cal_Fetu_Stan_DropDowns == null || formModel.Pre_Cal_Fetu_Stan_DropDowns.Count == 0)
			{
				await InsertDropDown(PreStanDropDownType.CollateralType);
				await InsertDropDown(PreStanDropDownType.PaymentHistory);
			}

			if (formModel.Pre_Cal_Fetu_Stan_Scores == null || formModel.Pre_Cal_Fetu_Stan_Scores.Count == 0)
			{
				await InsertScore(PreStanScoreType.WeightIncomeExpenses);
				await InsertScore(PreStanScoreType.WeighCollateraltDebtValue);
				await InsertScore(PreStanScoreType.WeighLiabilitieOtherIncome);
				await InsertScore(PreStanScoreType.CashBank);
				await InsertScore(PreStanScoreType.CollateralType);
				await InsertScore(PreStanScoreType.LoanValue);
				await InsertScore(PreStanScoreType.PaymentHistory);
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

		protected async Task InsertDropDown(int type)
		{
			if (formModel.Pre_Cal_Fetu_Stan_DropDowns == null) formModel.Pre_Cal_Fetu_Stan_DropDowns = new();

			formModel.Pre_Cal_Fetu_Stan_DropDowns.Add(new()
			{
				Id = Guid.NewGuid(),
				Status = StatusModel.Active,
				Type = type
			});

			await Task.Delay(1);
			StateHasChanged();
		}

		protected async Task RemoveDropDown(Guid ID, int type)
		{
			var itemToRemove = formModel.Pre_Cal_Fetu_Stan_DropDowns?.FirstOrDefault(r => r.Id == ID);
			if (itemToRemove != null)
				formModel.Pre_Cal_Fetu_Stan_DropDowns?.Remove(itemToRemove);

			await Task.Delay(1);
			StateHasChanged();
		}

		protected async Task InsertScore(int type)
		{
			if (formModel.Pre_Cal_Fetu_Stan_Scores == null) formModel.Pre_Cal_Fetu_Stan_Scores = new();

			formModel.Pre_Cal_Fetu_Stan_Scores.Add(new()
			{
				Id = Guid.NewGuid(),
				Status = StatusModel.Active,
				Type = type
			});

			await Task.Delay(1);
			StateHasChanged();
		}

		protected async Task RemoveScore(Guid ID, int type)
		{
			var itemToRemove = formModel.Pre_Cal_Fetu_Stan_Scores?.FirstOrDefault(r => r.Id == ID);
			if (itemToRemove != null)
				formModel.Pre_Cal_Fetu_Stan_Scores?.Remove(itemToRemove);

			await Task.Delay(1);
			StateHasChanged();
		}

	}
}