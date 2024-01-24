using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Authorizes.Auths;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.ViewModels.Wrapper
{
	public class ViewModelWrapper : ComponentBase
	{
		[CascadingParameter] protected LoginResponseModel UserInfo { get; set; } = default!;
		[CascadingParameter] protected List<MenuItemCustom> MenuItem { get; set; } = default!;

		[Inject] protected IJSRuntime _jsRuntimes { get; set; } = default!;
		[Inject] protected NavigationManager _Navs { get; set; } = default!;
		[Inject] protected IOptions<AppSettings> _appSet { get; set; } = default!;
		[Inject] protected UtilsViewModel _utilsViewModel { get; set; } = default!;
		[Inject] protected FileViewModel _fileViewModel { get; set; } = default!;
		[Inject] protected MasterViewModel _masterViewModel { get; set; } = default!;
		[Inject] protected ProcessSaleViewModel _processSaleViewModel { get; set; } = default!;
		[Inject] protected UserViewModel _userViewModel { get; set; } = default!;
		[Inject] protected CustomerViewModel _customerViewModel { get; set; } = default!;
		[Inject] protected SalesViewModel _salesViewModel { get; set; } = default!;
		[Inject] protected SystemViewModel _systemViewModel { get; set; } = default!;
		[Inject] protected AuthorizeViewModel _authorizeViewModel { get; set; } = default!;
	}
}
