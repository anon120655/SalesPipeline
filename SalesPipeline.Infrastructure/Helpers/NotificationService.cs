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
using Google.Apis.Auth.OAuth2;

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

		[JobDisplayName("SendNoti Mobile_Backup")]
		public async Task<NotificationMobileResponse?> SendNotificationAsync_Backup(NotificationMobile model)
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
						jsonTxt,
						Encoding.UTF8,
						"application/json"
					);

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

		[JobDisplayName("SendNoti Mobile")]
		public async Task<NotificationMobileResponse?> SendNotificationAsync(NotificationMobile model)
		{
			var response = new NotificationMobileResponse();
			try
			{
				if (_appSet.NotiMobile != null)
				{
					// Step 1: Read Service Account JSON and generate Access Token
					string[] scopes = { "https://www.googleapis.com/auth/firebase.messaging" };
					GoogleCredential credential;

					string fullPathService = $@"{_appSet.NotiMobile.PathServiceJson}\baac-rm-sales-firebase-adminsdk-tyaeu-731e511603.json";
					using (var stream = new FileStream(fullPathService, FileMode.Open, FileAccess.Read))
					{
						credential = GoogleCredential.FromStream(stream).CreateScoped(scopes);
					}

					var token = await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();

					var httpClient = new HttpClient(new HttpClientHandler()
					{
						ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
					});

					// Step 2: Set the notification priority and properties
					model.priority = "high";
					if (model.notification != null)
					{
						model.notification.vibrate = 1;
						model.notification.badge = "1";
						model.notification.contentavailable = 1;
						model.notification.forcestart = 1;
						model.notification.nocache = 1;
					}

					var jsonTxt_Old = JsonConvert.SerializeObject(model);

					//true false
					if (_appSet.ServerSite == ServerSites.DEV && false)
					{
						model.to = "9";
					}

					var message = new NotificationMobileNew()
					{
						message = new()
						{
							token = model.to,
							notification = new()
							{
								title = model.notification?.title,
								body = model.notification?.body
							},
							android = new()
							{
								priority = "high"
							},
							apns = new()
							{
								headers = new()
								{
									apnspriority = "10"
								}
							}
						}
					};
					//var message = new NotificationMobileNew()
					//{
					//	message = new()
					//	{
					//		token = "ffB93iaISyyvrDUOGDAljd:APA91bGQ_Qb7osi4tfFfLpBaH30P2wh80heroHDHVl9MVdrZTefAo51-PBpCHZl9D1Y5cURrzNBN_Ib9O-JRcXQmjyKqgSQUsnAEkvXWjy_iANEfu22SdyT55TQ4SwIwVc3kgOdvYCxe",
					//		notification = new()
					//		{
					//			title = "ทดสอบ noti new",
					//			body = "เนื้อหา noti new "
					//		},
					//		android = new()
					//		{
					//			priority = "high"
					//		},
					//		apns = new()
					//		{
					//			headers = new()
					//			{
					//				apnspriority = "10"
					//			}
					//		}
					//	}
					//};

					// Step 3: Serialize the model to JSON
					var jsonTxt = JsonConvert.SerializeObject(message);
					var postData = new StringContent(
						jsonTxt,
						Encoding.UTF8,
						"application/json"
					);

					// Step 4: Set the Authorization header with the generated Access Token
					httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

					// Step 5: Send the notification using the new API URL
					HttpResponseMessage responseAPI = await httpClient.PostAsync($"{_appSet.NotiMobile.baseUri}/v1/projects/{_appSet.NotiMobile.ProjectId}/messages:send", postData);
					if (responseAPI.IsSuccessStatusCode)
					{
						string responseBody = await responseAPI.Content.ReadAsStringAsync();
						response = JsonConvert.DeserializeObject<NotificationMobileResponse>(responseBody);
					}
					else
					{
						string responseBody = await responseAPI.Content.ReadAsStringAsync();
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
