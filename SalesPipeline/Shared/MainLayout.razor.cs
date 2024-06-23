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
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.NetworkInformation;
using SalesPipeline.Utils.ConstTypeModel;

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
		[Inject] protected SalesViewModel _salesViewModel { get; set; } = default!;

		[Inject] protected AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

		private HubConnection _hubUserConnection = null!;
		private LoginResponseModel UserInfo = new();
		private bool isAuthorize { get; set; }
		private bool isRedirecting = false;

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
			AuthenticationStateProvider.AuthenticationStateChanged += OnAuthenticationStateChanged;

			baseUriWeb = _appSet.Value.baseUriWeb;
			baseUriApi = _appSet.Value.baseUriApi;
			string? baseUrlWeb = _appSet?.Value?.baseUriWeb;
			string? _hubUrlWeb = baseUrlWeb?.TrimEnd('/') + SignalRUtls.HubUserUrl;
			_hubUserConnection = new HubConnectionBuilder().WithUrl(_hubUrlWeb).Build();

			///**** comment StartAsync ไว้ก่อน เพราะถ้าเปิดจะไปที่ OnAfterRender ทันที
			await _hubUserConnection.StartAsync();

			isAuthorize = await _authorizeViewModel.IsAuth();

			if (isAuthorize)
			{
				UserInfo = await _authorizeViewModel.GetUserInfo() ?? new();

				MenuItem = await _authorizeViewModel.GetMenuItem() ?? new();
				if (MenuItem.Count == 0)
				{
					_Navs.NavigateTo("/signin?p=timeout", true);
				}
				StateHasChanged();

				var remoteIpAddress = _accessor.HttpContext?.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress;
				if (_hubUserConnection is not null)
				{
					_UserKey = Guid.NewGuid().ToString();
					await _hubUserConnection.SendAsync(SignalRUtls.SendUserOnline, new UserOnlineModel() { UserKey = _UserKey, Id = UserInfo.Id, FullName = UserInfo.FullName, Ipaddress = $"{remoteIpAddress}", OnlineDate = DateTime.Now });
				}

				//await Task.Delay(1500);
				var sale = await _salesViewModel.GetOverdueCount(new() { userid = UserInfo.Id });
				if (sale != null)
				{
					UserInfo.OverdueNotify = sale.Data;
				}
			}
			else
			{
				_Navs.NavigateTo("/signin?p=init");
			}
		}

		protected async override Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{

				await _jsRuntimes.InvokeAsync<object>("AfterRenderMainLayout");
				await _jsRuntimes.InvokeVoidAsync("initializeinactivitytimer", DotNetObjectReference.Create(this));
				firstRender = false;
			}
		}

		private void OnAuthenticationStateChanged(Task<AuthenticationState> task)
		{
			InvokeAsync(async () =>
			{
				var authState = await task;
				var user = authState.User;

				isAuthorize = user.Identity != null && user.Identity.IsAuthenticated;
				StateHasChanged();

				if (isAuthorize && !isRedirecting)
				{
					isRedirecting = true;
					_Navs.NavigateTo("/?p=onauth");
				}
				else if (!isAuthorize && !isRedirecting)
				{
					isRedirecting = true;
					_Navs.NavigateTo("/signin?p=onauth");
				}

				StateHasChanged();
			});
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
			_Navs.NavigateTo("/signin?p=timeout", true);
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