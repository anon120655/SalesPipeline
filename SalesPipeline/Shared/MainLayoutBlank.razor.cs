using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.ManageSystems;
using SalesPipeline.ViewModels;

namespace SalesPipeline.Shared
{
	public partial class MainLayoutBlank
	{
		[Inject] protected IOptions<AppSettings> _appSet { get; set; } = null!;
		//[Inject] protected SystemViewModel _systemViewModel { get; set; } = default!;

		//private List<System_ConfigCustom>? ItemConfig = null;

		//protected override async Task OnInitializedAsync()
		//{
		//	var dataConfig = await _systemViewModel.GetConfig();
		//	if (dataConfig != null && dataConfig.Status && dataConfig.Data != null)
		//	{
		//		ItemConfig = dataConfig.Data;
		//	}
		//}

		//protected async override Task OnAfterRenderAsync(bool firstRender)
		//{
		//	if (firstRender)
		//	{
		//		firstRender = false;
		//	}
		//}

	}
}