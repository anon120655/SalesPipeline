using AutoMapper;
using Azure;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NetTopologySuite.Index.HPRtree;
using Newtonsoft.Json;
using SalesPipeline.Infrastructure.Helpers;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.ConstTypeModel;
using SalesPipeline.Utils.PropertiesModel;
using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Notifications;
using SalesPipeline.Utils.Resources.PreApprove;
using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Repositorys
{
	public class Notifys : INotifys
	{
		private IRepositoryWrapper _repo;
		private readonly IMapper _mapper;
		private readonly IRepositoryBase _db;
		private readonly AppSettings _appSet;
		private readonly NotificationService _notiService;
		private readonly IBackgroundJobClient _backgroundJobClient;

		public Notifys(IRepositoryWrapper repo, IRepositoryBase db, IOptions<AppSettings> appSet, IMapper mapper, NotificationService notiService, IBackgroundJobClient backgroundJobClient)
		{
			_db = db;
			_repo = repo;
			_mapper = mapper;
			_appSet = appSet.Value;
			_notiService = notiService;
			_backgroundJobClient = backgroundJobClient;
		}

		public async Task LineNotify(string msg)
		{
			try
			{
				using (var client = new HttpClient())
				{
					var request = new HttpRequestMessage(HttpMethod.Post, _appSet.LineNotify?.baseUri);
					request.Headers.Add("Authorization", $"Bearer {_appSet.LineNotify?.Token}");
					var collection = new List<KeyValuePair<string, string>>();
					collection.Add(new("message", msg));
					var content = new FormUrlEncodedContent(collection);
					request.Content = content;
					var response = await client.SendAsync(request);
					//response.EnsureSuccessStatusCode();
					//Console.WriteLine(await response.Content.ReadAsStringAsync());
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}

		public async Task<NotificationCustom> Create(NotificationCustom model)
		{
			DateTime _dateNow = DateTime.Now;

			var fromUserName = await _repo.User.GetFullNameById(model.FromUserId);
			var toUserName = await _repo.User.GetFullNameById(model.ToUserId);

			var eventName = PropertiesMain.PerNotiEventName(model.EventId.ToString())?.Name ?? string.Empty;

			var notification = new Data.Entity.Notification()
			{
				Status = StatusModel.Active,
				CreateDate = _dateNow,
				SaleId = model.SaleId,
				EventId = model.EventId,
				EventName = eventName,
				FromUserId = model.FromUserId,
				FromUserName = fromUserName,
				ToUserId = model.ToUserId,
				ToUserName = toUserName,
				IsRead = 0,
				RedirectUrl = model.RedirectUrl,
				ActionName1 = model.ActionName1,
				ActionName2 = model.ActionName2,
				ActionName3 = model.ActionName3,
			};
			await _db.InsterAsync(notification);
			await _db.SaveAsync();

			string _body = string.Empty;

			_body = $"{fromUserName} {eventName} {model.ActionName1} {model.ActionName2}";

			var userSendNoti = await GetUserSendNotiById(model.ToUserId);
			if (userSendNoti != null && userSendNoti.Count > 0)
			{
				foreach (var item in userSendNoti)
				{
					await NotiMobile(new()
					{
						to = item.tokenNoti,
						priority = "high",
						notification = new()
						{
							title = eventName,
							body = _body
						},
						data = new()
						{
							SaleId = model.SaleId,
							EventId = model.EventId,
							EventName = eventName,
							FromUserId = model.FromUserId,
							FromUserName = fromUserName,
							ToUserId = model.ToUserId,
							ToUserName = toUserName,
							ActionName1 = model.ActionName1,
							ActionName2 = model.ActionName2
						}
					});
				}
			}

			return _mapper.Map<NotificationCustom>(notification);
		}

		public async Task<PaginationView<List<NotificationCustom>>> GetList(NotiFilter model)
		{

			// ดึงข้อมูลทั้งหมดจากตาราง Users ทีละรายการ
			//var usersNotifications = _repo.Context.Notifications.AsEnumerable().ToList();

			var query = _repo.Context.Notifications
												 .Where(x => x.Status != StatusModel.Delete)
												 .OrderByDescending(x => x.CreateDate)
												 .AsQueryable();
			if (model.touserid.HasValue)
			{
				query = query.Where(x => x.ToUserId == model.touserid.Value);
			}

			if (model.eventid.HasValue)
			{
				query = query.Where(x => x.EventId == model.eventid.Value);
			}

			if (model.isread.HasValue)
			{
				query = query.Where(x => x.IsRead == model.isread.Value);
			}

			var pager = new Pager(query.Count(), model.page, model.pagesize, null);

			var items = query.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

			return new PaginationView<List<NotificationCustom>>()
			{
				Items = _mapper.Map<List<NotificationCustom>>(await items.ToListAsync()),
				Pager = pager
			};
		}

		public async Task UpdateRead(List<Guid> model)
		{
			using (var _transaction = _repo.BeginTransaction())
			{
				var notifications = _repo.Context.Notifications.Where(x => model.Contains(x.Id)).ToList();
				if (notifications.Count > 0)
				{
					foreach (var item in notifications)
					{
						item.IsRead = 1;
						item.ReadDate = DateTime.Now;
					}
					await _db.SaveAsync();
				}

				_transaction.Commit();
			}
		}

		public async Task<List<User_Login_TokenNotiCustom>> GetUserSendNotiById(int userid)
		{
			var query = await _repo.Context.User_Login_TokenNotis.Where(x => x.UserId == userid).ToListAsync();

			return _mapper.Map<List<User_Login_TokenNotiCustom>>(query);
		}

		public async Task<NotificationMobileResponse?> NotiMobile(NotificationMobile model)
		{
			var response = new NotificationMobileResponse();
			try
			{
				response = await _notiService.SendNotificationAsync(model);

				//Test
				//await _notiService.SendNotificationAsync(new()
				//{
				//	to = "dRrz4-ibTta7tGHVg0fpPQ:APA91bGOJ1MskCQVqzNo4BhLruvpzAcT-2MfWLJnCyT4J4CoTHmNCXSczWHeBouI5aEjIac7bUOGLTY1Bu9uqYSFyYiSDawwbJ8S8vriN-NIUOHJo1aVzt1BKzDmdM_Fy3FTdyrW84n8",
				//	notification = new()
				//	{
				//		title = "หัวข้อ01",
				//		body = "ทดสอบข้อความ body " + DateTime.Now.ToString("dd/MM/yy")
				//	}
				//});

				return response;
			}
			catch (Exception ex)
			{
				return response;
			}
		}

		public void SendNotification(string message)
		{
			// Logic to send notification (e.g., email, SMS, push notification)
			Console.WriteLine($"Notification: {message}");
		}

		public async Task<int> SetScheduleNoti()
		{
			int countSchedule = 0;
			if (_appSet.NotiMobile != null)
			{
				var calendarList = await _repo.ProcessSale.GetListCalendar(new() { isScheduledJob = 0 });

				if (calendarList.Count > 0)
				{
					var timeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

					foreach (var item in calendarList)
					{
						if (item.AppointmentDate.HasValue && item.AppointmentTime.HasValue && item.AssUserId.HasValue)
						{
							var userSendNoti = await _repo.Notifys.GetUserSendNotiById(item.AssUserId.Value);
							if (userSendNoti != null && userSendNoti.Count > 0)
							{
								foreach (var item_user in userSendNoti)
								{
									string dateString = $"{item.AppointmentDate.Value.ToString("dd-MM-yyyy")} {item.AppointmentTime.Value.ToString("HH:mm")}";
									bool success = DateTime.TryParseExact(dateString, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime NotifyAt);
									if (success)
									{
										//แปลง UTC เป็น Local เพื่อให้สามารถส่ง format-time เป็น local ได้
										var notiLocaltime = DateTime.SpecifyKind(NotifyAt, DateTimeKind.Local);
										var notifyAt = TimeZoneInfo.ConvertTime(notiLocaltime, timeZone).AddMinutes(-_appSet.NotiMobile.NotiBeforeMinutes);

										string? meetContent = item.MeetFullName != null ? item.MeetFullName : item.ContactFullName;

										GlobalJobFilters.Filters.Add(new JobDisplayNameFilter($"{item.NextActionName}"));

										_backgroundJobClient.Schedule(() => _notiService.SendNotificationAsync(new()
										{
											//to = "dRrz4-ibTta7tGHVg0fpPQ:APA91bGOJ1MskCQVqzNo4BhLruvpzAcT-2MfWLJnCyT4J4CoTHmNCXSczWHeBouI5aEjIac7bUOGLTY1Bu9uqYSFyYiSDawwbJ8S8vriN-NIUOHJo1aVzt1BKzDmdM_Fy3FTdyrW84n8",
											to = item_user.tokenNoti,
											notification = new()
											{
												title = $"{item.NextActionName}",
												body = $"{item.NextActionName} {meetContent}"
											},
											data = new()
											{
												EventId = NotifyEventIdModel.Calendar,
												EventName = "นัดหมาย"
											}
										}), notifyAt);
										countSchedule++;
										await _repo.ProcessSale.UpdateScheduledJob(item.Id);
									}
								}
							}


						}
					}
				}

			}
			return countSchedule;
		}

	}
}
