using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using NPOI.Util;
using SalesPipeline.Utils.ConstTypeModel;
using SalesPipeline.Utils.Resources.PreApprove;
using SalesPipeline.Utils.Resources.Shares;

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

			if (formModel.Id == Guid.Empty)
			{
				await InsertDropDown(PreStanDropDownType.CollateralType);
				await InsertDropDown(PreStanDropDownType.PaymentHistory);

				await InsertScore(PreStanScoreType.WeightIncomeExpenses);
				await InsertScore(PreStanScoreType.WeighCollateraltDebtValue);
				await InsertScore(PreStanScoreType.WeighLiabilitieOtherIncome);
				await InsertScore(PreStanScoreType.CashBank);
				//await InsertScore(PreStanScoreType.CollateralType);
				await InsertScore(PreStanScoreType.CollateralValue);
				//await InsertScore(PreStanScoreType.PaymentHistory);
			}

			await Task.Delay(1);
		}

		private async Task Seve()
		{
			ResultModel<Pre_Cal_Fetu_StanCustom> response;

			formModel.Pre_CalId = pre_CalId;
			formModel.CurrentUserId = UserInfo.Id;

			if (formModel.Id == Guid.Empty)
			{
				response = await _preCalStanViewModel.Create(formModel);
			}
			else
			{
				response = await _preCalStanViewModel.Update(formModel);
			}

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

		protected async Task InsertDropDown(int type)
		{
			if (formModel.Pre_Cal_Fetu_Stan_ItemOptions == null) formModel.Pre_Cal_Fetu_Stan_ItemOptions = new();

			//var sequenceNo = (formModel.Pre_Cal_Fetu_Stan_DropDowns.Where(x => x.Type == type).Max(p => (int?)p.SequenceNo) ?? 0) + 1;

			var pre_Cal_Fetu_StanDropDownId = Guid.NewGuid();
			formModel.Pre_Cal_Fetu_Stan_ItemOptions.Add(new()
			{
				Id = pre_Cal_Fetu_StanDropDownId,
				Status = StatusModel.Active,
				Type = type
			});

			CopyItemDropDownToScore(type);

			await Task.Delay(1);
			StateHasChanged();
		}

		protected async Task RemoveDropDown(Guid ID)
		{
			var itemToRemove = formModel.Pre_Cal_Fetu_Stan_ItemOptions?.FirstOrDefault(r => r.Id == ID);
			if (itemToRemove != null)
			{
				formModel.Pre_Cal_Fetu_Stan_ItemOptions?.Remove(itemToRemove);
			}

			var itemScoreToRemove = formModel.Pre_Cal_Fetu_Stan_Scores?.FirstOrDefault(r => r.Pre_Cal_Fetu_StanItemOptionId == ID);
			if (itemScoreToRemove != null)
			{
				formModel.Pre_Cal_Fetu_Stan_Scores?.Remove(itemScoreToRemove);
			}

			//await CalSequenceDropDown();

			await Task.Delay(1);
			StateHasChanged();
		}

		//protected async Task CalSequenceDropDown()
		//{
		//	if (formModel.Pre_Cal_Fetu_Stan_DropDowns != null)
		//	{
		//		var stan_DropDown = formModel.Pre_Cal_Fetu_Stan_DropDowns.Where(x => x.Type == PreStanDropDownType.CollateralType).OrderBy(x => x.SequenceNo);
		//		int index = 1;
		//		foreach (var item in stan_DropDown)
		//		{
		//			item.SequenceNo = index;
		//			index++;
		//		}

		//		var stan_DropDownPay = formModel.Pre_Cal_Fetu_Stan_DropDowns.Where(x => x.Type == PreStanDropDownType.PaymentHistory).OrderBy(x => x.SequenceNo);
		//		int index2 = 1;
		//		foreach (var item in stan_DropDownPay)
		//		{
		//			item.SequenceNo = index2;
		//			index2++;
		//		}
		//	}

		//	await Task.Delay(1);
		//	StateHasChanged();
		//}

		private void CopyItemDropDownToScore(int type)
		{
			if (formModel.Pre_Cal_Fetu_Stan_Scores == null) formModel.Pre_Cal_Fetu_Stan_Scores = new();

			var _typeScore = type == PreStanDropDownType.CollateralType ? PreStanScoreType.CollateralType : PreStanScoreType.PaymentHistory;

			var Stan_Scores = formModel.Pre_Cal_Fetu_Stan_Scores.Where(x => x.Type == _typeScore).ToList();
			if (Stan_Scores != null)
			{
				foreach (var itemR in Stan_Scores)
				{
					formModel.Pre_Cal_Fetu_Stan_Scores.Remove(itemR);
				}
			}

			if (formModel.Pre_Cal_Fetu_Stan_ItemOptions != null && formModel.Pre_Cal_Fetu_Stan_ItemOptions.Count > 0)
			{
				var itemCopy = formModel.Pre_Cal_Fetu_Stan_ItemOptions.Where(x => x.Type == type).ToList();
				foreach (var item in itemCopy)
				{
					formModel.Pre_Cal_Fetu_Stan_Scores.Add(new()
					{
						Id = Guid.NewGuid(),
						Status = StatusModel.Active,
						Pre_Cal_Fetu_StanItemOptionId = item.Id,
						Type = _typeScore,
						SequenceNo = item.SequenceNo,
						Name = item.Name
					});
				}
			}
		}

		protected async Task InsertScore(int type)
		{
			if (formModel.Pre_Cal_Fetu_Stan_Scores == null) formModel.Pre_Cal_Fetu_Stan_Scores = new();

			//var sequenceNo = (formModel.Pre_Cal_Fetu_Stan_Scores.Max(p => (int?)p.SequenceNo) ?? 0) + 1;

			formModel.Pre_Cal_Fetu_Stan_Scores.Add(new()
			{
				Id = Guid.NewGuid(),
				Status = StatusModel.Active,
				Type = type
			});

			await Task.Delay(1);
			StateHasChanged();
		}

		protected async Task RemoveScore(Guid ID)
		{
			var itemToRemove = formModel.Pre_Cal_Fetu_Stan_Scores?.FirstOrDefault(r => r.Id == ID);
			if (itemToRemove != null)
				formModel.Pre_Cal_Fetu_Stan_Scores?.Remove(itemToRemove);

			await Task.Delay(1);
			StateHasChanged();
		}

	}
}