using Asp.Versioning;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Helpers;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils;
using SalesPipeline.Utils.ValidationModel;
using Tesseract;
//using System.Drawing;
//using System.Drawing.Imaging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Azure;
using System.IO;

namespace SalesPipeline.API.Controllers
{
    //[Authorizes]
    [ApiVersion(1.0)]
    [ApiController]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [Route("v{version:apiVersion}/[controller]")]
    public class OCRController : ControllerBase
    {
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly NotificationService _notiService;
        private IRepositoryWrapper _repo;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly AppSettings _appSet;

        public OCRController(IRepositoryWrapper repo, IHttpClientFactory httpClientFactory, IOptions<AppSettings> appSet, IBackgroundJobClient backgroundJobClient, NotificationService notificationService)
        {
            _repo = repo;
            _httpClientFactory = httpClientFactory;
            _appSet = appSet.Value;
            _backgroundJobClient = backgroundJobClient;
            _notiService = notificationService;
        }

        [HttpGet("Process")]
        public async Task<IActionResult> Process([FromQuery] string imageUrl)
        {
            int number = 47;
            try
            {
                // ตั้งค่าเส้นทางไปยังไลบรารี Leptonica และ Tesseract
                var envPath = Environment.GetEnvironmentVariable("PATH");
                var currentDir = Directory.GetCurrentDirectory();
                var tessDir = Path.Combine(currentDir, "tesseract"); // โฟลเดอร์ที่เก็บไลบรารี
                Environment.SetEnvironmentVariable("PATH", envPath + ";" + tessDir);

                //throw new Exception($"{tessDir}");

                // กำหนดที่ตั้งของไฟล์ภาษาไทย
                string tessDataPath = @$"{_appSet.ContentRootPath}/tesseract";

                // บันทึกภาพลงไฟล์ชั่วคราว
                string tempImagePath = $"{tessDataPath}/tempimage/OCR_{DateTime.Now.ToString("yyyyMMddHHmmss")}.png";

                // ดาวน์โหลดภาพจาก URL
                using (var httpClient = new HttpClient())
                {
                    //try
                    //{
                    number = 67;
                    var imageBytes = await httpClient.GetByteArrayAsync(imageUrl);
                    number = 69;
                    using (var ms = new MemoryStream(imageBytes))
                    {
                        number = 72;
                        // โหลดภาพโดยใช้ ImageSharp
                        using (var image = Image.Load<Rgba32>(ms))
                        {
                            number = 76;

                            tessDataPath = tessDataPath.Replace(@"\\", "/");
                            tessDataPath = tessDataPath.Replace(@"\", "/");
                            tessDataPath = tessDataPath.Replace(@"\\", "/");
                            tessDataPath = tessDataPath.Replace("/", "/");

                            tempImagePath = tempImagePath.Replace(@"\\", "/");
                            tempImagePath = tempImagePath.Replace(@"\", "/");
                            tempImagePath = tempImagePath.Replace(@"\\", "/");
                            tempImagePath = tempImagePath.Replace("/", "/");

                            image.Save(tempImagePath);

                            number = 90;
                            // ใช้ TesseractEngine โดยกำหนดเส้นทางไปยังไฟล์ tessdata และภาษาไทย (tha)
                            using (var engine = new TesseractEngine($"{tessDataPath}/data", "tha", EngineMode.Default))
                            {
                                number = 83;
                                using (var img = Pix.LoadFromFile(tempImagePath))
                                {
                                    number = 86;
                                    using (var page = engine.Process(img))
                                    {
                                        number = 89;
                                        string text = page.GetText();

                                        // ลบไฟล์ชั่วคราวหลังจากประมวลผลเสร็จ
                                        try
                                        {
                                            System.IO.File.Delete(tempImagePath);
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine("เกิดข้อผิดพลาดขณะลบไฟล์: " + ex.Message);
                                        }

                                        return Ok(text);
                                    }
                                }
                            }


                        }
                    }
                    //}
                    //catch (Exception)
                    //{
                    //    throw new Exception($"{tessDataPath} {tempImagePath} {number}");
                    //}

                }
            }
            catch (Exception ex)
            {
                return new ErrorResultCustom(new ErrorCustom(), ex);
            }
        }

    }
}
