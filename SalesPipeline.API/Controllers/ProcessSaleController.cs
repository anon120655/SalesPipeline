using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Helpers;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.ConstTypeModel;
using SalesPipeline.Utils.DataCustom;
using SalesPipeline.Utils.Resources.Phoenixs;
using SalesPipeline.Utils.Resources.ProcessSales;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils.ValidationModel;
using System.Net.NetworkInformation;
using System.Net.Security;
using System.Text.RegularExpressions;

namespace SalesPipeline.API.Controllers
{
    [Authorizes]
    [ApiVersion(1.0)]
    [ApiController]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [Route("v{version:apiVersion}/[controller]")]
    public class ProcessSaleController : ControllerBase
    {
        private readonly IRepositoryWrapper _repo;
        private readonly AppSettings _appSet;
        private readonly bool isDevOrUat = false;

        public ProcessSaleController(IRepositoryWrapper repo, IOptions<AppSettings> appSet)
        {
            _repo = repo;
            _appSet = appSet.Value;
            isDevOrUat = _appSet.ServerSite == ServerSites.DEV || _appSet.ServerSite == ServerSites.UAT;
        }

        /// <summary>
        /// ข้อมูลฟอร์มกระบวนการขาย ById
        /// </summary>
        /// 
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            try
            {
                var data = await _repo.ProcessSale.GetById(id);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return new ErrorResultCustom(new ErrorCustom(), ex);
            }
        }

        /// <summary>
        /// แก้ไขฟอร์มกระบวนการขาย
        /// </summary>
        [HttpPut("Update")]
        public async Task<IActionResult> Update(ProcessSaleCustom model)
        {
            try
            {
                var data = await _repo.ProcessSale.Update(model);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return new ErrorResultCustom(new ErrorCustom(), ex);
            }
        }

        /// <summary>
        /// ข้อมูลฟอร์มกระบวนการขายทั้งหมด
        /// </summary>
        [HttpGet("GetList")]
        public async Task<IActionResult> GetList([FromQuery] allFilter model)
        {
            try
            {
                var response = await _repo.ProcessSale.GetList(model);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return new ErrorResultCustom(new ErrorCustom(), ex);
            }
        }

        /// <summary>
        /// เพิ่มกระบวนการขาย
        /// </summary>
        [HttpPost("CreateReply")]
        public async Task<IActionResult> CreateReply(Sale_ReplyCustom model)
        {
            try
            {
                var data = await _repo.ProcessSale.CreateReply(model);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return new ErrorResultCustom(new ErrorCustom(), ex);
            }
        }

        /// <summary>
        /// แก้ไขกระบวนการขาย
        /// </summary>
        [HttpPut("UpdateReply")]
        public async Task<IActionResult> UpdateReply(Sale_ReplyCustom model)
        {
            try
            {
                var data = await _repo.ProcessSale.UpdateReply(model);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return new ErrorResultCustom(new ErrorCustom(), ex);
            }
        }

