using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
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
				if (IsShowTab)
				{
					
				}
				StateHasChanged();
				firstRender = false;
			}
		}

		protected async Task SetModel()
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

			//if (formModel.Pre_Cal_Info_Scores == null || formModel.Pre_Cal_Info_Scores.Count == 0)
			//{
			//	formModel.Pre_Cal_Info_Scores = new() { new() { Id = Guid.NewGuid() } };
			//}

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
	}
}