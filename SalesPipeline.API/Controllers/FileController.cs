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
	//[Authorizes]
	[ApiVersion(1.0)]
	[ApiController]
	[Route("v{version:apiVersion}/[controller]")]
	public class FileController : ControllerBase
	{
		private IRepositoryWrapper _repo;
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
				//var parametere = Securitys.Base64StringEncode("all/1.jpg"); //YWxsLzEuanBn
				ResponseHeaders headers = this.Response.GetTypedHeaders();
				headers.CacheControl = new CacheControlHeaderValue
				{
					Public = true,
					MaxAge = TimeSpan.FromDays(30)
				};
				headers.Expires = new DateTimeOffset(DateTime.Now.AddDays(30));

				var parameter = Securitys.Base64StringDecode(id);
				var path = $@"{_appSet.ContentRootPath}/files/{parameter}";
				//var path = $@"C://DataRM//files//all//{id}.jpg";

				string? contentType = string.Empty;
				new FileExtensionContentTypeProvider().TryGetContentType(path, out contentType);

				//"image/jpeg"
				var filename = Path.GetFileName(path);
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

		[AllowAnonymous]
		[HttpPost("ImportZipCode")]
		public async Task<IActionResult> ImportZipCode(IFormFile files)
		{
			try
			{
				await Task.Delay(1);
				List<InfoTambolCustom> TambolList = new List<InfoTambolCustom>();

				if (files == null) throw new Exception("Select File.");

				int fileLimit = 100; //MB
				int TenMegaBytes = fileLimit * 1024 * 1024;
				var fileSize = files.Length;
				if (fileSize > TenMegaBytes)
				{
					throw new Exception($"ขนาดไฟล์ไม่เกิน {fileLimit} MB");
				}

				string folderName = @$"{_appSet.ContentRootPath}\import\excel";

				if (files.Length > 0)
				{
					string sFileExtension = Path.GetExtension(files.FileName).ToLower();
					if (sFileExtension != ".xls" && sFileExtension != ".xlsx" && sFileExtension != ".csv")
						throw new Exception("FileExtension Not Support.");

					ISheet sheet;
					string fullPath = Path.Combine(folderName, files.FileName);
					using (var stream = new FileStream(fullPath, FileMode.Create))
					{
						files.CopyTo(stream);
						stream.Position = 0;
						int sheetCount = 0;
						if (sFileExtension == ".xls")
						{
							throw new Exception("not support  Excel 97-2000 formats.");
						}

						XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
						sheetCount = hssfwb.NumberOfSheets;
						sheet = hssfwb.GetSheetAt(0);

						int firstRowNum = sheet.FirstRowNum;
						for (int i = (firstRowNum + 1); i <= sheet.LastRowNum; i++)
						{
							IRow row = sheet.GetRow(i);
							if (row == null) continue;
							if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

							var tambolCode = row.GetCell(0) != null ? row.GetCell(0).ToString() : null;
							var zipCode = row.GetCell(1) != null ? row.GetCell(1).ToString() : null;

							if (tambolCode != null)
							{
								TambolList.Add(new()
								{
									TambolCode = tambolCode,
									ZipCode = zipCode
								});
							}
						}

						if (TambolList.Count > 0)
						{
							await _repo.Thailand.MapZipCode(TambolList);
						}


					}
				}

				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

	}
}
