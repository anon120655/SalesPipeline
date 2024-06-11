using AutoMapper;
using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NetTopologySuite.Index.HPRtree;
using Newtonsoft.Json;
using SalesPipeline.Infrastructure.Helpers;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.PropertiesModel;
using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Notifications;
using SalesPipeline.Utils.Resources.PreApprove;
using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
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

		public Notifys(IRepositoryWrapper repo, IRepositoryBase db, IOptions<AppSettings> appSet, IMapper mapper)
		{
			_db = db;
			_repo = repo;
			_mapper = mapper;
			_appSet = appSet.Value;
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

			_body = $"{fromUserName} {model.ActionName1} {model.ActionName2}";

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
						}
					});
				}
			}

			return _mapper.Map<NotificationCustom>(notification);
		}

		public async Task<PaginationView<List<NotificationCustom>>> GetList(NotiFilter model)
		{
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
				if (_appSet.NotiMobile != null)
				{
					var httpClient = new HttpClient(new HttpClientHandler()
					{
						ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
					});

					if (model.notification != null)
					{
						model.notification.vibrate = 1;
						model.notification.badge = "1";
						model.notification.contentavailable = 1;
						model.notification.forcestart = 1;
						model.notification.nocache = 1;
					}

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
	}
}