        /// <summary>
        /// ข้อมูลกระบวนการขาย ById
        /// </summary>
        [AllowAnonymous]
        [HttpGet("GetReplyById")]
        public async Task<IActionResult> GetReplyById([FromQuery] Guid id)
        {
            try
            {
                var data = await _repo.ProcessSale.GetReplyById(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return new ErrorResultCustom(new ErrorCustom(), ex);
            }
        }

        /// <summary>
        /// ข้อมูลกระบวนการขายทั้งหมด
        /// </summary>
        [HttpGet("GetListReply")]
        public async Task<IActionResult> GetListReply([FromQuery] allFilter model)
        {
            try
            {
                var response = await _repo.ProcessSale.GetListReply(model);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return new ErrorResultCustom(new ErrorCustom(), ex);
            }
        }

        /// <summary>
        /// ข้อมูลเอกสารทั้งหมด
        /// </summary>
        [HttpGet("GetListDocument")]
        public async Task<IActionResult> GetListDocument([FromQuery] allFilter model)
        {
            try
            {
                var response = await _repo.ProcessSale.GetListDocument(model);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return new ErrorResultCustom(new ErrorCustom(), ex);
            }
        }

        /// <summary>
        /// ข้อมูลประวัติการติดต่อทั้งหมด
        /// </summary>
        [AllowAnonymous]
        [HttpGet("GetListContactHistory")]
        public async Task<IActionResult> GetListContactHistory([FromQuery] allFilter model)
        {
            try
            {
                var response = await _repo.ProcessSale.GetListContactHistory(model);

                short iSCloseSale = 0;
                short iSPhoenix = 0;
                if (model.id != Guid.Empty)
                {
                    var sale = await _repo.Sales.GetStatusById(model.id);
                    if (sale == null) throw new ExceptionCustom("saleid not found.");

                    if (sale.StatusSaleId == StatusSaleModel.WaitCloseSale)
                    {
                        iSCloseSale = 1;
                    }
                    if (sale.StatusSaleId > StatusSaleModel.WaitAPIPHOENIX)
                    {
                        iSPhoenix = 1;
                    }
                }

                return Ok(new ContactHistoryMain()
                {
                    History = response,
                    ISCloseSale = iSCloseSale,
                    ISPhoenix = iSPhoenix
                });
            }
            catch (Exception ex)
            {
                return new ErrorResultCustom(new ErrorCustom(), ex);
            }
        }

        /// <summary>
        /// ข้อมูลปฏิทิน
        /// </summary>
        [HttpPost("GetListCalendar")]
        public async Task<IActionResult> GetListCalendar(allFilter model)
        {
            try
            {
                var response = await _repo.ProcessSale.GetListCalendar(model);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return new ErrorResultCustom(new ErrorCustom(), ex);
            }
        }

        /// <summary>
        /// ทิ้งรายการลูกค้า
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("CreateContactDiscard")]
        public async Task<IActionResult> CreateContactDiscard(Sale_ContactCustom model)
        {
            try
            {
                using (var _transaction = _repo.BeginTransaction())
                {
                    var data = await _repo.ProcessSale.CreateContactDiscard(model);

                    await _transaction.CommitAsync();

                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                return new ErrorResultCustom(new ErrorCustom(), ex);
            }
        }

        [HttpGet("CallApiPhoenixByCIF")]
        [AllowAnonymous]
        public async Task<IActionResult> CallApiPhoenixByCIF([FromQuery] string? cif)
        {
            try
            {
                List<Sale_PhoenixCustom>? phoenixModel = null;

                // 1. ตรวจสอบ cif ว่าถูกต้องตามรูปแบบ
                if (string.IsNullOrWhiteSpace(cif))
                {
                    throw new ExceptionCustom("CIF ไม่ถูกต้องหรือรูปแบบไม่ถูกต้อง");
                }

                if (_appSet.Phoenix != null && _appSet.Phoenix.IsConnect &&
                    MoreDataModel.Phoenixs()?.Select(x => x.cif_no).Contains(cif) == false)
                {
                    // ตรวจสอบ VPN (DEV)
                    if (_appSet.ServerSite == ServerSites.DEV)
                    {
                        bool isVpnConnect = NetworkInterface.GetAllNetworkInterfaces()
                            .Any(adapter =>
                                adapter.Description.Contains("Array Networks VPN Adapter") &&
                                adapter.OperationalStatus == OperationalStatus.Up);

                        if (!isVpnConnect)
                            throw new ExceptionCustom($"เชื่อมต่อ Phoenix ไม่สำเร็จ เนื่องจากไม่ได้ต่อ VPN");
                    }

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

                    using var _httpClient = new HttpClient(handler);

                    // 2. สร้าง URL อย่างปลอดภัย: ใช้ UriBuilder + Path.Combine
                    var uriBuilder = new UriBuilder(_appSet.Phoenix.baseUri)
                    {
                        Path = Path.Combine("phoenixbybaac", "hisbycif", cif.Trim()).Replace("\\", "/")
                    };

                    string fullUrl = uriBuilder.ToString();

                    var request = new HttpRequestMessage(HttpMethod.Get, fullUrl);
                    request.Headers.Add("apikey", _appSet.Phoenix.ApiKey);

                    var response = await _httpClient.SendAsync(request);
                    var data = await response.Content.ReadFromJsonAsync<PhoenixResponse>();

                    if (data == null || data.status != "pass" || data.result == null || data.result.Count == 0)
                    {
                        throw new ExceptionCustom($"ไม่พบข้อมูล CIF {cif}");
                    }

                    phoenixModel = data.result.Select(item => new Sale_PhoenixCustom
                    {
                        workflow_id_int = int.TryParse(item.workflow_id, out int wid) ? wid : 0,
                        workflow_id = item.workflow_id,
                        app_no = item.app_no,
                        ana_no = item.ana_no,
                        fin_type = item.fin_type,
                        cif_no = item.cif_no,
                        cif_name = item.cif_name,
                        branch_customer = item.branch_customer,
                        branch_user = item.branch_user,
                        approve_level = item.approve_level,
                        status_type = item.status_type,
                        status_code = item.status_code,
                        create_by = item.create_by,
                        created_date = item.created_date,
                        update_by = item.update_by,
                        update_date = item.update_date,
                        approve_by = item.approve_by,
                        approve_date = item.approve_date
                    })
                    .OrderByDescending(x => x.workflow_id_int)
                    .ToList();
                }
                else
                {
                    var PhoenixsDataTest = MoreDataModel.Phoenixs()?.Where(x => x.cif_no == cif).ToList();
                    if (PhoenixsDataTest == null || PhoenixsDataTest.Count == 0)
                        throw new ExceptionCustom($"ไม่พบข้อมูล CIF {cif}");

                    phoenixModel = PhoenixsDataTest;
                }

                return Ok(phoenixModel);
            }
            catch (Exception ex)
            {
                return new ErrorResultCustom(new ErrorCustom(), ex);
            }
        }

        [AllowAnonymous]
        [HttpGet("GetPhoenixBySaleId")]
        public async Task<IActionResult> GetPhoenixBySaleId([FromQuery] Guid saleid)
        {
            try
            {
                var data = await _repo.ProcessSale.GetPhoenixBySaleId(saleid);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return new ErrorResultCustom(new ErrorCustom(), ex);
            }
        }

        [AllowAnonymous]
        [HttpPut("SyncPhoenixBySaleId")]
        public async Task<IActionResult> SyncPhoenixBySaleId(PhoenixModel model)
        {
            try
            {
                List<Sale_PhoenixCustom>? phoenixModel = null;

                var sale = await _repo.Sales.GetStatusById(model.SaleId);
                if (sale == null) throw new ExceptionCustom("saleid not found.");
                if (string.IsNullOrEmpty(sale.CIF)) throw new ExceptionCustom("ไม่พบข้อมูล CIF ในกระบวนการขายนี้");

                if (_appSet.Phoenix != null && _appSet.Phoenix.IsConnect && MoreDataModel.Phoenixs()?.Select(x => x.cif_no).Contains(sale.CIF) == false)
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
                                if (adapter.Description.Contains("Array Networks VPN Adapter") && adapter.OperationalStatus == OperationalStatus.Up)
                                {
                                    isVpnConnect = true;
                                }
                            }
                        }

                        if (!isVpnConnect) throw new ExceptionCustom($"เชื่อมต่อ Phoenix ไม่สำเร็จ เนื่องจากไม่ได้ต่อ VPN");


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

                        var _httpClient = new HttpClient(handler);

                        string? fullUrl = $"{_appSet.Phoenix.baseUri}/phoenixbybaac/hisbyana/{sale.CIF}";
                        var request = new HttpRequestMessage(HttpMethod.Get, fullUrl);

                        request.Headers.Add("apikey", _appSet.Phoenix.ApiKey);

                        // ส่ง request
                        var response = await _httpClient.SendAsync(request);

                        var data = await response.Content.ReadFromJsonAsync<PhoenixResponse>();
                        if (data == null || data.status == "fail")
                        {
                            throw new ExceptionCustom($"ไม่พบข้อมูล CIF {sale.CIF}");
                        }
                        else
                        {
                            if (data.result != null && data.result.Count > 0)
                            {
                                phoenixModel = new();
                                foreach (var item in data.result)
                                {
                                    int.TryParse(item.workflow_id, out int _workflow_id_int);

                                    phoenixModel.Add(new()
                                    {
                                        workflow_id_int = _workflow_id_int,
                                        workflow_id = item.workflow_id,
                                        app_no = item.app_no,
                                        ana_no = item.ana_no,
                                        fin_type = item.fin_type,
                                        cif_no = item.cif_no,
                                        cif_name = item.cif_name,
                                        branch_customer = item.branch_customer,
                                        branch_user = item.branch_user,
                                        approve_level = item.approve_level,
                                        status_type = item.status_type,
                                        status_code = item.status_code,
                                        create_by = item.create_by,
                                        created_date = item.created_date,
                                        update_by = item.update_by,
                                        update_date = item.update_date,
                                        approve_by = item.approve_by,
                                        approve_date = item.approve_date
                                    });
                                }
                                phoenixModel = phoenixModel.OrderByDescending(x => x.workflow_id_int).ToList();
                            }
                        }
                    }
                }
                else
                {
                    var PhoenixsDataTest = MoreDataModel.Phoenixs()?.Where(x => x.cif_no == sale.CIF).ToList();
                    if (PhoenixsDataTest == null || PhoenixsDataTest.Count == 0)
                        throw new ExceptionCustom($"ไม่พบข้อมูล CIF {sale.CIF}");

                    phoenixModel = PhoenixsDataTest;
                }

                await _repo.ProcessSale.SyncPhoenixBySaleId(model.SaleId, phoenixModel);

                return Ok();
            }
            catch (Exception ex)
            {
                return new ErrorResultCustom(new ErrorCustom(), ex);
            }
        }

        [AllowAnonymous]
        [HttpPost("UpdatePhoenix")]
        public async Task<IActionResult> UpdatePhoenix(PhoenixModel model)
        {
            try
            {
                // 1. ตรวจสอบ model และ CIF
                if (model == null || string.IsNullOrWhiteSpace(model.CIF))
                    throw new ExceptionCustom("ข้อมูลไม่ถูกต้อง");

                string cif = model.CIF.Trim();

                List<Sale_PhoenixCustom>? phoenixModel = null;

                // 2. ตรวจสอบว่าต้องเรียก API หรือใช้ mock data
                bool shouldCallApi = _appSet.Phoenix != null
                                     && _appSet.Phoenix.IsConnect
                                     && (MoreDataModel.Phoenixs()?.Any(x => x.cif_no == cif) != true);

                if (shouldCallApi)
                {
                    // --- ตรวจสอบ VPN (เฉพาะ DEV) ---
                    if (_appSet.ServerSite == ServerSites.DEV)
                    {
                        bool isVpnConnected = NetworkInterface.GetAllNetworkInterfaces()
                            .Any(adapter =>
                                adapter.Description.Contains("Array Networks VPN Adapter")
                                && adapter.OperationalStatus == OperationalStatus.Up);

                        if (!isVpnConnected)
                            throw new ExceptionCustom("เชื่อมต่อ Phoenix ไม่สำเร็จ เนื่องจากไม่ได้ต่อ VPN");
                    }

                    // --- สร้าง HttpClient อย่างปลอดภัย ---
                    var handler = new HttpClientHandler();
                    bool isDevOrUat = _appSet.ServerSite == ServerSites.DEV || _appSet.ServerSite == ServerSites.UAT;

                    if (isDevOrUat)
                    {
                        handler.ServerCertificateCustomValidationCallback =
                            (message, cert, chain, errors) => errors == SslPolicyErrors.None;
                    }

                    using var httpClient = new HttpClient(handler);

                    // --- สร้าง URL อย่างปลอดภัย (ป้องกัน Path Traversal) ---
                    var uriBuilder = new UriBuilder(_appSet.Phoenix.baseUri)
                    {
                        Path = Path.Combine("phoenixbybaac", "hisbyana", cif).Replace("\\", "/")
                    };
                    string fullUrl = uriBuilder.ToString();

                    var request = new HttpRequestMessage(HttpMethod.Get, fullUrl);
                    request.Headers.Add("apikey", _appSet.Phoenix.ApiKey);

                    // --- เรียก API ---
                    var response = await httpClient.SendAsync(request);
                    var data = await response.Content.ReadFromJsonAsync<PhoenixResponse>();

                    if (data == null || data.status == "fail" || data.result == null || data.result.Count == 0)
                    {
                        throw new ExceptionCustom($"ไม่พบข้อมูล CIF {cif}");
                    }

                    // --- แปลงข้อมูล ---
                    phoenixModel = data.result
                        .Select(item => new Sale_PhoenixCustom
                        {
                            workflow_id_int = int.TryParse(item.workflow_id, out int wid) ? wid : 0,
                            workflow_id = item.workflow_id,
                            app_no = item.app_no,
                            ana_no = item.ana_no,
                            fin_type = item.fin_type,
                            cif_no = item.cif_no,
                            cif_name = item.cif_name,
                            branch_customer = item.branch_customer,
                            branch_user = item.branch_user,
                            approve_level = item.approve_level,
                            status_type = item.status_type,
                            status_code = item.status_code,
                            create_by = item.create_by,
                            created_date = item.created_date,
                            update_by = item.update_by,
                            update_date = item.update_date,
                            approve_by = item.approve_by,
                            approve_date = item.approve_date
                        })
                        .OrderByDescending(x => x.workflow_id_int)
                        .ToList();
                }
                else
                {
                    // --- ใช้ข้อมูลจาก MoreDataModel (mock) ---
                    var phoenixData = MoreDataModel.Phoenixs()?.Where(x => x.cif_no == cif).ToList();
                    if (phoenixData == null || phoenixData.Count == 0)
                        throw new ExceptionCustom($"ไม่พบข้อมูล CIF {cif}");

                    phoenixModel = phoenixData;
                }

                // --- อัปเดตข้อมูลใน Repository ---
                await _repo.ProcessSale.UpdatePhoenix(model, phoenixModel);

                return Ok();
            }
            catch (Exception ex)
            {
                return new ErrorResultCustom(new ErrorCustom(), ex);
            }
        }

        [HttpPost("CreateDocumentFile")]
        public async Task<IActionResult> CreateDocumentFile([FromForm] Sale_Document_UploadCustom model)
        {
            try
            {
                using (var _transaction = _repo.BeginTransaction())
                {
                    var data = await _repo.ProcessSale.CreateDocumentFile(model);

                    await _transaction.CommitAsync();

                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                return new ErrorResultCustom(new ErrorCustom(), ex);
            }
        }

        [HttpGet("GetDocumentFileById")]
        public async Task<IActionResult> GetDocumentFileById([FromQuery] Guid id)
        {
            try
            {
                var data = await _repo.ProcessSale.GetDocumentFileById(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return new ErrorResultCustom(new ErrorCustom(), ex);
            }
        }

        [HttpGet("GetListDocumentFile")]
        public async Task<IActionResult> GetListDocumentFile([FromQuery] allFilter model)
        {
            try
            {
                var response = await _repo.ProcessSale.GetListDocumentFile(model);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return new ErrorResultCustom(new ErrorCustom(), ex);
            }
        }

        [HttpDelete("DocumentFileDeleteById")]
        public async Task<IActionResult> DocumentFileDeleteById([FromQuery] UpdateModel model)
        {
            try
            {
                await _repo.ProcessSale.DocumentFileDeleteById(model);
                return Ok();
            }
            catch (Exception ex)
            {
                return new ErrorResultCustom(new ErrorCustom(), ex);
            }
        }

    }
}
