using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SalesPipeline.Utils.ConstTypeModel;
using SalesPipeline.Utils.Resources.PreApprove;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Pages.Settings.PreApprove.Calculateds
{
	public partial class Partial_Cal_Info
	{
		[Parameter]
		public Guid pre_CalId { get; set; }

		[Parameter]
		public bool IsShowTab { get; set; }

		string? _errorMessage = null;
		private bool isLoading = false;
		private Pre_Cal_InfoCustom formModel = new();
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
			var data = await _preCalInfoViewModel.GetById(pre_CalId);
			if (data != null && data.Status)
			{
				if (data.Data != null)
				{
					formModel = data.Data;
					StateHasChanged();
				}
			}

			//»Ô´ªÑèÇ¤ÃÒÇ
			//if (formModel.Pre_Cal_Info_Scores == null || formModel.Pre_Cal_Info_Scores.Count == 0)
			//{
			formModel.HighScore = 0;
			formModel.Pre_Cal_Info_Scores = new() { new() { Id = Guid.NewGuid(), Name = 0 ,Score = 0 } };
			//}

			await Task.Delay(1);
		}

		private async Task Seve()
		{
			ResultModel<Pre_Cal_InfoCustom> response;

			formModel.Pre_CalId = pre_CalId;
			formModel.CurrentUserId = UserInfo.Id;

			if (formModel.Id == Guid.Empty)
			{
				response = await _preCalInfoViewModel.Create(formModel);
			}
			else
			{
				response = await _preCalInfoViewModel.Update(formModel);
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

		protected async Task InsertScore()
		{
			if (formModel.Pre_Cal_Info_Scores == null) formModel.Pre_Cal_Info_Scores = new();

			var sequenceNo = (formModel.Pre_Cal_Info_Scores.Max(p => (int?)p.SequenceNo) ?? 0) + 1;

			formModel.Pre_Cal_Info_Scores.Add(new()
			{
				Id = Guid.NewGuid(),
				Status = StatusModel.Active,
				SequenceNo = sequenceNo
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