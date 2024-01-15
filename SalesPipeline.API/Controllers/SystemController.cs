using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Helpers;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Customers;
using SalesPipeline.Utils.Resources.ManageSystems;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.API.Controllers
{
	[Authorizes]
	[ApiVersion(1.0)]
	[ApiController]
	[Route("v{version:apiVersion}/[controller]")]
	public class SystemController : ControllerBase
	{
		private IRepositoryWrapper _repo;
		private readonly AppSettings _appSet;

		public SystemController(IRepositoryWrapper repo, IOptions<AppSettings> appSet)
		{
			_repo = repo;
			_appSet = appSet.Value;
		}

		[HttpPost("CreateSignature")]
		public async Task<IActionResult> CreateSignature(System_SignatureCustom model)
		{
			try
			{
				var data = await _repo.System.CreateSignature(model);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpGet("GetSignatureLast")]
		public async Task<IActionResult> GetSignatureLast()
		{
			try
			{
				var data = await _repo.System.GetSignatureLast();
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPost("CreateSLA")]
		public async Task<IActionResult> CreateSLA(System_SLACustom model)
		{
			try
			{
				var data = await _repo.System.CreateSLA(model);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPut("UpdateSLA")]
		public async Task<IActionResult> UpdateSLA(System_SLACustom model)
		{
			try
			{
				var data = await _repo.System.UpdateSLA(model);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpDelete("DeleteSLAById")]
		public async Task<IActionResult> DeleteSLAById([FromQuery] UpdateModel model)
		{
			try
			{
				await _repo.System.DeleteSLAById(model);
				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpGet("GetSLAById")]
		public async Task<IActionResult> GetSLAById([FromQuery] Guid id)
		{
			try
			{
				var data = await _repo.System.GetSLAById(id);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpGet("GetSLAs")]
		public async Task<IActionResult> GetSLAs([FromQuery] allFilter model)
		{
			try
			{
				var response = await _repo.System.GetSLAs(model);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

	}
}
