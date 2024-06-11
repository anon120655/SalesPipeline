using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using SalesPipeline.Infrastructure.Helpers;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils;
using Microsoft.AspNetCore.Authorization;
using SalesPipeline.Utils.ValidationModel;
using SalesPipeline.Utils.Resources.Customers;
using SalesPipeline.Utils.Resources.Notifications;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SalesPipeline.Utils.Resources.Authorizes.Auths;
using System.Text;
using System.Net.Http.Headers;
using SalesPipeline.Utils.Resources.Loans;

namespace SalesPipeline.API.Controllers
{
	[Authorizes]
	[ApiVersion(1.0)]
	[ApiController]
	[ServiceFilter(typeof(ValidationFilterAttribute))]
	[Route("v{version:apiVersion}/[controller]")]
	public class NotifyController : ControllerBase
	{
		private IRepositoryWrapper _repo;
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly AppSettings _appSet;

		public NotifyController(IRepositoryWrapper repo, IHttpClientFactory httpClientFactory, IOptions<AppSettings> appSet)
		{
			_repo = repo;
			_httpClientFactory = httpClientFactory;
			_appSet = appSet.Value;
		}

		//[AllowAnonymous]
		[HttpGet("LineNotify")]
		public async Task<IActionResult> LineNotify([FromQuery] string msg)
		{
			try
			{
				await _repo.Notifys.LineNotify(msg);
				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		/// <summary>
		/// ข้อมูลแจ้งเตือนทั้งหมด
		/// </summary>
		[AllowAnonymous]
		[HttpGet("GetList")]
		public async Task<IActionResult> GetList([FromQuery] NotiFilter model)
		{
			try
			{
				var response = await _repo.Notifys.GetList(model);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		/// <summary>
		/// แก้ไขสถานะการอ่านแจ้งเตือน
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpPut("UpdateRead")]
		public async Task<IActionResult> UpdateRead(List<Guid> model)
		{
			try
			{
				await _repo.Notifys.UpdateRead(model);
				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[AllowAnonymous]
		[HttpPost("NotiMobile")]
		public async Task<IActionResult> NotiMobile(NotificationMobile model)
		{
			try
			{
				var response = new NotificationMobileResponse();
				if (_appSet.NotiMobile != null)
				{
					var httpClient = new HttpClient(new HttpClientHandler()
					{
						ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
					});

					var jsonTxt = JsonConvert.SerializeObject(model);
					var postData = new StringContent(
						jsonTxt, // แปลงข้อมูลเป็น JSON ก่อน
						Encoding.UTF8,
						"application/json"
					);

					//ใช้ key แล้ว error
					//httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("key", _appSet.NotiMobile.ApiKey);
					httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _appSet.NotiMobile.ApiKey);

					HttpResponseMessage responseAPI = await httpClient.PostAsync($"{_appSet.NotiMobile.baseUri}/fcm/send", postData);
					if (responseAPI.IsSuccessStatusCode)
					{
						string responseBody = await responseAPI.Content.ReadAsStringAsync();
						response = JsonConvert.DeserializeObject<NotificationMobileResponse>(responseBody);
					}
					else
					{
						throw new ExceptionCustom("Noti Error.");
					}
				}

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[AllowAnonymous]
		[HttpGet("GetUserSendNotiById")]
		public async Task<IActionResult> GetUserSendNotiById([FromQuery] int userid)
		{
			try
			{
				var data = await _repo.Notifys.GetUserSendNotiById(userid);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}


	}
}
