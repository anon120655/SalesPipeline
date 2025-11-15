using Google.Apis.Auth.OAuth2;
using Hangfire;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Notifications;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Text;

namespace SalesPipeline.Infrastructure.Helpers
{
	public class NotificationService
	{
		private readonly AppSettings _appSet;
        private readonly bool isDevOrUat = false;

        public NotificationService(IOptions<AppSettings> appSet)
		{
			_appSet = appSet.Value;
            isDevOrUat = _appSet.ServerSite == ServerSites.DEV || _appSet.ServerSite == ServerSites.UAT;
        }

		[JobDisplayName("SendNoti Mobile_Backup")]
		public async Task<NotificationMobileResponse?> SendNotificationAsync_Backup(NotificationMobile model)
		{
			var response = new NotificationMobileResponse();
			try
			{
				if (_appSet.NotiMobile != null)
				{
                    var handler = new HttpClientHandler();
                    if (isDevOrUat)
                    {
                        handler.ServerCertificateCustomValidationCallback =
                        (message, cert, chain, errors) =>
                        {
                            // ตรวจสอบเฉพาะ error ที่ยอมรับได้
                            if (errors == SslPolicyErrors.None)
                                return true;

                            // ยอมรับเฉพาะ self-signed cert ใน DEV/UAT
                            return errors == SslPolicyErrors.RemoteCertificateChainErrors;
                        };
                    }

                    var httpClient = new HttpClient(handler);

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
					string[] scopes = { "https://www.googleapis.com/auth/firebase.messaging" };
					GoogleCredential credential;

					string fullPathService = $@"{_appSet.NotiMobile.PathServiceJson}\baac-rm-sales-firebase-adminsdk-tyaeu-731e511603.json";
					using (var stream = new FileStream(fullPathService, FileMode.Open, FileAccess.Read))
					{
						credential = GoogleCredential.FromStream(stream).CreateScoped(scopes);
					}

					var token = await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();

                    var handler = new HttpClientHandler();
                    if (isDevOrUat)
                    {
                        handler.ServerCertificateCustomValidationCallback =
                        (message, cert, chain, errors) =>
                        {
                            // ตรวจสอบเฉพาะ error ที่ยอมรับได้
                            if (errors == SslPolicyErrors.None)
                                return true;

                            // ยอมรับเฉพาะ self-signed cert ใน DEV/UAT
                            return errors == SslPolicyErrors.RemoteCertificateChainErrors;
                        };
                    }
                    var httpClient = new HttpClient(handler);

					model.priority = "high";
					if (model.notification != null)
					{
						model.notification.vibrate = 1;
						model.notification.badge = "1";
						model.notification.contentavailable = 1;
						model.notification.forcestart = 1;
						model.notification.nocache = 1;
					}

					if (_appSet.ServerSite == ServerSites.DEV)
					{
						model.to = "cH6wn-FaQfue-jdbWXZL04:APA91bHSVWxWqKAxBlDJZvfRVgJ7rShpGHlDQCTdKeCC8hUoQ_DlwTMb-VJIwPxqoLm671lh-BuHNRn2V7z5tGth3LTXOYUptk1OiGqNT7-aHy9dH4CUyvAAoD_Ewrt4MvUWauFYRlwU";
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
					var jsonTxt = JsonConvert.SerializeObject(message);
					var postData = new StringContent(
						jsonTxt,
						Encoding.UTF8,
						"application/json"
					);

					httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

					HttpResponseMessage responseAPI = await httpClient.PostAsync($"{_appSet.NotiMobile.baseUri}/v1/projects/{_appSet.NotiMobile.ProjectId}/messages:send", postData);
					if (responseAPI.IsSuccessStatusCode)
					{
						string responseBody = await responseAPI.Content.ReadAsStringAsync();
						response = JsonConvert.DeserializeObject<NotificationMobileResponse>(responseBody);
					}
					else
					{
						await responseAPI.Content.ReadAsStringAsync();
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
