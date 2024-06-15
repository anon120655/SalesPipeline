using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesPipeline.Utils.Resources.Authorizes.Auths;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils.Resources.iAuthen;
using Asp.Versioning;
using SalesPipeline.Infrastructure.Helpers;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils.ValidationModel;
using Newtonsoft.Json;
using System.Text;
using SalesPipeline.Utils;
using Microsoft.Extensions.Options;

namespace SalesPipeline.API.Controllers
{
	[ApiVersion(1.0)]
	[ApiController]
	[ServiceFilter(typeof(ValidationFilterAttribute))]
	[Route("v{version:apiVersion}/[controller]")]
	public class AuthorizeController : ControllerBase
	{
		private IRepositoryWrapper _repo;
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly AppSettings _appSet;

		public AuthorizeController(IRepositoryWrapper repo, IHttpClientFactory httpClientFactory, IOptions<AppSettings> appSet)
		{
			_repo = repo;
			_httpClientFactory = httpClientFactory;
			_appSet = appSet.Value;
		}

		[AllowAnonymous]
		[HttpPost]
		public async Task<IActionResult> Authenticate(AuthenticateRequest model)
		{
			try
			{
				if (_appSet.iAuthen != null && _appSet.iAuthen.IsConnect)
				{
					var iAuthenRequest = new iAuthenRequest()
					{
						user = "6100004",
						password = "SmpAMTIzNDU2Nzg5",
						faceID = "",
						requester_id = "R00001",
						reference_id = "3",
						ipaddress = "172.25.25.2",
						authen_type = 4,
					};

					var httpClient = new HttpClient(new HttpClientHandler()
					{
						ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
					});

					var postData = new StringContent(
						JsonConvert.SerializeObject(iAuthenRequest), // แปลงข้อมูลเป็น JSON ก่อน
						Encoding.UTF8,
						"application/json"
					);
					httpClient.DefaultRequestHeaders.Add("Api-Key", _appSet.iAuthen.ApiKey);

					HttpResponseMessage responseAPI = await httpClient.PostAsync($"{_appSet.iAuthen.baseUri}/authen/authentication", postData);
					if (responseAPI.IsSuccessStatusCode)
					{
						string responseBody = await responseAPI.Content.ReadAsStringAsync();
						if (responseBody != null)
						{

						}
					}
					else
					{
						string responseBody = await responseAPI.Content.ReadAsStringAsync();
						if (responseBody != null)
						{

						}
					}
				}

				var response = await _repo.Authorizes.Authenticate(model);

				if (_appSet.ServerSite == ServerSites.DEV)
				{
					model.tokenNoti = "cn2akTlACf2yrfsPdrUGxj:APA91bHzBDCeLPRa7TfjYF6TYcZTlFOQwNbBrI_9qPlXhVyBySt-7ZU_yQONDHRCaM5rjrasTMyJUBGmNyP0XQlIyMz_hzEO6zMdrvhU9NG9TKyfWNbE7gSNu-GE7eFrrbZv0KrZJP4E";
				}

				if (response != null)
				{
					await _repo.User.LogLogin(new()
					{
						UserId = response.Id,
						IPAddress = model.IPAddress,
						DeviceId = model.DeviceId,
						DeviceVersion = model.DeviceVersion,
                        SystemVersion = model.SystemVersion,
						AppVersion = model.AppVersion,
						tokenNoti = model.tokenNoti
					});
				}

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[AllowAnonymous]
		[HttpGet("ExpireToken")]
		public IActionResult ExpireToken([FromQuery] string token)
		{
			try
			{
				var response = _repo.Authorizes.ExpireToken(token);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}


	}
}
