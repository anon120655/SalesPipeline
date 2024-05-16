using global::Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using SalesPipeline.ViewModels;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils.Resources.Authorizes.Auths;
using BlazorBootstrap;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.Http.Features;
using NPOI.SS.Formula.Functions;

namespace SalesPipeline.Shared
{
	public partial class MainLayout
	{
		[Inject] protected IJSRuntime _jsRuntimes { get; set; } = null!;

		[Inject] protected NavigationManager _Navs { get; set; } = null!;

		[Inject] protected IOptions<AppSettings> _appSet { get; set; } = null!;

		[Inject] protected IHttpContextAccessor _accessor { get; set; } = null!;

		[Inject] protected AuthorizeViewModel _authorizeViewModel { get; set; } = null!;
		[Inject] protected MasterViewModel _masterViewModel { get; set; } = default!;

		private HubConnection _hubUserConnection = null!;
		private LoginResponseModel UserInfo = new();
		private Boolean? isAuthorize { get; set; }

		private List<UserOnlineModel> UserOnlineModels = new();
		private List<MenuItemCustom> MenuItem = new();
		public string? baseUriWeb { get; set; }
		public string? baseUriApi { get; set; }

		private string? _UserKey;
		Modal modalConfirmLogout = default!;
		//CountDown
		private System.Timers.Timer timerCountDown = new System.Timers.Timer(1000);
		private int counterDate;
		protected override async Task OnInitializedAsync()
		{
			baseUriWeb = _appSet.Value.baseUriWeb;
			baseUriApi = _appSet.Value.baseUriApi;
			//Create Connection SignalR
			string? baseUrlWeb = _appSet?.Value?.baseUriWeb;
			string? _hubUrlWeb = baseUrlWeb?.TrimEnd('/') + SignalRUtls.HubUserUrl;
			_hubUserConnection = new HubConnectionBuilder().WithUrl(_hubUrlWeb).Build();
			await _hubUserConnection.StartAsync();

			//var dataMenuItem = await _masterViewModel.MenuItem(new allFilter() { status = StatusModel.Active });
			//if (dataMenuItem != null && dataMenuItem.Status && dataMenuItem.Data != null)
			//{
			//	MenuItem = dataMenuItem.Data;
			//	StateHasChanged();
			//}
		}

		protected async override Task OnAfterRenderAsync(bool firstRender)
		{
			isAuthorize = await _authorizeViewModel.IsAuth();
			if (firstRender)
			{
				// isAuthorize = await _authorizeViewModel.IsAuth();
				StateHasChanged();
				if (_Navs.Uri.Contains("www."))
				{
					string UrlNew = _Navs.Uri.Replace("www.", string.Empty);
					_Navs.NavigateTo(UrlNew, true);
				}

				if (isAuthorize == true)
				{
					var dataMenuItem = await _masterViewModel.MenuItem(new allFilter() { status = StatusModel.Active });
					if (dataMenuItem != null && dataMenuItem.Status && dataMenuItem.Data != null)
					{
						MenuItem = dataMenuItem.Data;
						StateHasChanged();
					}

					UserInfo = await _authorizeViewModel.GetUserInfo() ?? new();
					var remoteIpAddress = _accessor.HttpContext?.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress;
					if (_hubUserConnection is not null)
					{
						_UserKey = Guid.NewGuid().ToString();
						await _hubUserConnection.SendAsync(SignalRUtls.SendUserOnline, new UserOnlineModel() { UserKey = _UserKey, Id = UserInfo.Id, FullName = UserInfo.FullName, Ipaddress = $"{remoteIpAddress}", OnlineDate = DateTime.Now });
					}
				}
				else
				{
					_Navs.NavigateTo("/signin", true);
				}

				StateHasChanged();
				await _jsRuntimes.InvokeAsync<object>("AfterRenderMainLayout");
				await _jsRuntimes.InvokeVoidAsync("initializeinactivitytimer", DotNetObjectReference.Create(this));

				firstRender = false;
			}
		}

		[JSInvokable]
		public async Task ConfirmLogout()
		{
			counterDate = 10;
			await modalConfirmLogout.ShowAsync();
			timerCountDown = new System.Timers.Timer(1000);
			timerCountDown.Enabled = true;
			timerCountDown.Elapsed += async (sender, Eventargs) => await CountDownTimer();
			timerCountDown.Start();
		}

		private async Task CountDownTimer()
		{
			if (counterDate > 0)
			{
				counterDate -= 1;
			}
			else
			{
				timerCountDown.Stop();
				timerCountDown.Enabled = false;
				LogoutByTimeOut();
			}

			await InvokeAsync(StateHasChanged);
		}

		protected void LogoutByTimeOut()
		{
			_Navs.NavigateTo("/signin", true);
			// var authState = await _authorizeViewModel.GetAuthenticationStateAsync();
			// var user = authState.User;
			// if (user != null && user.Identity != null && user.Identity.IsAuthenticated)
			// {
			// 	_Navs.NavigateTo("/signin", true);
			// }
		}

		protected async Task WantContinue()
		{
			timerCountDown.Stop();
			timerCountDown.Enabled = false;
			await modalConfirmLogout.HideAsync();
		}

		public async ValueTask DisposeAsync()
		{
			timerCountDown?.Dispose();
			if (_hubUserConnection is not null && UserInfo is not null)
			{
				await _hubUserConnection.SendAsync(SignalRUtls.RemoveUserOnline, new UserOnlineModel() { UserKey = _UserKey, Id = UserInfo.Id });
			}
		}
	}
}