using Asp.Versioning;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SalesPipeline.Infrastructure.Helpers;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Authorizes.Auths;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.iAuthen;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils.ValidationModel;
using System.Net.NetworkInformation;
using System.Net.Security;
using System.Text;

namespace SalesPipeline.API.Controllers
{
    [ApiVersion(1.0)]
    [ApiController]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [Route("v{version:apiVersion}/[controller]")]
    public class AuthorizeController : ControllerBase
    {
        private readonly IRepositoryWrapper _repo;
        private readonly AppSettings _appSet;

        public AuthorizeController(IRepositoryWrapper repo, IOptions<AppSettings> appSet, DatabaseBackupService databaseBackup)
        {
            _repo = repo;
            _appSet = appSet.Value;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Authenticate(AuthenticateRequest model)
        {
            try
            {
                AuthenticateResponse? response = null;
                iAuthenResponse? iAuthenData = null;

                if (_appSet.iAuthen != null && _appSet.iAuthen.IsConnect && GeneralUtils.IsDigit(model.Username))
                {
                    if (_appSet.ServerSite == ServerSites.DEV)
                    {
                        bool isVpnConnect = false;
                        // รับรายการการเชื่อมต่อ VPN ที่ใช้งานอยู่
                        var adapters = NetworkInterface.GetAllNetworkInterfaces();
                        if (adapters.Length > 0)
                        {
                            foreach (NetworkInterface adapter in adapters)
                            {
                                if ((adapter.Description.Contains("Array Networks VPN Adapter") || adapter.Name.Contains("_Common_all-network - green.baac.or.th"))
                                    && adapter.OperationalStatus == OperationalStatus.Up)
                                {
                                    isVpnConnect = true;
                                }
                            }
                        }

                        if (!isVpnConnect) throw new ExceptionCustom($"เชื่อมต่อ iAuthen ไม่สำเร็จ เนื่องจากไม่ได้ต่อ VPN");
                    }
                    string base64password = Convert.ToBase64String(Encoding.UTF8.GetBytes(model.Password ?? ""));

                    var iAuthenRequest = new iAuthenRequest()
                    {
                        user = model.Username,
                        password = base64password,
                        requester_id = _appSet.iAuthen.Requester_ID,
                        reference_id = _appSet.iAuthen.Reference_ID,
                        ipaddress = _appSet.iAuthen.IPAddress,
                        authen_type = 4,
                    };


                    var handler = new HttpClientHandler();

                    if (_appSet.ServerSite == ServerSites.DEV || _appSet.ServerSite == ServerSites.UAT)
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

                    var postData = new StringContent(
                        JsonConvert.SerializeObject(iAuthenRequest), // แปลงข้อมูลเป็น JSON ก่อน
                        Encoding.UTF8,
                        "application/json"
                    );
                    httpClient.DefaultRequestHeaders.Add("apikey", _appSet.iAuthen.ApiKey);

                    HttpResponseMessage responseAPI = await httpClient.PostAsync($"{_appSet.iAuthen.baseUri}/authen/authentication", postData);
                    if (responseAPI.IsSuccessStatusCode)
                    {
                        string responseBody = await responseAPI.Content.ReadAsStringAsync();

                        iAuthenData = JsonConvert.DeserializeObject<iAuthenResponse>(responseBody);
                        if (iAuthenData == null || iAuthenData.response_status != "pass" || iAuthenData.response_data == null)
                            throw new ExceptionCustom($"เชื่อมต่อ iAuthen ไม่สำเร็จ กรุณาติดต่อผู้ดูแลระบบ");

                        iAuthenData.response_data.Username = model.Username;
                        response = await _repo.Authorizes.AuthenticateBAAC(model, iAuthenData.response_data);
                    }
                    else
                    {
                        string responseBody = await responseAPI.Content.ReadAsStringAsync();
                        iAuthenData = JsonConvert.DeserializeObject<iAuthenResponse>(responseBody);
                    }

                    if (iAuthenData != null)
                    {
                        if (response == null) response = new AuthenticateResponse(new(), string.Empty, string.Empty, string.Empty);
                        response.iauthen = iAuthenData;
                    }
                }
                else
                {
                    response = await _repo.Authorizes.Authenticate(model);
                }

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
        [HttpGet("RefreshJwtToken")]
        public async Task<IActionResult> RefreshJwtToken([FromQuery] string refreshToken)
        {
            try
            {
                var response = await _repo.Authorizes.RefreshJwtToken(refreshToken);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return new ErrorResultCustom(new ErrorCustom(), ex);
            }
        }

        [AllowAnonymous]
        [HttpGet("ExpireToken")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorCustom))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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


        [AllowAnonymous]
        [HttpPost("RemoveNotiToken")]
        public async Task<IActionResult> RemoveNotiToken(User_Login_LogCustom model)
        {
            try
            {
                await _repo.Authorizes.RemoveNotiToken(model);

                return Ok();
            }
            catch (Exception ex)
            {
                return new ErrorResultCustom(new ErrorCustom(), ex);
            }
        }

    }
}
