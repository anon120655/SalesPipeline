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
using Hangfire;

namespace SalesPipeline.API.Controllers
{
	[Authorizes]
	[ApiVersion(1.0)]
	[ApiController]
	[ServiceFilter(typeof(ValidationFilterAttribute))]
	[Route("v{version:apiVersion}/[controller]")]
	public class NotifyController : ControllerBase
	{
		private readonly IBackgroundJobClient _backgroundJobClient;
		private readonly NotificationService _notiService;
		private IRepositoryWrapper _repo;
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly AppSettings _appSet;

		public NotifyController(IRepositoryWrapper repo, IHttpClientFactory httpClientFactory, IOptions<AppSettings> appSet, IBackgroundJobClient backgroundJobClient, NotificationService notificationService)
		{
			_repo = repo;
			_httpClientFactory = httpClientFactory;
			_appSet = appSet.Value;
			_backgroundJobClient = backgroundJobClient;
			_notiService = notificationService;
		}

		[AllowAnonymous]
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
				var response = await _repo.Notifys.NotiMobile(model);

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

		[AllowAnonymous]
		[HttpPost("ScheduleNotification")]
		public IActionResult ScheduleNotification([FromBody] List<NotificationTestRequest> request)
		{
			var serverTime = DateTime.Now;
			var timeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
			var localTime = TimeZoneInfo.ConvertTime(serverTime, timeZone);

			foreach (var item in request)
			{
				var notifyAt = TimeZoneInfo.ConvertTime(item.NotifyAt, timeZone);
				//_backgroundJobClient.Schedule(() => _repo.Notifys.SendNotification(item.Message), item.NotifyAt);
				_backgroundJobClient.Schedule(() => _notiService.SendNotificationAsync(new()
				{
					to = "dRrz4-ibTta7tGHVg0fpPQ:APA91bGOJ1MskCQVqzNo4BhLruvpzAcT-2MfWLJnCyT4J4CoTHmNCXSczWHeBouI5aEjIac7bUOGLTY1Bu9uqYSFyYiSDawwbJ8S8vriN-NIUOHJo1aVzt1BKzDmdM_Fy3FTdyrW84n8",
					notification = new()
					{
						title = "หัวข้อ01",
						body = "ทดสอบข้อความ body " + DateTime.Now.ToString("t")
					}
				}), notifyAt);
			}

			return Ok(new { Message = "Notification scheduled successfully" });
		}


	}
}
