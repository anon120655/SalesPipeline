using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Helpers;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils.Resources.ManageSystems;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.StaticFiles;
using NPOI.HPSF;
using NPOI.OpenXmlFormats.Vml;
using Microsoft.AspNetCore.Authorization;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Thailands;

namespace SalesPipeline.API.Controllers
{
	[ApiVersion(1.0)]
	[ApiController]
	[Route("v{version:apiVersion}/[controller]")]
	public class FileController : ControllerBase
	{
		private readonly IRepositoryWrapper _repo;
		private readonly AppSettings _appSet;

		public FileController(IRepositoryWrapper repo, IOptions<AppSettings> appSet)
		{
			_repo = repo;
			_appSet = appSet.Value;
		}

		[HttpGet("v")]
		public IActionResult View(string id)
		{
			try
			{
				ResponseHeaders headers = this.Response.GetTypedHeaders();
				headers.CacheControl = new CacheControlHeaderValue
				{
					Public = true,
					MaxAge = TimeSpan.FromDays(30)
				};
				headers.Expires = new DateTimeOffset(DateTime.Now.AddDays(30));

				var parameter = Securitys.Base64StringDecode(id);
				var path = $@"{_appSet.ContentRootPath}/files/{parameter}";

				string? contentType = string.Empty;
				new FileExtensionContentTypeProvider().TryGetContentType(path, out contentType);

				return PhysicalFile(path, contentType ?? string.Empty);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPost("Upload")]
		public async Task<IActionResult> Upload([FromForm] FormFileUpload model)
		{
			try
			{
				var data = await _repo.Files.UploadFormFile(model);

				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

        [HttpPost("ImportZipCode")]
        public async Task<IActionResult> ImportZipCode(IFormFile file)
        {
            try
            {
                ValidateUploadFile(file);

                var uploadFolder = Path.Combine(_appSet.ContentRootPath, "import", "excel");
                Directory.CreateDirectory(uploadFolder);

                var safeFileName = Path.GetFileName(file.FileName); // ป้องกัน Path Traversal
                var fullPath = Path.Combine(uploadFolder, safeFileName);

                var listTambol = new List<InfoTambolCustom>();

                await using (var stream = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                {
                    await file.CopyToAsync(stream);
                    stream.Position = 0;

                    listTambol = ReadTambolFromExcel(stream, file.FileName);
                }

                if (listTambol.Count > 0)
                {
                    await _repo.Thailand.MapZipCode(listTambol);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return new ErrorResultCustom(new ErrorCustom(), ex);
            }
        }

        private void ValidateUploadFile(IFormFile file)
        {
            if (file == null)
                throw new ExceptionCustom("Select File.");

            const int limitMb = 100;
            const int maxBytes = limitMb * 1024 * 1024;

            if (file.Length > maxBytes)
                throw new ExceptionCustom($"ขนาดไฟล์ไม่เกิน {limitMb} MB");

            var ext = Path.GetExtension(file.FileName)?.ToLowerInvariant();

            switch (ext)
            {
                case ".xlsx":
                case ".csv":
                    break;

                case ".xls":
                    throw new ExceptionCustom("ไม่รองรับไฟล์ Excel 97-2000 (.xls)");

                default:
                    throw new ExceptionCustom("FileExtension Not Support.");
            }
        }

        private List<InfoTambolCustom> ReadTambolFromExcel(Stream stream, string fileName)
        {
            var result = new List<InfoTambolCustom>();
            var ext = Path.GetExtension(fileName).ToLowerInvariant();

            if (ext == ".csv")
                return ReadCsv(stream);

            // Default = xlsx
            var workbook = new XSSFWorkbook(stream);
            var sheet = workbook.GetSheetAt(0);

            for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);
                if (row == null) continue;
                if (row.Cells.All(c => c.CellType == CellType.Blank)) continue;

                var tambolCode = row.GetCell(0)?.ToString();
                if (string.IsNullOrWhiteSpace(tambolCode)) continue;

                result.Add(new InfoTambolCustom
                {
                    TambolCode = tambolCode,
                    ZipCode = row.GetCell(1)?.ToString()
                });
            }

            return result;
        }

        private List<InfoTambolCustom> ReadCsv(Stream stream)
        {
            var result = new List<InfoTambolCustom>();

            using var reader = new StreamReader(stream);

            // skip header
            _ = reader.ReadLine();

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split(',');

                if (parts.Length >= 1)
                {
                    result.Add(new InfoTambolCustom
                    {
                        TambolCode = parts[0],
                        ZipCode = parts.Length > 1 ? parts[1] : null
                    });
                }
            }
            return result;
        }


    }
}
