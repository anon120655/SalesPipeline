using Microsoft.JSInterop;

namespace SalesPipeline.Pages.Settings.PreApprove.Calculateds
{
	public partial class Partial_Cal_Fetu_Stan
	{
		private bool isLoading = false;

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