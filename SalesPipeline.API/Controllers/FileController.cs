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

	}
}
