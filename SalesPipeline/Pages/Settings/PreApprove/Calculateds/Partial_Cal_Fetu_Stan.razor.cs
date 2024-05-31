using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SalesPipeline.Utils.Resources.PreApprove;

namespace SalesPipeline.Pages.Settings.PreApprove.Calculateds
{
	public partial class Partial_Cal_Fetu_Stan
	{
		[Parameter]
		public Guid pre_CalId { get; set; }

		string? _errorMessage = null;
		private bool isLoading = false;
		private Pre_Cal_Fetu_StanCustom formModel = new();

		protected override async Task OnInitializedAsync()
		{
			await Task.Delay(1);
		}

		protected async override Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				StateHasChanged();
				firstRender = false;
			}
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