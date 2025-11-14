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
using System.Globalization;
using Hangfire.Common;
using Hangfire.States;

namespace SalesPipeline.API.Controllers
{
	[ApiVersion(1.0)]
	[ApiController]
	[ServiceFilter(typeof(ValidationFilterAttribute))]
	[Route("v{version:apiVersion}/[controller]")]
	public class NotifyController : ControllerBase
	{
		private readonly IBackgroundJobClient _backgroundJobClient;
		private readonly NotificationService _notiService;
		private IRepositoryWrapper _repo;
		private readonly AppSettings _appSet;

		public NotifyController(IRepositoryWrapper repo, IOptions<AppSettings> appSet, IBackgroundJobClient backgroundJobClient, NotificationService notificationService)
		{
			_repo = repo;
			_appSet = appSet.Value;
			_backgroundJobClient = backgroundJobClient;
			_notiService = notificationService;
		}

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

		[HttpPost("ScheduleNotiTest")]
		public IActionResult ScheduleNotiTest([FromBody] List<NotificationTestRequest> request)
		{
			if (_appSet.NotiMobile != null)
			{
				var timeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

				foreach (var item in request)
				{
					//แปลง UTC เป็น Local เพื่อให้สามารถส่ง format-time เป็น local ได้
					var notiLocaltime = DateTime.SpecifyKind(item.NotifyAt, DateTimeKind.Local);
					var notifyAt = TimeZoneInfo.ConvertTime(notiLocaltime, timeZone).AddMinutes(-_appSet.NotiMobile.NotiBeforeMinutes);

					//var job = Job.FromExpression(() => _notiService.SendNotificationAsync(new()
					//{
					//	to = "dRrz4-ibTta7tGHVg0fpPQ:APA91bGOJ1MskCQVqzNo4BhLruvpzAcT-2MfWLJnCyT4J4CoTHmNCXSczWHeBouI5aEjIac7bUOGLTY1Bu9uqYSFyYiSDawwbJ8S8vriN-NIUOHJo1aVzt1BKzDmdM_Fy3FTdyrW84n8",
					//	notification = new()
					//	{
					//		title = "หัวข้อ01",
					//		body = "ทดสอบข้อความ body " + notifyAt.ToString("t")
					//	}
					//}));
					//var client = new BackgroundJobClient();
					//client.Create(job, new ScheduledState(notifyAt));
					//_backgroundJobClient.Create(job, new ScheduledState(notifyAt));

					GlobalJobFilters.Filters.Add(new JobDisplayNameFilter($"{"ทดสอบ JobDisplay 01"}"));

					_backgroundJobClient.Schedule(() => _notiService.SendNotificationAsync(new()
					{
						to = "cDCOXTkURmKmNCWH6R1fXY:APA91bG2AUfiBzVysJEkjbJthojpX3n8iCV3o_O3LCNXEnbCWPZxRA4-tn9isrd5eLFMlG2O5U1wQnno4DgiZshngB_abF7f2denHj3XbJxv1c9HAkdf_fjFtmL50sg2WQ1m56lDSFJk",
						notification = new()
						{
							title = "หัวข้อ01",
							body = "ทดสอบข้อความ body " + notifyAt.ToString("t")
						}
					}), notifyAt);


				}

				return Ok(new { Message = "Notification scheduled successfully" });
			}
			return Ok(new { Message = "Notification failed" });
		}

		[HttpGet("SetScheduleNoti")]
		public async Task<IActionResult> SetScheduleNoti()
		{
			if (_appSet.NotiMobile != null)
			{
				int countSchedule = 0;

				countSchedule = await _repo.Notifys.SetScheduleNoti();

				return Ok(new { Message = $"Notification scheduled successfully {countSchedule}" });
			}
			return Ok(new { Message = "Notification failed" });
		}


	}
}
