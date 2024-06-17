using Newtonsoft.Json;
using SalesPipeline.Utils.Resources.Notifications;
using SalesPipeline.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Hangfire;
using Hangfire.Common;
using Hangfire.States;

namespace SalesPipeline.Infrastructure.Helpers
{
	public class NotificationService
	{
		private readonly HttpClient _httpClient;
		private readonly AppSettings _appSet;

		public NotificationService(HttpClient httpClient, IOptions<AppSettings> appSet)
		{
			_httpClient = httpClient;
			_appSet = appSet.Value;
		}

		[JobDisplayName("SendNoti Mobile")]
		public async Task<NotificationMobileResponse?> SendNotificationAsync(NotificationMobile model)
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

					model.priority = "high";
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
				return null;
			}
		}
	}
}
